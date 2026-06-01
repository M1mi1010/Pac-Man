using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostScatter : GhostBehaviour
{
    private void OnDisable()
    {
        this.ghost.chase.Enable();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled && !this.ghost.frightened.enabled) //Only when the behaviour is enabled because the function will always be called and not in frightened mode
        {
            Debug.Log("Something");
            int index = Random.Range(0, node.availableDirections.Count);

            //Prevent ghost going back and forth between two nodes 
            if (node.availableDirections[index] == -this.ghost.movement.direction && node.availableDirections.Count > 1)
            {
                index++;
                //What if theres an overflow?
                if(index >= node.availableDirections.Count)
                {
                    index = 0; //Wrap
                }
            }

            this.ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}
