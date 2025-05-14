using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour{

    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 lastKnownPos;

    public Animator animator;
    public NavMeshAgent Agent {get => agent;}
    public GameObject Player {get => player;}
    public Vector3 LastKnownPos {get => lastKnownPos; set => lastKnownPos =value;}
    public Path1 path;
    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public GameObject eye;
    [Header("Weapon Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate;
    [SerializeField]
    private string currentState;
    public GameObject muzzlePrefab;

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }
    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
    }

    public bool CanSeePlayer(){
        if(player != null){
            if(Vector3.Distance(eye.transform.position, player.transform.position) < sightDistance){
                Vector3 targetDirection = player.transform.position - eye.transform.position;
                float angleToPlayer = Vector3.Angle(targetDirection, eye.transform.forward);
                if(angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView){
                    Ray enemyRay = new Ray(eye.transform.position, targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(enemyRay, out hitInfo, sightDistance))
                    {
                        if(hitInfo.transform.gameObject == player){
                            Debug.DrawRay(eye.transform.position, targetDirection * sightDistance);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    public IEnumerator FlashMuzzle()
    {
        if (muzzlePrefab != null)
        {
            muzzlePrefab.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            muzzlePrefab.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Muzzle Prefab is not assigned!");
        }
    }
}