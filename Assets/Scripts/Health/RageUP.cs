using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageUP : MonoBehaviour
{
    [SerializeField] private float rageDuration;
    [SerializeField] private float ShootingSpeedReduce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //deactivating stops the scrips, try do disable boxcollider and the sprites
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //if object touches player it starts rage time 
            collision.GetComponent<PlayerAttack>().RageUP(rageDuration, ShootingSpeedReduce);
            gameObject.SetActive(false);
        }
    }


}

