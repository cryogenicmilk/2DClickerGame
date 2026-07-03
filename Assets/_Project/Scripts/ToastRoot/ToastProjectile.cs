using DG.Tweening;
using UnityEngine;

public class ToastProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Normalの動き")]
    [SerializeField] private Vector2 _normalOffset = new Vector2(-0.7f, 1.0f);
    [SerializeField] private float _normalMoveDuration = 0.45f;
    [SerializeField] private float _normalFadeStart = 0.25f;

    [Header("Critの動き")]
    [SerializeField] private float _critY = 1f;
    [SerializeField] private float _critX = 1f;
    [SerializeField] private float _critLifeTime = 1f;
    [SerializeField] private float _critFadeDuration = 1f;

    [Header("Directの動き")]
    [SerializeField] private float _directY = 1f;
    [SerializeField] private float _directX = 1f;
    [SerializeField] private float _directLifeTime = 3f;
    [SerializeField] private float _directFadeDuration = 2f;

    [Header("CritDirectの動き")]
    [SerializeField] private float _critDirectY = 1f;
    [SerializeField] private float _critDirectX = 1f;
    [SerializeField] private float _critDirectLifeTime = 1f;

    // animation
    private Sequence _sequence;
    private ToastPool _pool;

    public DamageType DamageType { get; private set; }

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

        _sequence?.Kill();

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
        AudioPlayer.Instance.PlaySE(1);

        // Normalは物理ではなく、赤丸方向にゆっくり移動して消える
        _rb.simulated = false;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + (Vector3)_normalOffset;

        _sequence?.Kill();
        _sequence = DOTween.Sequence();

        _sequence
       .Append(transform.DOMove(endPos, _normalMoveDuration)
           .SetEase(Ease.InOutSine))
       .Join(_spriteRenderer.DOFade(0f, _normalMoveDuration - _normalFadeStart)
           .SetDelay(_normalFadeStart))
       .OnComplete(ReturnToPool);
    }

    public void ShootCrit()
    {
        AudioPlayer.Instance.PlaySE(2);

        _rb.simulated = true;
        _rb.gravityScale = 1f;

        _rb.linearVelocity = new Vector2(_critX, _critY);

        _sequence?.Kill();
        _sequence = DOTween.Sequence();

        _sequence
            .AppendInterval(_critLifeTime)
            .Append(_spriteRenderer.DOFade(0f, _critLifeTime - _critFadeDuration))
            .OnComplete(ReturnToPool);
    }

    public void ShootDirect()
    {
        AudioPlayer.Instance.PlaySE(3);

        _rb.simulated = true;
        _rb.gravityScale = 1f;

        _rb.linearVelocity = new Vector2(_directX, _directY);

        _sequence?.Kill();
        _sequence = DOTween.Sequence();

        _sequence
            .AppendInterval(_directLifeTime)
            .Append(_spriteRenderer.DOFade(0f, _directLifeTime - _directFadeDuration))
            .OnComplete(ReturnToPool);
    }

    public void ShootCritDirect()
    {
        AudioPlayer.Instance.PlaySE(4);

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

    public void SetDamageType(DamageType damageType)
    {
        DamageType = damageType;
    }

    private void ReturnToPool()
    {
        _sequence?.Kill();
        _sequence = null;

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.simulated = false;

        _pool.ReturnToast(this);
    }
}
