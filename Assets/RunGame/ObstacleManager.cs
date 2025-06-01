using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RunGame
{
    public class ObstacleManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] private CubeController playerCube;
        [SerializeField] private GameCtl gameCtl;

        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private int maxObstacleCount = 30;
        [SerializeField] private float maxSpawnInterval = 1.0f;
        [SerializeField] private float minSpawnInterval = 0.2f;

        private const float LaneDistance = 3f;

        public readonly List<GameObject> ActiveObstacles = new();
        private float _nextSpawnTime;

        private void Update()
        {
            if (!playerCube) return;
            if (!gameCtl) return;

            if (!gameCtl.isPlaying)
            {
                CleanUpAllObstacles();
            }

            if (!gameCtl.isPlaying) return;
            if (!(_nextSpawnTime <= Time.time)) return;
            SetNextSpawnTime();
            SpawnObstacle();
            CleanUpObstacles();
        }

        private void SpawnObstacle()
        {
            if (maxObstacleCount <= ActiveObstacles.Count) return;
            for (var i = 0; i < 2; i++)
            {
                var obstacle = GenerateObstacleSpawnPosition();
                ActiveObstacles.Add(obstacle);
            }
        }

private GameObject GenerateObstacleSpawnPosition()
{
    var lane = Random.Range(0, 3);
    var laneZ = lane switch
    {
        0 => -LaneDistance,
        1 => 0f,
        _ => LaneDistance
    };
    const float baseSpawnDistanceMultiplier = 2f;
    var speedFactor = Mathf.Max(0.5f, 50f / gameCtl.speed); 
    var spawnDistance = gameCtl.speed * baseSpawnDistanceMultiplier * speedFactor;
    var spawnPosition = new Vector3(playerCube.transform.position.x, playerCube.transform.position.y, 0)
                        + new Vector3(spawnDistance, 0f, 0)
                        + new Vector3(0f, 0f, laneZ);

            var obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            return obstacle;
        }

        private void SetNextSpawnTime()
        {
            var speedFactor = Mathf.Max(0.5f, 100f / gameCtl.speed);
            var adjustedMinInterval = minSpawnInterval * speedFactor;
            var adjustedMaxInterval = maxSpawnInterval * speedFactor;
            var interval = Random.Range(adjustedMinInterval, adjustedMaxInterval);
            _nextSpawnTime = Time.time + interval;
        }

        private void CleanUpObstacles()
        {
            for (var i = ActiveObstacles.Count - 1; i >= 0; i--)
            {
                if (!ActiveObstacles[i])
                {
                    ActiveObstacles.RemoveAt(i);
                    continue;
                }

                ActiveObstacles[i].transform.position = new Vector3(
                    ActiveObstacles[i].transform.position.x - gameCtl.speed * 0.8f * Time.deltaTime,
                    ActiveObstacles[i].transform.position.y, ActiveObstacles[i].transform.position.z);
                if (!(ActiveObstacles[i].transform.position.x < playerCube.transform.position.x - 1f)) continue;
                Destroy(ActiveObstacles[i]);
                ActiveObstacles.RemoveAt(i);
            }
        }

        private void CleanUpAllObstacles()
        {
            for (var i = ActiveObstacles.Count - 1; i >= 0; i--)
            {
                Destroy(ActiveObstacles[i]);
                ActiveObstacles.RemoveAt(i);
            }
        }
    }
}