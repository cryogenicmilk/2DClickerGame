using System.Collections.Generic;
using UnityEngine;

public class ToastPool : MonoBehaviour
{
    [SerializeField] private ToastProjectile _toastPrefab;
    [SerializeField] private int _initialPoolSize = 20;

    private readonly Queue<ToastProjectile> _pool = new Queue<ToastProjectile>();

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for(int i = 0; i < _initialPoolSize; i++)
        {
            ToastProjectile toast = CreateToast();
            ReturnToast(toast);
        }
    }

    private ToastProjectile CreateToast()
    {
        ToastProjectile toast = Instantiate(_toastPrefab, transform);
        toast.SetPool(this);
        return toast;
    }

    public ToastProjectile GetToast(Vector3 position, Quaternion rotation)
    {
        ToastProjectile toast;

        if (_pool.Count > 0)
        {
            toast = _pool.Dequeue();
        }
        else
        {
            toast = CreateToast();
        }

        toast.transform.SetPositionAndRotation(position, rotation);
        toast.gameObject.SetActive(true);

        return toast;
    }

    public void ReturnToast(ToastProjectile toast)
    {
        toast.gameObject.SetActive(false);
        toast.transform.SetParent(transform);

        _pool.Enqueue(toast);
    }
}
