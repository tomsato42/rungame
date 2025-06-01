using UnityEngine;

namespace RunGame
{
    public class CollisionManager : MonoBehaviour
    {
        private static readonly int ObstacleCount = Shader.PropertyToID("obstacle_count");
        private static readonly int PlayerRadius = Shader.PropertyToID("player_radius");
        private static readonly int PlayerPosition = Shader.PropertyToID("player_position");
        private static readonly int CollisionCount = Shader.PropertyToID("collision_count");
        private static readonly int Obstacles = Shader.PropertyToID("obstacles");
        
        [SerializeField] private ComputeShader collisionShader;
        [SerializeField] private ObstacleManager obstacleManager;
        [SerializeField] private CubeController playerCube;
        [SerializeField] private GameCtl gameCtl;

        private ComputeBuffer _obstacleBuffer;
        private ComputeBuffer _resultBuffer;
        private readonly int[] _collisionResult = { 0 };
        private int _currentBufferSize;

        private struct Obstacle
        {
            public Vector3 Position;
            public float Radius;
        }

        private void OnDestroy()
        {
            ReleaseBuffers();
        }

        private void Update()
        {
            if (!gameCtl.isPlaying) return;
            DetectCollisions();
        }

        private void DetectCollisions()
        {
            var obstacles = obstacleManager.ActiveObstacles;
            if (obstacles.Count == 0) return;

            if (_obstacleBuffer == null || _currentBufferSize < obstacles.Count)
            {
                ReleaseBuffers();
                _currentBufferSize = obstacles.Count + 10;
                _obstacleBuffer = new ComputeBuffer(_currentBufferSize, sizeof(float) * 4);
                _resultBuffer = new ComputeBuffer(1, sizeof(int));
            }

            var obstacleData = new Obstacle[obstacles.Count];
            for (var i = 0; i < obstacles.Count; i++)
            {
                obstacleData[i] = new Obstacle
                {
                    Position = obstacles[i].transform.position,
                    Radius = 1.0f
                };
            }

            _obstacleBuffer.SetData(obstacleData);
            _resultBuffer.SetData(new[] { 0 });

            var kernelIndex = collisionShader.FindKernel("cs_collision_detection");
            collisionShader.SetBuffer(kernelIndex, Obstacles, _obstacleBuffer);
            collisionShader.SetBuffer(kernelIndex, CollisionCount, _resultBuffer);
            collisionShader.SetVector(PlayerPosition, playerCube.transform.position);
            collisionShader.SetFloat(PlayerRadius, 0.5f);
            collisionShader.SetInt(ObstacleCount, obstacles.Count);

            var threadGroups = Mathf.CeilToInt(obstacles.Count / 64f);
            collisionShader.Dispatch(kernelIndex, threadGroups, 1, 1);

            _resultBuffer.GetData(_collisionResult);
            if (_collisionResult[0] == 1)
            {
                gameCtl.OnDamaged();
            }
        }

        private void ReleaseBuffers()
        {
            if (_obstacleBuffer != null)
            {
                _obstacleBuffer.Release();
                _obstacleBuffer = null;
            }

            if (_resultBuffer != null)
            {
                _resultBuffer.Release();
                _resultBuffer = null;
            }
            
            _currentBufferSize = 0;
        }
    }
}