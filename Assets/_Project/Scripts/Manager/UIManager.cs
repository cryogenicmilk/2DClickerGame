using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private ScoreManager _scoreManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        _scoreText.text = $"Score : {_scoreManager.CurrentScore:0.0}";
    }

    public void ShowDamageTextType(DamageResult result)
    {
        switch (result.Type)
        {
            case DamageType.Normal:
                Debug.Log("ノーマル");
                break;
            case DamageType.Crit:
                Debug.Log("クリティカル");
                break;
            case DamageType.DirectHit:
                Debug.Log("ダイレクト");
                break;
            case DamageType.CritDirect:
                Debug.Log("クリダイ!!");
                break;
        }
    }
}
