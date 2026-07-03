using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SaveManager _saveManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioPlayer.Instance.PlayBGM(0);
        _saveManager.Load();
    }
}
