using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float _baseLoopTime;
    [SerializeField] private AnimationCurve _timeScaleCurve;
    [SerializeField] private float _baseTimeChangeRate;
    private float _currentLoopTime;
    private float _currentTimeChangeRate;
    private float _timer;
    public static TimeManager Instance { get; private set; }
    public static Action OnTimeStop;
    public static Action OnTimeBegin;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
        _timer = 0.0f;
        _currentLoopTime = _baseLoopTime;
        _currentTimeChangeRate = _baseTimeChangeRate;
    }

    private void Start()
    {
        StartCoroutine(MainLoop());
    }

    void Update()
    {

    }

    public void OnLoopBegin()
    {
        StartCoroutine(MainLoop());
    }

    public IEnumerator MainLoop()
    {
        OnTimeBegin?.Invoke();
        yield return StartTime();
        yield return new WaitForSeconds(_currentLoopTime);
        yield return StopTime();
        OnTimeStop?.Invoke();
    }

    public IEnumerator StopTime()
    {
        while (_timer < 1.0f && !GameManager.Paused) {
            _timer += Time.unscaledDeltaTime;
            Time.timeScale = _timeScaleCurve.Evaluate(1 - (_timer/_currentTimeChangeRate));
            yield return null;
        }
        _timer = 0.0f;
    }

    public IEnumerator StartTime()
    {
        while (_timer < 1.0f && !GameManager.Paused) {
            _timer += Time.unscaledDeltaTime;
            Time.timeScale = _timeScaleCurve.Evaluate(_timer/_currentTimeChangeRate);
            yield return null;
        }
        _timer = 0.0f;
    }

}