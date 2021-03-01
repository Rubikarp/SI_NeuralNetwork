using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CarController carController = null;

    private void Update()
    {
        carController.horizontalInput = Input.GetAxis("Horizontal");
        carController.verticalInput = Input.GetAxis("Vertical");
    }
}
