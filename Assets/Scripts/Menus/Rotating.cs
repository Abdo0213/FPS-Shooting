using UnityEngine;

public class Rotating : MonoBehaviour
{
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0,0,Time.deltaTime*speed);
    }
}
