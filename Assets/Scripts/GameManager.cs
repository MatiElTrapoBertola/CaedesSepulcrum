using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Progreso Actual")]
    [SerializeField] private int currentNight = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnNightCompleted()
    {
        Debug.Log("Noche " + currentNight + " superada");

        if (currentNight >= 3)
        {
           
            SceneManager.LoadScene("WinScene");
        }
        else
        {
           
            SceneManager.LoadScene("ChurchScene");
        }
    }

    public void ProceedToNextNight()
    {
        currentNight++;

        if (currentNight == 2)
        {
            SceneManager.LoadScene("Noche2");
        }
        else if (currentNight == 3)
        {
            SceneManager.LoadScene("Noche3");
        }
    }

    public int GetCurrentNight()
    {
        return currentNight;
    }
}