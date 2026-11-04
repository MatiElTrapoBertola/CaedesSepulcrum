using UnityEngine;
using UnityEngine.InputSystem;

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
            Debug.Log("Jugador encontrado por la puerta");
        }
        else
        {
            Debug.LogError("La puerta NO encontro al jugador");
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

     
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactionDistance)
        {
           
            if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
            {
                Debug.Log("Puerta utilizada - Avanzando de noche");

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ProceedToNextNight();
                }
                else
                {
                    Debug.LogError("No se encontró el GameManager en la escena.");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}