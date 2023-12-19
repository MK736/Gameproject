using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Stage3);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
