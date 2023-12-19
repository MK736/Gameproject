using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private List<AudioClip> floorAudioClips;

    [SerializeField]
    private List<AudioClip> gravelAudioClips;

    public enum eAundioType
    {
        Floor = 0,
        Gravel,
    }

    /// <summary>
    /// AudioClipを再生する
    /// </summary>
    /// <param name="audioType"></param>
    /// <param name="clipIndex">audioClipsリストの再生するインデックス</param>
    public void PlayAudioClip(eAundioType audioType, int clipIndex)
    {
        List<AudioClip> audioClips = new List<AudioClip>();
        switch (audioType)
        {
            case eAundioType.Floor:
                audioClips = floorAudioClips;
                break;

            case eAundioType.Gravel:
                audioClips = gravelAudioClips;
                break;
        }

        // 配列外チェック
        if (clipIndex < 0 || audioClips.Count <= clipIndex)
        {
            return;
        }

        audioSource.Stop();
        audioSource.clip = audioClips[clipIndex];
        audioSource.Play();
    }
}
