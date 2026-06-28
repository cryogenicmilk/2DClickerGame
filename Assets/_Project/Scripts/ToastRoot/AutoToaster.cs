using System.Collections;
using UnityEngine;

public class AutoToaster : MonoBehaviour
{
    [SerializeField] private ToasterReaction _toasterReaction;

    [Header("Auto Score")]
    [SerializeField] private float _interval = 20f;
    [SerializeField] private double _productionCount = 50;

    [Header("Start Delay")]
    [SerializeField] private bool _useRandomStartDelay = true;

    private ScoreManager _scoreManager;
    private UIManager _uiManager;

    private Coroutine _autoCoroutine;

    public void Initialize(ScoreManager scoreManager, UIManager uiManager)
    {
        _scoreManager = scoreManager;
        _uiManager = uiManager;

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
        if (_toasterReaction != null)
        {
            _toasterReaction.PlayReaction();
        }

        _scoreManager.AddScore(_productionCount);
        _uiManager.UpdateUI();
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