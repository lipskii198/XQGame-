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
            //if object touches player it starts rage time 
            collision.GetComponent<PlayerController>().RageUP(rageDuration, ShootingSpeedReduce);
            gameObject.SetActive(false);
        }
    }
}