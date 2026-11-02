using UnityEngine;

public class EnemyPortalSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem PortalFX;

    public void PlayPortalEffect()
    {
        if (PortalFX != null)
        {
            PortalFX.Play();
        }
    }
}
