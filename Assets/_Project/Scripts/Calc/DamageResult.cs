public struct DamageResult
{
    public double Damage;
    public DamageType Type;

    public DamageResult(double damage, DamageType type)
    {
        Damage = damage;
        Type = type;
    }
}
