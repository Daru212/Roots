using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public PlayerBehaviour _player;
    public Image Healthbar;

    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance;

    public Transform enemyGFX;

    Path _path;
    int _currentWaypoint = 0;
    bool _reachedEndOfPath = false;

    Seeker _seeker;
    Rigidbody2D _rb;

    //Combat
    public int maxHealth = 100;
    int _currentHealth;

    public float attackRate = 2f;
    float _nextAttackTime = 0f;
    public int damage = 25;

    // Start is called before the first frame update
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);
        _currentHealth = maxHealth;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }
    void UpdatePath()
    {
        if (_seeker.IsDone())
            _seeker.StartPath(_rb.position, target.position, OnPathComplete);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_path == null)
             return;
        
        if(_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath = true;
            return;
        } else
        {
            _reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        _rb.AddForce(force);

        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            _currentWaypoint++;
        }

       // if (force.x >= 0.01f)
        //{
          //  enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        //}
        //else if (force.x <= 0.01f)
        //{
          //  enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        //}
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        //Play hurt animation

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void Attack()
    {
        //Play attack animation

        //Deal damage
        _player.GetComponent<PlayerBehaviour>().TakeDamage(damage);
        Debug.Log("-" + damage);
    }

    void Die()
    {
        Debug.Log("Enemy Died");
        Destroy(gameObject);
        //Die animation

        //Disable the enemy
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Time.time >= _nextAttackTime)
            {
                    Debug.Log("Enemy Swings");
                    Attack();
                    _nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }
}
