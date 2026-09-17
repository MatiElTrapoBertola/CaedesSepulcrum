using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement3D : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float rayDistance = 1.2f;        
    [SerializeField] private LayerMask detectionLayer = ~0;  

    private Vector2 moveInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        float speedZ = moveInput.y;    
        float rotationX = moveInput.x; 

        transform.Rotate(Vector3.up * rotationX * rotationSpeed * Time.deltaTime);

        bool canMove = true;

        if (speedZ != 0)
        {
            Vector3 rayDirection = speedZ > 0 ? transform.forward : -transform.forward;
            Vector3 origin = transform.position + Vector3.up * 1f;

            RaycastHit hit;

            if (Physics.Raycast(origin, rayDirection, out hit, rayDistance, detectionLayer))
            {
                canMove = false;
                Debug.Log("Obstáculo detectado por Raycast (" + (speedZ > 0 ? "Raycast disparado por frente" : "Raycast disparado por atrás") + "): " + hit.collider.name);
            }
        }

        if (canMove)
        {
            transform.Translate(Vector3.forward * speedZ * speed * Time.deltaTime);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up * 1f;
        Gizmos.DrawRay(origin, transform.forward * rayDistance);
        Gizmos.DrawRay(origin, -transform.forward * rayDistance);
    }
}