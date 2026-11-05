using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement3D : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float rayDistance = 1.2f;
    [SerializeField] private LayerMask detectionLayer = ~0;

    private Vector2 moveInput;
    private Rigidbody rb; // <- Agregado

    void Start() => rb = GetComponent<Rigidbody>(); // <- Agregado

    public void OnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();

    void Update() => transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);

    void FixedUpdate() // <- Cambiado a FixedUpdate para que el Rigidbody responda
    {
        float speedZ = moveInput.y;
        bool canMove = true;

        if (speedZ != 0)
        {
            Vector3 rayDirection = speedZ > 0 ? transform.forward : -transform.forward;
            if (Physics.Raycast(transform.position + Vector3.up * 1f, rayDirection, rayDistance, detectionLayer))
                canMove = false;
        }

        // Aplica el movimiento directamente a la velocidad del Rigidbody
        rb.linearVelocity = (canMove && speedZ != 0) ? transform.forward * speedZ * speed : new Vector3(0, rb.linearVelocity.y, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up * 1f;
        Gizmos.DrawRay(origin, transform.forward * rayDistance);
        Gizmos.DrawRay(origin, -transform.forward * rayDistance);
    }
}