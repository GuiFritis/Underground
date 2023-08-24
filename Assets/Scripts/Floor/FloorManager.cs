using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Padrao.Core.Singleton;
using Padrao.Utils;
using DG.Tweening;

namespace Floors
{    
    public class FloorManager : Singleton<FloorManager>
    {
        public Player player;
        [Header("Floors")]
        public SOInt soFloor;
        public SOInt soEnemiesKilled;
        public List<Floor> floors = new();
        private Floor _currentFloor;
        private Floor _nextFloor;
        [Header("Main Platform Move")]
        public TextFadeHelper textCompleteFloor;
        public float moveDuration = 3f;
        public float moveDistance = -5f;
        public Ease moveEase = Ease.OutBounce;
        public List<Transform> movingObjects;
        [Header("Amount of Enemies Spawned")]
        public float minEnemySpawning = 1f;
        public float maxEnemySpawning = 4f;
        public float updateFloorMinEnemySpawning = .2f;
        public float updateFloorMaxEnemySpawning = .4f;
        [Header("Enemies Spawn Rate")]
        public float startSpawnRate = 1.4f;
        public float updateRatePerFloor = -.08f;
        public float minSpawnRate = .5f;
        private float _currentSpawnRate = 1f;
        [Space]
        public float spawnPointX = 2.88f;
        [Header("Hardener")]
        public TextFadeHelper textLifeIncreased;
        public float extraHealthPerFloor = .167f; //? 1 / 6
        public float extraHealthTextDuration = 2f;

        void Start(){
            soFloor.Value = 0;
            soEnemiesKilled.Value = 0;
            _currentSpawnRate = startSpawnRate;
            GenerateFirstFloor();
            StartNextFloor();
        }

        private void FloorCompleted()
        {
            _currentFloor.OnComplete -= FloorCompleted;
            _currentFloor.EndFloor();
            StartCoroutine(MovePlatformDown());
        }

        private IEnumerator MovePlatformDown()
        {
            textCompleteFloor.FadeIn();
            if(Mathf.Floor(extraHealthPerFloor * soFloor.Value) != Mathf.Floor(extraHealthPerFloor * (soFloor.Value + 1)))
            {
                StartCoroutine(ShowLifeIncreased());
            }
            yield return new WaitForSeconds(textCompleteFloor.fadeDuration);
            foreach (var item in movingObjects)
            {
                item.DOMoveY(moveDistance, moveDuration).SetEase(moveEase).SetRelative(true);
            }
            player.StartPlatformMove();
            player.transform.DOMoveY(moveDistance, moveDuration).SetEase(moveEase).SetRelative(true);
            textCompleteFloor.FadeOut();
            yield return new WaitForSeconds(moveDuration);
            player.EndPlatformMove();
            StartNextFloor();
        }

        private void StartNextFloor()
        {
            soFloor.Value++;
            if(_currentFloor != null)
            {
                Destroy(_currentFloor.gameObject);
            }
            _currentFloor = _nextFloor;
            _nextFloor = null;
            _currentFloor.enabled = true;
            _currentFloor.StartFloor(_currentSpawnRate);
            _currentFloor.OnComplete += FloorCompleted;
            _currentSpawnRate += updateRatePerFloor;
            minEnemySpawning += updateFloorMinEnemySpawning;
            maxEnemySpawning += updateFloorMaxEnemySpawning;
            if(_currentSpawnRate < minSpawnRate)
            {
                _currentSpawnRate = minSpawnRate;
            }
            GenerateNextFloor();
        }

        private void GenerateNextFloor()
        {
            for (int i = 0; i < 10; i++)
            {
                _nextFloor = floors.GetRandom();
                if(_nextFloor.floorWorld != _currentFloor.floorWorld)
                {
                    break;
                }
            }
            _nextFloor = Instantiate(_nextFloor, transform.position + Vector3.up * moveDistance, Quaternion.identity);
            _nextFloor.enabled = false;
            _nextFloor.enemiesToSpawn = Mathf.FloorToInt(
                Random.Range(minEnemySpawning, maxEnemySpawning)
            );
        }
        
        private void GenerateFirstFloor()
        {
            _nextFloor = floors.GetRandom();
            _nextFloor = Instantiate(_nextFloor, transform.position, Quaternion.identity);
            _nextFloor.enabled = false;
            _nextFloor.enemiesToSpawn = Mathf.FloorToInt(
                Random.Range(minEnemySpawning, maxEnemySpawning)
            );
        }      

        private IEnumerator ShowLifeIncreased()
        {
            textLifeIncreased.FadeIn();
            yield return new WaitForSeconds(extraHealthTextDuration);
            textLifeIncreased.FadeOut();
        }  
    }
}