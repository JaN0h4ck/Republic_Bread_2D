using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.AI;

public class MainCharMovement : MonoBehaviour
{
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
        if (calculateRaycast(Pointer.current.position,out RaycastHit hit)) {
            Vector3 hitpoint = new Vector3(hit.point.x, 0, hit.point.z);
            agent.SetDestination(hitpoint);
        } else {
            Debug.Log("No Hit");
        }
    }

    public void OnClickRight() {
        if (calculateRaycast(Pointer.current.position, out RaycastHit hit)) {
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

    private bool calculateRaycast(Vector2Control pointerPosition,out RaycastHit hit) {
        Ray ray = Camera.main.ScreenPointToRay(pointerPosition.value);
        return Physics.Raycast(ray, out hit, Mathf.Infinity);
    }


    private void Update() {
        if (agent.velocity.magnitude > 0) {
            Vector3 currentPos = transform.position;
            Vector3 direction = lastPos - currentPos;
            lastPos = currentPos;
            direction.Normalize();

            #region bool logic
            bool up = (direction.z <= -.5);// && direction.x <= .5 && direction.x >= -.5);
            bool down = (direction.z >= .5);// && direction.x <= .5 && direction.x >= -.5);
            bool left = (direction.x >= .5);// && direction.y < .5 && direction.y > -.5);
            bool right = (direction.x <= -.5);
            bool upleft = (up && left);
            bool upright = (up && right);
            bool upsideways = (upleft || upright);
            bool downleft = (down && left);
            bool downright = (down && right);
            bool downsideways = (downleft || downright);
            bool sideways = (left || right);
            #endregion

            if (up && !sideways) {
                spriteRenderer.sprite = sprites.up;
                return;
            } else if (down && !sideways) {
                spriteRenderer.sprite = sprites.down;
                return;
            } else if (sideways) {
                if (downsideways) {
                    if (downleft) {
                        spriteRenderer.sprite = sprites.downLeft;
                        return;
                    } else if (downright) {
                        spriteRenderer.sprite = sprites.downRight;
                        return;
                    }
                } else if (upsideways) {
                    if (upleft) {
                        spriteRenderer.sprite = sprites.upLeft;
                        return;
                    } else if (upright) {
                        spriteRenderer.sprite = sprites.upRight;
                        return;
                    }
                } else if(left) {
                    spriteRenderer.sprite = sprites.left;
                    return;
                } else if(right) {
                    spriteRenderer.sprite = sprites.right;
                    return;
                }
            }
        }
    }
}
