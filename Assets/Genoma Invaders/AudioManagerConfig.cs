using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioManagerConfig : SerializedScriptableObject
{
    public Dictionary<AudioClip, Sound> sounds = new();
}
