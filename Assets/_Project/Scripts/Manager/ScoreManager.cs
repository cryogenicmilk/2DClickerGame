using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] float _baseDMG = 1;

    private float _currentScore = 0;
    public float CurrentScore => _currentScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void AddBaseScore()
    {
        AddScore(_baseDMG);
    }

    private void AddScore(float addScore)
    {
        _currentScore += addScore;
        Debug.Log("ScoreManager:åªç›ÇÃÉXÉRÉA: " + _currentScore);
    }
}
