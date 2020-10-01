using UnityEngine;
using System;

public class GoalController : MonoBehaviour
{
    private Action triggerCallback;

    public void SetTriggerCallback(Action triggerCallback)
    {
        this.triggerCallback = triggerCallback;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Minion"))
        {
            other.gameObject.SetActive(false);
            triggerCallback();
        }
    }
}
