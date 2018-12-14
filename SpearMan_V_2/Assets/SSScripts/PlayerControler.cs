using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour, IdamageAble
{

    float AxisHor;
    [Range(10, 20)] public float speed;
    public float jumpForce;
    public Rigidbody2D rb2D;
    Vector2 velocity;
    bool isGrounded;
    bool isFalling;
    public Transform feetPos;
    float checkRadius = 0.4f;
    public LayerMask whatIsGround;
    public Animator playerAnimator;

    public float hp;
    public float damage;


    int faceingDir = 1; //he's faceing right

    public Transform spearSpawnPoint;
    public GameObject spearPrefab;
    GameObject spear;

    public GameObject shield;
    public float shieldSpeedToSpeedRatio;

    public GameObject hook;
    public HingeJoint2D rope;
    JointMotor2D motor2D;
    bool isSwinging = false;
    public float swingSpeed;



    [Range(0f, 5f)] public float wallColliderOffset;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        AxisHor = Input.GetAxis("Horizontal");
        if (AxisHor < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            playerAnimator.SetBool("isMoveing", true);
            faceingDir = -1;
        }
        else if (AxisHor > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            playerAnimator.SetBool("isMoveing", true);
            faceingDir = 1;
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            AxisHor = 0;
            playerAnimator.SetBool("isMoveing", false);
        }

        playerAnimator.SetFloat("speed", AxisHor * speed);

        if (Input.GetButtonDown("Jump") && (isGrounded || isSwinging))
        {
            jump();
        }

        if ((Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Fire1")) && Input.GetKey(KeyCode.C) == false)
        {
            throwSpear();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            activeShield(true);
            speed *= shieldSpeedToSpeedRatio;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            activeShield(false);
            speed /= shieldSpeedToSpeedRatio;
        }
        if (Input.GetKeyDown(KeyCode.Z) && isGrounded == false)
        {
            throwHook();
        }
        Debug.DrawLine(transform.position, hook.GetComponent<GraplingHook>().nearestGrabPlace.transform.position);
    }

    private void FixedUpdate()
    {


        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        if (isGrounded)
        {
            isSwinging = false;
        }

        if (Physics2D.BoxCast(transform.position, new Vector2(0f + wallColliderOffset, 1f + wallColliderOffset), 0, new Vector2(faceingDir, 0), wallColliderOffset, whatIsGround) == true)
        {
            //potensial wall jump implementatotion
        }

        else
        {
            if (isSwinging)
            {
                //swingMovement();
            }
            else
            {
                movement();
            }
        }


        falling();

    }

    void movement()
    {
        rb2D.velocity = new Vector2(AxisHor * speed, rb2D.velocity.y);
    }

    void falling()
    {

        if (rb2D.velocity.y < -3f)
        { isFalling = true; }
        else
        { isFalling = false; }

        if (rb2D.velocity.y < -30f)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, -30f);
        }

    }

    void swingMovement()
    {

        motor2D.motorSpeed = -AxisHor * swingSpeed;
        motor2D.maxMotorTorque = rope.motor.maxMotorTorque;
        rope.motor = motor2D;


    }

    void throwSpear()
    {
        spear = Instantiate<GameObject>(spearPrefab, spearSpawnPoint.position, Quaternion.identity);
        spear.GetComponent<spear>().direction = faceingDir;
        spear.GetComponent<spear>().spearDamage = damage;
    }

    void jump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        if (isSwinging)
        {
            stopSwinging();
        }
    }

    void activeShield(bool active)
    {
        shield.SetActive(active);
    }

    public void takeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Debug.Log("dead");
        }
    }

    void throwHook()
    {
        hook.GetComponent<GraplingHook>().findNearestGrabPlace();
        rope.anchor = hook.GetComponent<GraplingHook>().nearestGrabPlace.transform.position - transform.position;
        rope.connectedAnchor = hook.GetComponent<GraplingHook>().nearestGrabPlace.transform.position;
        isSwinging = true;
        rope.enabled = true;
    }

    void stopSwinging()
    {
        isSwinging = false;
        rope.enabled = false;
    }
}