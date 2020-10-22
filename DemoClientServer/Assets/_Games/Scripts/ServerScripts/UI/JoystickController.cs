using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    
    private InputSystem inputSystem;
    private DynamicJoystick joystick;

    private void Start()
    {
        joystick = GetComponent<DynamicJoystick>();
        inputSystem = FindObjectOfType<InputSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        var movement = new Vector2(joystick.Horizontal, joystick.Vertical).normalized;
        if (inputSystem != null)inputSystem.SetMovement(movement);
    }
}
