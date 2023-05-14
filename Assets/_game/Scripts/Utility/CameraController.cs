//Script for Main CameraController
//Currently allows for 2 modes
//  A snappy camera that stays exactly on the player
//  A physics based dynamic camera which is smoother

using UnityEngine;

namespace _game.Scripts.Utility
{
    public class CameraController : MonoBehaviour
    {

        [SerializeField] public float yOffset = -5f;
        //private GameObject cam;
        private Rigidbody2D rb;
        private Vector3 camPos;
        public bool snappy = false;
        public float forceMultiplier = 1000f;
        public Transform player;
        private Vector2 delta;

        public float defaultCamSize = 10f;
        public float maxCamSize = 20f;

        Camera m_MainCamera;
        // Start is called before the first frame update
        void Start()
        {
            rb = this.gameObject.GetComponent<Rigidbody2D>();

            //How "draggy" the camera is. Too high and cam will barely move, too low and camera will be very erratic
            rb.drag = 10;

            rb.freezeRotation = true;

            m_MainCamera = Camera.main;
            //defaultCamSize /= 2f;

        }

        // LateUpdate is called once per frame, after Update is called
        void LateUpdate()
        {
            if (snappy)
            {
                rb.simulated = false;

                //Literally just set the cam position to be player position. 
                this.transform.position = new Vector3(player.position.x, player.position.y + yOffset, -25);

            }
            else
            {
                //Add camera back to physics sim
                rb.simulated = true;

                camPos = this.transform.position;
                

                //Use physics to move camera, giving it a more dynamic and smooth feeling
                //The main concept behind how this works is simply applying a force to the camera so that it gets closer to the player
                //The farther the camera from the player, the stronger the force applied to the camera
                delta = new Vector2(camPos.x - player.position.x, camPos.y - player.position.y + yOffset);

                //Increase camera size if it can't catch up quickly enough e.g. falling great heights
                //Since I'm not lerping, this can be kinda stuttery. Idk if I care enough about that now
                if (Mathf.Abs(camPos.y-player.position.y) > m_MainCamera.orthographicSize*0.9) {
                    if (m_MainCamera.orthographicSize < maxCamSize) m_MainCamera.orthographicSize += (camPos.y - player.position.y)*Time.deltaTime;
                } else if (m_MainCamera.orthographicSize > defaultCamSize) {
                    m_MainCamera.orthographicSize -= 10f*Time.deltaTime;
                    Debug.Log("reducing cam size");
                }



            }
        }

        void FixedUpdate()
        {
            if (!snappy) rb.AddForce(-delta * Time.deltaTime * forceMultiplier, ForceMode2D.Force);
        }
    }
}
