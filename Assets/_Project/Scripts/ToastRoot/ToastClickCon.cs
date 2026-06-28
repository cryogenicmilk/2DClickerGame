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

    [Header("Click Reaction")]
    private Vector3 _defaultScale; // 記憶用
    [SerializeField] private Transform _toasterTrans;

    [SerializeField] private float _squashTime = 0.05f;
    [SerializeField] private float _stretchTime = 0.05f;
    [SerializeField] private float _returnTime = 0.05f;

    [SerializeField] private Vector3 _squashScaleMul = new Vector3(1.15f, 0.85f, 1f);
    [SerializeField] private Vector3 _stretchScaleMul = new Vector3(0.85f, 1.15f, 1f);

    private Coroutine _clickReactionCoroutine;

    void Start()
    {
        _button.onClick.AddListener(OnClickToastButton);
        _defaultScale = _toasterTrans.localScale;
    }

    // 全知全能すべての親
    private void OnClickToastButton()
    {
        DamageResult result = _damageCalculator.CalcDMG();

        _scoreManager.AddScore(result.Damage);
        _uiManager.UpdateUI();

        // reaction
        if (_clickReactionCoroutine != null)
        {
            StopCoroutine(_clickReactionCoroutine);
        }

        _clickReactionCoroutine = StartCoroutine(ClickReaction());

        SpawnToast(result.Type);
    }

    #region クリック時のリアクション関係

    private IEnumerator ClickReaction()
    {
        // 初期サイズに合わせる
        Vector3 squashScale = Vector3.Scale(_defaultScale, _squashScaleMul);
        Vector3 stretchScale = Vector3.Scale(_defaultScale, _stretchScaleMul);

        // 潰れる
        yield return ChangeScale(_defaultScale, squashScale, _squashTime);

        // 細く伸びる
        yield return ChangeScale(squashScale, stretchScale, _stretchTime);

        // 元に戻る
        yield return ChangeScale(stretchScale, _defaultScale, _returnTime);
    }

    private IEnumerator ChangeScale(Vector3 startScale, Vector3 endScale, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            t = Mathf.Clamp01(t);

            _toasterTrans.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        _toasterTrans.localScale = endScale;
    }

    #endregion

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
