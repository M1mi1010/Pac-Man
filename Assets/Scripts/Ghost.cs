using UnityEngine;

public class Ghost : MonoBehaviour
{

    public Movement movement {  get; private set; }

    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehaviour initialBehaviour;
    public Transform target; //Object chasing or running away from


    public int points = 200;

    private void Awake()
    {
        this.movement = GetComponent<Movement>();
        this.home = GetComponent<GhostHome>();
        this.scatter = GetComponent<GhostScatter>();
        this.chase = GetComponent<GhostChase>();
        this.frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }
    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.movement.ResetState();
        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Enable();

        if(this.home != this.initialBehaviour) //Ghosts will have the home behaviour enabled or disabled based on the ghost type
        {
            this.home.Disable();
        }
        if(initialBehaviour != null)
        {
            this.initialBehaviour.Enable();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if(this.frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }

            else
            {
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }



}
