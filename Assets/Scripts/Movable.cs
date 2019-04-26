using UnityEngine;

public class Movable : MonoBehaviour
{
    private Rigidbody rigidbody;
    
    public int force;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Push the movable object in the given direction
    /// Intensity is controlled by force
    /// </summary>
    /// <param name="direction">Direction of the push (will be normalized)</param>
    /// <param name="force">Intensity of the push</param>
    public void pushObject(Vector3 direction, float force)
    {
        rigidbody.AddForce(direction.normalized * force, ForceMode.Acceleration);
    }
}