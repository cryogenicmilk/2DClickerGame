using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager = null;
    [SerializeField] private UIManager _uiManager = null;

    [Header("Toast Test")]
    [SerializeField] private ToastPool _toastPool;
    [SerializeField] private Transform _toastSpawnPoint;

    [Header("Damage Type Buttons")]
    [SerializeField] private Button _normal;
    [SerializeField] private Button _crit;
    [SerializeField] private Button _direct;
    [SerializeField] private Button _critDirect;

    [Header("Score Cheat Buttons")]
    [SerializeField] private Button _addScore1000;
    [SerializeField] private Button _addScore1000000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _normal.onClick.AddListener(() => SpawnToast(DamageType.Normal));
        _crit.onClick.AddListener(() => SpawnToast(DamageType.Crit));
        _direct.onClick.AddListener(() => SpawnToast(DamageType.DirectHit));
        _critDirect.onClick.AddListener(() => SpawnToast(DamageType.CritDirect));

        _addScore1000.onClick.AddListener(AddScore1000);
        _addScore1000000.onClick.AddListener(AddScore1000000);
    }

    private void SpawnToast(DamageType damageType)
    {
        ToastProjectile toast = _toastPool.GetToast(
            _toastSpawnPoint.position,
            Quaternion.identity
        );

        toast.ShootToast(damageType);
    }

    private void AddScore1000()
    {
        _scoreManager.AddScore(1000);
        _uiManager.UpdateUI();
    }

    private void AddScore1000000()
    {
        _scoreManager.AddScore(1000000);
        _uiManager.UpdateUI();
    }
}