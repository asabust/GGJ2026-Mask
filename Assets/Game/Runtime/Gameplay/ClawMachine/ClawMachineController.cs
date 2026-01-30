using System.Collections;
using Game.Runtime.Core;
using UnityEngine;

public class ClawMachineController : MonoBehaviour
{
    [Tooltip("爪子中心，用来做抓取判定")] public Transform claw;

    [Header("抓移动速度")] public float moveSpeed = 3f;
    public float dropSpeed = 4f;
    public float riseSpeed = 4f;
    [Tooltip("抓取半径")] public float grabRadius = 0.6f;
    [Header("抓取成功率")] [Range(0f, 1f)] public float baseGrabChance = 0.35f;
    [Tooltip("可抓取娃娃的 Layer")] public LayerMask dollLayer;

    [Header("娃娃机范围")] public float minX = -8f;
    public float maxX = 8f;
    public float minY = -6f;
    public float maxY = 6f;

    // 当前抓住的娃娃
    private Doll grabbedDoll;
    private ClawMachinePanel clawMachinePanel;

    private const string clawOpen = "ClawOpen";
    private const string clawClose = "ClawClose";
    private Animation clawAnimation;
    private bool isGrabbing = false;


    private enum ClawState
    {
        Waiting, //显示说明，等待按开始
        Moving,
        Grabbing
    }

    private ClawState state = ClawState.Waiting;
    private Vector3 startPos;

    private void Start()
    {
        startPos = claw.position;
        clawAnimation = GetComponentInChildren<Animation>();
        clawMachinePanel = UIManager.Instance.Open<ClawMachinePanel>(this);
        clawMachinePanel.grabButton.onClick.AddListener(OnGrabPressed);
    }

    private void Update()
    {
        if (isGrabbing) return;
        switch (state)
        {
            case ClawState.Moving:
                HandleMove();
                break;

            case ClawState.Grabbing:
                HandleGrab();
                break;
        }
    }

    public void StartGame()
    {
        state = ClawState.Moving;
    }

    public void StopGame()
    {
    }

    /// <summary>
    /// 在“抓取”按钮被点击时调用
    /// </summary>
    public void OnGrabPressed()
    {
        if (state != ClawState.Moving) return;
        state = ClawState.Grabbing;
    }


    #region 抓娃娃机流程

    private void HandleMove()
    {
        var pos = claw.position;

        if (clawMachinePanel.moveButton.IsHolding)
            pos.x += moveSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        claw.position = pos;
    }

    private void HandleGrab()
    {
        StartCoroutine(DropAndGrab());
    }

    private IEnumerator DropAndGrab()
    {
        isGrabbing = true;

        // 1. 下落
        while (claw.position.y > minY)
        {
            claw.position += Vector3.down * (dropSpeed * Time.deltaTime);
            yield return null;
        }

        // 2. 抓取动画
        yield return PlayAnimationAndWait(clawClose);

        TryGrab();

        // 3. 上升
        while (claw.position.y < maxY)
        {
            claw.position += Vector3.up * (riseSpeed * Time.deltaTime);
            yield return null;
        }

        //4. 返回初始位置
        while (Vector3.Distance(claw.position, startPos) > 0.05f)
        {
            claw.position = Vector3.MoveTowards(claw.position, startPos,
                moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 5. 张爪动画
        if (grabbedDoll) grabbedDoll.transform.localPosition += Vector3.down * (dropSpeed * Time.deltaTime);
        yield return PlayAnimationAndWait(clawOpen);

        ReleaseAtExit();


        isGrabbing = false;
    }

    public void ReleaseAtExit()
    {
        state = ClawState.Waiting;
        clawMachinePanel.ReStart();


        if (!grabbedDoll) return;
        grabbedDoll.OnReleasedAtExit();
        grabbedDoll = null;
    }

    #endregion


    #region 抓娃娃逻辑

    private void TryGrab()
    {
        if (grabbedDoll) return;

        var target = FindBestDoll();
        if (!target)
        {
            Debug.Log("没有碰到娃娃");
            return;
        }

        var success = CalculateGrabSuccess(target);
        if (success)
        {
            AttachDoll(target);
            Debug.Log($"抓取成功: {target.name}");
        }
    }

    private Doll FindBestDoll()
    {
        var hits = Physics2D.OverlapCircleAll(claw.position, grabRadius, dollLayer);

        if (hits.Length == 0)
            return null;

        Doll best = null;
        var bestScore = float.MaxValue;

        foreach (var hit in hits)
        {
            var doll = hit.GetComponent<Doll>();
            if (doll == null) continue;

            // 找最近的娃娃
            var score = Vector2.Distance(claw.position, doll.transform.position);
            if (score < bestScore)
            {
                bestScore = score;
                best = doll;
            }
        }

        return best;
    }

    private bool CalculateGrabSuccess(Doll doll)
    {
        var distance = Vector2.Distance(claw.position, doll.transform.position);
        var positionBonus = 1f - Mathf.Clamp01(distance / grabRadius);

        var finalChance = Mathf.Clamp01(baseGrabChance * positionBonus);

        Debug.Log($"最后概率: {finalChance:F2} 距离={distance} 距离分={positionBonus}");

        return Random.value < finalChance;
    }

    private void AttachDoll(Doll doll)
    {
        grabbedDoll = doll;

        doll.OnGrabbed();
        doll.transform.SetParent(claw);
        doll.transform.localPosition = Vector3.zero;
    }

    private void ForceDrop()
    {
        if (grabbedDoll == null)
            return;

        grabbedDoll.OnForceDropped();
        grabbedDoll = null;
    }

    #endregion

    #region 辅助方法

    private IEnumerator PlayAnimationAndWait(string animName)
    {
        clawAnimation.Play(animName);
        yield return new WaitForSeconds(clawAnimation[animName].length);
    }

    private void OnDrawGizmosSelected()
    {
        if (!claw) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(claw.position, grabRadius);
    }

    #endregion
}