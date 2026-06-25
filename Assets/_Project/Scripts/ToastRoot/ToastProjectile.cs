using System.Collections;
using UnityEngine;

public class ToastProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Normal‚Ì“®‚«")]
    [SerializeField] private Vector2 _normalOffset = new Vector2(-0.7f, 1.0f);
    [SerializeField] private float _normalMoveDuration = 0.45f;
    [SerializeField] private float _normalFadeStart = 0.25f;

    [Header("Crit‚Ì“®‚«")]
    [SerializeField] private float _critY = 1f;
    [SerializeField] private float _critX = 1f;

    [Header("Direct‚Ì“®‚«")]
    [SerializeField] private float _directY = 1f;
    [SerializeField] private float _directX = 1f;

    [Header("CritDirect‚Ì“®‚«")]
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
        //StartCoroutine(NormalMovement());
        _rb.simulated = true;
        _rb.gravityScale = 1f;

        _rb.linearVelocity = new Vector2(_critX, _critY);

        //Destroy(gameObject, _destroyAfterSeconds);
    }

    //private IEnumerator NormalMovement()
    //{

    //}

    private void ShootCrit()
    {
    }

    private void ShootDirectHit()
    {
    }

    private void ShootCritDirect()
    {
    }
}
