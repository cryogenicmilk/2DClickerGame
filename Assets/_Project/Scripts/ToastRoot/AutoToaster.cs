using System.Collections;
using UnityEngine;

public class AutoToaster : MonoBehaviour
{
    [SerializeField] private ToasterReaction _toasterReaction;

    [Header("Auto Score")]
    [SerializeField] private float _interval = 20f;
    [SerializeField] private double _productionCount = 50;

    [Header("Spawn Point")]
    [SerializeField] private Transform _toastSpawnPoint;
    [SerializeField] private Transform _flyingTextSpawnPoint;

    [Header("Start Delay")]
    [SerializeField] private bool _useRandomStartDelay = true;

    private ScoreManager _scoreManager;
    private UIManager _uiManager;
    private ToastPool _toastPool;
    private FlyingTextSpawner _flyingTextSpawner;

    private Coroutine _autoCoroutine;

    public void Initialize(
        ScoreManager scoreManager,
        UIManager uiManager,
        ToastPool toastPool,
        FlyingTextSpawner flyingTextSpawner)
    {
        _scoreManager = scoreManager;
        _uiManager = uiManager;
        _toastPool = toastPool;
        _flyingTextSpawner = flyingTextSpawner;

        if (_autoCoroutine != null)
        {
            StopCoroutine(_autoCoroutine);
        }

        _autoCoroutine = StartCoroutine(AutoLoop());
    }

    private IEnumerator AutoLoop()
    {
        if (_useRandomStartDelay)
        {
            float randomDelay = Random.Range(0f, _interval);
            yield return new WaitForSeconds(randomDelay);
        }

        while (true)
        {
            ProduceToast();
            yield return new WaitForSeconds(_interval);
        }
    }

    private void ProduceToast()
    {
        _toasterReaction?.PlayReaction();

        _scoreManager.AddScore(_productionCount);
        _uiManager.UpdateUI();

        SpawnFlyingText();
        SpawnToastProjectile();
    }

    private void SpawnFlyingText()
    {
        if (_flyingTextSpawner == null)
        {
            return;
        }

        Vector3 spawnPosition = _flyingTextSpawnPoint != null
        ? _flyingTextSpawnPoint.position
        : transform.position;

        DamageResult result = new DamageResult(_productionCount, DamageType.Normal);

        _flyingTextSpawner.Spawn(result, spawnPosition);
    }

    private void SpawnToastProjectile()
    {
        if (_toastPool == null)
        {
            return;
        }

        Vector3 spawnPosition = _toastSpawnPoint != null
            ? _toastSpawnPoint.position
            : transform.position;

        ToastProjectile toast = _toastPool.GetToast(
            spawnPosition,
            Quaternion.identity
        );

        toast.ShootToast(DamageType.Normal);
    }

    private void OnDisable()
    {
        if (_autoCoroutine != null)
        {
            StopCoroutine(_autoCoroutine);
            _autoCoroutine = null;
        }
    }
}