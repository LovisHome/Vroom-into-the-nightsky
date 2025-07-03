using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] [Tooltip("Reference to the TextMesh for the Timer.")] private TextMeshProUGUI timerText;
    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime % 1f) * 1000f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
