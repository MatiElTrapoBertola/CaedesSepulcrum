using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image fill;
    public Image trail;

    public Gradient colorHealth;
    public float trailDelay = 0.3f;
    public float trailSpeed = 0.4f;
    private Coroutine trailCoroutine;

    private void OnEnable()
    {
        PlayerEvents.onHealthChanged.AddListener(OnHealthChanged); //Hay que poner este evento en el script del jugador para que funcione!
    }
    private void OnDisable()
    {
        PlayerEvents.onHealthChanged.RemoveListener(OnHealthChanged);
    } 
    void OnHealthChanged(float currentHealth, float maxHealth)
    {
        float percent = currentHealth / maxHealth;
        fill.fillAmount = percent;
        fill.color = colorHealth.Evaluate(percent); //Finalizar cuando se tenga el script del jugador y el evento de salud (si es que se quiere dejarla así)

        if (trailCoroutine != null)
        {
            StopCoroutine(trailCoroutine);
        }
        if (percent>=trail.fillAmount) 
            trail.fillAmount = percent;
        else        
            trailCoroutine = StartCoroutine(AnimeTraikl(percent));
        
    }
    IEnumerator AnimeTraikl(float targetAmount)
    {
        yield return new WaitForSeconds(trailDelay);
        while (trail.fillAmount > targetAmount)
        {
            trail.fillAmount -= Mathf.MoveTowards(trail.fillAmount, targetAmount, trailSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
