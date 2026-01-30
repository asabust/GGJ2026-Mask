using System;
using Game.Runtime.Core;
using Game.Runtime.Core.Attributes;
using UnityEngine;
using EventHandler = Game.Runtime.Core.EventHandler;


public class GameManager : Singleton<GameManager>
{
    public Player player;
    [SceneName] public string titleScene;
    [SceneName] public string openingScene;
    [SceneName] public string clawMachineScene;
    public GamePhase CurrentPhase { get; private set; }

    private string lastGameScene;

    public void SetGamePhase(GamePhase newPhase)
    {
        CurrentPhase = newPhase;
        EventHandler.CallGamePhaseChangedEvent(newPhase);
    }

    public bool IsGameplay => CurrentPhase == GamePhase.Gameplay;

    private void Start()
    {
        player.gameObject.SetActive(false);
        GameTitle(); //从标题界面开始
    }

    /// <summary>
    /// 开始新游戏
    /// </summary>
    public void StartNewGame()
    {
        SetGamePhase(GamePhase.Gameplay);
        TransitionManager.Instance.TransitionTo(openingScene);
    }

    /// <summary>
    /// 游戏开始界面
    /// </summary>
    public void GameTitle()
    {
        SetGamePhase(GamePhase.GameTitle);
        TransitionManager.Instance.Transition(string.Empty, titleScene);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartClawMachineGame()
    {
        SetGamePhase(GamePhase.ClawMachineGame);
        player.gameObject.SetActive(false);
        lastGameScene = TransitionManager.Instance.currentSceneName;
        TransitionManager.Instance.TransitionTo(clawMachineScene);
    }

    public void ExitClawMachineGame()
    {
        SetGamePhase(GamePhase.Gameplay);
        TransitionManager.Instance.TransitionTo(lastGameScene);
    }

    private void AfterSceneLoad(string toSceneName)
    {
        player.gameObject.SetActive(CurrentPhase == GamePhase.Gameplay);
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
    }
}