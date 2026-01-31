using UnityEngine;
using Game.Runtime.Core;
using Game.Runtime.Data;

// 目前暂时用来测试 异常物品的流程是否可以跑通
public class MenuInteractable : Interactable
{
    [SerializeField] private AnomalySO anomalySO;
    private bool completed;
    private Sprite normalSprite;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) normalSprite = sr.sprite;
    }

    private void OnEnable()
    {
        EventHandler.MaskStateChangedEvent += OnMaskChanged;

        // 获取面具状态初始化
        if (MaskManager.Instance != null)
            OnMaskChanged(MaskManager.Instance.MaskState);
    }

    private void OnDisable()
    {
        EventHandler.MaskStateChangedEvent -= OnMaskChanged;
    }

    private void OnMaskChanged(MaskState state)
    {
        if (sr == null) return;
        if (anomalySO == null) return;

        // 面具戴上：显示 abnormalSprite；摘下：恢复正常 sprite
        if (state == MaskState.MaskOn && anomalySO.abnormalSprite != null)
            sr.sprite = anomalySO.abnormalSprite;
        else
            sr.sprite = normalSprite;
    }

    public override void Interact()
    {
        if (completed) return;
        if (anomalySO == null) return;
        // 只有戴上面具才可以交互
        if (MaskManager.Instance == null || MaskManager.Instance.MaskState != MaskState.MaskOn) return;

        completed = true;

        EventHandler.CallAnomalyCompletedEvent(anomalySO.anomalyName.ToString());
        EventHandler.CallFragmentCollectedEvent(anomalySO.fragmentName.ToString());
    }
}
