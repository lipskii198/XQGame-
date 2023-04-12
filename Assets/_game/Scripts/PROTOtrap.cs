using _game.Scripts.Managers;
using UnityEngine;

namespace _game.Scripts
{
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
}
