using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, IInteractable
{
    [SerializeField] Animator animator;

    public void Interact()
    {
        animator.SetTrigger("Open");
        this.gameObject.GetComponent<Collider>().enabled = false;
    }
}
