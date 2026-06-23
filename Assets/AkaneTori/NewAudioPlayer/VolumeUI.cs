using UnityEngine;
using UnityEngine.UI;

//スライダーを指定することで、音量を調整できるクラス
public class VolumeUI : MonoBehaviour
{
    [Header("SettingsUI")]

    [SerializeField]
    [Tooltip("BGMスライダーを指定する")]
    private Slider bgmSlider = null;
    [SerializeField]
    [Tooltip("SEスライダーを指定する")]
    private Slider seSlider = null;

    private void Start()
    {
        //スライダーの値を指定する
        bgmSlider.value = AudioPlayer.Instance.BGM_Volume;
        seSlider.value = AudioPlayer.Instance.SE_Volume;

        ///イベントの登録
        //BGMの調整
        bgmSlider.onValueChanged.AddListener((value) => { AudioPlayer.Instance.BGM_Volume = value; });
        //SEの調整
        seSlider.onValueChanged.AddListener((value) => { AudioPlayer.Instance.SE_Volume = value; });
    }
}