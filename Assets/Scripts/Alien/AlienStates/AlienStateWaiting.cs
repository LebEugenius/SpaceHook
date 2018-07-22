using System;
using System.Collections;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Zenject;

/// <summary>
/// Floating in space
/// </summary>
public class AlienStateWaiting : AlienState
{
    private readonly Alien _alien;
    private readonly Settings _settings;
    private readonly LevelHelper _levelHelper;

    private float _theta;

    private float _delta;
    private Vector3 _lastPosition;

    public AlienStateWaiting(Alien alien, Settings settings, LevelHelper levelHelper)
    {
        _alien = alien;
        _settings = settings;
        _levelHelper = levelHelper;
    }

    public override void Start()
    {
        _alien.SetRandomAnimatorController();

        _alien.Position = new Vector3(0, _levelHelper.Top + _alien.transform.localScale.y);

        _lastPosition = _alien.Position;
        _delta = 0;
    }

    public override void Update()
    {
        if (_delta <= 1f)
        {
            _alien.Position = Vector3.Lerp(_lastPosition, _settings.startAlienPosition, _delta);
            _delta += _settings.appearSpeed * Time.deltaTime;
        }

        if (_delta < 1f) return;

        _alien.Position = _settings.startAlienPosition + Vector3.up * _settings.Amplitude * Mathf.Sin(_theta);
        _theta += Time.deltaTime * _settings.Frequency;
    }

    [Serializable]
    public class Settings
    {
        public Vector3 startAlienPosition;
        public float appearSpeed;

        public float Amplitude;
        public float Frequency;
    }

    public class Factory : Factory<AlienStateWaiting>
    {
    }
}
