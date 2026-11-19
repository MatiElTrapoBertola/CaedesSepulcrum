using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Basic Stats")]
    public float maxhealth = 20f;
    public float attackDamage = 10f;
    public float moveSpeed = 3.5f;

    [Header("Combat")]
    public float atttackRange = 1.8f;
    public float attackCooldown = 2f;

    [Header("Rewards")]
    public int materialReward = 2;

   
}
