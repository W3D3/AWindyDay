using UnityEngine;

public class Movable : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public static float Threshold = 0.001f;

    private bool _positionChange;
    private Vector3 _currentPos;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.drag = 1;
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY | _rigidbody.constraints;
        _rigidbody.freezeRotation = true;

        _positionChange = false;
        _currentPos = transform.position;
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(_currentPos.x - transform.position.x) < Threshold
            && Mathf.Abs(_currentPos.z - transform.position.z) < Threshold)
        {
            _positionChange = false;
        }
        else
        {
            _positionChange = true;
        }

        _currentPos = transform.position;
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

    public bool HasPositionChanged()
    {
        return _positionChange;
    }
}