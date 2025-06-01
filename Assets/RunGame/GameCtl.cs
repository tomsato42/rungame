using UnityEngine;
using UnityEngine.Serialization;

namespace RunGame
{
    public class GameCtl : MonoBehaviour
    {
        private const float MaxSpeed = 1000f;
        private const float MinSpeed = 10f;
        private const float InvincibleDuration = 2f;
        
        public bool isPlaying;
        public int score;
        public float speed;
        public float distance;
        public float startTime;
        public float remainTime;
        public int life;

        private float _lastDamagedTime = -InvincibleDuration;
        [FormerlySerializedAs("IsInvincible")] public bool isInvincible;

        private void Update()
        {
            switch (isPlaying)
            {
                case false when Input.GetKeyDown(KeyCode.Space):
                    isPlaying = true;
                    life = 3;
                    speed = 5f;
                    distance = 0;
                    score = 0;
                    startTime = Time.time;
                    _lastDamagedTime = -InvincibleDuration;
                    
                    break;
                case true when life == 0:
                    isPlaying = false;
                    speed = 0;
                    break;
            }
            isInvincible =  Time.time - _lastDamagedTime < InvincibleDuration;
            if (!isPlaying) return;
            UpdateGameProgress();
        }

        private void UpdateGameProgress()
        {
            distance +=  speed * Time.deltaTime;
            if (distance > score)
            {
                score = Mathf.FloorToInt(distance);
            }
            speed = (float)(speed + 2.5 * Time.deltaTime);
            if (speed > MaxSpeed) speed = MaxSpeed;
            remainTime = startTime + 60 - Time.time;
        }

        public void OnDamaged()
        {
            isInvincible =  Time.time - _lastDamagedTime < InvincibleDuration;
            if (isInvincible) return;
            speed = Mathf.Max(MinSpeed ,speed * 0.5f);
            _lastDamagedTime = Time.time;
            life--;
        }

    }
}
