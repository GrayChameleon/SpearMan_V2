using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spear : MonoBehaviour
{
    public float speed;
    public float direction;
    Rigidbody2D rb2D;
    public float startY_Vel;
    float x_Vel;
    float angle;

    public float spearDamage;
    // Use this for initialization
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if (direction == -1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        rb2D.velocity = new Vector2(rb2D.velocity.x, startY_Vel);

        x_Vel = Mathf.Sqrt(speed * speed - rb2D.velocity.y * rb2D.velocity.y);
        Debug.Log(x_Vel);
        angle = Mathf.Atan(rb2D.velocity.y / x_Vel) * 180 / Mathf.PI;
        Debug.Log(angle);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, angle);
    }

    private void FixedUpdate()
    {
        x_Vel = Mathf.Sqrt(speed*speed - rb2D.velocity.y*rb2D.velocity.y);
        angle = Mathf.Atan(rb2D.velocity.y / x_Vel) * 180 / Mathf.PI;
        rb2D.velocity = new Vector2(x_Vel * direction, rb2D.velocity.y);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y , angle);
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
