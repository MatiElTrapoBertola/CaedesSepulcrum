using UnityEngine;

public class ZombieAnimationRelay : MonoBehaviour
{
    private EnemyAI enemyAI;

    private void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    public void RelayZombieDamage()
    {
        if (enemyAI != null)
        {
            enemyAI.DealZombieDamage();
        }
    }
}