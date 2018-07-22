using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Alien moving with Hook
/// </summary>
public class AlienStateMoving : AlienState
{
    private Alien _alien;

    public AlienStateMoving( Alien alien)
    {
        _alien = alien;
    }

    public override void Start()
    {
        
    }

    public override void Update ()
	{

	}

    public override void Dispose()
    {

    }

    [Serializable]
    public class Settings
    {
        
    }

    public class Factory : Factory<AlienStateMoving>
    {

    }

}
