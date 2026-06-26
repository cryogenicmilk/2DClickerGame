using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioPlayer.Instance.PlayBGM(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
