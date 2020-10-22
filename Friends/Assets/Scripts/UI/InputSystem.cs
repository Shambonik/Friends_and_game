using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AureoleCore.API;
using UnityEngine;
using AureoleCore.Models;

public class InputSystem : MonoBehaviour
{
    private static Vector2 movement;
    //private static InputSystem inputSystem;
    private static Character player;
    [SerializeField] private Transform cam;

    [SerializeField] private float speed = 60f, turnSmoothTime = 0.1f;


    //public delegate void OnAttack();

    //public static event OnAttack OnAttackEvent;

    // private static GameBuffer buffer;

    private float turnSmoothVelocity;

    public static void setPlayer(Character player1)
    {
        player = player1;
    }

    // public static void SetBuffer(GameBuffer buffer1)
    // {
    //     buffer = buffer1;
    // }

    public void Attack()
    {
        GameAPI.SendAttack();
    }

    public void SetMovement(Vector2 direction)
    {
        //Debug.Log(player);
        if (player != null)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            
            //player.transform.rotation = Quaternion.Euler(0, angle, 0);


            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward *
                              Vector2.Distance(direction, new Vector2(0, 0));
            moveDir.y = 0;
            var move = moveDir.normalized * speed * Time.deltaTime;
            if (Vector3.Distance(player.NextPosition, player.transform.position) < 1f)
            {
                player.NextPosition = move + player.transform.position;
            }
            movement = new Vector2(move.x, move.z);
        }
    }

    public static Vector2 GetMovement()
    {
        return movement;
    }

}