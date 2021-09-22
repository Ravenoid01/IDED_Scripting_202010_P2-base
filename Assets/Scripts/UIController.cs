using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Player playerRef;

    [SerializeField]
    private Image[] lifeImages;

    [SerializeField]
    private Text scoreLabel;

    [SerializeField]
    private Button restartBtn;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        ToggleRestartButton(false);

        playerRef = FindObjectOfType<Player>();
    }

    private void ToggleRestartButton(bool val)
    {
        if (restartBtn != null)
        {
            restartBtn.gameObject.SetActive(val);
        }
    }

    public void Subscribe1()
    {
        playerRef.OnPlayerHit += HealthBar;
    }

    public void Subscribe2()
    {
        if (scoreLabel != null)
        {
            playerRef.OnPlayerScoreChanged += ChangeScore;
        }
    }

    public void Subscribe3()
    {
        if (playerRef.Lives <= 0)
        {
            playerRef.OnPlayerDied += Gameover;
        }
    }

    private void Gameover()
    {
        if (scoreLabel != null)
        {
            scoreLabel.text = "Game Over";
        }

        ToggleRestartButton(true);
    }

    private void ChangeScore()
    {
        scoreLabel.text = playerRef.Score.ToString();
    }

    private void HealthBar()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (lifeImages[i] != null && lifeImages[i].enabled)
            {
                lifeImages[i].gameObject.SetActive(playerRef.Lives >= i + 1);
            }
        }
    }
}