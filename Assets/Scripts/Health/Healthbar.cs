using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Healthbar : MonoBehaviour
{ 
    //imported unityengine.ui to make the image class work 
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealthbar;
    [SerializeField] private Image currentHealthbar;
    private void Start ()
    {
        totalHealthbar.fillAmount = playerHealth.currentHealth / 10;
    }

    // Update is called once per frame
    private void Update()
    {
        currentHealthbar.fillAmount = playerHealth.currentHealth / 10; //for the health sprite 0.1 is 1 hearth, 02 hearths, etc
    }
}
