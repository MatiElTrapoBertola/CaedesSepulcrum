
using UnityEngine;

public class EnemyPortalSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem PortalFX;
    [Header("Portal Audio")]
    [SerializeField][Range(0f, 2f)] private float portalVolume = 1f;
    [SerializeField] private AudioClip[] portalclips;
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    public void PlayPortalEffect()
    {
        if (PortalFX != null)
        {
            PortalFX.Play();
        }

        PlayPortalSound();
    }

    private void PlayPortalSound()
    {
        if (portalclips == null || portalclips.Length == 0)
        {
            return;
        }

        AudioClip selectedClip = portalclips[Random.Range(0, portalclips.Length)];

        GameObject tempAudio = new GameObject("Portal Audio");

        tempAudio.transform.position = transform.position;

        AudioSource source = tempAudio.AddComponent<AudioSource>();

        source.spatialBlend = 1f;

        source.pitch = Random.Range(minPitch, maxPitch);

        source.PlayOneShot(selectedClip, portalVolume);

        Destroy(tempAudio, selectedClip.length);
    }
}
