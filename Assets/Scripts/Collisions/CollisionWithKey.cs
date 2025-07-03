using UnityEngine;

public class CollisionWithKey : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.PointsForScore(100);
            //Adds Key (1 out of 3)
            gameObject.SetActive(false);
        }
    }
}
