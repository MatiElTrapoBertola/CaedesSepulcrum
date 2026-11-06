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

    [Header("Footsteps Sound Configuration")]
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip[] grassClips;
    [SerializeField] private AudioClip[] concreteClips;
    [SerializeField] private float footstepInterval = 0.5f;
    [SerializeField] private float sprintFootstepMultiplier = 0.7f;
    private float footstepTimer;

    private Vector2 moveInput;
    private bool isSprintPressed;
    private Animator animator;

    public bool SpeedBoostActive { get; set; }

    private void Start()
    {
        // Corregido para encontrar el Animator incluso si está en un objeto hijo
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("No se encontró ningún componente Animator en el jugador ni en sus hijos.");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprintPressed = true;
        }
        else if (context.canceled)
        {
            isSprintPressed = false;
        }
    }

    void Update()
    {
        float currentSpeed = speed;
        bool isRunning = false;

        if (SpeedBoostActive)
        {
            currentSpeed *= 1.5f;
        }

        if (isSprintPressed && moveInput.magnitude > 0)
        {
            if (activeSkills != null && activeSkills.InfiniteResources)
            {
                currentSpeed *= sprintMultiplier;
                isRunning = true;
            }
            else
            {
                if (stamina != null && stamina.ConsumeStamina(staminaCostPerSecond * Time.deltaTime))
                {
                    currentSpeed *= sprintMultiplier;
                    isRunning = true;
                }
            }
        }

        if (animator != null)
        {
            animator.SetFloat("MoveX", moveInput.x, 0.1f, Time.deltaTime);
            animator.SetFloat("MoveY", moveInput.y, 0.1f, Time.deltaTime);
            animator.SetBool("isRunning", isRunning);
        }

        if (cameraTransform == null) return;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;

        bool canMove = true;

        if (moveDirection.magnitude > 0.1f)
        {
            RaycastHit hit;
            Vector3 origin = transform.position + Vector3.up;

            if (Physics.Raycast(origin, moveDirection.normalized, out hit, rayDistance, detectionLayer, QueryTriggerInteraction.Ignore))
            {
                canMove = false;
            }
        }

        if (canMove && moveDirection.magnitude > 0.1f)
        {
            transform.position += moveDirection.normalized * currentSpeed * Time.deltaTime;

            // Rotar suavemente hacia la dirección del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);

            HandleFootsteps(isRunning);
        }
    }

    private void HandleFootsteps(bool isRunning)
    {
        if (footstepAudioSource == null) return;

        float currentInterval = isRunning ? footstepInterval * sprintFootstepMultiplier : footstepInterval;
        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            PlaySurfaceFootstep();
            footstepTimer = currentInterval;
        }
    }

    private void PlaySurfaceFootstep()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.2f;

        if (Physics.Raycast(origin, Vector3.down, out hit, 1.5f))
        {
            string surfaceTag = hit.collider.tag;

            AudioClip[] selectedClips = grassClips;

            if (surfaceTag == "Cemento" || surfaceTag == "Concrete")
            {
                selectedClips = concreteClips;
            }
            else if (surfaceTag == "Tierra" || surfaceTag == "Grass")
            {
                selectedClips = grassClips;
            }

            if (selectedClips != null && selectedClips.Length > 0)
            {
                AudioClip clip = selectedClips[Random.Range(0, selectedClips.Length)];
                footstepAudioSource.pitch = Random.Range(0.9f, 1.1f);
                footstepAudioSource.PlayOneShot(clip);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up;
        Gizmos.DrawRay(origin, transform.forward * rayDistance);
    }
}