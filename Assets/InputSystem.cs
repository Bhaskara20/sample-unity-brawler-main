using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public GameObject GO;
    public float moveSpeed;
    public float rotationSpeed;

    public GameObject carriedObject;
    private void Update()
    {
        
        if (TiltFive.Input.TryGetStickTilt(out var joystickValue))
        {
            GO.transform.Translate((new Vector3(joystickValue.x, 0, joystickValue.y) * moveSpeed * Time.deltaTime));
        }
        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.One, out var button1Pressed))
        {
            if (button1Pressed)
            {
                GO.transform.localScale *= 0.95f;
            }
        }
        if (TiltFive.Input.TryGetButton(TiltFive.Input.WandButton.Two, out var button2Pressed))
        {
            if (button2Pressed)
            {
                GO.transform.localScale *= 0.95f;
            }
        }

        if (TiltFive.Input.TryGetTrigger(out var triggerValue))
        {
            if (triggerValue < 0.5f && carriedObject != null)
            {
                carriedObject.gameObject.transform.parent = null;
                carriedObject.GetComponent<Rigidbody>().useGravity = true;
                carriedObject.GetComponent<Rigidbody>().isKinematic = false;
                carriedObject = null;
            }
        }

        //gunain ini kalau mau pakai player movement
        /*
        if (TiltFive.Wand.TryGetWandDevice(TiltFive.PlayerIndex.One, TiltFive.ControllerIndex.Right, out var wandDevice))
        {
            Vector2 inputMovement = wandDevice.Stick.ReadValue();
            Vector3 movementDirection = new Vector3(inputMovement.x, 0, inputMovement.y);
            GO.GetComponent<Rigidbody>().MovePosition(GO.transform.position + movementDirection * moveSpeed * Time.deltaTime);
            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                GO.transform.rotation = Quaternion.RotateTowards(GO.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }*/
    }

    private void OnTriggerStay(Collider other)
    {
        if (TiltFive.Input.TryGetTrigger(out var triggerValue))
        {
            if (triggerValue > 0.5f && carriedObject == null)
            {
                other.gameObject.transform.parent = this.transform;
                carriedObject = other.gameObject;
                carriedObject.GetComponent<Rigidbody>().useGravity = false;
                carriedObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

}
