using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Alien : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _graphics;
    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private Animator _anim;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private Settings _settings;

    private AlienStateFactory _stateFactory;
    private AlienState _state;

    [Inject]
    public void Construct(AlienStateFactory stateFactory, Settings settings)
    {
        _stateFactory = stateFactory;
        _settings = settings;
    }

    public Collider2D Collider
    {
        get { return _collider2D; }
    }
    public Rigidbody2D Rigidbody
    {
        get { return _rb2D; }
    }
    public Animator Animator
    {
        get { return _anim; }
    }
    public SpriteRenderer Graphics
    {
        get { return _graphics; }
    }

    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public Quaternion Rotation
    {
        get { return transform.rotation; }
        set { transform.rotation = value; }
    }

    public void Start()
    {
        ChangeState(AlienStates.Waiting);
    }

    public void Update()
    {
        _state.Update();
    }

    public void FixedUpdate()
    {
        _state.FixedUpdate();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        _state.OnTriggerEnter2D(other);
    }

    public void ChangeState(AlienStates state)
    {
        if (_state != null)
        {
            _state.Dispose();
            _state = null;
        }

        _state = _stateFactory.CreateState(state);
        _state.Start();
    }

    public void SetRandomAnimatorController()
    {
        _anim.runtimeAnimatorController = _settings.controllers[Random.Range(0, _settings.controllers.Length)];
    }

    public class Factory : Factory<Alien>
    {

    }

    [System.Serializable]
    public class Settings
    {
        public AnimatorOverrideController[] controllers;
    }
}

