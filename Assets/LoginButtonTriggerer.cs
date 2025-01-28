using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButtonTriggerer : MonoBehaviour
{
    public bool isTriggered;
    public NameEntry nameEntry;
    private void Update()
    {
        if (isTriggered) {
            if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.One, out var button1Pressed))
            {
                if (button1Pressed)
                {
                    nameEntry.SubmitName();
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
