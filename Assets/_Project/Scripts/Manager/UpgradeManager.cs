using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private DamageCalculator _damageCalculator;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private AutoToastGen _autoToastGen;
    [SerializeField] private SaveManager _saveManager;

    [Header("ボタン")]
    [SerializeField] private Button _baseUpgradeButton;
    [SerializeField] private Button _critUpgradeButton;
    [SerializeField] private Button _directUpgradeButton;
    [SerializeField] private Button _addToasterUpgradeButton;

    [Header("最大時の見た目")]
    [SerializeField] private GameObject _critMaxOverlay;
    [SerializeField] private GameObject _directMaxOverlay;

    [Header("基本アップ")]
    [SerializeField] private int _baseLv = 1;
    [SerializeField] private double _baseStartCost = 50;
    [SerializeField] private double _baseCostGrowh = 1.15;
    private double _baseCurrentCost;

    [Header("クリティカル率")]
    [SerializeField] private int _critLv = 1;
    [SerializeField] private double _critStartCost = 100;
    [SerializeField] private double _critCostGrowh = 1.2;
    [SerializeField] private float _critAddRate = 0.01f; // 1%
    private double _critCurrentCost;

    [Header("ダイレクト率")]
    [SerializeField] private int _directLv = 1;
    [SerializeField] private double _directStartCost = 100;
    [SerializeField] private double _directCostGrowh = 1.1;
    [SerializeField] private float _directAddRate = 0.01f;
    private double _directCurrentCost;

    [Header("自動トースター")]
    [SerializeField] private int _addToasterLv = 1;
    [SerializeField] private double _addToasterStartCost = 50;
    [SerializeField] private double _addToasterCostGrowh = 1.05;
    private double _addToasterCurrentCost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _baseUpgradeButton.onClick.AddListener(OnClickBaseUpgrade);
        _critUpgradeButton.onClick.AddListener(OnClickCritUpgrade);
        _directUpgradeButton.onClick.AddListener(OnClickDirectUpgrade);
        _addToasterUpgradeButton.onClick.AddListener(OnClickAddAutoToaster);

        _baseCurrentCost = GetCost(_baseStartCost, _baseCostGrowh, _baseLv);
        _critCurrentCost = GetCost(_critStartCost, _critCostGrowh, _critLv);
        _directCurrentCost = GetCost(_directStartCost, _directCostGrowh, _directLv);
        _addToasterCurrentCost = GetCost(_addToasterStartCost, _addToasterCostGrowh, _addToasterLv);

        RefreshButtonView();
        _uiManager.UpdateUI();
    }
    //====================================================================
    //各ボタン処理
    //====================================================================
    private void OnClickBaseUpgrade()
    {
        if (!CanBuy(_baseCurrentCost))
        {
            AudioPlayer.Instance.PlaySE(0);
            return;
        }

        AudioPlayer.Instance.PlaySE(5);
        SpendScore(_baseCurrentCost);

        _baseLv++;
        _damageCalculator.AddBase();

        _baseCurrentCost = GetCost(_baseStartCost, _baseCostGrowh, _baseLv);

        _saveManager.Save();

        RefreshButtonView();
        _uiManager.UpdateUI();
    }

    private void OnClickCritUpgrade()
    {
        // コストが足りてない
        if (!CanBuy(_critCurrentCost))
        {
            AudioPlayer.Instance.PlaySE(0);
            return;
        }
        // Max
        if (_damageCalculator.IsCritRateMax)
        {
            AudioPlayer.Instance.PlaySE(0);
            return;
        }

        AudioPlayer.Instance.PlaySE(5);
        SpendScore(_critCurrentCost);

        _critLv++;
        _damageCalculator.AddCrit(_critAddRate);

        _critCurrentCost = GetCost(_critStartCost, _critCostGrowh, _critLv);

        _saveManager.Save();

        RefreshButtonView();
        _uiManager.UpdateUI();
    }

    private void OnClickDirectUpgrade()
    {
        // コストが足りてない
        if (!CanBuy(_directCurrentCost))
        {
            AudioPlayer.Instance.PlaySE(0);
            return;
        }
        // Max
        if (_damageCalculator.IsDirectRateMax)
        {
            AudioPlayer.Instance.PlaySE(0);
            return;
        }

        AudioPlayer.Instance.PlaySE(5); 
        SpendScore(_directCurrentCost);

        _directLv++;
        _damageCalculator.AddDirect(_directAddRate);

        _directCurrentCost = GetCost(_directStartCost, _directCostGrowh, _directLv);

        _saveManager.Save();

        RefreshButtonView();
        _uiManager.UpdateUI();
    }

    private void OnClickAddAutoToaster()
    {
        if(!CanBuy(_addToasterCurrentCost))
        {
            AudioPlayer.Instance.PlaySE(0);
            return;
        }

        AudioPlayer.Instance.PlaySE(5);
        SpendScore(_addToasterCurrentCost);
        _autoToastGen.AddAutoToaster();

        _addToasterLv++;

        _addToasterCurrentCost = GetCost(_addToasterStartCost, _addToasterCostGrowh, _addToasterLv);

        _saveManager.Save();

        RefreshButtonView();
        _uiManager.UpdateUI();
    }

    //====================================================================
    //レベルごとコストが上がる計算
    //====================================================================
    private double GetCost(double startCost, double growth, int level)
    {
        return System.Math.Floor(startCost * System.Math.Pow(growth, level - 1));
    }

    //====================================================================
    //判定
    //====================================================================
    private bool CanBuy(double cost)
    {
        return _scoreManager.CurrentScore >= cost;
    }

    private void SpendScore(double cost)
    {
        _scoreManager.SpendScore(cost);
    }

    public int BaseLevel => _baseLv;
    public int CritLevel => _critLv;
    public int DirectLevel => _directLv;
    public int ToasterLevel => _addToasterLv;

    public double BaseCurrentCost => _baseCurrentCost;
    public double CritCurrentCost => _critCurrentCost;
    public double DirectCurrentCost => _directCurrentCost;
    public double ToasterCurrentCost => _addToasterCurrentCost;

    //====================================================================
    //ビジュアル、SE
    //====================================================================
    private void RefreshButtonView()
    {
        if (_critMaxOverlay != null)
        {
            _critMaxOverlay.SetActive(_damageCalculator.IsCritRateMax);
        }

        if (_directMaxOverlay != null)
        {
            _directMaxOverlay.SetActive(_damageCalculator.IsDirectRateMax);
        }
    }

    //====================================================================
    //ロード
    //====================================================================
    public void LoadLevels(int baseLevel, int critLevel, int directLevel, int toasterLevel)
    {
        _baseLv = baseLevel;
        _critLv = critLevel;
        _directLv = directLevel;
        _addToasterLv = toasterLevel;

        RecalculateCosts();
        ApplyLoadedUpgradeEffects();
        RefreshButtonView();
    }

    private void RecalculateCosts()
    {
        _baseCurrentCost = GetCost(_baseStartCost, _baseCostGrowh, _baseLv);
        _critCurrentCost = GetCost(_critStartCost, _critCostGrowh, _critLv);
        _directCurrentCost = GetCost(_directStartCost, _directCostGrowh, _directLv);
        _addToasterCurrentCost = GetCost(_addToasterStartCost, _addToasterCostGrowh, _addToasterLv);
    }

    private void ApplyLoadedUpgradeEffects()
    {
        for (int i = 1; i < _baseLv; i++)
        {
            _damageCalculator.AddBase();
        }

        for (int i = 1; i < _critLv; i++)
        {
            _damageCalculator.AddCrit(_critAddRate);
        }

        for (int i = 1; i < _directLv; i++)
        {
            _damageCalculator.AddDirect(_directAddRate);
        }

        for (int i = 1; i < _addToasterLv; i++)
        {
            _autoToastGen.AddAutoToaster();
        }
    }
}
