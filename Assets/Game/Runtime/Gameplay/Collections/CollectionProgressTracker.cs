using System.Collections.Generic;
using UnityEngine;
namespace Game.Runtime.Core
{
    public class CollectionProgressTracker : MonoBehaviour
    {
        /// <summary>
        /// CollectionProgressTracker（场景级）：收集进度管理器。
        ///
        /// 放置范围：
        /// - 场景级/关卡级单例职责组件。
        /// - 不需要挂在道具上，只用来监听事件。
        ///
        /// 工作方式：
        /// - 监听 EventHandler.AnomalyCompletedEvent(string name)（道具触发完成时广播）；
        /// - 内部用 HashSet 去重并累计已完成的收集项；
        /// - 对外提供 IsAllCollected/IsCollected 等查询，用于“点击照片时判定 Good End / Bad End”。
        ///
        /// 典型用法：
        /// - 各个道具 Controller：触发异常完成 -> CallAnomalyCompletedEvent("Doll"/"Boxing"/...)
        /// - 结局交互点：if (tracker.IsAllCollected()) 进入 Good End，否则 Bad End。
        ///
        /// TODO : 可能需要添加 DontDestroyOnLoad
        /// </summary>

        [Header("Config")]
        [SerializeField] private int requiredCount = 4;

        private readonly HashSet<string> completedNames = new HashSet<string>();

        public int CompletedCount => completedNames.Count;

        private void OnEnable()
        {
            EventHandler.AnomalyCompletedEvent += OnCollected;
        }

        private void OnDisable()
        {
            EventHandler.AnomalyCompletedEvent -= OnCollected;
        }

        private void OnCollected(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;

            // 去重：同一个收集项只算一次
            if (!completedNames.Add(name)) return;

            // 收齐逻辑（可以写收集触发逻辑；否则留空）
            if (completedNames.Count >= requiredCount)
            {
                // e.g. set a flag, enable photo interaction, etc.
            }
        }

        public bool IsCollected(string name)
        {
            return completedNames.Contains(name);
        }

        public bool IsAllCollected()
        {
            return completedNames.Count >= requiredCount;
        }

        // 提供重置进度的选项
        public void ResetProgress()
        {
            completedNames.Clear();
        }

    }
}