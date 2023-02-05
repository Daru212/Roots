using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    

    private Rigidbody2D rb2d;
    private Animator anim;
    private bool isinvincible = false;

    public PlayerRespawn respawnScript;

    public float speed;
    public float jumpHeight;
    bool jumping = false;

    //For checking if we are on the ground
    public bool isGrounded;
    public LayerMask groundLayers;

    //player behaviour stuff

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;
    float _nextAttackTime = 0f;

    public int maxHealth = 4;
    public int _currentHealth;
    public HealthBar healthBar;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isGrounded = false;
        _currentHealth = maxHealth;
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
        
       
        //jumping
        if (Input.GetButton("Jump") && isGrounded)
        {
           if (jumping == false)
            {
                rb2d.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                //anim.SetTrigger("jump");
                StartCoroutine(jumpdelay());//delays ability to jump
                
            }                    
           
        }
        
       


        if (rb2d.velocity.y < -0.01f) {
            anim.SetTrigger("falling");
        }
        //if horizontal input detected, set animation running variable to true
        anim.SetBool("running", (Input.GetAxis("Horizontal") != 0));
        anim.SetBool("grounded", isGrounded);



        IEnumerator jumpdelay() 
        {
            jumping = true;
            yield return new WaitForSeconds(0.01f);
            jumping = false;
        }

        if (Time.time >= _nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("Leftclick");
                Attack();
                _nextAttackTime = Time.time + 0.5f;
               
                
            }
        }





    }
    //collectable logic
    private void OnCollisionEnter2D(Collision2D otherobject)
    {
       
       

        if (otherobject.gameObject.CompareTag("enemy"))
        {
            if (isinvincible == false)
            {
                print("ENEMYATTACK");
                TakeDamage(1);
                StartCoroutine(invisframes());
            }

        }


    }
    private void OnTriggerEnter2D(Collider2D otherobject)
    {
        if (otherobject.gameObject.CompareTag("Collectable"))
        {
            Destroy(otherobject.gameObject);
            print("PICKUP");
            TakeDamage(-1);
        }
        if (otherobject.gameObject.CompareTag("FallOffPoint"))
        {
            print("FAUL MCARTNEY");
            Die();
        }
        if (otherobject.gameObject.CompareTag("Finish"))
        {
            print("Quitting");
            Application.Quit();
        }
    }

    IEnumerator invisframes()
    {
        isinvincible = true;
        yield return new WaitForSeconds(1);
        isinvincible = false;
    }

    void Attack()
    {
        //Play attack anim

        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage enemy
        foreach (Collider2D enemy in hitEnemies)
        {
            print("We hit " + enemy.name);
            Destroy(enemy.gameObject);
        }
    }



    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        healthBar.SetHealth(_currentHealth);

        

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    //for dying LOL
    void Die()
    {
        print("Died");
        
        respawnScript.Respawn();



    }



    private void OnDrawGizmos()
    {
        //for debugging if the player is grounded or not
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - 2.5f), new Vector2(1, 0.01f));
    }
    public void Respawn()
    {
        TakeDamage(-maxHealth);
        anim.Play("Idle");
    }


}
