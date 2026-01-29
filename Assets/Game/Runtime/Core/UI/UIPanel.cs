using System;
using UnityEngine;


public abstract class UIPanel : MonoBehaviour
{
    public UILayer layer = UILayer.Normal;

    /// <summary>
    /// 创建UI面板时初始化
    /// 只在prefab创建的时候调用，之后会被UIManager缓存
    /// </summary>
    public virtual void OnInit()
    {
    }

    /// <summary>
    /// 打开时由UIManager调用
    /// 每次UImanager.Open<T>()打开面板都调用
    /// </summary>
    /// <param name="data">打开面板时传入的参数。为了通用统一用object，每个面板自己解析</param>
    public virtual void OnOpen(object data = null)
    {
    }

    /// <summary>
    /// 关闭的时由UIManager调用
    /// 关闭只是SetActive(false)并不会销毁面板
    /// </summary>
    public virtual void OnClose()
    {
    }
}