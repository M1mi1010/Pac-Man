using UnityEngine;
[RequireComponent(typeof(Collider2D))]

public class Pellet : MonoBehaviour
{
    public int points = 10;

    protected virtual void Eat() // Protected allows subclasses to access it (powerpellet), virtual allows you to override it
    {
        //Multiplier when you eat ghosts, 
        FindObjectOfType<GameManager>().PelletEaten(this);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Need to check only pacman can collide with the pellets, not ghosts
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman")) 
        {
            Eat();
            UnityEngine.Debug.Log("Is pacman colliding with pellets?");
        }
    }
}
