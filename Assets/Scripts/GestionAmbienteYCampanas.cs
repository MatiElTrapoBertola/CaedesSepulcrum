using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlAudioCementerio : MonoBehaviour
{
    [Header("Ambiente y Campanas")]
    [SerializeField] private AudioSource viento;
    [SerializeField] private AudioSource campanas;
    [SerializeField] private string escenaIglesia = "EscenaIglesia";

    [Header("Eventos del Mapa")]
    [SerializeField] private Transform jugador;

    private float cronoBuho, cronoGrillos, esperaBuho = 4f, esperaGrillos = 5f;
    private bool bossIniciado, esIglesia;

    void Start()
    {
        if (viento == null) return;
        esIglesia = SceneManager.GetActiveScene().name == escenaIglesia;
        viento.volume = esIglesia ? 0.02f : 0.09f; viento.pitch = esIglesia ? 0.7f : 1.0f; viento.Play();
    }

    void Update()
    {
        if (esIglesia) return;

        
        GameObject[] arboles = GameObject.FindGameObjectsWithTag("HollowTree");
        GameObject arbolCercano = null; float distMin = Mathf.Infinity;

        foreach (GameObject a in arboles)
        {
            float d = Vector3.Distance(jugador.position, a.transform.position);
            if (d < distMin) { distMin = d; arbolCercano = a; }
        }
        
        GameObject[] mausoleos = GameObject.FindGameObjectsWithTag("Mausoleum"); 
        GameObject mausoleoCercano = null; float distMinMausoleo = Mathf.Infinity;

        foreach (GameObject m in mausoleos)
        {
            float d = Vector3.Distance(jugador.position, m.transform.position);
            if (d < distMinMausoleo) { distMinMausoleo = d; mausoleoCercano = m; }
        }

        
        if (distMinMausoleo < 15f && mausoleoCercano != null)
        {
            AudioSource audioMausoleo = mausoleoCercano.GetComponentInChildren<AudioSource>();
            if (audioMausoleo != null && !audioMausoleo.isPlaying)
            {
                audioMausoleo.volume = 0.3f;
                audioMausoleo.Play();
            }
        }

        
        if (distMin < 25f && arbolCercano != null)
        {
            
            AudioSource[] fuentesArbol = arbolCercano.GetComponentsInChildren<AudioSource>();
            if (fuentesArbol.Length < 2) return; 

            AudioSource gArbol = fuentesArbol[0]; 
            AudioSource bArbol = fuentesArbol[1]; 

            
            cronoGrillos += Time.deltaTime;
            if (cronoGrillos >= esperaGrillos)
            {
                cronoGrillos = 0f; esperaGrillos = Random.Range(4f, 9f);
                if (Random.value > 0.25f && !gArbol.isPlaying) { gArbol.volume = 0.5f; gArbol.Play(); }
            }

            
            cronoBuho += Time.deltaTime;
            if (cronoBuho >= esperaBuho)
            {
                cronoBuho = 0f; esperaBuho = Random.Range(3f, 7f);
                if (Random.value > 0.25f && !bArbol.isPlaying) { bArbol.volume = 0.6f; bArbol.Play(); }
            }
        }
    }

    public void ReproducirCampanadas() { if (campanas != null && !campanas.isPlaying) { campanas.volume = 0.6f; campanas.Play(); } }
    public void ActivarBatallaBoss() => bossIniciado = true;
}
