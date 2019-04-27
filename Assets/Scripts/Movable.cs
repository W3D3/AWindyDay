using UnityEngine;

public class Movable : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.drag = 1;
    }

    /// <summary>
    /// Push the movable object in the given direction
    /// Intensity is controlled by force
    /// </summary>
    /// <param name="direction">Direction of the push (will be normalized)</param>
    /// <param name="force">Intensity of the push</param>
    public void PushObject(Vector3 direction, float force)
    { 
        _rigidbody.AddForce(direction.normalized * force * 0.03f, ForceMode.VelocityChange);
    }
}