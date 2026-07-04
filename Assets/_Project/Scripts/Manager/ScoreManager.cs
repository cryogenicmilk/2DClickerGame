using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private double _currentScore = 0;
    public double CurrentScore => _currentScore;

    public bool SpendScore(double cost)
    {
        if (_currentScore < cost)
        {
            return false;
        }
        _currentScore -= cost;
        return true;
    }

    public void AddScore(double score)
    {
        _currentScore += score;
        //Debug.Log("ScoreManager:現在のスコア: " + _currentScore);
    }

    // データロード
    public void SetScore(double score)
    {
        _currentScore = score;
    }
}
