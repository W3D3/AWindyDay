using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject CenterObject;
    public float Speed = 10f;

    private Vector3 _point;

    // Start is called before the first frame update
    void Start()
    {//Set up things on the start method
        _point = CenterObject.transform.position;//get target's coords
        transform.LookAt(_point);//makes the camera look to it
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(_point, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * Speed);
    }
}