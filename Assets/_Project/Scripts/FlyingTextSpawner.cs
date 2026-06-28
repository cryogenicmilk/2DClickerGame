using System.Collections.Generic;
using UnityEngine;

public class FlyingTextSpawner : MonoBehaviour
{
    [SerializeField] private FlyingText _flyingTextPrefab;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private RectTransform _spawnPoint;

    [Header("Canvas")]
    [SerializeField] private Canvas _canvas;

    [Header("Camera")]
    [SerializeField] private Camera _worldCamera;

    [Header("Pool")]
    [SerializeField] private int _initialPoolSize = 30;

    [Header("Random Offset")]
    [SerializeField] private float _randomX = 40f;
    [SerializeField] private float _randomY = 20f;

    private readonly Queue<FlyingText> _pool = new Queue<FlyingText>();

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            FlyingText text = CreateFlyingText();
            ReturnFlyingText(text);
        }
    }

    private FlyingText CreateFlyingText()
    {
        FlyingText text = Instantiate(_flyingTextPrefab, _parent);
        text.SetSpawner(this);
        return text;
    }

    private FlyingText GetFlyingText()
    {
        FlyingText text;

        if (_pool.Count > 0)
        {
            text = _pool.Dequeue();
        }
        else
        {
            text = CreateFlyingText();
        }

        text.transform.SetParent(_parent, false);
        text.gameObject.SetActive(true);

        return text;
    }

    public void ReturnFlyingText(FlyingText text)
    {
        text.gameObject.SetActive(false);
        text.transform.SetParent(_parent, false);
        _pool.Enqueue(text);
    }

    public void Spawn(DamageResult result)
    {
        SpawnAtAnchoredPosition(result, _spawnPoint.anchoredPosition);
    }

    public void Spawn(DamageResult result, Vector3 worldPosition)
    {
        Vector2 anchoredPosition = WorldToCanvasPosition(worldPosition);
        SpawnAtAnchoredPosition(result, anchoredPosition);
    }

    private void SpawnAtAnchoredPosition(DamageResult result, Vector2 basePosition)
    {
        FlyingText text = GetFlyingText();

        Vector2 randomOffset = new Vector2(
            Random.Range(-_randomX, _randomX),
            Random.Range(-_randomY, _randomY)
        );

        text.Play(result.Damage, result.Type, basePosition + randomOffset);
    }

    private Vector2 WorldToCanvasPosition(Vector3 worldPosition)
    {
        if (_canvas == null)
        {
            Debug.LogError("FlyingTextSpawner: Canvas Ç™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
            return Vector2.zero;
        }

        if (_worldCamera == null)
        {
            _worldCamera = Camera.main;
        }

        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(
            _worldCamera,
            worldPosition
        );

        Camera canvasCamera = null;

        if (_canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            canvasCamera = _canvas.worldCamera;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parent,
            screenPosition,
            canvasCamera,
            out Vector2 localPosition
        );

        return localPosition;
    }
}