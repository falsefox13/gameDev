using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            playerController.SetInputDirection(Direction.LEFT);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            playerController.SetInputDirection(Direction.RIGHT);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            playerController.SetInputDirection(Direction.UP);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            playerController.SetInputDirection(Direction.DOWN);
    }
}
