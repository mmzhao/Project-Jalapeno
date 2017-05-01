using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour {

    public CutsceneDialogue cd;
    public bool triggered;

    void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.tag == "Player") activateDialogue();
        }
    }

    void activateDialogue()
    {
        cd.gameObject.SetActive(true);
        cd.UpdateDialogue();
        triggered = true;
    }
}
