using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    public GameController Controller;
    
    void OnCollisionEnter(Collision collision)
    {
        var go = collision.gameObject;

        if (go.transform.CompareTag("Movable"))
        {
            Controller?.TriggerGameOver();
        }
    }
}
