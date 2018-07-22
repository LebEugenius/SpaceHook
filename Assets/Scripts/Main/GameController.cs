using System;
using UnityEngine;
using System.Collections;
using Zenject;
using ModestTree;
using System.Runtime.InteropServices;

public enum GameStates
{
    WaitingToStart,
    Playing,
    GameOver,
}

public class GameController : IInitializable, ITickable, IDisposable
{
    readonly Hook _hook;
    readonly AsteroidManager _asteroidSpawner;

    private HookCrashedSignal _hookCrashedSignal;
    private AlienCatchedSignal _alienCatchedSignal;

    GameStates _state = GameStates.WaitingToStart;

    private int _aliensCatched;
    private int _bestAliensCatched;

    public GameController(
        Hook hook, 
        AsteroidManager asteroidSpawner,
        HookCrashedSignal hookCrashedSignal,
        AlienCatchedSignal alienCatchedSignal)
    {
        _hookCrashedSignal = hookCrashedSignal;
        _asteroidSpawner = asteroidSpawner;
        _hook = hook;
        _alienCatchedSignal = alienCatchedSignal;
    }

    public int AliensCatched
    {
        get { return _aliensCatched; }
    }

    public int BestAliensCatched
    {
        get { return _bestAliensCatched; }
    }

    public GameStates State
    {
        get { return _state; }
    }

    public void Initialize()
    {
        Cursor.visible = false;

        _hookCrashedSignal += OnHookCrashed;
        _alienCatchedSignal += OnAlienCatched;
    }

    public void Dispose()
    {
        _hookCrashedSignal -= OnHookCrashed;
    }

    public void Tick()
    {
        switch (_state)
        {
            case GameStates.WaitingToStart:
            {
                UpdateStarting();
                break;
            }
            case GameStates.Playing:
            {
                UpdatePlaying();
                break;
            }
            case GameStates.GameOver:
            {
                UpdateGameOver();
                break;
            }
            default:
            {
                Assert.That(false);
                break;
            }
        }
    }

    void UpdateGameOver()
    {
        Assert.That(_state == GameStates.GameOver);

        if (Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }

    void OnHookCrashed()
    {
        if(_state != GameStates.Playing)
            return;

        _state = GameStates.GameOver;
        _asteroidSpawner.Stop();

        PlayerPrefs.SetInt("aliens", _bestAliensCatched);
    }

    void UpdatePlaying()
    {
        Assert.That(_state == GameStates.Playing);
    }

    void UpdateStarting()
    {
        Assert.That(_state == GameStates.WaitingToStart);

        if (Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        Assert.That(_state == GameStates.WaitingToStart || _state == GameStates.GameOver);

        _aliensCatched = 0;
        _bestAliensCatched = PlayerPrefs.GetInt("aliens");
        _asteroidSpawner.Start();
        _hook.ChangeState(HookStates.Waiting);
        _hook.ChangeState(HookStates.Moving);
        _state = GameStates.Playing;
    }

    private void OnAlienCatched()
    {
        _aliensCatched++;

        if (_aliensCatched > _bestAliensCatched)
            _bestAliensCatched = _aliensCatched;
    }
}
