using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Camera cam;
    private float xRotation;
    public float xSensitivity = 25f;
    public float ySensitivity = 25f;
    void Start()
    {
        xSensitivity = PlayerPrefs.GetFloat("XSensitivity");
		ySensitivity = PlayerPrefs.GetFloat("YSensitivity");
    }

    public void ProcessLook(Vector2 input){
        float mouseX = input.x;
        float mouseY = input.y;
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
