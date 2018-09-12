using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spear : MonoBehaviour
{
    public float speed;
    public float direction;
    Rigidbody2D rb2D;

    public float spearDamage;
    // Use this for initialization
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if (direction == -1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void FixedUpdate()
    {
        if (direction == -1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        rb2D.velocity = new Vector2(speed * direction, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IdamageAble>() != null)
        {
            collision.gameObject.GetComponent<IdamageAble>().takeDamage(spearDamage);
        }

        //spawn particle system (destroing spear)


        Destroy(this.gameObject);
    }
}
