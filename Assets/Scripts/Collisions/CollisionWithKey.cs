using UnityEngine;

public class CollisionWithKey : MonoBehaviour
{
    [SerializeField] private KeyManager _keyManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.PointsForScore(100);
            _keyManager.AddKeyToManager();
            gameObject.SetActive(false);
        }
    }
}
