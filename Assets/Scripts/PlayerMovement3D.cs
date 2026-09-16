using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("Ajustes de movimiento")]
    [SerializeField] private float speed = 25f;
    [SerializeField] private float sprintMultiplier = 3f;
    [SerializeField] private float rotationspeed = 200f;

    private Vector2 moveInput;
    private bool isSprinting;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed) isSprinting = true;
        if (context.canceled) isSprinting = false;
    }

    // Update is called once per frame
    void Update()
    {
        float speedZ = moveInput.y;
        float rotationX = moveInput.x;

        transform.Translate(Vector3.forward * speedZ * Time.deltaTime);
        transform.Rotate(Vector3.up * rotationX * rotationspeed * Time.deltaTime);
    }
}
