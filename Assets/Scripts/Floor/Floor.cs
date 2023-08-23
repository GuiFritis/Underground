using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Padrao.Utils;

namespace Floors
{
    public enum FloorWorld
    {
        FLOATING_ROCKS,
        WARPED_CAVES,
        GRAVEYARD,
        SUNNY_LANDS,
        SWAMP,
        FUTURE
    }

    public class Floor : MonoBehaviour
    {
        public FloorWorld floorWorld;
        public List<FloorEnemy> floorEnemies = new();
        public List<FloorItem> floorItens = new();
        public Action OnComplete;
        public int enemiesToSpawn = 0;
        private float _enemySpawnRate = 1f;
        private float _enemySpawnCooldown;
        private int _spawnedEnemies = 0;
        private int _killedEnemies = 0;

        void OnValidate()
        {
            if(floorItens.Count == 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    floorItens.Add(transform.GetComponentInChildren<FloorItem>());
                }
            }
        }

        void Update()
        {
            if(_spawnedEnemies < enemiesToSpawn)
            {
                if(_enemySpawnCooldown < 0)
                {
                    SpawnEnemy();
                    _enemySpawnCooldown = _enemySpawnRate;
                }
                else
                {
                    _enemySpawnCooldown -= Time.deltaTime;
                }
            }
            else
            {
                if(_spawnedEnemies == _killedEnemies)
                {
                    OnComplete?.Invoke();
                }
            }
        }

        public void StartFloor(float spawnRate)
        {
            _enemySpawnRate = spawnRate;
            foreach (var item in floorItens)
            {
                item.EnableItem();
            }
        }

        public void SpawnEnemy()
        {
            FloorEnemy floorEnemy = floorEnemies.GetRandom();
            EnemyBase enemy = Instantiate(
                floorEnemy.pfbEnemy, 
                new Vector3(
                    FloorManager.Instance.spawnPointX * (UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1), 
                    transform.position.y + floorEnemy.startPointY
                ), 
                floorEnemy.pfbEnemy.transform.rotation, 
                transform
            );
            enemy.SetTarget(FloorManager.Instance.player);
            _spawnedEnemies++;
            enemy.health.OnDeath += EnemyKilled;
        }

        private void EnemyKilled(HealthBase hp)
        {
            _killedEnemies++;
            FloorManager.Instance.soEnemiesKilled.Value++;
        }

        public void EndFloor()
        {
            foreach (var item in floorItens)
            {
                item.DisableItem();
            }
        }
    }

    [Serializable]
    public class FloorEnemy
    {
        public EnemyBase pfbEnemy;
        public float startPointY = 0;
    }
}