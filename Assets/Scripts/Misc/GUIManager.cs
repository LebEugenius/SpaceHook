using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class GUIManager : MonoBehaviour
{
    private GameController _gameController;

    [Header("WaitingToStart")]
    [SerializeField] private GameObject _preparePanel;

    [Header("Game")]
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private Text _score;
    [SerializeField] private Text _bestScore;
    
    [Header("GameOver")]
    [SerializeField] private GameObject _gameOverPanel;

    [Inject]
    public void Construct(GameController gameController)
    {
        _gameController = gameController;
    }

    private GameStates _previousState = GameStates.GameOver;

    void Update()
    {
        OnStateChanged();

        if (_gameController.State != GameStates.Playing) return;

        _score.text = _gameController.AliensCatched.ToString();
        _bestScore.text = _gameController.BestAliensCatched.ToString();
    }

    void OnStateChanged()
    {
        if(_previousState == _gameController.State) return;

        switch (_gameController.State)
        {
            case GameStates.WaitingToStart:
                _preparePanel.SetActive(true);
                _gamePanel.SetActive(false);
                _gameOverPanel.SetActive(false);
                break;
            case GameStates.Playing:
                _preparePanel.SetActive(false);
                _gamePanel.SetActive(true);
                _gameOverPanel.SetActive(false);
                break;
            case GameStates.GameOver:
                _preparePanel.SetActive(false);
                _gamePanel.SetActive(false);
                _gameOverPanel.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _previousState = _gameController.State;
    }
}
