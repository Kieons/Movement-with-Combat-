using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public variables
    [Range(1, 10)]
    public float jumpVelocity;
    public Collider2D PlayerCollider;
    [SerializeField] private LayerMask m_WhatIsGround;
    public Transform grounded;
    public float fallMultiplayer;
    public float lowJumpMultiplier;
    [Range(1, 10)] public float speed;
    public float maximumMomentum;

    //Animations
    public Animator animator;
    private float timeSinceLast;


    //private variables
    private float k_GroundedRadius = .1f;
    private bool m_Grounded;
    private Rigidbody2D rb;
    private float momentum;
    public int JumpDirection;
    public int JumpDirectionAllTime;
    private int totalColiders;
    private float MovementSmoothing = .05f;
    private Vector2 targetVelocity;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }




    private void Update()
    {

        LeftRightMovement(Input.GetAxisRaw("Horizontal"));
        jumping();
        betterJumping();
        downAir();
        AnimationHandeling();

    }

    private void AnimationHandeling()
    {
        if (m_Grounded && Input.GetAxisRaw("Horizontal") == 0)
        {
            timeSinceLast += Time.deltaTime;
            if (timeSinceLast > 10f)
            {
                animator.SetBool("Idol", true);
            }

        }
        else
        {
            animator.SetBool("Idol", false);

            timeSinceLast = 0;
        }
        if (JumpDirectionAllTime == 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (JumpDirectionAllTime == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
            Debug.Log("turn of animation");
        }
        if (!m_Grounded)
        {
           animator.SetBool("Jumping", true);
        }
        else
        {
            animator.SetBool("Jumping", false);
        }
    }

    private void betterJumping()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplayer - 1) * Time.deltaTime;

        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    private void jumping()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(grounded.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;



            }
        }
        totalColiders = colliders.Length;
        if (totalColiders == 0)
        {
            m_Grounded = false;
        }

        if (Input.GetButtonDown("Jump") && m_Grounded)
        {
            rb.velocity = Vector2.up * jumpVelocity;


        }
        if (rb.velocity.x != 0f)
        {
            if (rb.velocity.x < 0f)
            {
                JumpDirection = 0;
            }
            else
            {
                JumpDirection = 1;
            }
        }
        else
        {
            JumpDirection = 2;
        }
    }
    private void LeftRightMovement(float moving)
    {
        //Checking left and right when moving
        if (moving != 0f)
        {
            if (moving < 0f)
            {
                JumpDirectionAllTime = 0;

            }
            else
            {
                JumpDirectionAllTime = 1;
            }
        }
        else
        {
            JumpDirectionAllTime = 2;
        }

        if (m_Grounded)
        {
            if (momentum < maximumMomentum)
            {
                momentum += 0.005f;
            }
            rb.velocity = new Vector2(moving * speed * momentum, rb.velocity.y);
        }
        else
        {

            if ((JumpDirectionAllTime != JumpDirection) && JumpDirection != 2f)
            {
                targetVelocity = new Vector2(rb.velocity.x + moving * momentum * 0.9f, rb.velocity.y);
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, MovementSmoothing);
            }
            else
            {
                targetVelocity = new Vector2(moving * speed * momentum, rb.velocity.y);
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, MovementSmoothing);
            }
        }
        if (moving == 0)
        {
            momentum = 1;
        }

    }

    private void downAir()
    {
        if (Input.GetKeyDown("s"))
        {
            rb.velocity = new Vector2(rb.velocity.x, -5f);
        }
    }

}
