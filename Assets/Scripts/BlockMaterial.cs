using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block Material", menuName = "Blocky/Block Material")]
public class BlockMaterial : ScriptableObject
{
    public AudioClip[] breakingSounds;
    public AudioClip[] placingSounds;
    public AudioClip[] footstepSounds;

    public enum AudioEvent
    {
        Breaking,
        Placing,
        Footstep
    }

    public AudioClip GetAudioClip(AudioEvent _audioEvent)
    {
        AudioClip[] selectedEventArray;
        switch (_audioEvent)
        {
            case AudioEvent.Breaking:
                selectedEventArray = breakingSounds;
                break;
            case AudioEvent.Placing:
                selectedEventArray = placingSounds;
                break;
            case AudioEvent.Footstep:
                selectedEventArray = footstepSounds;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_audioEvent), _audioEvent, null);
        }

        return selectedEventArray[Mathf.FloorToInt(Time.time * 1000) % selectedEventArray.Length];
    }
}
