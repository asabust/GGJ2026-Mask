using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InspectPanel : UIPanel, IPointerClickHandler
{
    [SerializeField] private Image centerImage;
    [SerializeField] public TextMeshProUGUI descriptionText;

    public override void OnOpen(object data = null)
    {
        if (data is not InspectData d) return;

        if (centerImage != null) centerImage.sprite = d.sprite;
        if (descriptionText != null) descriptionText.text = d.description ?? "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CloseSelf();
    }

    public void CloseSelf()
    {
        Game.Runtime.Core.UIManager.Instance.Close<InspectPanel>();
    }
}
