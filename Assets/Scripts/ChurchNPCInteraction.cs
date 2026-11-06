using UnityEngine;

public class ChurchNPCInteraction : MonoBehaviour
{
    [Header("UI Priest Shop")]
    [SerializeField] private GameObject skillShopUI;
    [SerializeField] private float interactionDistance = 3.0f;
    [SerializeField] private MonoBehaviour cameraScript; // Arrastra aquí el script que mueve tu cámara

    private Transform playerTransform;
    private bool isShopOpen = false;

    private void Start()
    {
        FindPlayer();

        if (skillShopUI != null)
        {
            skillShopUI.SetActive(false);
        }
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Health playerHealth = FindFirstObjectByType<Health>();
            if (playerHealth != null)
            {
                player = playerHealth.gameObject;
            }
        }

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("ChurchNPCInteraction: No se encontró al jugador en la escena.");
        }
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            FindPlayer();
            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Tecla F presionada cerca del cura");
            ToggleShop();
        }
    }

    private void ToggleShop()
    {
        isShopOpen = !isShopOpen;

        if (skillShopUI != null)
        {
            skillShopUI.SetActive(isShopOpen);
        }

        if (isShopOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SetPlayerControlsActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SetPlayerControlsActive(true);
        }
    }

    public void CloseShopButton()
    {
        isShopOpen = false;
        if (skillShopUI != null)
        {
            skillShopUI.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetPlayerControlsActive(true);
    }

    public void ClosePanel()
    {
        CloseShopButton();
    }

    private void SetPlayerControlsActive(bool isActive)
    {
        if (playerTransform != null)
        {
            // Desactivamos/Activamos el movimiento
            PlayerMovement3D movementScript = playerTransform.GetComponent<PlayerMovement3D>();
            if (movementScript != null) movementScript.enabled = isActive;

            // Desactivamos/Activamos el combate
            PlayerCombat combatScript = playerTransform.GetComponent<PlayerCombat>();
            if (combatScript != null) combatScript.enabled = isActive;

            // Desactivamos/Activamos el script de la cámara asignado en el Inspector
            if (cameraScript != null)
            {
                cameraScript.enabled = isActive;
            }
        }
    }
}