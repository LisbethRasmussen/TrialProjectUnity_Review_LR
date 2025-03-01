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
    public float IdleRangeDetection = 10f;
    public LayerMask PlayerLayer;

    [Header("Patrol")]
    public float PatrolingRangeDetection = 5f;
    public Transform[] PossiblePatrolPoints;
    public GameObject PatrolIcon;

    [Header("Chasing")]
    public float MaxChaseRange = 6f;
    public float AttackRange = 1f;
    public GameObject ChasingIcon;

    [Header("Attacking")]
    public float AttackCooldown = 1f;
    public float AttackDamage = 1f;
    public GameObject AttackingIcon;

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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, PatrolingRangeDetection);
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

    public void Move(Vector3 deltaPosition)
    {
        if (Physics2D.OverlapCircle(transform.position + deltaPosition, 0.1f, WallLayer) == null)
        {
            transform.position += deltaPosition;
            EnemySprite.transform.up = deltaPosition.normalized;
        }
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

        switch (_currentState)
        {
            case IdleEnemyState _:
                PatrolIcon.SetActive(false);
                ChasingIcon.SetActive(false);
                AttackingIcon.SetActive(false);
                break;
            case ChaseEnemyState _:
                PatrolIcon.SetActive(false);
                ChasingIcon.SetActive(true);
                AttackingIcon.SetActive(false);
                break;
            case PatrolEnemyState _:
                PatrolIcon.SetActive(true);
                ChasingIcon.SetActive(false);
                AttackingIcon.SetActive(false);
                break;
            case AttackEnemyState _:
                PatrolIcon.SetActive(false);
                ChasingIcon.SetActive(false);
                AttackingIcon.SetActive(true);
                break;
        }
    }
}
