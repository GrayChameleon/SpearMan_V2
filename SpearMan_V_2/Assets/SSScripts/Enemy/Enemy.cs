using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public float walkSpeed;
    public float runSpeed;
    int faceingDirection;

    bool isGrounded;
    float checkRadius = 0.4f;
    public Transform feetPos;
    public LayerMask whatIsGround;

    public Transform nextStepPos;
    float checkStepRadius = 0.2f;

    public Transform spearSpawnPoint;
    public GameObject spearPrefab;
    GameObject spear;

    public float sightDystans;
    public float rememberTime;
    float rememberTimeValue;
    public LayerMask whatIsPlayer;

    public float relodTime;
    float relodTimeValue;


    // Use this for initialization
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        faceingDirection = 1;           //He's faceing right
        relodTimeValue = relodTime;
        rememberTimeValue = rememberTime;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (relodTime >= 0)
        {
            relodTime -= Time.deltaTime;
        }
        if (rememberTime >= 0)
        {
            rememberTime -= Time.deltaTime;
        }
    }

    public void restRelTime()
    {
        relodTime = relodTimeValue;
    }

    public void resetRememberTime()
    {
        rememberTime = rememberTimeValue;
    }

    //patrol from edge to edge
    public void patrol()
    {
        if (isGrounded)
        {
            if (CanMoveForward())
            {
                moveForward();
            }
            else
            {
                turnAround();
                moveForward();
            }
        }
    }

    public void moveForward()
    {
        rb2D.velocity = new Vector2(walkSpeed * faceingDirection, rb2D.velocity.y);
    }

    public void runForward()
    {
        rb2D.velocity = new Vector2(runSpeed * faceingDirection, rb2D.velocity.y);
    }

    public void moveToEdge()
    {
        if (CanMoveForward())
        {
            moveForward();
        }
    }

    public void RunToEdge()
    {
        if (CanMoveForward())
        {
            runForward();
        }
    }

    public void turnAround()
    {
        if (faceingDirection == -1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            faceingDirection = 1;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            faceingDirection = -1;
        }
    }

    public bool CanMoveForward()
    {
        if (Physics2D.OverlapCircle(nextStepPos.position, checkStepRadius, whatIsGround))
            return true;
        else
            return false;
    }

    public bool canAttackPlayer()
    {
        Debug.DrawRay(spearSpawnPoint.position, new Vector2(faceingDirection, 0), Color.red, 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(spearSpawnPoint.position, new Vector2(faceingDirection, 0), sightDystans, whatIsPlayer);
        if (hit.collider != null)
        {
            resetRememberTime();
            return true;
        }
        else
            return false;
    }

    public void thowrSpear()
    {
        spear = Instantiate<GameObject>(spearPrefab, spearSpawnPoint.position, Quaternion.identity);
        spear.GetComponent<spear>().direction = faceingDirection;
        restRelTime();
    }

}