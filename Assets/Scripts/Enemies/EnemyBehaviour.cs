using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    #region Serialized Fields
    [Header("Speed & properties")]
    public float Speed = 2f;
    public LayerMask WallLayer;
    public GameObject EnemySprite;

    [Header("Idle")]
    public float MinTimeWaitIdleAnimation = 2f;
    public float MaxTimeWaitIdleAnimation = 5f;
    public float IdleRangeDetection = 5f;
    public LayerMask PlayerLayer;

    [Header("Chasing")]
    public float MaxChaseRange = 6f;
    public float AttackRange = 1f;

    [Header("Attacking")]
    public float AttackCooldown = 1f;
    public float AttackDamage = 1f;

    [Header("Debug")]
    [SerializeField] private bool _showDebug = false;
    #endregion

    // EnemyStates Variables
    private EnemyState _currentState;
    [HideInInspector] public IdleEnemyState IdleState;
    [HideInInspector] public ChaseEnemyState ChaseState;
    [HideInInspector] public PatrolEnemyState PatrolState;
    [HideInInspector] public AttackEnemyState AttackState;

    [HideInInspector] public Transform ChasingTransform;

    #region Gizmos
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, IdleRangeDetection);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, MaxChaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
#endif
    #endregion

    private void Start()
    {
        // Initialize EnemyStates
        IdleState = new IdleEnemyState(this);
        ChaseState = new ChaseEnemyState(this);
        PatrolState = new PatrolEnemyState(this);
        AttackState = new AttackEnemyState(this);

        ChangeState(IdleState);
    }

    private void Update()
    {
        _currentState?.Update();
    }

    public void ChangeState(EnemyState newState)
    {
        if (_showDebug)
        {
            Debug.Log($"Changing state from {_currentState?.GetType().Name} to {newState.GetType().Name}");
        }

        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    }
}
