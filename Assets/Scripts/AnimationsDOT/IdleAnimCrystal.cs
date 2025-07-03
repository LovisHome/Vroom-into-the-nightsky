using DG.Tweening;
using UnityEngine;

public class IdleAnimCrystal : MonoBehaviour
{
    [SerializeField][Tooltip("Reference of the child of this parent.")] private Transform model;
    private float endPosition;

    private void Awake()
    {
        endPosition = transform.position.y + 2f;

        if (transform.childCount > 0)
        {
            model = transform.GetChild(0);
        }
        else
        {
            Debug.LogError("No Child Model found for the Animation.");
        }
    }

    private void Start()
    {
        if (model == null) return;

        // Model moving up and down
        model.DOLocalMoveY(endPosition, 1f).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        // Model rotation
        model.DOLocalRotate(new Vector3(0f, 360f, 0f), 5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
