using UnityEngine;

namespace RunGame
{
    public class TextureController : MonoBehaviour
    {
        private static readonly int Speed = Shader.PropertyToID("_speed");
        [SerializeField] private GameCtl gameCtl;
        private float _speed;
        private Material _material;

        private void Start()
        {
            if (!gameCtl) return;
            _speed = gameCtl.speed;
            _material = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            _speed = gameCtl.speed;
            if (!_material) return;
            _material.SetFloat(Speed, _speed);
        }
    }
}