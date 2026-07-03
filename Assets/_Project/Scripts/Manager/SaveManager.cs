using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private UIManager _uiManager;

    private const string ScoreKey = "Score";
    private const string BaseLevelKey = "BaseLevel";
    private const string CritLevelKey = "CritLevel";
    private const string DirectLevelKey = "DirectLevel";
    private const string ToasterLevelKey = "ToasterLevel";

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetString(ScoreKey, _scoreManager.CurrentScore.ToString());
        PlayerPrefs.SetInt(BaseLevelKey, _upgradeManager.BaseLevel);
        PlayerPrefs.SetInt(CritLevelKey, _upgradeManager.CritLevel);
        PlayerPrefs.SetInt(DirectLevelKey, _upgradeManager.DirectLevel);


        PlayerPrefs.Save();

        Debug.Log($"保存した Score: {_scoreManager.CurrentScore}");
        Debug.Log($"保存した BaseLv: {_upgradeManager.BaseLevel}");
        Debug.Log($"保存した CritLv: {_upgradeManager.CritLevel}");
        Debug.Log($"保存した DirectLv: {_upgradeManager.DirectLevel}");
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey(ScoreKey))
        {
            Debug.Log("セーブデータなし");
            return;
        }

        string scoreText = PlayerPrefs.GetString(ScoreKey, "0");

        if (double.TryParse(scoreText, out double loadedScore))
        {
            _scoreManager.SetScore(loadedScore);
        }

        int baseLevel = PlayerPrefs.GetInt(BaseLevelKey, 1);
        int critLevel = PlayerPrefs.GetInt(CritLevelKey, 1);
        int directLevel = PlayerPrefs.GetInt(DirectLevelKey, 1);
        int toasterLevel = PlayerPrefs.GetInt(ToasterLevelKey, 1);

        _upgradeManager.LoadLevels(baseLevel, critLevel, directLevel, toasterLevel);
        _uiManager.UpdateUI();

        Debug.Log("ロードしました");
    }
}
