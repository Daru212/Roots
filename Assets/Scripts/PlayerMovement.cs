using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpHeight;
    private Rigidbody2D rb2d;

    Vector2 _move;

    //For checking if we are on the ground
    public bool isGrounded;
    public LayerMask groundLayers;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isGrounded = false;

    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //checks if the player is grounded by cretaing an overlap area checking for the ground layer
        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 1f, transform.position.y - 2.5f),
                                new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), groundLayers);
        

        //basic movement and jumping
        rb2d.velocity = (new Vector2(Input.GetAxis("Horizontal") * speed, rb2d.velocity.y));

        if (rb2d.velocity.y > 30)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 30);

        //flips direction

       

        if ((Input.GetAxis("Horizontal") > 0.01f))
            transform.localScale = Vector3.one;
        else if ((Input.GetAxis("Horizontal") < -0.01f))
            transform.localScale = new Vector3(-1, 1, 1);


        if (Input.GetButton("Jump") && isGrounded )
        {
            //rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
            rb2d.AddForce (Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }




    }
    private void OnDrawGizmos()
    {
        //for debugging if the player is grounded or not
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - 2.5f), new Vector2(1, 0.01f));
    }


}
