using UnityEngine;

namespace RunGame
{
    public class MoveForward : MonoBehaviour
    {
        [SerializeField] private GameCtl gameCtl;

        private void Update()
        {
            if (!gameCtl) return;
            if (!gameCtl.isPlaying) return;
            transform.Translate(Vector3.right * (gameCtl.speed * Time.deltaTime), Space.World);
        }
    }
}
