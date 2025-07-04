using UnityEngine;
using TMPro;

public class KeyManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Text on UI for the key counter.")] private TextMeshProUGUI keysText;
    [SerializeField] [Tooltip("Reference to the player in order to deactivate it.")] private GameObject playerController;
    [SerializeField] [Tooltip("Reference to the player in order to activate it.")] private GameObject endScreen;
    [SerializeField] [Tooltip("Reference to the GameplayUI in order to deactivate it.")] private GameObject gameplayUI;

    private int keyAmount;

    private void Awake()
    {
        keyAmount = 0;
    }

    private void Update()
    {
        keysText.text = "Keys: " + keyAmount.ToString();

        if (keyAmount == 3)
        {
            EndGame();
        }
    }

    public void AddKeyToManager()
    {
        keyAmount++;
    }

    private void EndGame()
    {
        playerController.SetActive(false);
        gameplayUI.SetActive(false);
        endScreen.SetActive(true);

    }
}
