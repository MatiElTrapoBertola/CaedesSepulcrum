using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Actual Night")]
    [SerializeField] private int currentNight = 1;

    [Header("SpawnBetweenScenes")]
    [HideInInspector] public string spawnPointName = "";

    [Header("HUD Configuration")]
    [SerializeField] private GameObject hudPrefab;

    private GameObject currentHudIstance;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName == "MainMenu" || sceneName == "WinScene" || sceneName == "DefeatedScene")
        {
            if (currentHudIstance != null)
            {
                Destroy(currentHudIstance);
            }
        }
        else
        {
            if (currentHudIstance == null && hudPrefab != null)
            {
                currentHudIstance = Instantiate(hudPrefab);
                DontDestroyOnLoad(currentHudIstance);
            }
        }
    }

    public void OnNightCompleted()
    {
        Debug.Log("Noche " + currentNight + " superada");

        spawnPointName = "SpawnChurch";

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
        Debug.Log("ProceedToNextNight ejecutado");

        currentNight++;

        Debug.Log("Nueva noche: " + currentNight);

        spawnPointName = "SpawnNight";

        if (currentNight == 2)
        {
            Debug.Log("Intentando cargar Noche2");
            SceneManager.LoadScene("Night 2");
        }
        else if (currentNight == 3)
        {
            Debug.Log("Intentando cargar Noche3");
            SceneManager.LoadScene("Night 3");
        }
    }

    public int GetCurrentNight()
    {
        return currentNight;
    }
}