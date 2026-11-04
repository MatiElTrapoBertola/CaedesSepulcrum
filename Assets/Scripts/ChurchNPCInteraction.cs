using UnityEngine;

public class ChurchNPCInteraction : MonoBehaviour
{
    [Header("UI del Menu de Habilidades")]
    [SerializeField] private GameObject skillShopUI;
    [SerializeField] private float interactionDistance = 3.0f;

    private Transform playerTransform;
    private MonoBehaviour playerMovementScript;
    private MonoBehaviour playerCameraScript;
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

            // Buscamos el script de movimiento en el objeto Player (por ejemplo: PlayerMovement3D)
            // Cambia "PlayerMovement3D" por el nombre exacto de tu script de movimiento si es diferente
            playerMovementScript = player.GetComponent<MonoBehaviour>(); // O usa GetComponent específico si lo sabes

            // Buscamos la cámara del jugador (suele estar en el hijo PlayerCamera)
            Transform playerCameraObj = player.transform.Find("PlayerCamera");
            if (playerCameraObj != null)
            {
                // Buscamos cualquier script de rotación o control en la cámara
                playerCameraScript = playerCameraObj.GetComponent<MonoBehaviour>();
            }
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

        // Si está cerca y presiona 'F'
        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.F))
        {
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

            // Desactivamos los scripts de control para congelar al jugador y la vista
            SetPlayerControlsActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Devolvemos el control
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

    private void SetPlayerControlsActive(bool isActive)
    {
        // Desactiva o activa los scripts de movimiento y combate para que no reaccionen al teclado/mouse
        if (playerTransform != null)
        {
            // Desactiva los scripts del jugador (puedes ajustar según los componentes que tenga tu personaje)
            MonoBehaviour[] scripts = playerTransform.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                // Evitamos desactivar este mismo script u otros esenciales de UI/Salud
                if (script != this && script.GetType().Name != "Health")
                {
                    script.enabled = isActive;
                }
            }
        }
    }
}