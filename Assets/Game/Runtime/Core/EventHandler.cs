using System;

namespace Game.Runtime.Core
{
    public static class EventHandler
    {

        public static event Action<string> BeforeSceneUnloadEvent;

        public static void CallBeforeSceneUnloadEvent(string oldScene)
        {
            BeforeSceneUnloadEvent?.Invoke(oldScene);
        }

        public static event Action<string> AfterSceneLoadEvent;

        public static void CallAfterSceneLoadEvent(string newScene)
        {
            AfterSceneLoadEvent?.Invoke(newScene);
        }

        public static event Action<GamePhase> GamePhaseChangeEvent;

        public static void CallGamePhaseChangedEvent(GamePhase gamePhase)
        {
            GamePhaseChangeEvent?.Invoke(gamePhase);
        }
        
        public static event Action<int> DialogueStartdEvent;

        public static void CallDialogueStartEvent(int dialogueID)
        {
            DialogueStartdEvent?.Invoke(dialogueID);
        }

        public static event Action<int> DialogueFinishedEvent;

        public static void CallDialogueFinishedEvent(int dialogueID)
        {
            DialogueFinishedEvent?.Invoke(dialogueID);
        }

        public static event Action<int> SelectDialogueOptionEvent;

        public static void CallSelectDialogueOptionEvent(int optionId)
        {
            SelectDialogueOptionEvent?.Invoke(optionId);
        }

        // 面具道具使用 和 收集异常 事件监听
        public static event Action<MaskState> MaskStateChangedEvent;

        public static void CallMaskStateChangedEvent(MaskState maskState)
        {
            MaskStateChangedEvent?.Invoke(maskState);
        }

        // 完成该异常
        public static event Action<string> AnomalyCompletedEvent;

        public static void CallAnomalyCompletedEvent(string anomalyName)
        {
            if (string.IsNullOrWhiteSpace(anomalyName)) return;
            AnomalyCompletedEvent?.Invoke(anomalyName);
        }

        // 收集碎片
        public static event Action<string> FragmentCollectedEvent;

        public static void CallFragmentCollectedEvent(string fragmentName)
        {
            if (string.IsNullOrWhiteSpace(fragmentName)) return;
            FragmentCollectedEvent?.Invoke(fragmentName);
        }
    }
}