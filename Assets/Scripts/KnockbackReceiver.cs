using System.Collections;
using UnityEngine;

public class KnockbackReceiver : MonoBehaviour
{
    [SerializeField] private float knockbackDuration = 0.15f;

    private Coroutine currentKnockback;

    public void ApplyKnockback(Vector3 sourcePosition, float force)
    {
        if (currentKnockback != null)
        {
            StopCoroutine(currentKnockback);
        }

        currentKnockback = StartCoroutine(
            KnockbackRoutine(sourcePosition, force));
    }

    private IEnumerator KnockbackRoutine(
        Vector3 sourcePosition,
        float force)
    {
        Vector3 direction =
            (transform.position - sourcePosition).normalized;

        direction.y = 0f;

        float timer = 0f;

        while (timer < knockbackDuration)
        {
            transform.position +=
                direction *
                force *
                Time.deltaTime;

            timer += Time.deltaTime;

            yield return null;
        }
    }
}