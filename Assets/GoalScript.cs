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
        HandlePlayerCollision(go);
    }

    void OnCollisionEnter(Collision triggerCollision)
    {
        var go = triggerCollision.gameObject;
        HandlePlayerCollision(go);
    }

    private void HandlePlayerCollision(GameObject go)
    {
        if (go.GetComponent<PlayerScript>() != null)
        {
           
            var rb = go.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.None;

            go.transform.position = transform.position + HatPosition;
            Controller.TriggerWin();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + HatPosition, 0.1f);
    }
}
