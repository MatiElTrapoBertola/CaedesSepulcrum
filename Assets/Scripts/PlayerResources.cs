using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int materials;

    private HUD hud;

    public int Materials => materials;

    private void Start()
    {
        hud = FindFirstObjectByType<HUD>();

        if (hud != null)
        {
            hud.UpdateMaterialsUI(materials);
        }
    }

    public void AddMaterials(int amount)
    {
        materials += amount;

        if (hud != null)
        {
            hud.UpdateMaterialsUI(materials);
        }

        Debug.Log("Materiales actuales: " + materials);
    }

    public bool SpendMaterials(int amount)
    {
        if (materials < amount)
            return false;

        materials -= amount;

        if (hud != null)
        {
            hud.UpdateMaterialsUI(materials);
        }

        Debug.Log("Materiales restantes: " + materials);

        return true;
    }
}