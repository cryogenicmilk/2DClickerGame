using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private double _currentScore = 0;
    public double CurrentScore => _currentScore;

    //public void AddScore(DamageResult result)
    //{
    //    _currentScore += result.Damage;

    //    switch (result.Type)
    //    {
    //        case DamageType.Normal:
    //            Debug.Log("ノーマル");
    //            break;
    //        case DamageType.Crit:
    //            Debug.Log("クリティカル");
    //            break;
    //        case DamageType.DirectHit:
    //            Debug.Log("ダイレクト");
    //            break;
    //        case DamageType.CritDirect:
    //            Debug.Log("クリダイ!!");
    //            break;
    //    }
    //}

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
        Debug.Log("ScoreManager:現在のスコア: " + _currentScore);
    }
}
