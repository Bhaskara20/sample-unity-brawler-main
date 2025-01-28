using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    public void MoveCube(InputAction.CallbackContext context)
    {
        this.transform.Translate(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
    }
}
