using UnityEngine;

public class ChurchDoorController : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 3.0f;
    private Transform playerTransform;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

      
        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.F))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ProceedToNextNight();
            }
            else
            {
                Debug.LogWarning("No se encontró el GameManager en la escena.");
            }
        }
    }
}