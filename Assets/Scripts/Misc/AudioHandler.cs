using System;
using UnityEngine;
using Zenject;

public class AudioHandler : IInitializable, IDisposable
{
    readonly Settings _settings;
    readonly AudioSource _audioSource;

    HookCrashedSignal _hookCrashedSignal;

    public AudioHandler(
        AudioSource audioSource,
        Settings settings,
        HookCrashedSignal hookCrashedSignal)
    {
        _hookCrashedSignal = hookCrashedSignal;
        _settings = settings;
        _audioSource = audioSource;
    }

    public void Play(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void Initialize()
    {
        _hookCrashedSignal += OnHookCrashed;
    }

    public void Dispose()
    {
        _hookCrashedSignal -= OnHookCrashed;
    }

    private void OnHookCrashed()
    {
        _audioSource.PlayOneShot(_settings.CrashSound);
    }

    [Serializable]
    public class Settings
    {
        public AudioClip CrashSound;
    }
}
