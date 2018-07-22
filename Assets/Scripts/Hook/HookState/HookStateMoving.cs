using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// While player hold input, move Up until reached top
/// </summary>
public class HookStateMoving: HookState
{
    private Settings _settings;
    private Hook _hook;
    private readonly AudioHandler _audio;

    public HookStateMoving(Settings settings, Hook hook, AudioHandler audio)
    {
        _settings = settings;
        _hook = hook;
        _audio = audio;
    }

	public override void Update ()
	{
	    if (Input.GetMouseButton(0))
	    {
	        _hook.Rigidbody.velocity = Vector2.up * _settings.moveSpeed;
        }
	    if (Input.GetMouseButtonUp(0))
	    {
	        _hook.ChangeState(HookStates.Returning);
	    }
	}

    public override void Dispose()
    {
        _hook.Rigidbody.velocity *= 0;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        var asteroid = other.GetComponent<Asteroid>();
        if(asteroid)
            _hook.ChangeState(HookStates.Dead);

        if(_hook.CatchedAlien) return;

        _hook.CatchedAlien = other.GetComponent<Alien>();

        if (_hook.CatchedAlien)
        {
            _hook.Animator.SetTrigger("Catch");

            _hook.ChangeState(HookStates.Returning);

            _hook.CatchedAlien.transform.SetParent(_hook.transform);
            _hook.CatchedAlien.ChangeState(AlienStates.Moving);

            _audio.Play(_settings.hookOnAudioClip);
        }
    }

    [Serializable]
    public class Settings
    {
        public float moveSpeed;

        public AudioClip hookOnAudioClip;
    }

    public class Factory : Factory<HookStateMoving>
    {

    }

}
