using baskara.FusionBites;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class NameEntry : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_InputField nameField;
    [SerializeField] Button submitButton;

    public void SubmitName()
    {
        int randomInt = UnityEngine.Random.Range(1000, 9999);
        /*if (nameField.text == "")
        {
            FusionConnection.Instance.ConnectToLobby("Player-" + randomInt.ToString());
            //FusionConnection.Instance.ConnectToRunner("Player-" + randomInt.ToString());
            canvas.SetActive(false);
        }
        else
        {
            FusionConnection.Instance.ConnectToLobby(nameField.text);
            //FusionConnection.Instance.ConnectToRunner(nameField.text);
            canvas.SetActive(false);
        }*/

        FusionConnection.Instance.ConnectToLobby("Player-" + randomInt.ToString());
        //FusionConnection.Instance.ConnectToRunner("Player-" + randomInt.ToString());
        canvas.SetActive(false);
        //FusionConnection.Instance.ConnectToLobby(nameField.text);
        //canvas.SetActive(false);
    }

    public void ActivateButton()
    {
        submitButton.interactable = true;
    }
}
