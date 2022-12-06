using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PROTOtrap : MonoBehaviour
{
    [SerializeField] private float damage;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerManager>().TakeDamage(damage);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
