using UnityEngine;
using UnityEngine.Audio;

//BGMとSEの再生、停止を行うクラス
//
//UIから音量調整を行う場合は、VolumeUI.csを使用すること
//スクリプトから音量調整を行う場合、
//AudioPlayer.Instance.BGM_Volume , AudioPlayer.Instance.SE_Volumeを呼び出すこと
public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance;

    [Header("AudioClips")]

    [SerializeField]
    [Tooltip("BGMを格納する")]
    private AudioClip[] bgmClips = null;

    [SerializeField]
    [Tooltip("SEを格納する")]
    private AudioClip[] seClips = null;

    [Header("AudioSources\n上はBGM,下はSE")]
    [SerializeField]
    [Tooltip("BGMを再生するAudioSource")]
    private AudioSource bgmSource = null;

    [SerializeField]
    [Tooltip("SEを再生するAudioSource")]
    private AudioSource seSource = null;

    //===音量関連===

    [SerializeField]
    [Tooltip("AudioMixerを指定する")]
    private AudioMixer audioMixer;

    public AudioMixer AudioMixer { get { return audioMixer; } }

    [SerializeField]
    private float bgmDecibel = -10f;

    //BGMのボリュームを取得または設定する
    public float BGM_Volume
    {
        get
        {
            //デシベルをボリュームに変換する(10に decibelBGM / 20f 乗する)
            return Mathf.Pow(10f, bgmDecibel / 20f);
        }
        set
        {
            //ボリュームをデシベルに変換する(log10をとって20をかける)
            float decibel = 20f * Mathf.Log10(value);
            //-80から0にClampする
            decibel = Mathf.Clamp(decibel, -80f, 0f);
            bgmDecibel = decibel;
            audioMixer.SetFloat("BGMParam", decibel);
        }
    }

    [SerializeField]
    private float seDecibel = -10f;

    // SEのボリュームを取得または設定する
    public float SE_Volume
    {
        get
        {
            return Mathf.Pow(10f, seDecibel / 20f);
        }
        set
        {
            //ボリュームをデシベルに変換する(log10をとって20をかける)
            float decibel = 20f * Mathf.Log10(value);
            //-80から0にClampする
            decibel = Mathf.Clamp(decibel, -80f, 0f);
            seDecibel = decibel;
            audioMixer.SetFloat("SEParam", decibel);
        }
    }

    //====================================================================
    //初期化処理
    //====================================================================

    private void Awake()
    {
        //シングルトン
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        //音量を初期の状態に設定する
        AudioMixer.SetFloat("BGMParam", bgmDecibel);
        AudioMixer.SetFloat("SEParam", seDecibel);
    }

    //====================================================================
    //BGM処理
    //====================================================================

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="bgmIndex">BGMの配列インデックス</param>
    public void PlayBGM(int bgmIndex)
    {

        if (bgmIndex < 0 || bgmIndex >= bgmClips.Length)
        {
            Debug.LogError("BGMのインデックスが範囲外です");
            return;
        }

        //BGMが再生中なら停止する
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        //BGMを再生する
        bgmSource.clip = bgmClips[bgmIndex];
        bgmSource.Play();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    //====================================================================
    //SE処理
    //====================================================================

    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="seIndex">SEの配列インデックス</param>
    public void PlaySE(int seIndex)
    {
        if (seIndex < 0 || seIndex >= seClips.Length)
        {
            Debug.LogError("SEのインデックスが範囲外です");
            return;
        }

        //SEを再生する
        seSource.PlayOneShot(seClips[seIndex]);
    }

    /// <summary>
    /// SEを停止する
    /// </summary>
    public void StopSE()
    {
        seSource.Stop();
    }
}
