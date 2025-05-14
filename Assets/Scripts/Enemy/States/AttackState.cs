using System.Collections;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shootTimer;
    
    public override void Enter()
    {
        
    }
    public override void Perform()
    {
        
        if(enemy.CanSeePlayer() && !enemy.animator.GetBool("hit")){
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shootTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);
            if(shootTimer > enemy.fireRate){
                Shoot();
            }
            if(moveTimer > Random.Range(3,7)){
                enemy.Agent.SetDestination(enemy.Player.transform.position);
                //enemy.Agent.SetDestination(enemy.transform.position  + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
            enemy.LastKnownPos = enemy.Player.transform.position;
        }
        else{
            losePlayerTimer += Time.deltaTime;
            if(losePlayerTimer > 5){
                // change to search state
                stateMachine.ChangeState(new SearchState());
            }
        }
    }
    public override void Exit()
    {
        
    }
    
    public void Shoot(){
        //store refrence to the gun barrel
        Transform gunBarrel = enemy.gunBarrel;
        //instantiate a new bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/C_Bullet") as GameObject, gunBarrel.position, enemy.transform.rotation);
        enemy.StartCoroutine(enemy.FlashMuzzle());
        //calculate the direction to the player
        Vector3 shootDirection = (enemy.Player.transform.position - gunBarrel.transform.position).normalized;
        // add force rigidbody of the bullet
        //bullet.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("musicVolume");
        bullet.GetComponent<AudioSource>().Play();
        bullet.GetComponent<Rigidbody>().linearVelocity = Quaternion.AngleAxis(Random.Range(-3f,3f), Vector3.up) * shootDirection * 50;
        //enemy.audioSource.Play();
        shootTimer = 0;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
