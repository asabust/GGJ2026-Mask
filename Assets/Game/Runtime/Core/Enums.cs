namespace Game.Runtime.Core
{
    public enum GamePhase
    {
        GameOpening, //开场剧情
        GameTitle, // 游戏标题界面
        Gameplay, // 正常游戏中（可移动、可交互）
        Dialogue, // 对话中（不能移动）
        Cutscene, // 演出中（不能移动）
        GameOver // 游戏结束
    }

    public enum MaskState
    {
        MaskOn, // 戴上面具（异常画面）
        MaskOff // 摘下面具（恢复正常）
    }

}