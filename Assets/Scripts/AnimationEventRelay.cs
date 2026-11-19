using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private PlayerCombat playerCombat;

    private void Awake()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }

    public void DealShovelDamage()
    {
        if(playerCombat != null)
        {
            playerCombat.DealShovelDamage();
        }
    }
}
