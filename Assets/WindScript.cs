using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Enums;
using UnityEngine;

public class WindScript : MonoBehaviour
{
    public WindDirection Direction;

    public bool Blowing;

    public float Force;

    public int RaycastCount= 40;

    private Vector3 _raycastDirection;

    private List<Vector3> RayOrigins = new List<Vector3>();

    private Dictionary<int, Movable> _moveableObjects = new Dictionary<int, Movable>();

    // Start is called before the first frame update
    void Start()
    {
        switch (Direction)
        {
            case WindDirection.Back:
                _raycastDirection = Vector3.back;
                break;
            case WindDirection.Right:
                _raycastDirection = Vector3.right;
                break;
            case WindDirection.Forward:
                _raycastDirection = Vector3.forward;
                break;
            default:
                _raycastDirection = Vector3.left;
                break;
        }

        float width;
        if (Direction == WindDirection.Back || Direction == WindDirection.Forward)
        {
            width = transform.localScale.x;
        }
        else
        {
            width = transform.localScale.z;
        }
        var current = -width / 2f;
        var step = width / RaycastCount;
        for (var i = 0; i <= RaycastCount; i++)
        {
            Vector3 origin = transform.position;
            if (Direction == WindDirection.Back || Direction == WindDirection.Forward)
            {
                origin += new Vector3(current, 0, 0);
            }
            else
            {
                origin += new Vector3(0, 0, current);
            }
            current += step;

            RayOrigins.Add(origin);
        }

        Blowing = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Blowing)
        {
            _moveableObjects.Clear();

            foreach (var origin in RayOrigins)
            {
                Debug.DrawRay(origin, _raycastDirection * 100, Color.red);

                var hits = Physics.RaycastAll(origin, _raycastDirection, 100f)
                    .OrderBy(x => x.distance)
                    .ToList();
                
                if (hits.Count < 1 || !hits[0].transform.CompareTag("Movable")) continue;

                
                var objectId = hits[0].transform.GetInstanceID();
                var movable = hits[0].transform.GetComponent<Movable>();
                if (movable != null && !_moveableObjects.ContainsKey(objectId))
                {
                    _moveableObjects.Add(objectId, movable);
                }
            }

            foreach (var rec in _moveableObjects.Values)
            {
                rec.PushObject(_raycastDirection, Force);
            }
            
        }
    }

    public void SetBlowing(bool blowing)
    {
        Blowing = blowing;
        var particles = GetComponentInChildren<ParticleSystem>(true);
        particles?.gameObject.SetActive(blowing);
    }
}
