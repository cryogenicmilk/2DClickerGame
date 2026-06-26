using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ToastProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Normal‚Ì“®‚«")]
    [SerializeField] private Vector2 _normalOffset = new Vector2(-0.7f, 1.0f);
    [SerializeField] private float _normalMoveDuration = 0.45f;
    [SerializeField] private float _normalFadeStart = 0.25f;
    // animation
    private Sequence _normalSequence;

    [Header("Crit‚Ì“®‚«")]
    [SerializeField] private float _critY = 1f;
    [SerializeField] private float _critX = 1f;

    [Header("Direct‚Ì“®‚«")]
    [SerializeField] private float _directY = 1f;
    [SerializeField] private float _directX = 1f;

    [Header("CritDirect‚Ì“®‚«")]
    [SerializeField] private float _critDirectY = 1f;
    [SerializeField] private float _critDirectX = 1f;
    //[SerializeField] private float _critDirectSpeed = 10f;


    private void Awake()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }


    public void ShootToast(DamageType damageType)
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        switch(damageType)
        {
            case DamageType.Normal:
                ShootNormal();
                break;
            case DamageType.Crit:
                ShootCrit();
                break;
            case DamageType.DirectHit:
                ShootDirectHit();
                break;
            case DamageType.CritDirect:
                ShootCritDirect();
                break;
        }
    }

    private void ShootNormal()
    {
        // Normal‚Í•¨—‚Å‚Í‚È‚­AÔŠÛ•ûŒü‚É‚ä‚Á‚­‚èˆÚ“®‚µ‚ÄÁ‚¦‚é
        _rb.simulated = false;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + (Vector3)_normalOffset;

        _normalSequence?.Kill();
        _normalSequence = DOTween.Sequence();

        _normalSequence
       .Append(transform.DOMove(endPos, _normalMoveDuration)
           .SetEase(Ease.InOutSine))
       .Join(_spriteRenderer.DOFade(0f, _normalMoveDuration - _normalFadeStart)
           .SetDelay(_normalFadeStart))
       .OnComplete(() =>
       {
           Destroy(gameObject);
       });
    }

    private void ShootCrit()
    {
        _rb.simulated = true;
        _rb.gravityScale = 1f;

        _rb.linearVelocity = new Vector2(_critX, _critY);
    }

    private void ShootDirectHit()
    {
        _rb.simulated = true;
        _rb.gravityScale = 1f;

        _rb.linearVelocity = new Vector2(_directX, _directY);
    }

    private void ShootCritDirect()
    {
        _rb.simulated = true;
        _rb.gravityScale = 0f;

        _rb.linearVelocity = new Vector2(_critDirectX, _critDirectY);
    }
}
