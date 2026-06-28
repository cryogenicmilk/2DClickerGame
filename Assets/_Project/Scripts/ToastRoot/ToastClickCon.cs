using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToastClickCon : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] ScoreManager _scoreManager;
    [SerializeField] UIManager _uiManager;
    [SerializeField] DamageCalculator _damageCalculator;

    [Header("ToastProjectile")]
    [SerializeField] private ToastPool _toastPool;
    [SerializeField] Transform _toastSpawnPoint;

    [Header("Flying Text")]
    [SerializeField] private FlyingTextSpawner _flyingTextSpawner;

    [Header("Click Reaction")]
    [SerializeField] private ToasterReaction _toasterReaction;

    private Coroutine _clickReactionCoroutine;

    void Start()
    {
        _button.onClick.AddListener(OnClickToastButton);
    }

    // 全知全能すべての親
    private void OnClickToastButton()
    {
        DamageResult result = _damageCalculator.CalcDMG();

        _scoreManager.AddScore(result.Damage);
        _uiManager.UpdateUI();

        // フライングテキスト
        _flyingTextSpawner.Spawn(result);

        // reaction
        if (_clickReactionCoroutine != null)
        {
            StopCoroutine(_clickReactionCoroutine);
        }

        _toasterReaction.PlayReaction();

        SpawnToast(result.Type);
    }

    #region クリック時に飛ばすパン・トースター

    private void SpawnToast(DamageType damageType)
    {
        ToastProjectile toast = _toastPool.GetToast(
        _toastSpawnPoint.position,
        Quaternion.identity
         );

        toast.ShootToast(damageType);
    }
    #endregion
}
