using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ClawMachinePanel : UIPanel
{
    public ClawMachineController clawMachineController;
    public HoldButton moveButton;
    public Button grabButton;

    public Transform rulePanel;
    public Button startButton;
    public TextMeshProUGUI timeText;


    public float totalTime = 10f;
    private float remainTime;
    private bool isCounting = false;

    public override void OnInit()
    {
        grabButton.onClick.AddListener(OnGrabPressed);
        startButton.onClick.AddListener(StartGame);
    }

    public override void OnOpen(object data = null)
    {
        clawMachineController = data as ClawMachineController;
    }

    private void OnGrabPressed()
    {
        isCounting = false;
        clawMachineController?.OnGrabPressed();
    }

    public void StartGame()
    {
        rulePanel.gameObject.SetActive(false);
        clawMachineController?.StartGame();

        remainTime = totalTime;
        isCounting = true;
    }

    public void ReStart()
    {
        rulePanel.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isCounting) return;

        remainTime -= Time.deltaTime;

        if (remainTime <= 0f)
        {
            remainTime = 0f;
            OnGrabPressed();
        }

        timeText.text = remainTime.ToString("F2");
    }
}