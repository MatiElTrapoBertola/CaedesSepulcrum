using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySFX(AudioClip clip, float volume, float pitch)
    {
        if (clip == null)
            return;

        sfxSource.pitch = pitch;

        sfxSource.PlayOneShot(clip,volume);

        sfxSource.pitch = 1f;
    }
}
