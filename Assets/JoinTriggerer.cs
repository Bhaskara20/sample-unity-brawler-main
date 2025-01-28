using baskara.FusionBites;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinTriggerer : MonoBehaviour
{
    public bool isTriggered;
    public SessionEntryPrefab SE;
    private void Update()
    {
        if (isTriggered)
        {
            if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.One, out var button1Pressed))
            {
                if (button1Pressed)
                {
                    //nameEntry.SubmitName();
                    //FC.CreateSession();
                    SE.JoinSession();
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
