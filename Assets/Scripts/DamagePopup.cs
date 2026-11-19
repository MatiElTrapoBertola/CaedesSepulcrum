using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;

    private float moveSpeed = 2f;
    private float lifetime = 1f;

    public void Setup(float damage)
    {
        textMesh.text = Mathf.RoundToInt(damage).ToString();
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        lifetime -= Time.deltaTime;

        if(lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
