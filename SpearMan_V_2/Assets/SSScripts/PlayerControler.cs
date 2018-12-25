using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour, IdamageAble
{

    Vector2 AxisInput;
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
    public DistanceJoint2D rope;
    bool isSwinging = false;
    public float swingForse;
    public float swingBreakingForse;
    public float ropeClimbSpeed;
    public SpriteRenderer playerSprite;


    [Range(0f, 5f)] public float wallColliderOffset;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        AxisInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (isSwinging)
        {
            //swing animation control
            if (AxisInput.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, -1 * Mathf.Asin((transform.position.x - rope.connectedAnchor.x) / rope.distance) * 180f / Mathf.PI);
                faceingDir = -1;
            }
            else if (AxisInput.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, Mathf.Asin((transform.position.x - rope.connectedAnchor.x) / rope.distance) * 180f / Mathf.PI);
                faceingDir = 1;
            }
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                AxisInput.x = 0;
            }
        }
        else
        {
            //moveing animation control
            if (AxisInput.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, transform.eulerAngles.z);
                playerAnimator.SetBool("isMoveing", true);
                faceingDir = -1;
            }
            else if (AxisInput.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
                playerAnimator.SetBool("isMoveing", true);
                faceingDir = 1;
            }
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                AxisInput.x = 0;
                playerAnimator.SetBool("isMoveing", false);
            }

            playerAnimator.SetFloat("speed", AxisInput.x * speed);
        }
        if (Input.GetButtonDown("Jump") && (isGrounded || isSwinging))
        {
            jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
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
        if (Input.GetKeyDown(KeyCode.Z) && isGrounded == false && isSwinging == false)
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
            stopSwinging();
        }

        if (Physics2D.BoxCast(transform.position, new Vector2(0f + wallColliderOffset, 1f + wallColliderOffset), 0, new Vector2(faceingDir, 0), wallColliderOffset, whatIsGround) == true)
        {
            //potensial wall jump implementatotion
        }

        else
        {
            if (isSwinging)
            {
                swingMovement();

                // playerSprite.transform.eulerAngles = new Vector3(playerSprite.transform.eulerAngles.x, playerSprite.transform.eulerAngles.y, Mathf.Atan((rope.anchor.x / rope.anchor.y)) * 180 / Mathf.PI);
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
        rb2D.velocity = new Vector2(AxisInput.x * speed, rb2D.velocity.y);
    }

    void falling()
    {

        if (rb2D.velocity.y < -3f)
        { isFalling = true; }
        else
        { isFalling = false; }

        if (rb2D.velocity.y < -25f)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, -25f);
        }

    }

    void swingMovement()
    {
        if (AxisInput.x < 0)
        {
            rb2D.AddForce(new Vector2(swingForse * -1f, 0));
        }
        else if (AxisInput.x > 0)
        {
            rb2D.AddForce(new Vector2(swingForse, 0));
        }
        if (AxisInput.x == 0)
        {
            swingBrakeing();
        }

        //angle
        if (faceingDir == 1)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, Mathf.Asin((transform.position.x - rope.connectedAnchor.x) / rope.distance) * 180f / Mathf.PI);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, -1 * Mathf.Asin((transform.position.x - rope.connectedAnchor.x) / rope.distance) * 180f / Mathf.PI);
        }

        //climbing
        rope.distance -= AxisInput.y * ropeClimbSpeed;

    }

    void swingBrakeing()
    {
        if (rb2D.velocity.x < 0.1 && rb2D.velocity.x > -0.1)
        {

        }
        else if (rb2D.velocity.x > 0.1)
        {
            rb2D.AddForce(new Vector2(swingBreakingForse * -1f, 0));
        }
        else
        {
            rb2D.AddForce(new Vector2(swingBreakingForse, 0));
        }
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
            Destroy(this.gameObject);
        }
    }

    void throwHook()
    {
        hook.GetComponent<GraplingHook>().findNearestGrabPlace();
        rope.connectedAnchor = hook.GetComponent<GraplingHook>().nearestGrabPlace.transform.position;
        rope.distance = Vector2.Distance(transform.position, hook.GetComponent<GraplingHook>().nearestGrabPlace.transform.position);


        startSwinging();
    }

    void startSwinging()
    {
        // here set rotation (head point grapplac
        rb2D.freezeRotation = false;
        isSwinging = true;
        rope.enabled = true;
        playerAnimator.SetBool("isMoveing", false);
        playerAnimator.SetFloat("speed", 0f);
    }


    void stopSwinging()
    {
        isSwinging = false;
        rope.enabled = false;
        rb2D.freezeRotation = true;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

    }
}