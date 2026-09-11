using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class FireTrap : MonoBehaviour
{
    [Header("TrapDamage")]
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;
    private bool activated;

    private bool triggered; //cuando la trampa se triggeree
    private bool active; //cuando la trampa se active

    private void Awake()
    {
       anim = GetComponent<Animator>();
       spriteRend = GetComponent<SpriteRenderer>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
            if(active)
                collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        spriteRend.color = Color.red;//change color to red to advice the player

        //delay auntil the trap activates again
        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white;//change color to white
        active = true;
        anim.SetBool("activated", true);
      
        //lo que tarda la trampa en activarse desde que el player la toca, y luego se desactiva
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
