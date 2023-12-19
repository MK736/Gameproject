using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHook : MonoBehaviour
{
    static public AnimationEventHook instance;
    [SerializeField]
    private AudioPlayer audioPlayer;

    // 鳴らす音の種類
    public AudioPlayer.eAundioType audioType;


    public int Index = 0;
    public int IndexJump = 3;
    int clipIndex = 0;

    //public int clipIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public void Step(AnimationEvent animationEvent)
    {
        // Intはどの音を鳴らすかのインデックス（AudioPlayerのAudioClipリストの再生するインデックス）
        //int clipIndex = animationEvent.intParameter;

        switch (Index)
        {
            case 0:
                clipIndex = 0;
                break;
            case 1:
                clipIndex = 1;
                break;
            case 2:
                clipIndex = 2;
                break;
        }
        audioPlayer.PlayAudioClip(audioType, clipIndex);
    }

    public void Jump(AnimationEvent animationEvent)
    {
        // Intはどの音を鳴らすかのインデックス（AudioPlayerのAudioClipリストの再生するインデックス）
        //int clipIndex = animationEvent.intParameter;

        switch (IndexJump)
        {
            case 3:
                clipIndex = 3;
                break;
            case 4:
                clipIndex = 4;
                break;
            case 5:
                clipIndex = 5;
                break;
        }
        audioPlayer.PlayAudioClip(audioType, clipIndex);
    }
}
