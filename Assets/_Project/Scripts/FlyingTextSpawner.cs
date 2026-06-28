using UnityEngine;

public class FlyingTextSpawner : MonoBehaviour
{
    [SerializeField] private FlyingText _flyingTextPrefab;
    [SerializeField] private RectTransform _parent;
    [SerializeField] private RectTransform _spawnPoint;

    [Header("Random Offset")]
    [SerializeField] private float _randomX = 40f;
    [SerializeField] private float _randomY = 20f;

    public void Spawn(DamageResult result)
    {
        FlyingText text = Instantiate(_flyingTextPrefab, _parent);

        Vector2 basePosition = _spawnPoint.anchoredPosition;

        Vector2 randomOffset = new Vector2(
            Random.Range(-_randomX, _randomX),
            Random.Range(-_randomY, _randomY)
        );

        text.Play(result.Damage, result.Type, basePosition + randomOffset);
    }
}
