using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HeartBeat : MonoBehaviour
{
    private AudioSource fuenteLatidos;
    [SerializeField] private float factorAceleracion = 0.5f;
    [SerializeField] private float factorRecuperacion = 0.3f;

    void Start()
    {
        fuenteLatidos = GetComponent<AudioSource>();
        fuenteLatidos.pitch = 1.0f; // Ritmo normal inicial
    }

    void Update()
    {
        // Lógica condicional: si mantiene Shift presionado, el corazón se acelera
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Sube el pitch gradualmente hasta un máximo de 2.2f (latido muy rápido)
            fuenteLatidos.pitch = Mathf.MoveTowards(fuenteLatidos.pitch, 2.2f, factorAceleracion * Time.deltaTime);
        }
        else
        {
            // Lógica condicional: si no corre, el corazón recupera su ritmo base (1.0f)
            fuenteLatidos.pitch = Mathf.MoveTowards(fuenteLatidos.pitch, 1.0f, factorRecuperacion * Time.deltaTime);
        }
    }
}