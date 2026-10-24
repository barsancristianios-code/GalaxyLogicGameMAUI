"""Procedurally synthesize the game's sound effects and music loop.

Everything is generated from math, so there are no third-party licenses.
Usage: python3 gen_audio.py <output_dir> [<output_dir> ...]
"""
import sys, wave
import numpy as np

SR = 22050
rng = np.random.default_rng(7)


def env(n, attack=0.005, release=None):
    t = np.arange(n) / SR
    a = np.clip(t / attack, 0, 1)
    if release is None:
        return a
    return a * np.exp(-t / release)


def tone(freq, dur, release, harmonics=(1.0, 0.3, 0.12), glide=0.0):
    n = int(SR * dur)
    t = np.arange(n) / SR
    f = freq * (1 + glide * t / dur)
    phase = 2 * np.pi * np.cumsum(f) / SR
    sig = sum(amp * np.sin(phase * (i + 1)) for i, amp in enumerate(harmonics))
    return sig * env(n, 0.004, release)


def save(path, sig, peak=0.8):
    sig = sig / (np.max(np.abs(sig)) + 1e-9) * peak
    data = (sig * 32767).astype(np.int16)
    with wave.open(path, "wb") as w:
        w.setnchannels(1)
        w.setsampwidth(2)
        w.setframerate(SR)
        w.writeframes(data.tobytes())


def place():
    # soft "pop": quick downward glide with a little noise transient
    s = tone(520, 0.16, 0.05, harmonics=(1.0, 0.25), glide=-0.45)
    noise = rng.normal(0, 1, len(s)) * env(len(s), 0.001, 0.008) * 0.25
    return s + noise


def merge(level):
    # bright two-note chime, pitch rises with combo level
    base = 440 * 2 ** ((level - 1) * 2 / 12)
    a = tone(base, 0.45, 0.18, harmonics=(1.0, 0.4, 0.2, 0.08))
    b = tone(base * 1.5, 0.45, 0.2, harmonics=(1.0, 0.3, 0.1))
    off = int(SR * 0.06)
    out = np.zeros(len(a) + off)
    out[: len(a)] += a
    out[off:] += b[: len(out) - off] * 0.8
    return out


def click():
    return tone(1200, 0.06, 0.012, harmonics=(1.0, 0.2))


def gameover():
    notes = [392, 330, 262, 196]
    out = np.zeros(int(SR * 1.6))
    for i, f in enumerate(notes):
        s = tone(f, 0.9, 0.35, harmonics=(1.0, 0.5, 0.25, 0.1))
        start = int(SR * 0.22 * i)
        out[start : start + len(s)] += s[: len(out) - start]
    return out


def explosion():
    n = int(SR * 0.9)
    noise = rng.normal(0, 1, n)
    # crude low-pass by cumulative averaging
    k = 40
    lp = np.convolve(noise, np.ones(k) / k, mode="same")
    boom = tone(70, 0.9, 0.3, harmonics=(1.0, 0.5), glide=-0.5)
    return lp * env(n, 0.002, 0.25) * 3 + boom


def music(dur=32.0):
    # slow ambient pad: Am - F - C - G, each chord 8 s, seamless loop
    n = int(SR * dur)
    t = np.arange(n) / SR
    chords = [(220.0, 261.63, 329.63), (174.61, 220.0, 261.63), (130.81, 196.0, 261.63), (196.0, 246.94, 293.66)]
    seg = n // len(chords)
    out = np.zeros(n)
    fade = int(SR * 2.0)
    for ci, chord in enumerate(chords):
        s0 = ci * seg
        length = seg + fade
        tt = np.arange(length) / SR
        e = np.ones(length)
        e[:fade] = np.linspace(0, 1, fade)
        e[-fade:] = np.linspace(1, 0, fade)
        pad = np.zeros(length)
        for f in chord:
            for detune in (-0.6, 0.6):
                ff = f + detune
                pad += np.sin(2 * np.pi * ff * tt) + 0.3 * np.sin(2 * np.pi * ff * 2 * tt)
        pad *= e
        idx = (s0 + np.arange(length)) % n  # wrap so the loop point is seamless
        np.add.at(out, idx, pad)
    # gentle shimmer
    out += 0.15 * np.sin(2 * np.pi * 880 * t) * (0.5 + 0.5 * np.sin(2 * np.pi * t / 8.0)) ** 4
    lfo = 0.85 + 0.15 * np.sin(2 * np.pi * t / dur * 4)
    return out * lfo


def main():
    sounds = {
        "sfx_place.wav": place(),
        "sfx_click.wav": click(),
        "sfx_gameover.wav": gameover(),
        "sfx_explosion.wav": explosion(),
        "music_space.wav": music(),
    }
    for lvl in range(1, 6):
        sounds[f"sfx_merge{lvl}.wav"] = merge(lvl)
    for d in sys.argv[1:]:
        for name, sig in sounds.items():
            save(f"{d}/{name}", sig, 0.5 if name.startswith("music") else 0.8)
    print("wrote", len(sounds), "files to", len(sys.argv) - 1, "dirs")


if __name__ == "__main__":
    main()
