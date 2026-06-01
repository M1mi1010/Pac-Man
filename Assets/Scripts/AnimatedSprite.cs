using UnityEngine;
[RequireComponent (typeof(SpriteRenderer))]

public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }

    public Sprite[] sprites; //Array to iterate through sprites

    public float animationTime = 0.25f; //Change the image of the sprite every 0.25 seconds

    public int animationFrame { get; private set; }//Store which index of the images we are on
    public bool loop = true; //For some animations we may want them to not loop so the loop variable allows more choice later on regarding this

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer> ();
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), this.animationTime, this.animationTime);
    }

    private void Advance()
    {

        if (!this.spriteRenderer.enabled)
        {
            return;
        }
        this.animationFrame++;

        //Check when we need to wrap or I could set back to zero

        if (this.animationFrame >= this.sprites.Length && this.loop)
        {
            this.animationFrame = 0; //Set back to zero
        } 
        //Make sure there will never be an index out of range error
        if (this.animationFrame >=0 && this.animationFrame < this.sprites.Length)
        {
            this.spriteRenderer.sprite = this.sprites[this.animationFrame];
        }
    }

    public void Restart()
    {
        //To prevent repeating code if you set to 0, instead set to negative 1 and call already made function
        this.animationFrame = -1;
        Advance(); //animationFrame = 0 now
    }
}
