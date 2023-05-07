using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    //I want these lights to be similar to the ones in katana zero. They might be reactive as well i.e. change when triggered by some event
    //https://www.youtube.com/watch?v=x3qZ05kW_go <- Katana zero disco
    [SerializeField] Color lightColor;
    private SpriteRenderer spriteR;

    private float timer;
    [SerializeField] private int flickerNum = 6; //Even number to return to previous state, odd number to change state
    private int flickeringNum; //Used to countdown, "instance" of flickerNum
    [SerializeField] private float maxTime;
    [SerializeField] private float minTime;

    private bool flickerTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteR.material.SetColor("_Color", lightColor);
        timer = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        flickerTrigger();
        if (flickerTriggered) flicker(spriteR);
        
    }

    void LateUpdate()
    {
        
    }

    void flicker(SpriteRenderer spR) {
        flickeringNum--;
            Color color = spriteR.material.color;
            if (color.a == 255f)
            {
                color.a = 0f;
            }
            else
            {
                color.a = 255f;
            }
            spriteR.material.SetColor("_Color", color);
            if (flickeringNum == 0) {
                flickerTriggered = false;
            }
    }

    void flickerTrigger() {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        if (timer < 0) {
            flickeringNum = flickerNum;
            timer = Random.Range(minTime, maxTime);
            if (!flickerTriggered) {
                flickerTriggered = true;
            } 
        }
    }

    
}
