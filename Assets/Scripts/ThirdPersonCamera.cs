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

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;

        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, -40f, 70f);

        player.rotation = Quaternion.Euler(0f, yaw, 0f);

        pivot.localRotation = Quaternion.Euler(pitch,0f, 0f);
    }
}
