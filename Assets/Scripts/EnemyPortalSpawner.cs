
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

        float randomPitch = Random.Range(minPitch, maxPitch);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(selectedClip, portalVolume, randomPitch);
        }

      
    }
}
