using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform pivot;

    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 150f;

    private float pitch;
    private float yaw;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Usamos LateUpdate para que la cámara calcule su posición justo después 
    // de que el jugador se mueva en el Update, eliminando el efecto de trabado o tirones.
    private void LateUpdate()
    {
        if (player == null || pivot == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, -40f, 70f);

        // Aplicamos la rotación limpia al jugador y al pivote de la cámara
        player.rotation = Quaternion.Euler(0f, yaw, 0f);
        pivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}