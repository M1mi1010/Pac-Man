using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehaviour
{
    //Reference to a transform in the middle of the home and then blinky's default position
    public Transform inside;
    public Transform outside;

    private void OnEnable()
    {
        StopAllCoroutines();
    }
    private void OnDisable()
    {
        //Use co-rousines to pause the execution of exit transition temporarily
        //Turn off movement script and rigid body (so collisions aren't triggered) and then move to inside position, then move from inside to outside position
        if (this.gameObject.activeSelf) //On disable is called when the objects are destroyed and so need to check that the object isn't destroyed before the coroutine
        {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            this.ghost.movement.SetDirection(-this.ghost.movement.direction);
        }
    }


    private IEnumerator ExitTransition()
    {
        //Turn off all the movement, gravity etc.
        this.ghost.movement.SetDirection(Vector2.up, true); //Force the ghost to go up without causing collisions
        this.ghost.movement.rigidbody.isKinematic = true; // Turns off physics during transition
        this.ghost.movement.enabled = false;

        //Animation-----------------------------------
        Vector3 position = this.transform.position; //Store the initial position
        float duration = 0.5f;
        float elapsed = 0.0f;
        
        while (elapsed < duration)
        {

            //Linear interpolation between the current position and the transform the ghost is moving towardas
            Vector3 newPosition = Vector3.Lerp(position, this.inside.position, elapsed / duration);
            newPosition.z = position.z; //I don't want the z position of the ghost to change

            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime; //I used delta time to prevent users having different experiences if the time elapsed is based on frame rate

            yield return null; //Wait one frame until elapsed = duration
        }

        //Reset the elapsed time so the animations are separate
        elapsed = 0.0f;

        while (elapsed < duration)
        {

            //Linear interpolation between the current position and the transform the ghost is moving towardas
            Vector3 newPosition = Vector3.Lerp(this.inside.position, this.outside.position, elapsed / duration);
            newPosition.z = position.z; //I don't want the z position of the ghost to change

            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime; //I used delta time to prevent users having different experiences if the time elapsed is based on frame rate

            yield return null; //Wait one frame until elapsed = duration
        }


        //End of animation----------------------------

        //Reset all the values, pick a random direction to go in (left or right)
        this.ghost.movement.SetDirection(new Vector2(Random.value <0.5f ? -1.0f: 1.0f, 0.0f), true); //Force this 
        this.ghost.movement.rigidbody.isKinematic = false; 
        this.ghost.movement.enabled = true;
    }

}
