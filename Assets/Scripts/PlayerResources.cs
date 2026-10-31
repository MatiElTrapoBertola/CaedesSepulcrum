using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int materials;

    public int Materials => materials;

    public void AddMaterials(int amount)
    {
        materials += amount;

        Debug.Log("Materiales actuales: " + materials);
    }

    public bool SpendMaterials(int amount)
    {
        if (materials < amount)
            return false;

        materials -= amount;

        Debug.Log("Materiales restantes: " + materials);

        return true;    
    }


}
