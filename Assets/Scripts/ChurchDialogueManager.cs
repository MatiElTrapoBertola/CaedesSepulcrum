using UnityEngine;

public class ChurchDialogueManager : MonoBehaviour
{
    [Header("Dialogue Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] nightDialogues;

    private void Start()
    {
        if (GameManager.Instance == null)
            return;

        int currentNight = GameManager.Instance.GetCurrentNight();

        int dialogueIndex = currentNight - 1;

        if (dialogueIndex >= 0 &&
            dialogueIndex < nightDialogues.Length &&
            nightDialogues[dialogueIndex] != null)
        {
            audioSource.PlayOneShot(nightDialogues[dialogueIndex]);
        }
    }
}