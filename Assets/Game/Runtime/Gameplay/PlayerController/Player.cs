using System;
using System.Collections;
using System.Collections.Generic;
using Game.Runtime.Core;
using Game.Runtime.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    [HideInInspector] public Vector2 moveDelta;
    public float moveSpeed;
    public float maxDownNavDistance;

    [Header("Interact")] public LayerMask interactLayer;
    public float interactRadius = 1.8f;

    public LayerMask obstacleLayer;
    private Collider2D playerCollider;
    private ContactFilter2D contactFilter;
    private Collider2D[] hits = new Collider2D[3];

    private NavMeshAgent agent;
    private Action onNavigationCompletedAction;

    #region InputSystem

    public Vector2 moveInputValue { get; private set; }
    public InputAction moveAction { get; private set; }
    public InputAction interactAction { get; private set; }
    public InputAction maskAction { get; private set; }

    #endregion

    #region PlayerState 角色状态机

    private PlayerStateMachine stateMachine;
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    #endregion


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "idle");
        moveState = new PlayerMoveState(this, stateMachine, "move");

        playerCollider = GetComponent<Collider2D>();
        contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.SetLayerMask(obstacleLayer);
        contactFilter.useTriggers = false;
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        interactAction = InputSystem.actions.FindAction("Interact");
        maskAction = InputSystem.actions.FindAction("Ability");

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        stateMachine.Initialize(idleState);
    }


    private void Update()
    {
        moveInputValue = moveAction.ReadValue<Vector2>();
        // 寻路过程中有键盘输入则打断寻路
        if (moveInputValue.sqrMagnitude > 0.01f && agent.hasPath) StopNavigation();

        if (moveInputValue.sqrMagnitude > 1f)
            moveInputValue = moveInputValue.normalized;


        stateMachine.currentState.Update();
        HandleInteract();
        HandleMask();
    }

    public void SetSpawnPosition(Vector3 position)
    {
        transform.position = position;
        stateMachine.Initialize(idleState);
    }

    #region 角色运动控制

    public void Move()
    {
        if (moveInputValue == Vector2.zero)
        {
            moveDelta = Vector2.zero;
            return;
        }

        var hitCount = Physics2D.OverlapCollider(playerCollider, contactFilter, hits);
        if (hitCount > 0)
        {
            var dist = playerCollider.Distance(hits[0]);
            var normal = dist.normal;
            var dot = Vector2.Dot(moveInputValue, normal);
            // 几乎正面撞墙
            if (dot > 0.9f)
            {
                moveDelta = Vector2.zero;
                return;
            }

            // 计算沿墙方向
            moveInputValue -= normal;
        }

        moveDelta = moveInputValue * (moveSpeed * Time.deltaTime);
        transform.position += (Vector3)moveDelta;
    }


    public bool IsMove()
    {
        return agent.hasPath || moveInputValue != Vector2.zero;
    }

    #endregion

    #region 寻路

    public void NavigationToPosition(Vector3 position, Action onNavigationCompleted = null)
    {
        onNavigationCompletedAction = onNavigationCompleted;
        agent.SetDestination(FindReachablePoint(position));
        StartCoroutine(WaitArrive());
    }

    private Vector3 FindReachablePoint(Vector3 clickPoint)
    {
        var downPoint = clickPoint + Vector3.down * maxDownNavDistance;
        var wall = Physics2D.OverlapPoint(clickPoint, obstacleLayer);
        if (wall) return new Vector3(clickPoint.x, wall.bounds.min.y, clickPoint.z);

        return clickPoint;
    }

    private bool HasArrived(NavMeshAgent _agent)
    {
        if (_agent.pathPending) return false;

        if (_agent.remainingDistance <= _agent.stoppingDistance)
            // 没有路径 或 已经几乎停下
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                return true;

        moveDelta = agent.desiredVelocity;
        return false;
    }

    private IEnumerator WaitArrive()
    {
        yield return new WaitUntil(() => HasArrived(agent));

        StopNavigation();
        OnNavigationCompleted();
    }

    private void OnNavigationCompleted()
    {
        onNavigationCompletedAction?.Invoke();
        onNavigationCompletedAction = null;
    }

    private void StopNavigation()
    {
        agent.ResetPath();
    }

    #endregion

    #region 道具交互

    private void HandleInteract()
    {
        if (GameManager.Instance.CurrentPhase != GamePhase.Gameplay) return;
        if (interactAction == null) return;
        if (!interactAction.WasPressedThisFrame()) return;

        Vector2 pos = transform.position;
        var hits = Physics2D.OverlapCircleAll(pos, interactRadius, interactLayer);

        Interactable best = null;
        var bestSqr = float.MaxValue;

        for (var i = 0; i < hits.Length; i++)
        {
            var it = hits[i].GetComponent<Interactable>();
            if (it == null) continue;

            var sqr = ((Vector2)it.transform.position - pos).sqrMagnitude;
            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                best = it;
            }
        }

        best?.Interact();
    }

    #endregion

    #region 面具能力
    private void HandleMask()
    {
        if (GameManager.Instance.CurrentPhase != GamePhase.Gameplay) return;
        if (maskAction == null) return;

        if (maskAction.WasPressedThisFrame())
        {
            MaskManager.Instance.ToggleMask();
        }
    }
    #endregion

    private void OnEnable()
    {
        InputSystem.actions.FindActionMap("Player").Enable();
        //EventHandler.GamePhaseChangeEvent += OnGameStateChanged;
        //EventHandler.AfterSceneLoadEvent += OnLoadNewScene;
    }

    private void OnDisable()
    {
        InputSystem.actions.FindActionMap("Player").Disable();
        //EventHandler.GamePhaseChangeEvent -= OnGameStateChanged;
        //EventHandler.AfterSceneLoadEvent -= OnLoadNewScene;
    }
}