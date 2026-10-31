using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement3D : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float staminaCostPerSecond = 20f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private PlayerStamina stamina;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float rayDistance = 1.2f;        
    [SerializeField] private LayerMask detectionLayer = ~0;  
    [SerializeField] private PlayerActiveSkills activeSkills;

    private Vector2 moveInput;
    public bool SpeedBoostActive
    {
        get; set;
    }

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

            if (Physics.Raycast(origin, rayDirection, out hit, rayDistance, detectionLayer, QueryTriggerInteraction.Ignore))
            {
                canMove = false;
                Debug.Log("Obstáculo detectado por Raycast (" + (speedZ > 0 ? "Raycast disparado por frente" : "Raycast disparado por atrás") + "): " + hit.collider.name);
            }
        }

        float currentSpeed = speed;
        if (SpeedBoostActive)
        {
            currentSpeed *= 1.5f;
        }

        if (Keyboard.current.leftShiftKey.isPressed && speedZ != 0)
        {
            if (activeSkills != null && activeSkills.InfiniteResources)
            {
                currentSpeed *= sprintMultiplier;
            }
            else
            {
                if (stamina.ConsumeStamina(staminaCostPerSecond * Time.deltaTime))
                {
                    currentSpeed *= sprintMultiplier;
                }
            }
           
        }

        if (canMove)
        {
            transform.Translate(
                Vector3.forward *
                speedZ *
                currentSpeed *
                Time.deltaTime);
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