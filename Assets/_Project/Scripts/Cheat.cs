using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    [SerializeField] private Button _normal;
    [SerializeField] private Button _crit;
    [SerializeField] private Button _direct;
    [SerializeField] private Button _critDirect;

    //[SerializeField] private Button _spawn = null;

    [SerializeField] private ToastProjectile _toastProjectilePrefab;
    [SerializeField] private Transform _toastSpawnPoint;

    private void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _normal.onClick.AddListener(() => SpawnToast(DamageType.Normal));
        _crit.onClick.AddListener(() => SpawnToast(DamageType.Crit));
        _direct.onClick.AddListener(() => SpawnToast(DamageType.DirectHit));
        _critDirect.onClick.AddListener(() => SpawnToast(DamageType.CritDirect));
    }

    private void SpawnToast(DamageType damageType)
    {
        ToastProjectile toast = Instantiate(
            _toastProjectilePrefab,
            _toastSpawnPoint.position,
            Quaternion.identity
        );

        toast.ShootToast(damageType);
    }
}