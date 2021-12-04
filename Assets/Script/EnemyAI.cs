using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWayPointDistance = 1f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;

    Seeker seeker;
    Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidBody2D = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 1f);
        
    }

    void UpdatePath(){
        if(seeker.IsDone()){
            seeker.StartPath(rigidBody2D.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    private void OnDrawGizmos() {
        // Gizmos.color = Color.red;
        // Debug.Log(path.vectorPath.Count);
        // Gizmos.DrawWireSphere((Vector2)path.vectorPath[currentWaypoint], 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null){
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        }else{
            reachedEndOfPath = false;
        }
        
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidBody2D.position).normalized;
        // Vector2 force = direction * speed * Time.deltaTime;

        // rigidBody2D.AddForce(force);
        rigidBody2D.MovePosition(rigidBody2D.position + direction * speed * Time.fixedDeltaTime);

        float distance = Vector2.Distance(rigidBody2D.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }
    }
}
