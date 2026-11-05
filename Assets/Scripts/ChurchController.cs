using UnityEngine;

public class ChurchController : MonoBehaviour
{
    public void ProceedToNextNight()
    {
        if (GameManager.Instance != null)
        {
          
            GameManager.Instance.ProceedToNextNight();
        }
        else
        {
            Debug.LogWarning("No se encontro el GameManager en la escena.");
        }
    }
}