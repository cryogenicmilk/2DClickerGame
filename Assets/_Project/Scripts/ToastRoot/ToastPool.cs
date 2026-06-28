using System.Collections.Generic;
using UnityEngine;

public class ToastPool : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private ToastProjectile _normalPrefab;
    [SerializeField] private ToastProjectile _critPrefab;
    [SerializeField] private ToastProjectile _directPrefab;
    [SerializeField] private ToastProjectile _critDirectPrefab;

    [SerializeField] private int _initialPoolSize = 10;

    private readonly Dictionary<DamageType, Queue<ToastProjectile>> _pools = new();

    private void Awake()
    {
        CreatePool(DamageType.Normal, _normalPrefab);
        CreatePool(DamageType.Crit, _critPrefab);
        CreatePool(DamageType.DirectHit, _directPrefab);
        CreatePool(DamageType.CritDirect, _critDirectPrefab);
    }

    private void CreatePool(DamageType type, ToastProjectile prefab)
    {
        Queue<ToastProjectile> pool = new Queue<ToastProjectile>();
        _pools.Add(type, pool);

        for (int i = 0; i < _initialPoolSize; i++)
        {
            ToastProjectile toast = CreateToast(type, prefab);
            ReturnToast(toast);
        }
    }

    private ToastProjectile CreateToast(DamageType type, ToastProjectile prefab)
    {
        ToastProjectile toast = Instantiate(prefab, transform);
        toast.SetPool(this);
        toast.SetDamageType(type);
        return toast;
    }

    public ToastProjectile GetToast(DamageType type, Vector3 position, Quaternion rotation)
    {
        Queue<ToastProjectile> pool = _pools[type];

        ToastProjectile toast;

        if (pool.Count > 0)
        {
            toast = pool.Dequeue();
        }
        else
        {
            toast = CreateToast(type, GetPrefab(type));
        }

        toast.transform.SetParent(null);
        toast.transform.SetPositionAndRotation(position, rotation);
        toast.gameObject.SetActive(true);

        return toast;
    }

    public void ReturnToast(ToastProjectile toast)
    {
        DamageType type = toast.DamageType;

        toast.gameObject.SetActive(false);
        toast.transform.SetParent(transform);

        _pools[type].Enqueue(toast);
    }

    private ToastProjectile GetPrefab(DamageType type)
    {
        switch (type)
        {
            case DamageType.Normal:
                return _normalPrefab;

            case DamageType.Crit:
                return _critPrefab;

            case DamageType.DirectHit:
                return _directPrefab;

            case DamageType.CritDirect:
                return _critDirectPrefab;

            default:
                return _normalPrefab;
        }
    }
}