using UnityEngine;

public class AutoToastGen : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private ToastPool _toastPool;
    [SerializeField] private FlyingTextSpawner _flyingTextSpawner;

    [Header("Auto Toaster Visual")]
    [SerializeField] private AutoToaster _autoToasterPrefab;
    [SerializeField] private Transform _autoToasterParent;

    [Header("Spawn Area")]
    [SerializeField] private BoxCollider2D _spawnArea;

    private int _autoLevel = 0;

    public int AutoLevel => _autoLevel;

    public void AddAutoToaster()
    {
        _autoLevel++;
        SpawnAutoToaster();
    }

    // 現状トースターのレベルに応じて生成数を保存しているが、位置は保存していないため、ロード時にランダムな位置に生成される
    public void LoadAutoToasters(int level)
    {
        _autoLevel = level;
        for (int i = 0; i < _autoLevel; i++)
        {
            SpawnAutoToaster();
        }
    }

    private void SpawnAutoToaster()
    {
        Vector3 spawnPosition = GetRandomPositionInBox();

        AutoToaster autoToaster = Instantiate(
            _autoToasterPrefab,
            spawnPosition,
            Quaternion.identity,
            _autoToasterParent
        );

        autoToaster.Initialize(
            _scoreManager,
            _uiManager,
            _toastPool,
            _flyingTextSpawner
        );
    }

    private Vector3 GetRandomPositionInBox()
    {
        Bounds bounds = _spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y, 0f);
    }
}