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
    public GameObject CurrentHUD => currentHudIstance;

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

        if (sceneName == "MainMenu" ||
            sceneName == "Victory" ||
            sceneName == "Defeat")
        {
            if (currentHudIstance != null)
            {
                Destroy(currentHudIstance);
                currentHudIstance = null;
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

        if (currentNight >= 7)
        {
            SceneManager.LoadScene("Victory");
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
            SceneManager.LoadScene("Night 2");
        }
        else if (currentNight == 3)
        {
            SceneManager.LoadScene("Night 3");
        }
        else if (currentNight == 4)
        {
            SceneManager.LoadScene("Night 4");
        }
        else if (currentNight == 5)
        {
            SceneManager.LoadScene("Night 5");
        }
        else if (currentNight == 6)
        {
            SceneManager.LoadScene("Night 6");
        }
        else if (currentNight == 7)
        {
            SceneManager.LoadScene("Night 7");
        }
    }

    public int GetCurrentNight()
    {
        return currentNight;
    }
}