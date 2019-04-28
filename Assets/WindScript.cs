using System;
using System.Collections;
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

    private List<Vector3> RayOrigins;

    private Dictionary<int, Movable> _moveableObjects = new Dictionary<int, Movable>();

    public int particleEmmision = 2000;

    // Start is called before the first frame update
    void Start()
    {
        RayOrigins = new List<Vector3>();
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.hasChanged) Start();
        if (Blowing)
        {
            SoundManager.Instance.playWind();
            _moveableObjects.Clear();

            foreach (var origin in RayOrigins)
            {
                Debug.DrawRay(origin, _raycastDirection * 100, Color.red);

                var hits = Physics.RaycastAll(origin, _raycastDirection, 100f)
                    .OrderBy(x => x.distance)
                    .ToList();

                foreach (var raycastHit in hits)
                {
                    if (raycastHit.transform.CompareTag("IgnoreWind")) continue;

                    if (!raycastHit.transform.CompareTag("Movable")) break;

                    var rb = raycastHit.transform.gameObject.GetComponent<Rigidbody>();

                    // Object hitting movable that is frozen in Z
                    if ((rb.constraints & RigidbodyConstraints.FreezePositionZ) != 0
                        && (_raycastDirection == Vector3.forward || _raycastDirection == Vector3.back)) break;

                    // Object hitting movable that is frozen in X
                    if ((rb.constraints & RigidbodyConstraints.FreezePositionX) != 0
                        && (_raycastDirection == Vector3.left || _raycastDirection == Vector3.right)) break;


                    var objectId = raycastHit.transform.GetInstanceID();
                    var movable = raycastHit.transform.GetComponent<Movable>();
                    if (movable != null && !_moveableObjects.ContainsKey(objectId))
                    {
                        _moveableObjects.Add(objectId, movable);
                    }
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
        StartCoroutine(DelayBlowing(blowing));
        var particles = GetComponentInChildren<ParticleSystem>(true);
        if (particles == null) return;

        var emission = particles.emission;
        emission.rateOverTime = blowing ? particleEmmision : 0;
    }

    IEnumerator DelayBlowing(bool blowing)
    {
        yield return new WaitForSeconds(0.5f);
        Blowing = blowing;
    }
}
