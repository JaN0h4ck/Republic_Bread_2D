using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.AI;
using System;

public class MainCharMovement : MonoBehaviour
{
    public enum Directions {
        up = 0x01, // 0001
        down = 0x02, // 0010
        left = 0x04, // 0100
        right = 0x08, // 1000
        upright = 0x09, //1001
        downright = 0x0A, //1010
        upleft = 0x05, // 0101
        downleft = 0x06 // 0110
    }

    private Directions movementDirection = 0x00;

    public Directions playerDirection { 
        get { return movementDirection; } 
    }

    private NavMeshAgent agent;

    private SpriteRenderer spriteRenderer;

    private Vector3 lastPos;

    [SerializeField]
    private CharacterSprites sprites;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPos = transform.position;
    }

    public void OnClickLeft() {
        if (CalculateRaycast(Pointer.current.position,out RaycastHit hit)) {
            Vector3 hitpoint = new Vector3(hit.point.x, 0, hit.point.z);
            agent.SetDestination(hitpoint);
        } else {
            Debug.Log("No Hit");
        }
    }

    public void OnClickRight() {
        if (CalculateRaycast(Pointer.current.position, out RaycastHit hit)) {
            Vector3 hitpoint = new Vector3(hit.point.x, 0, hit.point.z);
            Vector3 distance = transform.position - hit.point;
            if(distance.magnitude >= (agent.stoppingDistance + .75f))
                agent.SetDestination(hitpoint);
            else {
                switch(hit.transform.tag) {
                    case "Interactable":
                        Debug.Log("Interactable");
                        break;
                    default:
                        Debug.Log("No Tag");
                        break;
                }
            }
            // Interaktions Logik
        } else {
            Debug.Log("No Hit");
        }
    }

    private bool CalculateRaycast(Vector2Control pointerPosition,out RaycastHit hit) {
        Ray ray = Camera.main.ScreenPointToRay(pointerPosition.value);
        return Physics.Raycast(ray, out hit, Mathf.Infinity);
    }

    /// <summary>
    /// Calculates the direction the Player is moving towards and set the movementDirection Variable accordingly
    /// </summary>
    private void CalculateDirection() {
        movementDirection = 0x00;
        Vector3 currentPos = transform.position;
        Vector3 agentDirection = lastPos - currentPos;
        lastPos = currentPos;
        agentDirection.Normalize();
        switch(agentDirection.z) {
            case <= -.5f: // up
                movementDirection += 0x01;
                break;
            case >= .5f: // down
                movementDirection += 0x02;
                break;
        }
        switch(agentDirection.x) {
            case >= .5f: // left
                movementDirection += 0x04;
                break;
            case <= -.5f: //right
                movementDirection += 0x08;
                break;
        }
    }

    private void Update() {
        if (agent.velocity.magnitude > 0) {
            CalculateDirection();
            switch(movementDirection) {
                case Directions.up:
                    spriteRenderer.sprite = sprites.up;
                    break;
                case Directions.down:
                    spriteRenderer.sprite = sprites.down;
                    break;
                case Directions.left:
                    spriteRenderer.sprite = sprites.left;
                    break;
                case Directions.right:
                    spriteRenderer.sprite = sprites.right;
                    break;
                case Directions.upleft:
                    spriteRenderer.sprite = sprites.upLeft;
                    break;
                case Directions.downleft:
                    spriteRenderer.sprite = sprites.downLeft;
                    break;
                case Directions.upright:
                    spriteRenderer.sprite = sprites.upRight;
                    break;
                case Directions.downright:
                    spriteRenderer.sprite = sprites.downRight;
                    break;
            }
        }
    }
}
