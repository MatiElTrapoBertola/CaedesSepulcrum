using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject crosshair;

    private bool isPaused;

    private void Start()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
            {
                OpenPauseMenu();
            }
            else
            {
                if (controlsPanel.activeSelf)
                {
                    CloseControls();
                }
                else
                {
                    ResumeGame();
                }
            }
        }
    }

    public void OpenPauseMenu()
    {
        pausePanel.SetActive(true);
        controlsPanel.SetActive(false);

        // Ocultar HUD
        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentHUD != null)
        {
            GameManager.Instance.CurrentHUD.SetActive(false);
        }

        // Ocultar retícula
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);

        // Mostrar HUD
        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentHUD != null)
        {
            GameManager.Instance.CurrentHUD.SetActive(true);
        }

        // Mostrar retícula
        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }

        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isPaused = false;
    }

    public void OpenControls()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}