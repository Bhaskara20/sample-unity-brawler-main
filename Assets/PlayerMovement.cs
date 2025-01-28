using System.Collections;
using System.Collections.Generic;
using TiltFive;
using UnityEngine;
using static TiltFive.Input;


public class PlayerMovement : MonoBehaviour
{
    public GameObject GO;
    public float moveSpeed;
    public float rotationSpeed;
    // Pilih controller (default Right untuk Wand di tangan kanan)
    [SerializeField] private ControllerIndex controllerIndex = ControllerIndex.Right;

    // Pilih pemain (default Player One)
    [SerializeField] private PlayerIndex playerIndex = PlayerIndex.One;

    public SyncIntegerFusion2 playerScript;

    [Tooltip("Jumlah skor yang diberikan saat berinteraksi.")]
    [SerializeField] private int scoreValue = 10;
    //public Wand wand;
    // Update is called once per frame
    void Update()
    {
        if (GO != null) {
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


            }

            if (TiltFive.Input.TryGetButtonUp(TiltFive.Input.WandButton.B, out var button2Pressed))
            {
                if (button2Pressed)
                {
                    Debug.Log("Global Score Must be Updated!");
                    playerScript.RPC_UpdateValueOnServer(playerScript.SyncedValue + scoreValue);
                }
            }
        }
        





    }
}
