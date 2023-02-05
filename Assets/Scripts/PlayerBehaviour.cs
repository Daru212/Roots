using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    //Movement
    public float runSpeed = 30f;
    Vector2 _move;
    Rigidbody2D _rb;

    //Combat
    public Animator animator;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public float attackRate = 2f;
    float _nextAttackTime = 0f;

    public int maxHealth = 100;
    public int _currentHealth;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        _move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Time.time >= _nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Leftclick");
                Attack();
                _nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_move * runSpeed * Time.fixedDeltaTime);
    }

    void Attack()
    {
        //Play attack anim

        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage enemy
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
         //   enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        healthBar.SetHealth(_currentHealth);

        //Play hurt animation

        if (_currentHealth <= 0)
        {
            //Die();
        }
    }

 

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}