using System;
using UnityEngine;
using System.Collections;
using Zenject;
using ModestTree;

public class Asteroid : MonoBehaviour
{
    private LevelHelper _level;
    private Rigidbody2D _rigidBody;
    private SpriteRenderer _sprite;
    private Settings _settings;

    [Inject]
    public void Construct(LevelHelper level, Settings settings)
    {
        _level = level;
        _settings = settings;
        _rigidBody = GetComponent<Rigidbody2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public SpriteRenderer Sprite
    {
        get { return _sprite; }
    }

    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public float Mass
    {
        get { return _rigidBody.mass; }
        set { _rigidBody.mass = value; }
    }

    public float Scale
    {
        get
        {
            var scale = transform.localScale;
            // We assume scale is uniform
            Assert.That(scale[0] == scale[1] && scale[1] == scale[2]);

            return scale[0];
        }
        set
        {
            transform.localScale = new Vector3(value, value, value);
            _rigidBody.mass = value;
        }
    }

    public Vector3 Velocity
    {
        get { return _rigidBody.velocity; }
        set { _rigidBody.velocity = value; }
    }

    public void FixedTick()
    {
        // Limit speed to a maximum
        var speed = _rigidBody.velocity.magnitude;

        if (speed > _settings.maxSpeed)
        {
            var dir = _rigidBody.velocity / speed;
            _rigidBody.velocity = dir * _settings.maxSpeed;
        }
    }

    public void Tick()
    {
        CheckForTeleport();
    }

    void CheckForTeleport()
    {
        if (Position.x > _level.Right + Scale && IsMovingInDirection(Vector3.right))
        {
            transform.SetX(_level.Left - Scale);
        }
        else if (Position.x < _level.Left - Scale && IsMovingInDirection(Vector3.left))
        {
            transform.SetX(_level.Right + Scale);
        }
    }

    bool IsMovingInDirection(Vector3 dir)
    {
        return Vector3.Dot(dir, _rigidBody.velocity) > 0;
    }

    [Serializable]
    public class Settings
    {
        public float massScaleFactor;
        public float maxSpeed;
    }

    public class Factory : Factory<Asteroid>
    {
    }
}
