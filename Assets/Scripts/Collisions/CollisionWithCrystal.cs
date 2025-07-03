using UnityEngine;

public class CollisionWithCrystal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.PointsForScore(700);
            gameObject.SetActive(false);
        }
    }
}
