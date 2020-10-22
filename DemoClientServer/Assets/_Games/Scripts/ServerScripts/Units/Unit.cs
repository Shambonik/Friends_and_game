﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    protected Vector3 nextPosition;
    protected Vector3 direction;
    protected static readonly int Walking = Animator.StringToHash("isWalking");
    protected float health;
    protected Animator animator;

    public float Health
    {
        get => health;
        set => health = value;
    }

    public Vector3 NextPosition
    {
        get => nextPosition;
        set => nextPosition = value;
    }

    public Vector3 Direction
    {
        set => direction = value;
    }

    void Start()
    {
        nextPosition = transform.position;
        try
        {
            animator = GetComponent<Animator>();
        }catch(UnityException e){}
    }

    void Update()
    {
        changePosition(); 
        changeRotation();
    }

    protected void changeRotation()
    {
        if (direction != Vector3.zero)
            transform.localRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 240);
    }

    protected void changePosition()
    {
        if (Mathf.Abs(Vector3.Distance(nextPosition, transform.position)) > 0.05f)
        {
            animator.SetBool(Walking, true);
            direction = nextPosition - transform.position;
            Rotate(direction);
        }
        else animator.SetBool(Walking, false);
        /*Vector3 deltaPositon = nextPosition - transform.position;
        Vector3 maxMovement= Vector3.Normalize(deltaPositon)*Time.deltaTime*15;
        Vector3 lerpMovement = Vector3.Lerp(new Vector3(0, 0, 0), deltaPositon, 0.2f);
        if (Vector3.Distance(Vector3.zero, maxMovement) < Vector3.Distance(Vector3.zero, lerpMovement))
            transform.position += maxMovement;
        else transform.position += lerpMovement;*/
        transform.position = Vector3.Lerp(transform.position+new Vector3(0,0.5f,0), nextPosition, 0.2f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            transform.position =  new Vector3(transform.position.x,
                hit.point.y,
                transform.position.z);
            Debug.Log(hit.transform.name);
            /*if (hit.transform.tag == "Terrain")
            {
                transform.position =  new Vector3(transform.position.x,
                    hit.transform.GetComponent<Terrain>().SampleHeight(new Vector3(transform.position.x, 0, transform.position.z)),
                    transform.position.z);
            }
            else
            {
                transform.position =  new Vector3(transform.position.x,
                    hit.transform.position.y,
                    transform.position.z);
            }*/
        }
        /*foreach (var hit in hits)
        {
            if (hit.transform.tag == "Terrain")
            {
                terrain = hit.transform.GetComponent<Terrain>();
                break;
            }
        }*/
        
        //transform.position = new Vector3(transform.position, ,transform.position);
    }
    
    private void Rotate(Vector3 direction)
    {
        transform.localRotation = Quaternion.RotateTowards(from: transform.rotation,
            to: Quaternion.LookRotation(direction), maxDegreesDelta: Time.deltaTime * 720);
    }
    
    public virtual void AttackAnimation()
    {
        /*if (canAttack)
        {
            StartCoroutine(CanAttack());
            attackCount++;
            animator.SetInteger("Attack_Count", attackCount % 4);
            timeLastAttack = Time.time;
        }*/
    }
}
