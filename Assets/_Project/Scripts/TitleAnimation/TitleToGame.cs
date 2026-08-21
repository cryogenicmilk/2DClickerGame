using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleToGame : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera _mainCamera;

    [Header("Camera Points")]
    [SerializeField] private Transform _titleCameraPoint;
    [SerializeField] private Transform _gameCameraPoint;
    
    [Header("Camera Size")]
    [SerializeField] private float _titleSize = 1f;
    [SerializeField] private float _gameSize = 5f;

    [Header("UI")]
    [SerializeField] private CanvasGroup _titleUI;
    [SerializeField] private CanvasGroup _gameUI;

    [Header("Transition")]
    [SerializeField] private float _duration = 1f;

    private bool _isStarted;

    private void Start()
    {
        SetupTitleView();
    }

    private void Update()
    {
        if (_isStarted) return;

        // ここ！メソッドにしたほうがいいよ～w
        // うーんinputSystemはメンディー
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartGame();
        }
        // スマホやるかわからーん
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            StartGame();
        }
    }

    private void SetupTitleView()
    {
        _isStarted = false;

        _mainCamera.transform.position = new Vector3(
            _titleCameraPoint.position.x,
            _titleCameraPoint.position.y,
            _mainCamera.transform.position.z
        );

        _mainCamera.orthographicSize = _titleSize;

        _titleUI.alpha = 1f;
        _titleUI.interactable = true;
        _titleUI.blocksRaycasts = true;

        _gameUI.alpha = 0f;
        _gameUI.interactable = false;
        _gameUI.blocksRaycasts = false;
    }

    private void StartGame()
    {
        _isStarted = true;

        // uo!
        _titleUI.interactable = false;
        _titleUI.blocksRaycasts = false;

        Vector3 gameCameraPosition = new Vector3(
            _gameCameraPoint.position.x,
            _gameCameraPoint.position.y,
            _mainCamera.transform.position.z
        );

        Sequence sequence = DOTween.Sequence();

        sequence.Join(_mainCamera.transform.DOMove(gameCameraPosition, _duration));
        sequence.Join(DOTween.To(
            () => _mainCamera.orthographicSize,
            size => _mainCamera.orthographicSize = size,
            _gameSize,
            _duration
        ));

        sequence.Join(_titleUI.DOFade(0f, 0.4f));

        sequence.Join(_gameUI.DOFade(1f, 0.4f).SetDelay(_duration * 0.5f));
        sequence.OnComplete(() =>
        {
            if (_gameUI != null)
            {
                _gameUI.interactable = true;
                _gameUI.blocksRaycasts = true;
            }
        });
    }
}
