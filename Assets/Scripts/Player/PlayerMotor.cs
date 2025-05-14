using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour, IDataPersistence
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool crouching = false;
    private bool sprinting = false;
    public GunManager gun;
    

    private bool lerpCrouch = false;
    private float crouchTimer = 0f;
    private float speed;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;
    public Canvas crosshair;


    public AudioClip footStepSound;
    public float footStepDelay;

    private float nextFootstep = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if(SceneManager.sceneCount > 1)
            crosshair.gameObject.SetActive(false);
        else
            crosshair.gameObject.SetActive(true);
            controller = GetComponent<CharacterController>();
            speed = walkSpeed;
            gun = GetComponentInChildren<GunManager>();
        }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if(lerpCrouch){
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if(crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);
            if(p > 1){
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }
    public void LoadData(GameData data){
        controller.enabled = false;
        transform.position = data.playerPos;
        controller.enabled = true;
        
    }
    public void SaveData(ref GameData data){
        data.playerPos = this.transform.position;
    }
    // recive the inputs for our InputManager.cs and apply them to our character controller.
    public void ProcessMove(Vector2 input){
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime; 
        if(isGrounded && playerVelocity.y < 0){
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
        
    }
    public void Attack(){
        if(!PauseMenuManager.isPaused && !GameOverManager.isOver && !WinManager.isWin){
            gun.Shoot();
        }
    }
    public void Reload(){
        if(!PauseMenuManager.isPaused && !GameOverManager.isOver && !WinManager.isWin){
            gun.Reload();
        }
    }
    public void Jump(){
        if(isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
    public void Sprint(){
        sprinting = !sprinting;
        if(sprinting && crouching == false)
            speed = sprintSpeed;
        else
            speed = walkSpeed;
    }
    public void Crouch(){
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
        speed = walkSpeed;
    }
}
