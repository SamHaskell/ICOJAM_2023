using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private float _spawnTime;
        [SerializeField] private float _spawnRadius;
        private float _timer;
    	void Awake()
    	{
            _timer = 0.0f;
    	}

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > _spawnTime) {
                Vector2 spawnPoint = Random.insideUnitCircle * _spawnRadius;
                Vector3 spawnPosition = new Vector3(spawnPoint.x, 0.0f, spawnPoint.y); 
                Spawn(spawnPosition);
                _timer = 0.0f;
            }
        }

        void Spawn(Vector3 position)
        {
            Instantiate(_enemyPrefab, position, Quaternion.identity);
        }
    }
