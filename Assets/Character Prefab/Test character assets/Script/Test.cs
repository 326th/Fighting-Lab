using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    //PLayer Components
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D collider2D;
    //Inspector variable
    private float padding = 0.1f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float SPEED = 5f;
    [SerializeField] private float JUMP_VEL = 10f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private bool Facing_right = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        if (!Facing_right)
        {
            rb.transform.localScale = new Vector2(-1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float hdirection = Input.GetAxis("Horizontal");
        // go left
        if (hdirection < 0)
        {
            rb.velocity = new Vector2(-1 * SPEED, rb.velocity.y);
        }
        // go right
        else if (hdirection > 0)
        {
            rb.velocity = new Vector2(SPEED, rb.velocity.y);
        }
        // jump
        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D hit = Physics2D.Raycast(collider2D.bounds.center, Vector2.down, collider2D.bounds.extents.y + padding, ground);
            if (hit.collider != null) {
                rb.velocity = new Vector2(rb.velocity.x, JUMP_VEL);
            }
        }
    }
}
