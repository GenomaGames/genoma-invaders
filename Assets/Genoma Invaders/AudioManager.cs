using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField]
    private AudioManagerConfig config;

    public void Play(AudioClip clip)
    {
        if (config.sounds.TryGetValue(clip, out Sound sound))
        {
            float pitch = Random.Range(0.75f, 1.25f);
            sound.audioSource.pitch = pitch;
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
            Sound sound = config.sounds[clip];

            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.volume = config.volume;
            sound.Clip = clip;
        }
    }
}
