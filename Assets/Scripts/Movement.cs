using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] //The script won't run unless the sprite has a rigid body 2D component
public class Movement : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; } //New allows me to use this variable name without clashes
    public float speed = 8f;
    public float speedMultiplier = 1.0f;

    public Vector2 initialDirection;
    public LayerMask obstacleLayer;    //Layermask allows me to chose which layer I want to do boxcasts on for wall collisions etc. I need to check obstacle layer for this to ensure Pacman collides with the walls
    public Vector2 direction {  get; private set; }
    public Vector2 nextDirection { get; private set; } //For queueing directions (e.g. automatically move up when there's an opening
    public Vector3 startingPosition { get; private set; } //Can be reset

    private void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        //Define variables for customisation such as movement settings
        this.startingPosition = this.transform.position;
    }
    private void Start()
    {
        //Reset the state based on the variabless
        ResetState();
    }

    public void ResetState()
    {
        this.speedMultiplier = 1.0f;
        this.direction = this.initialDirection;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startingPosition;
        this.rigidbody.isKinematic = false; //When the ghost exists the loading bay, I'll set them to be kinematic so they can pass through walls
        this.enabled = true;
    }

    private void FixedUpdate() //Called automatically at a specific interval - update runs per frame
    {
        //Physics will be done in fixed update to ensure user experience is the same regardless of frame rate
        Vector2 position = this.rigidbody.position;
        Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;

        this.rigidbody.MovePosition(position + translation);
    }

    private void Update()
    {
        //Automatically check continuously if the sprite can move in the direction every frame
        if (this.nextDirection != Vector2.zero)
        {
            SetDirection(this.nextDirection);
        }
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        //Assign the direction to the pacman or ghost if the sprite can move in the direction
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            this.nextDirection = Vector2.zero;
        }
        else
        {
            this.nextDirection = direction; 
        }

    }

    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacleLayer);
        return hit.collider != null;
    }
}
