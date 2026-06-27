using UnityEngine;
using DG.Tweening;

public class ToastProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("")]
    [SerializeField] private float _lifeTime = 2f;

    [Header("Normalの動き")]
    [SerializeField] private Vector2 _normalOffset = new Vector2(-0.7f, 1.0f);
    [SerializeField] private float _normalMoveDuration = 0.45f;
    [SerializeField] private float _normalFadeStart = 0.25f;

    [Header("Critの動き")]
    [SerializeField] private float _critY = 1f;
    [SerializeField] private float _critX = 1f;
    [SerializeField] private float _critLifeTime = 1f;
    [SerializeField] private float _critFadeStart = 1f;

    [Header("Directの動き")]
    [SerializeField] private float _directY = 1f;
    [SerializeField] private float _directX = 1f;
    [SerializeField] private float _directLifeTime = 3f;
    [SerializeField] private float _directFadeStart = 2f;

    [Header("CritDirectの動き")]
    [SerializeField] private float _critDirectY = 1f;
    [SerializeField] private float _critDirectX = 1f;
    [SerializeField] private float _critDirectLifeTime = 1f;

    // animation
    private Sequence _Sequence;

    private ToastPool _pool;

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

    //====================================================================
    //入口
    //====================================================================
    public void ShootToast(DamageType damageType)
    {

        ResetToastState();

        switch(damageType)
        {
            case DamageType.Normal:
                ShootNormal();
                break;
            case DamageType.Crit:
                ShootCrit();
                break;
            case DamageType.DirectHit:
                ShootDirect();
                break;
            case DamageType.CritDirect:
                ShootCritDirect();
                break;
        }
    }

    // オブジェクトプールで再利用するためトースト出す前にリセット
    private void ResetToastState()
    {
        CancelInvoke();

        _Sequence?.Kill();

        transform.DOKill();

        _spriteRenderer.color = new Color(
            _spriteRenderer.color.r,
            _spriteRenderer.color.g,
            _spriteRenderer.color.b,
            1f
        );

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
    }

    //====================================================================
    //各動き
    //====================================================================
    public void ShootNormal()
    {
        // Normalは物理ではなく、赤丸方向にゆっくり移動して消える
        _rb.simulated = false;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + (Vector3)_normalOffset;

        _Sequence?.Kill();
        _Sequence = DOTween.Sequence();

        _Sequence
       .Append(transform.DOMove(endPos, _normalMoveDuration)
           .SetEase(Ease.InOutSine))
       .Join(_spriteRenderer.DOFade(0f, _normalMoveDuration - _normalFadeStart)
           .SetDelay(_normalFadeStart))
       .OnComplete(ReturnToPool);
    }

    public void ShootCrit()
    {
        _rb.simulated = true;
        _rb.gravityScale = 1f;

        _rb.linearVelocity = new Vector2(_critX, _critY);

        _Sequence?.Kill();
        _Sequence = DOTween.Sequence();

        _Sequence
            .AppendInterval(_critLifeTime)
            .Append(_spriteRenderer.DOFade(0f, _critLifeTime - _critFadeStart))
            .OnComplete(ReturnToPool);
    }

    public void ShootDirect()
    {
        _rb.simulated = true;
        _rb.gravityScale = 1f;

        _rb.linearVelocity = new Vector2(_directX, _directY);

        _Sequence?.Kill();
        _Sequence = DOTween.Sequence();

        _Sequence
            .AppendInterval(_directLifeTime)
            .Append(_spriteRenderer.DOFade(0f, _directLifeTime - _directFadeStart))
            .OnComplete(ReturnToPool);
    }

    public void ShootCritDirect()
    {
        _rb.simulated = true;
        _rb.gravityScale = 0f;

        _rb.linearVelocity = new Vector2(_critDirectX, _critDirectY);

        // 消えるときは画面外だからなくてもいいかする
      //  _normalSequence
      //.Join(_spriteRenderer.DOFade(0f, _normalMoveDuration - _normalFadeStart)
      //    .SetDelay(_normalFadeStart));

        Invoke(nameof(ReturnToPool), _critDirectLifeTime);
    }

    //====================================================================
    //オブジェクトプール
    //====================================================================
    public void SetPool(ToastPool pool)
    {
        _pool = pool;
    }

    private void ReturnToPool()
    {
        _Sequence?.Kill();
        _Sequence = null;

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.simulated = false;

        _pool.ReturnToast(this);
    }
}
