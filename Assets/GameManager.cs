using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject gameOverMenu; 

      public GameObject winMenu;

    void Start()
    {
        Time.timeScale = 0f;
        startMenu.SetActive(true);

        if (gameOverMenu != null)
            gameOverMenu.SetActive(false);
    }

    public void StartGame()
    {
        FindObjectOfType<AmbientAudio>().gameObject.SetActive(true);
        startMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");
        FindObjectOfType<AmbientAudio>().FadeOut();
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void WinGame()
{
    Debug.Log("YOU WIN");

    Time.timeScale = 0f;

    if (winMenu != null)
        winMenu.SetActive(true);
}
}