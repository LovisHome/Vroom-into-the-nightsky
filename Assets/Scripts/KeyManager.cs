using UnityEngine;
using TMPro;

public class KeyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keysText;
    [SerializeField] private GameObject playerController;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject gameplayUI;

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
