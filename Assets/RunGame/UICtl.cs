using System;
using TMPro;
using UnityEngine;

namespace RunGame
{
    public class UICtl : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI distanceText;
        [SerializeField] private TextMeshProUGUI lifeText;
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private GameObject infoPanel2;

        [SerializeField] private CubeController playerCube;
        [SerializeField] private GameCtl gameCtl;

        [Obsolete("Obsolete")]
        private void Update()
        {
            if (!playerCube) return;
            if (!gameCtl) return;

            if (!gameCtl.isPlaying)
            {
                infoText.text = "Press Space to Start";
            }
            else
            {
                infoText.text = "";
            }

            if (scoreText)
            {
                scoreText.text = $"Score: {gameCtl.score}";
            }

            if (speedText)
            {
                speedText.text = $"Speed: {gameCtl.speed:F1}";
            }

            if (distanceText)
            {
                distanceText.text = $"Dist: {gameCtl.distance:F1}m";
            }

            if (lifeText)
            {
                lifeText.text = $"Life: {gameCtl.life}";
            }

            if (gameCtl.isInvincible)
            {
                ShowDamagePanel(true);
            }
            else
            {
                ShowDamagePanel(false);
            }
        }

        private void ShowDamagePanel(bool show)
        {
            if (infoPanel)
            {
                infoPanel.SetActive(show);
            }

            if (infoPanel2)
            {
                infoPanel2.SetActive(show);
            }
        }
    }
}