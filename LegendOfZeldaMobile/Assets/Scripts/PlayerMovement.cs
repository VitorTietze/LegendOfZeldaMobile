using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAttack playerAttack;
    [SerializeField] private float movementTime;
    [SerializeField] private float movementDistance;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private float startMovementThreshold;
    private Vector2 inputDirection;
    private new Rigidbody2D rigidbody;
    public bool isMoving;
    private Vector2 movementDirection;
    public int horizontal;
    public int vertical;

    [SerializeField] private Rigidbody2D cameraRigidbody;
    [SerializeField] private Vector2 roomIntervals;
    [SerializeField] private float panningTime;
    [SerializeField] private float doorMovements;
    private bool lockedMovement;
    private Coroutine moveCoroutine;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputDirection = joystick.Direction;
        if (inputDirection != Vector2.zero){
            ChangeDirection();
        }

        if (!isMoving && inputDirection.magnitude > startMovementThreshold){
            moveCoroutine = StartCoroutine(Move());
        }
    }

    private void ChangeDirection()
    {
        if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y)){
            if (inputDirection.x > 0){
                horizontal = 1;
            } else if (inputDirection.x < 0){
                horizontal = -1;
            }
            vertical = 0;
        } else {
            if (inputDirection.y > 0){
                vertical = 1;
            } else if (inputDirection.y < 0){
                vertical = -1;
            }
            horizontal = 0;
        }
        
        /* if (!isMoving && !playerAttack.isAttacking){
            animator.SetInt("horizontal", horizontal);
            animator.SetInt("vertical", vertical);
        } */
    }

    private IEnumerator Move()
    {
        if (!lockedMovement)
        {
            isMoving = true;
            movementDirection = new Vector2((float)horizontal, (float)vertical).normalized;
            rigidbody.velocity = movementDirection * movementDistance / movementTime;
            yield return new WaitForSeconds(movementTime);
            rigidbody.velocity = Vector2.zero;
            isMoving = false;
        }
    }

    private IEnumerator PanCamera()
    {
        StartCoroutine(MoveThroughDoor());
        movementDirection = new Vector2((float)horizontal, (float)vertical).normalized;
        float panningDistance = horizontal != 0 ? roomIntervals.x : roomIntervals.y;
        cameraRigidbody.velocity = movementDirection * panningDistance / panningTime;
        yield return new WaitForSeconds(panningTime - 0.01f);
        cameraRigidbody.velocity = Vector2.zero;
    }

    private IEnumerator MoveThroughDoor()
    {
        lockedMovement = true;
        isMoving = true;
        movementDirection = new Vector2((float)horizontal, (float)vertical).normalized;
        rigidbody.velocity = movementDirection * movementDistance / movementTime;
        yield return new WaitForSeconds(movementTime * doorMovements);
        rigidbody.velocity = Vector2.zero;
        isMoving = false;
        lockedMovement = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Doors")){
            StartCoroutine(PanCamera());
            StopCoroutine(moveCoroutine);
        }
    }
}
