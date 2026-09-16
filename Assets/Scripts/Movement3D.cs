using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float rotation = 200f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //float speedX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        //float speedY = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        //Vector3 posicion = transform.position;

        // transform.position = new Vector3(speedX + posicion.x, speedY + posicion.y, posicion.z);

        float speedZ = Input.GetAxis("Vertical");
        float rotationX = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * speedZ * speed * Time.deltaTime);
        transform.Rotate(Vector3.up * rotationX * rotation * Time.deltaTime);
    }

   
}
