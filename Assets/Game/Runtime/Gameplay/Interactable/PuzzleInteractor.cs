using System.Collections;
using System.Collections.Generic;
using Game.Runtime.Core;
using UnityEngine;

public class PuzzleInteractor : Interactable
{
    public override void Interact()
    {
        base.Interact();
        UIManager.Instance.Open<JigsawPuzzlePanel>();
    }
}