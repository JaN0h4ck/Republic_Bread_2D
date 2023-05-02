using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.AI;
using System;

public class Player : MonoBehaviour
{
    private Unified_Input inputActions;
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

    public InventoryObject inventory;

    [SerializeField]
    private CharacterSprites sprites;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPos = transform.position;
    }

    private void Start() {
        inputActions = InputContainer.Instance.inputActions;

        inputActions.Player.Interact.performed += _ => OnClickLeft();
        inputActions.Player.Inspect.performed += _ => OnClickRight();
        inputActions.Player.Save.performed += _ => OnSave();
        inputActions.Player.Load.performed += _ => OnLoad();
    }

    public void OnClickLeft() {
        StopAllCoroutines();
        if (CalculateRaycast(Pointer.current.position,out RaycastHit hit)) {
            GroundItem item;
            switch (hit.transform.tag) {
                case "Walkable":
                    agent.SetDestination(new Vector3(hit.point.x, 0, hit.point.z));
                    break;
                case "Item":
                    if (!CheckAgentInRange(hit)) {
                        agent.SetDestination(new Vector3(hit.point.x, 0, hit.point.z));
                        StartCoroutine(WaitForAgentToReachItem(hit));
                    } else {
                        item = hit.transform.GetComponent<GroundItem>();
                        if (item) {
                            inventory.AddItem(new Item(item.item), 1);
                            Destroy(hit.transform.gameObject);
                        } else {
                            Debug.Log("No Item");
                        }
                    }
                    break;
            }
            
        } else {
            Debug.Log("No Hit");
        }
    }

    private IEnumerator WaitForAgentToReachItem(RaycastHit hit) {
        yield return new WaitForEndOfFrame();
        while (!CheckAgentInRange(hit)) {
            yield return new WaitForEndOfFrame();
        }
        GroundItem item = hit.transform.GetComponent<GroundItem>();
        if (item) {
            inventory.AddItem(new Item(item.item), 1);
            Destroy(hit.transform.gameObject);
        } else {
            Debug.Log("No Item");
        }
    }

    public void OnClickRight() {
        if (CalculateRaycast(Pointer.current.position, out RaycastHit hit)) {
            if(!CheckAgentInRange(hit))
                agent.SetDestination(new Vector3(hit.point.x, 0, hit.point.y));
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

    public void OnSave() {
        inventory.Save();
    }

    public void OnLoad() {
        inventory.Load();
    }

    private bool CheckAgentInRange(RaycastHit hit) {
        Vector3 distance = transform.position - hit.point;
        if (distance.magnitude >= (agent.stoppingDistance + .5f))
            return false;
        else
            return true;
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
            switch (movementDirection) {
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

    private void OnApplicationQuit() {
        inventory.Container.Items = new InventorySlot[24];
    }
}
