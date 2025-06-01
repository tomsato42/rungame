using UnityEngine;

namespace RunGame
{
    public class CubeController : MonoBehaviour
    {
        [SerializeField] private GameCtl gameCtl;
        private MeshRenderer _meshRenderer;
        private Color _originalColor;

        private const int LaneRight = 2;
        private const int LaneLeft = 0;

        private const float LaneDistance = 3f;
        private static readonly float[] LanePositions = { LaneDistance, 0f, -LaneDistance };

        public int currentLane = 1;

        private bool _isMoving;
        private const float MoveSpeed = 15f;
        private float _targetPos;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _originalColor = _meshRenderer.material.color;
        }

        private void Update()
        {
            if (_isMoving)
            {
                var pos = transform.position;
                pos.z = Mathf.Lerp(pos.z, _targetPos, Time.deltaTime * MoveSpeed);
                pos.z = Mathf.Clamp(pos.z, -3f, 3f);
                transform.position = pos;

                if (Mathf.Abs(pos.z - _targetPos) < 0.05f)
                {
                    var exactLanePosition = LanePositions[currentLane];
                    transform.position = new Vector3(pos.x, pos.y, exactLanePosition);
                    _isMoving = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.A) && LaneLeft < currentLane)
            {
                currentLane--;
                var pos = transform.position;
                _targetPos = pos.z + 3;
                _isMoving = true;
            }

            if (Input.GetKeyDown(KeyCode.D) && currentLane < LaneRight)
            {
                currentLane++;
                var pos = transform.position;
                _targetPos = pos.z - 3;
                _isMoving = true;
            }

            if (gameCtl.isInvincible)
            {
                var alpha = Mathf.PingPong(Time.time * 5f, 1f);
                _meshRenderer.material.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
            }
            else
            {
                _meshRenderer.material.color = _originalColor;
            }
        }
    }
}