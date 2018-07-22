using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Do nothing. Waiting for player input
/// </summary>
public class HookStateWaiting : HookState
{
    private readonly Hook _hook;
    private readonly Settings _settings;

    private float _theta;

    public HookStateWaiting(Hook hook, Settings settings)
    {
        _hook = hook;
        _settings = settings;
    }

    public override void Start()
    {
        _hook.Position = _settings.startHookPosition;
        _hook.ShipPosition = _settings.startShipPosition;
    }

    public override void Update()
    {
        _hook.Position = _settings.startHookPosition + Vector3.up * _settings.Amplitude * Mathf.Sin(_theta);
        _hook.ShipPosition = _settings.startShipPosition + Vector3.up * _settings.Amplitude * Mathf.Sin(_theta);
        _theta += Time.deltaTime * _settings.Frequency;
    }

    public override void Dispose()
    {
        _hook.Position = _settings.startHookPosition;
        _hook.ShipPosition = _settings.startShipPosition;
    }

    [Serializable]
    public class Settings
    {
        public Vector3 startHookPosition;
        public Vector3 startShipPosition;

        public float Amplitude;
        public float Frequency;
    }

    public class Factory : Factory<HookStateWaiting>
    {
    }
}
