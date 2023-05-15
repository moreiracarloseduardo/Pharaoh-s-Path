using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
public enum EnemyStates { Idle, Chasing, Attacking };

public class AnubisEnemy_ : MonoBehaviour {
    public Transform[] waypoints;
    public float speed = 2f;
    public float detectionRadius = 4f;
    public float chaseTime = 3f;
    public float attackRadius = .5f;

    private StateMachine<EnemyStates> enemyStates;
    private int currentWaypoint = 0;
    private float chaseTimer = 0f;
    private GameObject player;
    private Animator animator;

    void Awake() {
        enemyStates = StateMachine<EnemyStates>.Initialize(this);
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Update() {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= detectionRadius) {
            enemyStates.ChangeState(EnemyStates.Chasing);
        }
    }

    void Idle_Enter() {
        animator.SetTrigger("Stop");
    }

    void Idle_Update() {
        if (waypoints.Length == 0) return;

        Transform waypoint = waypoints[currentWaypoint];
        transform.position = Vector3.MoveTowards(transform.position, waypoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoint.position) < 0.1f) {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void Chasing_Enter() {
        chaseTimer = chaseTime;
        animator.SetTrigger("Run");
    }

    void Chasing_Update() {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);

        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        chaseTimer -= Time.deltaTime;
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRadius) {
            enemyStates.ChangeState(EnemyStates.Attacking);
            return;
        }

        if (chaseTimer <= 0) {
            enemyStates.ChangeState(EnemyStates.Idle);
        }
    }
    void Attacking_Enter() {
        animator.SetTrigger("Attack");
        // Log que o player foi atacado
        Debug.Log("Player morreu");
        enemyStates.ChangeState(EnemyStates.Idle);
    }

    void Attacking_Update() {
        Debug.Log("Player morreu");
    }
}
