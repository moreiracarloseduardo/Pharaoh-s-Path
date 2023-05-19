using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
public enum EnemyStates { Idle, Chasing, Attacking };

public class AnubisEnemy_ : MonoBehaviour {
    public Transform[] waypoints;
    public float speed = 2f;
    public float detectionRadius = 4f;
    public float attackRadius = .5f;

    private StateMachine<EnemyStates> enemyStates;
    private int currentWaypoint = 0;
    // private GameObject player;
    public Animator animator;
    private bool isAttacking = false;
    [SerializeField] private Collider[] weaponColliders;
    private DrawPath_ playerDrawing;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    void Awake() {
        enemyStates = StateMachine<EnemyStates>.Initialize(this, EnemyStates.Idle);
        // player = GameObject.FindGameObjectWithTag("Player");
        playerDrawing = Game_.instance.player_.GetComponent<DrawPath_>();
        // animator = GetComponent<Animator>();
        weaponColliders = GetComponentsInChildren<Collider>();
        if (animator == null) {
            Debug.LogError("Failed to find Animator");
        }
    }

    void Start() {
        if (playerDrawing == null) {
            Debug.LogError("Failed to find PlayerDrawing");
        }
    }

    void Update() {
        if (isAttacking || enemyStates == null) return;

        float distanceToPlayer = Vector3.Distance(Game_.instance.player_.gameObject.transform.position, transform.position);
        if (playerDrawing != null && playerDrawing.playerStates != null) {
            if (distanceToPlayer <= detectionRadius && playerDrawing.playerStates.State == PlayerStates.Drawing) {
                enemyStates.ChangeState(EnemyStates.Chasing);
            } else if (enemyStates.State == EnemyStates.Chasing && (distanceToPlayer > detectionRadius || playerDrawing.playerStates.State != PlayerStates.Drawing)) {
                enemyStates.ChangeState(EnemyStates.Idle);
            }
        }
    }

    void Idle_Enter() {
        if (animator == null) {
            Debug.LogError("Animator is null in Idle_Enter");
            return;
        }
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
        animator.SetTrigger("Run");
    }

    void Chasing_Update() {
        Vector3 directionToPlayer = (Game_.instance.player_.gameObject.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, Game_.instance.player_.gameObject.transform.position) > attackRadius) {
            transform.position = Vector3.MoveTowards(transform.position, Game_.instance.player_.gameObject.transform.position, speed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, Game_.instance.player_.gameObject.transform.position) <= attackRadius) {
            enemyStates.ChangeState(EnemyStates.Attacking);
        }
    }

    void Attacking_Enter() {
        animator.SetTrigger("Attack");
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
