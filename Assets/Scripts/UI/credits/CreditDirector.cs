using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(LoadOnClick))]
public class CreditDirector : MonoBehaviour {

    public Canvas credits;
    public CanvasGroup[] canvasGroups;

    public int panelRiseTime = 1;
    public int panelStayTime = 2;
    public int panelFallTime = 1;

    private float panelStayTimer = 0;

    private int panelIndex = 0;
    private int panelState = 0; // 0 means alpha rising, 1 means alpha max, 2 means alpha declining
	// Use this for initialization
	void Start () {
        if (credits == null) credits = GetComponent<Canvas>();
        canvasGroups = credits.GetComponentsInChildren<CanvasGroup>();
        foreach (CanvasGroup cg in canvasGroups)
        {
            cg.alpha = 0;
        }
	}
	
	void FixedUpdate () {
        if (panelIndex == canvasGroups.Length) LoadOnClick.StaticLoadSceneByName("MainMenu");
        switch (panelState)
        {
            case 0:
                canvasGroups[panelIndex].alpha += Time.deltaTime / panelRiseTime;
                if (canvasGroups[panelIndex].alpha >= 1)
                {
                    panelState++;
                }
                break;
            case 1:
                panelStayTimer += Time.deltaTime;
                if (panelStayTimer >= panelStayTime)
                {
                    panelState++;
                    panelStayTimer = 0;
                }
                break;
            case 2:
                canvasGroups[panelIndex].alpha -= Time.deltaTime / panelFallTime;
                if (canvasGroups[panelIndex].alpha <= 0)
                {
                    panelIndex++;
                    panelState = 0;
                }
                break;
            default:
                break;
        }
    }
}
