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
    public Vector2 movementDirection;
    private int horizontal;
    private int vertical;

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
            StartCoroutine(Move());
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
}
