using UnityEngine;
using UnityEngine.UI;
using Game.Runtime.Core;

// 背包逻辑：按钮打开，关上
// 收集到碎片后，若背包接收到收集碎片信息，则刷新相应的碎片UI
public class BackpackToggleUI : MonoBehaviour
{
    [SerializeField] private Button openButton;
    [SerializeField] private GameObject slotsRoot;
    [SerializeField] private InventorySlotsUI slotsUI;

    private bool isOpen;

    private void Awake()
    {
        if (openButton != null)
            openButton.onClick.AddListener(Toggle);

        if (slotsRoot != null)
            slotsRoot.SetActive(false);
    }

    private void OnEnable()
    {
        EventHandler.FragmentCollectedEvent += OnFragmentCollected;
    }

    private void OnDisable()
    {
        EventHandler.FragmentCollectedEvent -= OnFragmentCollected;
    }

    private void Toggle()
    {
        isOpen = !isOpen;
        slotsRoot.SetActive(isOpen);

        if (isOpen) slotsUI?.Refresh();
    }

    private void OnFragmentCollected(string _)
    {
        if (isOpen) slotsUI?.Refresh();
    }
}
