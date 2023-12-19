using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHook : MonoBehaviour
{
    static public AnimationEventHook instance;
    [SerializeField]
    private AudioPlayer audioPlayer;

    // �炷���̎��
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
        // Int�͂ǂ̉���炷���̃C���f�b�N�X�iAudioPlayer��AudioClip���X�g�̍Đ�����C���f�b�N�X�j
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
        // Int�͂ǂ̉���炷���̃C���f�b�N�X�iAudioPlayer��AudioClip���X�g�̍Đ�����C���f�b�N�X�j
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
