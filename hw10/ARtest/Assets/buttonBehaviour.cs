using UnityEngine;
using Vuforia;

public class buttonBehaviour : MonoBehaviour, IVirtualButtonEventHandler
{
    public GameObject vbs;
    public Animator animator;

    void Start()
    {
        VirtualButtonBehaviour vbb = vbs.GetComponent<VirtualButtonBehaviour>();
        if (vbb)
        {
            vbb.RegisterEventHandler(this);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("Pressed");
        animator.transform.Translate(Vector3.up /100);
        
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        
        Debug.Log("Released");
        animator.transform.Translate(Vector3.down / 100);
    }
}

