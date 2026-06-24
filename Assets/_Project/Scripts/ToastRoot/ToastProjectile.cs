using System.Collections;
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

    [Header("Directの動き")]
    [SerializeField] private float _directY = 1f;
    [SerializeField] private float _directX = 1f;

    [Header("CritDirectの動き")]
    [SerializeField] private float _critDirectSpeed = 10f;
    [SerializeField] private Vector2 _CritDirectMovement = new Vector2(1f, 1f);


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
        StartCoroutine(NormalMovement());
    }

    private IEnumerator NormalMovement()
    {

    }

    private void ShootCrit()
    {
        // 上に出てから落下する
        _rb.gravityScale = _critGravity;
        _rb.linearVelocity = Vector2.up * 2;

        Destroy(gameObject, _toastLifeTime);
    }

    private void ShootDirectHit()
    {
        // 上方向ランダム。下方向は含めない
        _rb.gravityScale = _directGravity;

        float randomX = Random.Range(-0.8f, 0.8f);
        float randomY = Random.Range(0.4f, 1.0f);

        Vector2 direction = new Vector2(randomX, randomY).normalized;
        _rb.linearVelocity = direction * 2;

        Destroy(gameObject, _toastLifeTime);
    }

    private void ShootCritDirect()
    {
        // 画面外までぶっ飛ぶ
        _rb.gravityScale = _noGravity;

        //float randomX = Random.Range(-0.3f, 0.3f);
        Vector2 direction = new Vector2(1f, 1f).normalized;

        _rb.linearVelocity = direction * 2;

        Destroy(gameObject, _toastLifeTime);
    }
}
