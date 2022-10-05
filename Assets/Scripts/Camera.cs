//Script for Main Camera
//Currently allows for 2 modes
//  A snappy camera that stays exactly on the player
//  A physics based dynamic camera which is smoother





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    //private GameObject cam;
    private Rigidbody2D rb;
    private Vector3 camPos;
    public bool snappy = false;
    public float forceMultiplier = 1000f;
    public Transform player;
    private Vector2 delta;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();

        //How "draggy" the camera is. Too high and cam will barely move, too low and camera will be very erratic
        rb.drag = 10;

        rb.freezeRotation = true;
        
    }

    // LateUpdate is called once per frame, after Update is called
    void LateUpdate()
    {
        if (snappy) {
            rb.simulated = false;
            
            //Literally just set the cam position to be player position. 
            this.transform.position = new Vector3(player.position.x, player.position.y, -10);
            
        } else {
            //Add camera back to physics sim
            rb.simulated = true;
            
            camPos = this.transform.position;
            
            //Use physics to move camera, giving it a more dynamic and smooth feeling
            //The main concept behind how this works is simply applying a force to the camera so that it gets closer to the player
            //The farther the camera from the player, the stronger the force applied to the camera
            delta = new Vector2(camPos.x - player.position.x, camPos.y - player.position.y);
            


        }
    }

    void FixedUpdate()
    {
        if (!snappy) rb.AddForce(-delta * Time.deltaTime * forceMultiplier, ForceMode2D.Force);
    }
}
