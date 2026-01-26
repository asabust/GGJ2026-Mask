using UnityEngine;

/// <summary>
/// 交互物
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour, IInteractable
{
    
    protected Collider2D cld;
    protected SpriteRenderer sr;

    protected virtual void Start()
    {
        cld = GetComponent<Collider2D>();
        sr = transform.GetComponent<SpriteRenderer>();
    }

    public virtual void Interact()
    {
        Debug.Log($"点击了[{gameObject.name}]");
    }
    
    #region 初始化

    protected virtual bool Init()
    {

        return true;
    }

    private void OnEnable()
    {
        // EventHandler.DialogueFinishedEvent += OnFinishedDialogue;
        // EventHandler.SelectDialogueOptionEvent += OnSelectOption;
    }


    private void OnDisable()
    {
        // EventHandler.DialogueFinishedEvent -= OnFinishedDialogue;
        // EventHandler.SelectDialogueOptionEvent -= OnSelectOption;
    }

    #endregion
}