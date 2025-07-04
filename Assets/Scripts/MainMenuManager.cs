using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject SelectScreen;
    [SerializeField] private GameObject character;

    private Transform characterPos;

    private void Awake()
    {
        characterPos = character.transform;
    }

    public void PlayButton()
    {
        mainMenu.SetActive(false);
        SelectScreen.SetActive(true);
        character.transform.DOMove(new Vector3(519, -234, -97), 1f);
    }

    public void GoBackButton()
    {
        characterPos.position = new Vector3(-1102, 93, 860);
        SelectScreen.SetActive(false);
        mainMenu.SetActive(true);
        character.transform.DOMove(new Vector3(-215, -86, 336), 1f);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void GoToLevelZero()
    {
        SceneManager.LoadScene("LevelZero");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
