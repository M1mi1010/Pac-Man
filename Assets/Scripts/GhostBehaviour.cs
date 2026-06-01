using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehaviour : MonoBehaviour //Abstract means you can never add a GhostBehaviour to a prefab by itself, it needs a class which inherits it
{
    public Ghost ghost { get; private set; }
    public float duration;

    private void Awake()
    {
        this.ghost = GetComponent<Ghost>();
        this.enabled = false;
    }

    public void Enable()
    {
        Enable(this.duration);
    }

    public virtual void Enable(float duration)
    {
        //Enable frightenedfor however long the power pellet lasts

        this.enabled = true;
        CancelInvoke();
        Invoke(nameof(Disable), duration);

    }

    public virtual void Disable()
    {
        this.enabled = false;
        CancelInvoke();
    }

}
