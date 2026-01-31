using Game.Runtime.Core;
using Game.Runtime.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;

    private InvEntry entry;

    public void Clear()
    {
        entry = null;

        if (icon == null) return;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void SetEntry(InvEntry e)
    {
        entry = e;

        if (icon == null) return;
        icon.sprite = (e != null) ? e.icon : null;
        icon.enabled = (e != null && e.icon != null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (entry == null) return;
        if (entry.icon == null) return; // 空槽无效（你要的行为）

        var spriteToShow = entry.icon ?? null;

        UIManager.Instance.Open<InspectPanel>(
            new InspectData(spriteToShow, entry.description)
        );
    }
}
