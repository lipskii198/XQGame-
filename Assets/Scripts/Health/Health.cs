using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; } //can get it anywhere but can only modify it in this script

    [Header("IFrame")]//deals with invincibility frame 
    [SerializeField] private float iFrameDuration;
    [SerializeField] private float numberOfFlashes;
    private SpriteRenderer spriteRend; 
    void Awake()
    {
        currentHealth = startingHealth;
        spriteRend = GetComponent<SpriteRenderer>(); 

    }

    // Update is called once per frame
    public void TakeDamage(float _damage)
    {
        //subtracts damage and checks is health is >0 and <maxHealth
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            //take damage 
            //becomes invincible and flashes red 
            StartCoroutine(Invulnerability());
        }
        else
        {
            //player dead 
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth +  _value, 0, startingHealth);
    }
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true); // layer 10(player) ignores things from layer 11(enemy)
        //wait time before turning collision back on
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0,0.5f); //turn player sprite red, 0.5 is to make it a little bit transparent  
            yield return new WaitForSeconds(iFrameDuration/(numberOfFlashes*2));//waits half the time of invincibility/nflash 
            spriteRend.color = Color.white; // turn player back white
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));//waits  
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}
