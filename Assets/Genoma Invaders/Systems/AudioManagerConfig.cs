using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioManagerConfig : SerializedScriptableObject
{
    [Range(0f, 1f)]
    public float volume;
    public Dictionary<AudioClip, Sound> sounds = new();
}
