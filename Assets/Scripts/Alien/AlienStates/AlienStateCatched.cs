using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Alien clear off hook
/// </summary>
public class AlienStateCatched : AlienState
{
    private Alien _alien;
    private Settings _settings;

    private AlienCatchedSignal _catched;
    private LevelHelper _levelHelper;

    private bool _shouldRotateRight;
    public AlienStateCatched(Alien alien, Settings settings, AlienCatchedSignal catchedSignal, LevelHelper levelHelper)
    {
        _alien = alien;
        _settings = settings;
        _catched = catchedSignal;
        _levelHelper = levelHelper;
    }

    public override void Start()
    {
        _alien.transform.SetParent(null);
        _alien.Animator.SetTrigger("Released");
        _alien.Collider.enabled = false;

        _alien.Rigidbody.velocity = Vector2.down * _settings.speed;

        _catched.Fire();

        _shouldRotateRight = UnityEngine.Random.Range(0, 2) != 0;
    }

    public override void Update()
    {
        float speed = _shouldRotateRight ? _settings.rotationSpeed : -_settings.rotationSpeed;
        _alien.Rigidbody.MoveRotation(_alien.Rigidbody.rotation + speed * Time.fixedDeltaTime);

        if (IsOutOfBottomBorder())
        {
            _alien.ChangeState(AlienStates.Waiting);
        }
    }

    public override void Dispose()
    {
        _alien.Rigidbody.velocity *= 0;
        _alien.Collider.enabled = true;
        _alien.Rotation = Quaternion.identity;
    }

    public bool IsOutOfBottomBorder()
    {
        return _alien.Position.y < _levelHelper.Bottom - _alien.transform.localScale.y;
    }

    [Serializable]
    public class Settings
    {
        public float speed;
        public float rotationSpeed;
    }

    public class Factory : Factory<AlienStateCatched> { }
}
