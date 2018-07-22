using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Hook : MonoBehaviour
{
    [SerializeField] private GameObject _graphics;
    [SerializeField] private GameObject _ship;
    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private Animator _animator;

    private HookStateFactory _stateFactory;
    private HookState _state;

    [Inject]
    public void Construct(HookStateFactory stateFactory)
    {
        _stateFactory = stateFactory;
    }

    public GameObject Graphics
    {
        get { return _graphics; }
    }

    public Rigidbody2D Rigidbody
    {
        get { return _rb2D; }
    }

    public Animator Animator
    {
        get { return _animator; }
    }

    public GameObject Ship
    {
        get { return _ship; }
    }

    public Vector3 ShipPosition
    {
        get { return _ship.transform.position; }
        set { _ship.transform.position = value; }
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

    public Alien CatchedAlien { get; set; }

    public void Start()
    {
        ChangeState(HookStates.Waiting);
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

    public void ChangeState(HookStates state)
    {
        if (_state != null)
        {
            _state.Dispose();
            _state = null;
        }

        _state = _stateFactory.CreateState(state);
        _state.Start();
    }
}
