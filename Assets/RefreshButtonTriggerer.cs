using baskara.FusionBites;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using baskara.FusionBites;

public class RefreshButtonTriggerer : MonoBehaviour
{
    public bool isTriggered;
    public FusionConnection FC;
    public RefreshButton refreshButton;
    private void Update()
    {
        if (isTriggered)
        {
            if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.One, out var button1Pressed))
            {
                if (button1Pressed)
                {
                    //nameEntry.SubmitName();
                    refreshButton.Refresh();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cursor"))
        {
            Debug.Log("Login Triggered!!!");
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("cursor"))
        {
            Debug.Log("Login Triggered!!!");
            isTriggered = false;
        }
    }
}
