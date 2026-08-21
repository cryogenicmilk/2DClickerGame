using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private float _baseDamage = 1f;
    [SerializeField] private float _baseDamageUpAmount = 0.1f;

    [Header("Critical")]
    [SerializeField] private float _critRate = 0.05f; // 初期5%とする
    [SerializeField] private float _maxCritRate = 0.50f; // 最大50%とする
    [SerializeField] private float _critMul = 2.0f;

    [Header("Direct Hit")]
    [SerializeField] private float _directHitRate = 0.25f; // 初期25%とする
    [SerializeField] private float _maxDirectHitRate = 0.75f; // 最大75%とする
    [SerializeField] private float _directHitMul = 1.25f;

    // 上限
    public bool IsCritRateMax => _critRate >= _maxCritRate;
    public bool IsDirectRateMax => _directHitRate >= _maxDirectHitRate;

    public DamageResult CalcDMG()
    {
        float damage = _baseDamage;

        // CriticalとDirect Hitは別々に抽選し、
        // 両方が同時に発生する結果も残している
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
        // 特殊結果が常時発生して特別感が薄れないよう、
        // Critical率には上限を設けている。
        _critRate = Mathf.Clamp(_critRate + addRate, _critRate, _maxCritRate);// minを初期値にして安全
    }

    public void AddDirect(float addRate)
    {
        // 通常・Critical・Direct Hit・複合結果の
        // 出現バランスを終盤まで残すため上限を設けている。
        _directHitRate = Mathf.Clamp(_directHitRate + addRate, _directHitRate, _maxDirectHitRate);// minを初期値にして安全
    }

    public float CritRate => _critRate;
    public float DirectRate => _directHitRate;
}
