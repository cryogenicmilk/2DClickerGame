using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private UpgradeManager _upgradeManager;

    private const string ScoreKey = "Score";
    private const string BaseLevelKey = "BaseLevel";
    private const string CritLevelKey = "CritLevel";
    private const string DirectLevelKey = "DirectLevel";

    public void Save()
    {
        PlayerPrefs.SetString(ScoreKey, _scoreManager.CurrentScore.ToString());
        PlayerPrefs.SetInt(BaseLevelKey, _upgradeManager.BaseLevel);
        PlayerPrefs.SetInt(CritLevelKey, _upgradeManager.CritLevel);
        PlayerPrefs.SetInt(DirectLevelKey, _upgradeManager.DirectLevel);

        PlayerPrefs.Save();

        Debug.Log("ÉZÅ[ÉuÇµÇ‹ÇµÇΩ");
    }
}
