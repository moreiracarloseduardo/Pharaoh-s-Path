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
    private bool isAttacking = false;
    [SerializeField] private Collider[] weaponColliders;

    void Awake() {
        enemyStates = StateMachine<EnemyStates>.Initialize(this);
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        weaponColliders = GetComponentsInChildren<Collider>();
    }

    void Update() {
        if (isAttacking) return;

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

        if (Vector3.Distance(transform.position, player.transform.position) > attackRadius) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

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
        Debug.Log("Player morreu");
        isAttacking = true;
        EnableWeaponColliders();
        StartCoroutine(ReturnToChasingAfterDelay(1f)); 
        EventsManager_.instance.PlayerDeath();
    }
    void Attacking_Exit() {
        DisableWeaponColliders();
    }

    IEnumerator ReturnToChasingAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
        enemyStates.ChangeState(EnemyStates.Chasing);
    }
    private void EnableWeaponColliders() {
        foreach (var collider in weaponColliders) {
            collider.enabled = true;
        }
    }

    private void DisableWeaponColliders() {
        foreach (var collider in weaponColliders) {
            collider.enabled = false;
        }
    }

}
