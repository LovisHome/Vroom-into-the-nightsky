using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private Timer timer;

    public void PauseGame()
    {
        gameplayUI.SetActive(false);
        pauseScreen.SetActive(true);
        playerController.enabled = false;
        playerRB.isKinematic = true;
        timer.PauseTimer();
    }

    private void Update()
    {
        // ESC Screen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DispauseGame();
        }
    }

    public void DispauseGame()
    {
        gameplayUI?.SetActive(true);
        pauseScreen?.SetActive(false);
        playerController.enabled = true;
        playerRB.isKinematic = false;
        timer.ResumeTimer();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
