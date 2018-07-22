using System;
using UnityEngine;
using System.Collections;
using Zenject;

public abstract class AlienState : IDisposable
{
    public abstract void Update();

    public virtual void FixedUpdate()
    {
        // optionally overridden
    }

    public virtual void Start()
    {
        // optionally overridden
    }

    public virtual void Dispose()
    {
        // optionally overridden
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        // optionally overridden
    }
}
