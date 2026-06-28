using DG.Tweening;
using UnityEngine;

public class ToasterReaction : MonoBehaviour
{
    [Header("Reaction Target")]
    [SerializeField] private Transform _target;

    [Header("Scale")]
    [SerializeField] private Vector3 _squashScaleMul = new Vector3(1.15f, 0.85f, 1f);
    [SerializeField] private Vector3 _stretchScaleMul = new Vector3(0.85f, 1.15f, 1f);

    [Header("Time")]
    [SerializeField] private float _squashTime = 0.05f;
    [SerializeField] private float _stretchTime = 0.05f;
    [SerializeField] private float _returnTime = 0.05f;

    private Vector3 _defaultScale;
    private Sequence _reactionSequence;

    private void Awake()
    {
        if (_target == null)
        {
            _target = transform;
        }

        _defaultScale = _target.localScale;
    }

    private void OnDisable()
    {
        _reactionSequence?.Kill();
        _target.localScale = _defaultScale;
    }

    public void PlayReaction()
    {
        Vector3 squashScale = Vector3.Scale(_defaultScale, _squashScaleMul);
        Vector3 stretchScale = Vector3.Scale(_defaultScale, _stretchScaleMul);

        _reactionSequence?.Kill();
        _target.localScale = _defaultScale;

        _reactionSequence = DOTween.Sequence();

        _reactionSequence
            .Append(_target.DOScale(squashScale, _squashTime))
            .Append(_target.DOScale(stretchScale, _stretchTime))
            .Append(_target.DOScale(_defaultScale, _returnTime));
    }
}
