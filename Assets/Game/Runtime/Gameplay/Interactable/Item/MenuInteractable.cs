using UnityEngine;
using Game.Runtime.Core;
using Game.Runtime.Data;

// 目前暂时用来测试 异常物品的流程是否可以跑通
public class MenuInteractable : Interactable
{
    [SerializeField] private AnomalySO anomalySO;
    private bool completed;

    public override void Interact()
    {
        if (completed) { return; }
        if (anomalySO == null) { return; }

        completed = true;

        EventHandler.CallAnomalyCompletedEvent(anomalySO.anomalyName.ToString());
        EventHandler.CallFragmentCollectedEvent(anomalySO.fragmentName.ToString());
    }
}
