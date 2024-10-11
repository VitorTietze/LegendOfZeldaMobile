using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private FixedJoystick joystick;
    private Vector2 inputDirection;
    private new Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputDirection = joystick.Direction;
    }

    private void FixedUpdate()
    {
        //rigidbody.MovePosition((Vector2)transform.position + (direction * speed * Time.fixedDeltaTime));
    }
}
