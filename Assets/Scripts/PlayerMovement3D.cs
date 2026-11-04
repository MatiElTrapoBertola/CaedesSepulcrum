using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement3D : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float staminaCostPerSecond = 20f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private PlayerStamina stamina;
    [SerializeField] private PlayerActiveSkills activeSkills;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Obstacle Detection")]
    [SerializeField] private float rayDistance = 1.2f;        
    [SerializeField] private LayerMask detectionLayer = ~0;  
    private Vector2 moveInput;
    private Animator animator;


    public bool SpeedBoostActive
    {
        get; set;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
   
    


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        float currentSpeed = speed;

        bool isRunning = false;
        
        if (SpeedBoostActive)
        {
            currentSpeed *= 1.5f; 
        }

       
        
        if (Keyboard.current.leftShiftKey.isPressed && moveInput.magnitude > 0)
        {
            if (activeSkills != null && activeSkills.InfiniteResources)
            {
                currentSpeed *= sprintMultiplier;
                isRunning = true;
            }
            else
            {
                if (stamina.ConsumeStamina(staminaCostPerSecond * Time.deltaTime))
                {
                    currentSpeed *= sprintMultiplier;
                    isRunning =true;
                }
            }
        }
        
        if(animator != null)
        {
            animator.SetFloat("MoveX", moveInput.x, 0.1f, Time.deltaTime);
            animator.SetFloat("MoveY", moveInput.y, 0.1f, Time.deltaTime);
            animator.SetBool("isRunning", isRunning);
        }

       

   
        
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;

        bool canMove = true;

       if(moveDirection.magnitude > 0.1f)
       {
            RaycastHit hit;

            Vector3 origin = transform.position + Vector3.up;

            if (Physics.Raycast(origin, moveDirection.normalized, out hit, rayDistance,detectionLayer, QueryTriggerInteraction.Ignore))
            {
                canMove = false;

                Debug.Log("Hitbox de Obstaculo detectado: " + hit.collider.name);
            }
       }

       if (canMove && moveDirection.magnitude > 0.1f)
       {
            transform.position += moveDirection.normalized * currentSpeed * Time.deltaTime;
       }
        
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up;
        Gizmos.DrawRay(origin, transform.forward * rayDistance);
       
    }
}