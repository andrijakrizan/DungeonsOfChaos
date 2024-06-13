using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            bool success = MovePlayer(moveInput);

            if (!success)
            {
                success = MovePlayer(new Vector2(moveInput.x, 0));
                if (!success) MovePlayer(new Vector2(0, moveInput.y));
            }

            print(success);
            animator.SetBool("IsMoving", success);
        }
        else animator.SetBool("IsMoving", false);
    }

    public bool MovePlayer(Vector2 direction)
    {
        // Check for potential collisions
        int count = rb.Cast(
            direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
            movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
            castCollisions, // List of collisions to store the found collisions into after the Cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

        if (count == 0)
        {
            Vector2 moveVector = direction * moveSpeed * Time.fixedDeltaTime;

            // No collisions
            rb.MovePosition(rb.position + moveVector);
            return true;
        }
        else
        {
            // Print collisions
            foreach (RaycastHit2D hit in castCollisions)
            {
                print(hit.ToString());
            }

            return false;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        // Only set the animation direction if the player is trying to move
        if (moveInput != Vector2.zero)
        {
            animator.SetFloat("XInput", moveInput.x);
            animator.SetFloat("YInput", moveInput.y);

            print($"{moveInput.x}, {moveInput.y}");

        }
        
    }

    public void OnFire(InputValue value)
    {
        print("Shots fired");
    }


    // Update is called once per frame
    //void Update()
    //{
    //    moveInput.x = Input.GetAxisRaw("Horizontal");
    //    moveInput.y = Input.GetAxisRaw("Vertical");

    //    moveInput.Normalize();

    //    rb.velocity = moveInput * moveSpeed;

    //    if (moveInput != Vector2.zero) 
    //    {
    //        animator.SetFloat("xImput", moveInput.x);
    //        animator.SetFloat("yImput", moveInput.y);
    //    }


    //}
}
