using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions playerInput;
    public InputSystem_Actions.PlayerActions player;
    private PlayerLook look;

    private PlayerMotor motor;
    private PlayerInteract playerInteract;
    [SerializeField] private PauseMenuManager pauseMenuManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new InputSystem_Actions();
        player = playerInput.Player;
        motor = GetComponent<PlayerMotor>();
        playerInteract = GetComponent<PlayerInteract>();
        look = GetComponent<PlayerLook>();
        player.Jump.performed += ctx => motor.Jump();
        player.Sprint.performed += ctx => motor.Sprint();
        player.Sprint.canceled += ctx => motor.Sprint();
        player.Crouch.performed += ctx => motor.Crouch();
        player.Crouch.canceled += ctx => motor.Crouch();
        player.Attack.performed += ctx => motor.Attack();
        player.Reload.performed += ctx => motor.Reload();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!PauseMenuManager.isPaused && !GameOverManager.isOver){
            motor.ProcessMove(player.Move.ReadValue<Vector2>());
        }
    }
    private void LateUpdate()
    {
        if(!PauseMenuManager.isPaused && !GameOverManager.isOver){
            look.ProcessLook(player.Look.ReadValue<Vector2>());
        }
    }
    private void OnEnable() {
        player.Enable();
    }
    private void OnDisable()
    {
        player.Disable();
    }
}
