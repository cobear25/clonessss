using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject playerPrefab;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; 
        Invoke("StartGame", 1);
    }

    void StartGame() {
        var player = Instantiate(playerPrefab).GetComponent<SnakeController>();
        player.gameController = this;
        player.scoreText = scoreText;
    }

    public void GameOver() {
        gameOverPanel.SetActive(true);
    }

    public void PlayAgain() {
        SceneManager.LoadScene("SampleScene");
    }
}
