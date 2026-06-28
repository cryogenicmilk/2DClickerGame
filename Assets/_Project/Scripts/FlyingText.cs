using TMPro;
using UnityEngine;
using DG.Tweening;

public class FlyingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private RectTransform _rectTransform;

    [Header("Move")]
    [SerializeField] private float _moveY = 80f;
    [SerializeField] private float _duration = 0.6f;

    [Header("Scale")]
    [SerializeField] private float _normalScale = 1f;
    [SerializeField] private float _specialScale = 1.25f;

    [Header("CritDirect")]
    [SerializeField] private float _critDirectScale = 1.5f;
    [SerializeField] private Vector2 _critDirectOffset = new Vector2(60f, 0f);
    [SerializeField] private float _critDirectPopScale = 1.8f;
    [SerializeField] private float _critDirectBaseScale = 1.4f;
    [SerializeField] private float _critDirectPopTime = 0.08f;
    [SerializeField] private float _critDirectReturnTime = 0.12f;
    [SerializeField] private float _critDirectStayTime = 0.25f;
    [SerializeField] private float _critDirectFadeTime = 0.25f;

    private Sequence _sequence;

    public void Play(double score, DamageType type, Vector2 anchoredPosition)
    {
        if (type == DamageType.CritDirect)
        {
            PlayCritDirect(score, type, anchoredPosition);
            return;
        }

        PlayNormal(score, type, anchoredPosition);
    }

    private void PlayNormal(double score, DamageType type, Vector2 anchoredPosition)
    {
        ResetState(anchoredPosition);

        _text.text = GetText(score, type);
        _text.color = GetColor(type);
        _rectTransform.localScale = Vector3.one * GetScale(type);

        _sequence = DOTween.Sequence();

        _sequence
            .Append(_rectTransform.DOAnchorPosY(anchoredPosition.y + _moveY, _duration)
                .SetEase(Ease.OutCubic))
            .Join(_text.DOFade(0f, _duration))
            .OnComplete(() => Destroy(gameObject));
    }

    private void PlayCritDirect(double score, DamageType type, Vector2 anchoredPosition)
    {
        Vector2 spawnPosition = anchoredPosition + _critDirectOffset;

        ResetState(spawnPosition);

        _text.text = GetText(score, type);
        _text.color = GetColor(type);

        float baseScale = _critDirectBaseScale;
        float popScale = _critDirectPopScale;

        _rectTransform.localScale = Vector3.one * baseScale;

        _sequence = DOTween.Sequence();

        _sequence
            .Append(_rectTransform.DOScale(popScale, _critDirectPopTime)
                .SetEase(Ease.OutBack))
            .Append(_rectTransform.DOScale(baseScale, _critDirectReturnTime)
                .SetEase(Ease.OutCubic))
            .AppendInterval(_critDirectStayTime)
            .Append(_text.DOFade(0f, _critDirectFadeTime))
            .OnComplete(() => Destroy(gameObject));
    }

    //====================================================================
    //
    //====================================================================
    private void ResetState(Vector2 anchoredPosition)
    {
        _sequence?.Kill();

        _rectTransform.anchoredPosition = anchoredPosition;

        Color color = _text.color;
        color.a = 1f;
        _text.color = color;
    }

    private string GetText(double score, DamageType type)
    {
        string scoreText = FormatNumber(score);

        switch (type)
        {
            case DamageType.Crit:
                return $"+{scoreText}!";

            case DamageType.DirectHit:
                return $"+{scoreText}";

            case DamageType.CritDirect:
                return $"+{scoreText}!!";

            default:
                return $"+{scoreText}";
        }
    }

    private Color GetColor(DamageType type)
    {
        switch (type)
        {
            case DamageType.Crit:
                return new Color(1f, 0.45f, 0f); // オレンジ

            case DamageType.DirectHit:
                return Color.yellow;

            case DamageType.CritDirect:
                return Color.crimson; // 仮。後で虹色演出にしてもOK

            default:
                return Color.white;
        }
    }

    private float GetScale(DamageType type)
    {
        switch (type)
        {
            case DamageType.Crit:
            case DamageType.DirectHit:
                return _specialScale;

            case DamageType.CritDirect:
                return _critDirectScale;

            default:
                return _normalScale;
        }
    }

    private string FormatNumber(double value)
    {
        if (value < 1_000_000)
        {
            return value.ToString("0");
        }

        return value.ToString("0.##e+0");
    }
}
