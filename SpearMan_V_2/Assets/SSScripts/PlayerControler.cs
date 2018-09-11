using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
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


    int faceingDir = 1; //he's faceing right

    public Transform spearSpawnPoint;
    public GameObject spearPrefab;
    GameObject spear;


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

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jump();
        }

        if (Input.GetKeyDown(KeyCode.X)||Input.GetButtonDown("Fire1"))
        {
            throwSpear();
        }
    }

    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(AxisHor * speed, rb2D.velocity.y);

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (rb2D.velocity.y < -3f)
        { isFalling = true; }
        else
        { isFalling = false; }

        if (rb2D.velocity.y < -30f)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, -30f);
        }

    }

    void throwSpear()
    {
        spear = Instantiate<GameObject>(spearPrefab, spearSpawnPoint.position, Quaternion.identity);
        spear.GetComponent<spear>().direction = faceingDir;
    }

    void jump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        isGrounded = false;
    }







}