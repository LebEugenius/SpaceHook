using System;
using UnityEngine;
using Zenject;

/// <summary>
/// Just moving Back To Hook ignoring player input
/// </summary>
public class HookStateReturning : HookState
{
    private readonly Hook _hook;
    private readonly Settings _settings;
    private readonly AudioHandler _audio;

    private float _delta;
    private Vector3 _lastPosition;

    public HookStateReturning(Hook hook, Settings settings, AudioHandler audio)
    {
        _hook = hook;
        _settings = settings;
        _audio = audio;
    }

    public override void Start()
    {
        _lastPosition = _hook.Position;
        _delta = 0;
    }

    public override void Update()
    {
        _delta += _settings.returningSpeed * Time.deltaTime / (_settings.returningPosition - _lastPosition).magnitude;

        _delta = Mathf.Clamp01(_delta);
        _hook.Position = Vector3.Lerp(_lastPosition, _settings.returningPosition, _delta);

        if (_delta < 1f) return;

        _hook.ChangeState(HookStates.Moving);

        if (_hook.CatchedAlien)
        {
            _hook.Animator.SetTrigger("Release");
            _hook.CatchedAlien.ChangeState(AlienStates.Catched);
            _hook.CatchedAlien = null;
            _audio.Play(_settings.unhookAudioClip);
        }
    }

    [Serializable]
    public class Settings
    {
        public float returningSpeed;
        public Vector3 returningPosition;


        public AudioClip unhookAudioClip;
    }

    public class Factory : Factory<HookStateReturning>
    {
    }
}
