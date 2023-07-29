using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

    public class MainCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _suspendMenu;
    	void Awake()
    	{
            GameManager.OnGamePause += OnGamePause;
            GameManager.OnGameResume += OnGameResume;
            TimeManager.OnTimeStop += OnTimeStop;
            _pauseMenu.SetActive(false);
            _suspendMenu.SetActive(false);
    	}

        public void OnGamePause()
        {
            _pauseMenu.SetActive(true);
        }

        public void OnGameResume()
        {
            _pauseMenu.SetActive(false);
        }

        void OnTimeStop()
        {
            _suspendMenu.SetActive(true);
        }

        public void TimeBegin()
        {
            _suspendMenu.SetActive(false);
            TimeManager.Instance.OnLoopBegin();
        }
    }
