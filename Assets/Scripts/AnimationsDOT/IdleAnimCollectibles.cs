using UnityEngine;
using DG.Tweening;

public class IdleAnimCollectibles : MonoBehaviour
{
    [SerializeField] [Tooltip("Reference of the child of this parent.")] private Transform model;
    private float endPosition;

    private void Awake()
    {
        endPosition = transform.position.y + 1f;

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
        model.DOLocalMoveX(endPosition, 1f).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        // Model rotating on the X axis (Model came with wrong pivot)
        DOTween.To(() => 0f, angle => {
            model.localRotation = Quaternion.Euler(angle, 0f, 0f);
        }, 360f, 2f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
