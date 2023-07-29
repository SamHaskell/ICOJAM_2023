using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public enum GameState {
        PLAYING,
        PAUSED
    }

    public static GameManager Instance { get; private set; }
    public static bool Paused { get; private set; }
    public static event Action OnGamePause;
    public static event Action OnGameResume;
    private PlayerControls _playerControls;
    private GameState _gameState;
    private float _globalRuntime;
    private float _playRuntime;
    private float _cachedTimescale;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }

        _gameState = GameState.PLAYING;
        Paused = false;

        _playerControls = new PlayerControls();
        _playerControls.Enable();
        _playerControls.Player.Pause.started += OnPauseInput;
        _cachedTimescale = 1.0f;
        _globalRuntime = 0.0f;
        _playRuntime = 0.0f;
    }

    private void Update()
    {
        _globalRuntime += Time.unscaledDeltaTime;
        switch (_gameState) {
            case GameState.PLAYING:
                _playRuntime += Time.deltaTime;
                _cachedTimescale = Time.timeScale;
                break;
            case GameState.PAUSED:
                break;
        }     
    }

    private void OnPauseInput(InputAction.CallbackContext ctx)
    {
        switch (_gameState) {
            case GameState.PLAYING:
                PauseGame();
                break;
            case GameState.PAUSED:
                ResumeGame();
                break;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        _gameState = GameState.PAUSED;
        Paused = true;
        OnGamePause?.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = _cachedTimescale;
        _gameState = GameState.PLAYING;
        Paused = false;
        OnGameResume?.Invoke();
    }
}