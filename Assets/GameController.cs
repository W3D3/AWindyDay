using System.Collections.Generic;
using System.Linq;
using Assets.Enums;
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

    private GameState _state;
    public string NextLevel;

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
        
        TitleText = texts[0];
        InfoText = texts[1];

        _state = GameState.Playing;

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
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.Space) && _state == GameState.Won)
        {
            if (!string.IsNullOrEmpty(NextLevel))
            {
                SceneManager.LoadScene(NextLevel);
            }
            else
            {
                RestartLevel();
            }
        }
    }

    void EnableBlowChecking()
    {
        _checkBlowing = true;
    }

    private void RestartLevel()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    
    public void TriggerGameOver()
    {
        Debug.Log("Game Over");

        GameOverPanel.SetActive(true);
        TitleText.text = "Level Failed :(";
        InfoText.text = "Press R to restart";

        _state = GameState.Lost;
    }

    public void TriggerWin()
    {
        Debug.Log("Win");

        GameOverPanel.SetActive(true);
        TitleText.text = "Level Clear :)";
        InfoText.text = "Press Space to continue";

        _state = GameState.Won;

    }

    /// <summary>
    /// Check status of all movable objects
    /// </summary>
    /// <returns>True no movable object is moving</returns>
    public bool CheckStatus()
    {
        return _state == GameState.Playing && _movables != null && _movables.All(x => x.velocity == Vector3.zero);
    }
}
