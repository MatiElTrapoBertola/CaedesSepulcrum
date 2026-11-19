using UnityEngine;
using UnityEngine.InputSystem;

public class ChurchDoorController : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 4.0f;
    private Transform playerTransform;

    private void Start()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("Puerta: Jugador encontrado exitosamente -> " + player.name);
        }
        else
        {
            Debug.LogError("La puerta NO encontró ningún objeto con la etiqueta 'Player'. Asegúrate de que el jugador tenga el Tag 'Player'.");
        }
    }

    private void Update()
    {
        // Si por alguna razón se perdió la referencia, intentamos buscarlo de nuevo
        if (playerTransform == null)
        {
            FindPlayer();
            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // Opcional para depurar: descomenta la siguiente línea si quieres ver la distancia en consola cada frame
        // Debug.Log("Distancia a la puerta: " + distance);

        // Cambia esto dentro del Update de ChurchDoorController.cs:
        if (distance <= interactionDistance)
        {
            // Usamos la tecla E en lugar de la F para evitar que colisione con el cura
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                Debug.Log("¡Tecla E presionada cerca de la puerta de la iglesia!");

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ProceedToNextNight();
                }
                else
                {
                    Debug.LogError("¡ERROR CRÍTICO! No se encontró ninguna instancia del GameManager en la escena.");
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