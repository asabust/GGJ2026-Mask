using UnityEngine;

namespace Game.Runtime.Data
{
    [CreateAssetMenu(fileName = "AnomalySO", menuName = "Anomaly")]
    public class AnomalySO : ScriptableObject
    {
        [Header("Identity")]
        public AnomalyName anomalyName = AnomalyName.None; // 建议用 enum，避免字符串拼错

        [Header("Monitor (Normal Snapshot)")]
        public Sprite monitorNormalSprite; // 监控室/Tab：正常画面截图

        [Header("Completion")]
        [TextArea]
        public string completionHintText;  // 完成提示文本
        public FragmentName fragmentName = FragmentName.None; // 对应碎片

        [Header("Optional Presentation")]
        public Sprite abnormalSprite;      // 戴面具时：异常贴图
        public AudioClip abnormalAudio;    // 戴面具交互：异常音效
    }

    public enum AnomalyName
    {
        None,
        Doll,        // 娃娃机：背包恐怖娃娃
        Boxing,      // 拳击机：争吵/搏斗声
        FoodTruck,   // 餐车：菜单变寻人启事
        Balloon      // 气球贩子：红->黑 + 玩法触发
    }

    // 收集异常碎片占位符
    public enum FragmentName
    {
        None,
        Fragment1,
        Fragment2,
        Fragment3,
        Fragment4
    }
}
