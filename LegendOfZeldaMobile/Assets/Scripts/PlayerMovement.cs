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

    //[SerializeField] private Rigidbody2D cameraRigidbody;
    [SerializeField] new private Transform camera;
    [SerializeField] private Vector2 roomIntervals;
    [SerializeField] private float doorMovements;
    private Coroutine moveCoroutine;
    private int roomsLayer;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        rigidbody = GetComponent<Rigidbody2D>();

        roomsLayer = LayerMask.NameToLayer("Rooms");
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
        isMoving = true;
        movementDirection = new Vector2((float)horizontal, (float)vertical).normalized;
        rigidbody.velocity = movementDirection * movementDistance / movementTime;
        yield return new WaitForSeconds(movementTime);
        rigidbody.velocity = Vector2.zero;
        isMoving = false;
    }

    [SerializeField] private Transform focusedRoom;
    private IEnumerator PanCamera()
    {
        while (camera.position.x != focusedRoom.position.x || camera.position.y != focusedRoom.position.y)
        {
            Vector2 newPosition = Vector2.MoveTowards(
                new Vector2(camera.position.x, camera.position.y),
                new Vector2(focusedRoom.position.x, focusedRoom.position.y),
                0.35f
            );

            camera.position = new Vector3(newPosition.x, newPosition.y, camera.position.z);
            yield return null;
        }
    }

    private IEnumerator MoveThroughDoor(Vector2 doorPos)
    {
        PlayerHealth.immune = true;

        Vector2 direction = new Vector2((float)horizontal, (float)vertical).normalized;
        rigidbody.velocity = direction * movementDistance / movementTime;
        yield return new WaitForSeconds(movementTime * doorMovements);
        rigidbody.velocity = Vector2.zero;

        isMoving = false;
        PlayerHealth.immune = false;
    }

    public IEnumerator GetKnockedBack(Vector2 direction)
    {
        isMoving = true;
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        rigidbody.velocity = direction * movementDistance * 12;
        yield return new WaitForSeconds(movementTime * 6);
        rigidbody.velocity = Vector2.zero;
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Doors")){
            isMoving = true;
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            StartCoroutine(MoveThroughDoor(other.transform.position));
        }

        if (other.gameObject.layer == roomsLayer)
        {
            if (other.transform != focusedRoom){
                focusedRoom = other.transform;
                StartCoroutine(PanCamera());
            }
        }
    }
}
