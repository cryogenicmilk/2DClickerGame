using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private float _baseDamage = 1f;
    [SerializeField] private float _baseDamageUpAmount = 0.1f;

    [Header("Critical")]
    [SerializeField] private float _critRate = 0.05f; // 初期5%
    [SerializeField] private float _maxCritRate = 0.50f; // 最大50%
    [SerializeField] private float _critMul = 2.0f;

    [Header("Direct Hit")]
    [SerializeField] private float _directHitRate = 0.25f; // 初期25%
    [SerializeField] private float _maxDirectHitRate = 0.75f; // 最大75%
    [SerializeField] private float _directHitMul = 1.25f;

    // 上限
    public bool IsCritRateMax => _critRate >= _maxCritRate;
    public bool IsDirectRateMax => _directHitRate >= _maxDirectHitRate;

    public DamageResult CalcDMG()
    {
        // 676767676767676767
        float damage = _baseDamage;

        bool isCrit = Random.value < _critRate;
        bool isDirectHit = Random.value < _directHitRate;

        if (isCrit)
        {
            damage *= _critMul;
        }

        if (isDirectHit)
        {
            damage *= _directHitMul;
        }

        DamageType damageType = GetDamageType(isCrit, isDirectHit);

        return new DamageResult(damage, damageType);
    }

    private DamageType GetDamageType(bool isCrit, bool isDirectHit)
    {
        if(isCrit && isDirectHit)
        {
            return DamageType.CritDirect;
        }

        if(isCrit)
        {
            return DamageType.Crit;
        }

        if( isDirectHit)
        {
            return DamageType.DirectHit;
        }

        return DamageType.Normal;
    }

    /// <summary>
    /// 強化で上昇するステータス
    /// </summary>
    public void AddBase()
    {
        _baseDamage += _baseDamageUpAmount;
    }

    public void AddCrit(float addRate)
    {
        _critRate = Mathf.Clamp(_critRate + addRate, _critRate, _maxCritRate);// minを初期値にして安全
    }

    public void AddDirect(float addRate)
    {
        _directHitRate = Mathf.Clamp(_directHitRate + addRate, _directHitRate, _maxDirectHitRate);// minを初期値にして安全
    }

    public float CritRate => _critRate;
    public float DirectRate => _directHitRate;
}
