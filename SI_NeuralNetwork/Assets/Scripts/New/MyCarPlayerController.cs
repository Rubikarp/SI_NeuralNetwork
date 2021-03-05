using UnityEngine;

public class MyCarPlayerController : MonoBehaviour
{
    public MyArcadeCarController carController = null;

    private void Update()
    {
        carController.horizontalInput = Input.GetAxis("Horizontal");
        carController.verticalInput = Input.GetAxis("Vertical");
    }
}
