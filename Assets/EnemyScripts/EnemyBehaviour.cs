using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;

    Path _path;
    int _currentWaypoint = 0;
    bool _reachedEndOfPath = false;

    Seeker _seeker;
    Rigidbody2D _rb;

    //Combat
    public int maxHealth = 100;
    int _currentHealth;

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

    void Die()
    {
        Debug.Log("Enemy Died");
        //Die animation

        //Disable the enemy
    }
}
