using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] [Tooltip("Reference to the TextMesh for the Score.")] private TextMeshProUGUI scoreText;

    private int score;

    private void Awake()
    {
        if (instance == null) instance = this;
        score = 0;
    }

    private void Update()
    {
        scoreText.text = string.Format("Score: {0:00000}", score);
    }

    public void PointsForScore(int _pointsReceived)
    {
        score += _pointsReceived;
    }

    public void RestartScore()
    {
        score = 0;
    }
}
