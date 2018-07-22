using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

/// <summary>
/// Waiting for restart Game
/// </summary>\
public class ExplosionFactory : GameObjectFactory { }

public class BrokenHookFactory : GameObjectFactory { }

public class HookStateDead : HookState
{
    private readonly HookCrashedSignal _hookCrashedSignal;
    private readonly ExplosionFactory _explosionFactory;
    private readonly BrokenHookFactory _brokenHookFactory;
    private readonly Settings _settings;
    private readonly Hook _hook;

    GameObject _hookBroken;
    GameObject _explosion;

    public HookStateDead(HookCrashedSignal hookCrashedSignal,
        ExplosionFactory explosionFactory,
        BrokenHookFactory brokenHookFactory,
        Settings settings,
        Hook hook)
    {
        _hookCrashedSignal = hookCrashedSignal;
        _explosionFactory = explosionFactory;
        _brokenHookFactory = brokenHookFactory;
        _settings = settings;
        _hook = hook;
    }

    public override void Start()
    {
        _hook.Graphics.SetActive(false);

        _explosion = _explosionFactory.Create();
        _explosion.transform.position = _hook.Position;

        _hookBroken = _brokenHookFactory.Create();
        _hookBroken.transform.position = _hook.Position;
        _hookBroken.transform.rotation = _hook.Rotation;

        foreach (var rigidBody in _hookBroken.GetComponentsInChildren<Rigidbody2D>())
        {
            var randomTheta = UnityEngine.Random.Range(0, Mathf.PI * 2.0f);
            var randomDir = new Vector3(Mathf.Cos(randomTheta), Mathf.Sin(randomTheta), 0);
            rigidBody.AddForce(randomDir * _settings.explosionForce);
            rigidBody.AddTorque(_settings.explosionTorque);
        }

        _hookCrashedSignal.Fire();
    }

    public override void Update() { }

    public override void Dispose()
    {
        _hook.Graphics.SetActive(true);

        GameObject.Destroy(_explosion);
        GameObject.Destroy(_hookBroken);
    }

    [Serializable]
    public class Settings
    {
        public float explosionForce;
        public float explosionTorque;
    }

    public class Factory : Factory<HookStateDead> { }
}
