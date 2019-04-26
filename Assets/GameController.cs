using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Rigidbody> Movables = new List<Rigidbody>();

    public WindScript ForwardBlower;
    public WindScript RightBlower;
    public WindScript BackBlower;
    public WindScript LeftBlower;

    private List<WindScript> _blowers = new List<WindScript>();

    private bool _actionLastFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        if (ForwardBlower != null) _blowers.Add(ForwardBlower);
        if (RightBlower != null) _blowers.Add(RightBlower);
        if (BackBlower != null) _blowers.Add(BackBlower);
        if (LeftBlower != null) _blowers.Add(LeftBlower);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var canBlow = CheckStatus();

        if (canBlow)
        {
            if (!_actionLastFrame)
            {
                ForwardBlower?.SetBlowing(false);
                RightBlower?.SetBlowing(false);
                BackBlower?.SetBlowing(false);
                LeftBlower?.SetBlowing(false);
            }

            _actionLastFrame = false;

            if (Input.GetKeyDown(KeyCode.W))
            {
                ForwardBlower?.SetBlowing(true);
                _actionLastFrame = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RightBlower?.SetBlowing(true);
                _actionLastFrame = true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                BackBlower?.SetBlowing(true);
                _actionLastFrame = true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                LeftBlower?.SetBlowing(true);
                _actionLastFrame = true;
            }
        }
    }

    public void TriggerGameOver()
    {
        Debug.Log("Game Over");
        _blowers.ForEach(x => x.SetBlowing(false));
    }

    public bool CheckStatus()
    {
        return Movables != null && Movables.All(x => x.velocity == Vector3.zero) || Movables.Count == 0;
    }
}
