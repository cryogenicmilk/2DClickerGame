using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private Button _resetButton;
    void Awake()
    {
        _resetButton.onClick.AddListener(ResetSaveData);
    }

    [Header("参照")]
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField ] private AutoToastGen _autoToastGen;

    private const string ScoreKey = "Score";
    private const string BaseLevelKey = "BaseLevel";
    private const string CritLevelKey = "CritLevel";
    private const string DirectLevelKey = "DirectLevel";
    // トースターレベルと生成したオートトースターを保存
    private const string ToasterLevelKey = "ToasterLevel";

    [SerializeField] private float _saveInterval = 10f;

    void Start()
    {
        Load();
        StartCoroutine(AutoSaveLoop());
    }

    private IEnumerator AutoSaveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_saveInterval);
            Save();
        }
    }

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
        PlayerPrefs.SetInt(ToasterLevelKey, _upgradeManager.ToasterLevel);

        PlayerPrefs.Save();

        Debug.Log($"保存した Score: {_scoreManager.CurrentScore}");
        Debug.Log($"保存した BaseLv: {_upgradeManager.BaseLevel}");
        Debug.Log($"保存した CritLv: {_upgradeManager.CritLevel}");
        Debug.Log($"保存した DirectLv: {_upgradeManager.DirectLevel}");
        Debug.Log($"保存した ToasterLv: {_upgradeManager.ToasterLevel}");
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
        int toasterCount = Mathf.Max(0, toasterLevel - 1);
        _autoToastGen.LoadAutoToasters(toasterCount);

        _uiManager.UpdateUI();

        Debug.Log("ロードしました");
    }

    public void ResetSaveData()
    {
        Debug.Log("ResetSaveData 呼ばれた");

        PlayerPrefs.DeleteKey(ScoreKey);
        PlayerPrefs.DeleteKey(BaseLevelKey);
        PlayerPrefs.DeleteKey(CritLevelKey);
        PlayerPrefs.DeleteKey(DirectLevelKey);
        PlayerPrefs.DeleteKey(ToasterLevelKey);

        PlayerPrefs.Save();

        Debug.Log("セーブデータをリセットしました");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
