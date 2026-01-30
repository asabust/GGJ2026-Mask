using System;
using System.Collections;
using System.Collections.Generic;
using Game.Runtime.Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class JigsawPuzzlePanel : UIPanel
{
    public Button closeButton;
    public List<PuzzlePiece> pieces;
    public float spawnRange = 300f;
    private int placedCount;

    public override void OnInit()
    {
        base.OnInit();
        closeButton?.GetComponent<Button>()?.onClick.AddListener(() => UIManager.Instance.Close<JigsawPuzzlePanel>());
    }

    public override void OnOpen(object data = null)
    {
        ScatterPieces();
    }

    private void ScatterPieces()
    {
        foreach (var piece in pieces)
        {
            piece.panel = this;
            piece.rect.anchoredPosition = Random.insideUnitCircle * spawnRange;
            piece.isPlaced = false;
            piece.canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnPiecePlaced(PuzzlePiece piece)
    {
        placedCount++;
        if (placedCount == pieces.Count)
        {
            Debug.Log("Puzzle Completed!");
            UIManager.Instance.Close<JigsawPuzzlePanel>();
        }
    }
}