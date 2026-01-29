using System;
using System.Collections;
using System.Collections.Generic;
using Game.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

public class OpenPuzzle : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => UIManager.Instance.Open<JigsawPuzzlePanel>());
    }
}