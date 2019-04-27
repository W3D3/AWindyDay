using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public GameController Controller;
    public Vector3 HatPosition;
    
    void OnTriggerEnter(Collider triggerCollider)
    {
        var go = triggerCollider.gameObject;

        if (go.GetComponent<PlayerScript>() != null)
        {
            Controller.TriggerWin();

            var rb = go.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            go.transform.position = transform.position + HatPosition;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + HatPosition, 0.1f);
    }
}
