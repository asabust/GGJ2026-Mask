using UnityEngine;
using Game.Runtime.Core;
using Game.Runtime.Data;

public class MenuInteractable : Interactable
{
    [SerializeField] private AnomalySO anomalySO;
    private bool completed;

    public override void Interact()
    {
        if (completed) return;
        if (anomalySO == null) return;

        // 先写其他交互逻辑.......比如......
        completed = true;

        EventHandler.CallAnomalyCompletedEvent(anomalySO.anomalyName.ToString());
        EventHandler.CallFragmentCollectedEvent(anomalySO.fragmentName.ToString());
    }
}
