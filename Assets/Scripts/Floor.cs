using DG.Tweening;
using UnityEngine;

public class Floor : MonoBehaviour
{
    void Start()
    {
        // TODO 種類ごとに挙動を設定出来るように
        var sequence = DOTween.Sequence();
        sequence
            .AppendInterval(3)
            .Append(transform.DOMoveY(3.5f, 3))
            .AppendInterval(3)
            .Append(transform.DOMoveY(0f, 3))
            .SetLoops(-1);
    }
}
