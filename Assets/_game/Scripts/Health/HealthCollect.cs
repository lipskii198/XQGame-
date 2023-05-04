using UnityEngine;

namespace _game.Scripts.Health
{
    public class HealthCollect : MonoBehaviour
    {
        [SerializeField] private float Healthvalue;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player")
            {
                collision.GetComponent<Health>().AddHealth(Healthvalue);
                gameObject.SetActive(false);
            }
        }

    }
}
