using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]private float duration;
    private bool hit;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private float direction;
    private float lifeTime; //so a fireball doesnt get lost if stuck somewhere 
    // Start is called before the first frame update
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //if it hits we dont do anything otherwise it keeps moving 
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime*direction;
        transform.Translate(movementSpeed, 0, 0);

        //increase the time a fireball has been active and deactivates them
        lifeTime += Time.deltaTime;
        if (lifeTime > duration)
            gameObject.SetActive(false);
    }
    //if it hits something it explodes 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explosion");
    }
    public void SetDirection(float _direction)
    {
        lifeTime = 0; // when we set direction(so shoot) the lifetime is 0
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
