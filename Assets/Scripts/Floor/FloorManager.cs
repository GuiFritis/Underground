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
        public List<Floor> floors = new();
        private Floor _currentFloor;
        private Floor _nextFloor;
        [Header("Main Platform Move")]
        public float moveDuration = 3f;
        public float moveDistance = -5f;
        public Ease moveEase = Ease.OutBounce;
        public List<Transform> movingObjects;
        [Header("Enemy Spawn Rate")]
        public float startSpawnRate = 1.4f;
        public float updateRatePerFloor = -.08f;
        public float minStartSpawnRate = .5f;
        private float _currentSpawnRate = 1f;
        [Space]
        public float spawnPointX = 2.88f;

        void Start(){
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
            foreach (var item in movingObjects)
            {
                item.DOMoveY(moveDistance, moveDuration).SetEase(moveEase).SetRelative(true);
            }
            yield return new WaitForSeconds(moveDuration);
            StartNextFloor();
        }

        private void StartNextFloor()
        {
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
        }
        
        private void GenerateFirstFloor()
        {
            _nextFloor = floors.GetRandom();
            _nextFloor = Instantiate(_nextFloor, transform.position, Quaternion.identity);
            _nextFloor.enabled = false;
        }
    }
}