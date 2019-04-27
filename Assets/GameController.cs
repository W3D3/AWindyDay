using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private List<Rigidbody> _movables;

    public WindScript ForwardBlower;
    public WindScript RightBlower;
    public WindScript BackBlower;
    public WindScript LeftBlower;

    private List<WindScript> _blowers = new List<WindScript>();

    public GameObject GameOverPanel;
    private Text TitleText;
    private Text InfoText;

    private bool _checkBlowing = false;

    // Start is called before the first frame update
    void Start()
    {
        if (ForwardBlower != null) _blowers.Add(ForwardBlower);
        if (RightBlower != null) _blowers.Add(RightBlower);
        if (BackBlower != null) _blowers.Add(BackBlower);
        if (LeftBlower != null) _blowers.Add(LeftBlower);

        _movables = FindObjectsOfType<Movable>().Select(x => x.GetComponent<Rigidbody>()).ToList();

        _checkBlowing = true;

        var texts = GameOverPanel.GetComponentsInChildren<Text>();
        ;
        TitleText = texts[0];
        InfoText = texts[1];

    }

    // Update is called once per frame
    void LateUpdate()
    {
        var canBlow = _checkBlowing && CheckStatus();

        if (canBlow)
        {
            
            ForwardBlower?.SetBlowing(false);
            RightBlower?.SetBlowing(false);
            BackBlower?.SetBlowing(false);
            LeftBlower?.SetBlowing(false);
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                ForwardBlower?.SetBlowing(true);
                _checkBlowing = false;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RightBlower?.SetBlowing(true);
                _checkBlowing = false;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                BackBlower?.SetBlowing(true);
                _checkBlowing = false;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                LeftBlower?.SetBlowing(true);
                _checkBlowing = false;
            }

            if (!_checkBlowing)
            {
                Invoke("EnableBlowChecking", 1f);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public void EnableBlowChecking()
    {
        _checkBlowing = true;
    }

    public void TriggerGameOver()
    {
        Debug.Log("Game Over");
        _blowers.ForEach(x => x.SetBlowing(false));

        GameOverPanel.SetActive(true);
        TitleText.text = "Level Failed :(";
        InfoText.text = "Press R to restart";
    }

    public void TriggerWin()
    {
        Debug.Log("Win");

        GameOverPanel.SetActive(true);
        TitleText.text = "Level Clear :)";
        InfoText.text = "Press Space to continue";

    }

    /// <summary>
    /// Check status of all movable objects
    /// </summary>
    /// <returns>True no movable object is moving</returns>
    public bool CheckStatus()
    {
        return _movables != null && _movables.All(x => x.velocity == Vector3.zero);
    }
}
