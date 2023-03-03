using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField]
    private AudioManagerConfig config;

    public void Play(AudioClip clip)
    {
        if (config.sounds.TryGetValue(clip, out Sound sound))
        {
            sound.Play();
        }
        else
        {
            Debug.LogWarning($"Sound {clip.name} not found");
        }
    }

    private new void Awake()
    {
        base.Awake();

        foreach (AudioClip clip in config.sounds.Keys)
        {
            config.sounds[clip].audioSource = gameObject.AddComponent<AudioSource>();
            config.sounds[clip].Clip = clip;
        }
    }
}
