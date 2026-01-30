using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTitle : MonoBehaviour
{
    public Button StartButton;

    private void Start()
    {
        StartButton?.onClick.AddListener(() => GameManager.Instance.StartNewGame());
    }
}