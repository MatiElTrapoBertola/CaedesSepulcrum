using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;


    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.spawnPointName))
        {
            GameObject spawnPoint = GameObject.Find(GameManager.Instance.spawnPointName);

            if(spawnPoint != null)
            {
               Rigidbody rb = spawnPoint.GetComponent<Rigidbody>();

                if(rb != null)
                {
                    rb.isKinematic = true;
                }

                transform.position = spawnPoint.transform.position;
                transform.rotation = spawnPoint.transform.rotation;

                if(rb != null)
                {
                    rb.isKinematic = false;
                }

                Debug.Log("Heroe posicionado con exito en " + spawnPoint.name);
            }

            else
            {
                Debug.LogWarning("Cuidado, no se encontro ningun objeto llamado '" + GameManager.Instance.spawnPointName + "' en esta escena.");
            }
        }
    }
}
