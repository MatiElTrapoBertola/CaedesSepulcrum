using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private float sensX;
    private float sensY;
    private float rotationX;
    private float rotationY;

    public Transform orientation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX;
    }

    
}
