using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NightTransitionController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField][TextArea] private string transitionMessage;

    [Header("Next Scene Settings")]
    [SerializeField] private string nextSceneName;

    private void Start()
    {
        if (messageText != null && !string.IsNullOrEmpty(transitionMessage))
        {
            messageText.text = transitionMessage;
        }
    }

    public void ProceedToNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("No se definio la siguiente escena en el inspector.");
        }
    }
}
