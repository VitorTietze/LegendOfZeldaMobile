using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private AnimatedSpritePlus animatedPlus;
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

    [SerializeField] new private Transform camera;
    [SerializeField] private Vector2 roomIntervals;
    [SerializeField] private float doorMovements;
    private Coroutine moveCoroutine;
    private int roomsLayer;
    private List<Transform> currentRooms = new List<Transform>();

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        animatedPlus = transform.Find("Sprite").GetComponent<AnimatedSpritePlus>();
        rigidbody = GetComponent<Rigidbody2D>();

        roomsLayer = LayerMask.NameToLayer("Rooms");
    }

    private void Update()
    {
        inputDirection = joystick.Direction;
        if (inputDirection != Vector2.zero){
            ChangeDirection();
        }

        if (!isMoving && inputDirection.magnitude > startMovementThreshold && !playerAttack.isAttacking){
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
        
        if (!isMoving && !playerAttack.isAttacking){
            //animator.SetInt("horizontal", horizontal);
            //animator.SetInt("vertical", vertical);

            animatedPlus.horizontal = horizontal;
            animatedPlus.vertical = vertical;
        }
    }

    private IEnumerator Move()
    {
        animatedPlus.running = true;
        isMoving = true;
        movementDirection = new Vector2((float)horizontal, (float)vertical).normalized;
        rigidbody.velocity = movementDirection * movementDistance / movementTime;
        yield return new WaitForSeconds(movementTime);
        rigidbody.velocity = Vector2.zero;
        isMoving = false;
        animatedPlus.running = false;
    }

    [SerializeField] private Transform focusedRoom;
    private IEnumerator PanCamera()
    {
        while (camera.position.x != focusedRoom.position.x || camera.position.y != focusedRoom.position.y)
        {
            if (focusedRoom == null) break;

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
        animatedPlus.running = false;
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
            animatedPlus.running = true;
            isMoving = true;
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            StartCoroutine(MoveThroughDoor(other.transform.position));
        }

        if (other.gameObject.layer == roomsLayer)
        {
            currentRooms.Add(other.transform);
            /* if (other.transform != focusedRoom){
                focusedRoom = other.transform;
                StartCoroutine(PanCamera());
            } */
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (this == null || this.gameObject == null) return;
        
        if (other.gameObject.layer == roomsLayer)
        {
            currentRooms.Remove(other.transform);
            if (currentRooms.Count > 0) focusedRoom = currentRooms[0];
            
            if (gameObject.activeSelf || gameObject.activeInHierarchy){
                StartCoroutine(PanCamera());
            }
        }
    }
}
