using System.Collections.Generic;
using System.Linq;
using Assets.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private List<Movable> _movables;

    public WindScript ForwardBlower;
    public WindScript RightBlower;
    public WindScript BackBlower;
    public WindScript LeftBlower;

    private List<WindScript> _blowers = new List<WindScript>();

    public GameObject GameOverPanel;
    private RawImage WinImage;
    private RawImage LoseImage;
    private TextMeshProUGUI InfoText;

    private bool _checkBlowing = false;

    private GameState _state;
    public string NextLevel;
    public string PreviousLevel;

    // Start is called before the first frame update
    void Start()
    {
        if (ForwardBlower != null) _blowers.Add(ForwardBlower);
        if (RightBlower != null) _blowers.Add(RightBlower);
        if (BackBlower != null) _blowers.Add(BackBlower);
        if (LeftBlower != null) _blowers.Add(LeftBlower);

        _movables = FindObjectsOfType<Movable>().ToList();

        _checkBlowing = true;

        GameOverPanel.SetActive(false);
        var images = GameOverPanel.GetComponentsInChildren<RawImage>(true);
        WinImage = images[0];
        LoseImage = images[1];
        InfoText = GameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
        
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

            if (noParticles())
            {
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
                    Invoke("EnableBlowChecking", 0.6f);
                }
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

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (!string.IsNullOrEmpty(NextLevel))
            {
                SceneManager.LoadScene(NextLevel);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!string.IsNullOrEmpty(NextLevel))
            {
                SceneManager.LoadScene(PreviousLevel);
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
//        _blowers.ForEach(x => x.SetBlowing(false));

        
        WinImage.enabled = false;
        LoseImage.enabled = true;
        InfoText.SetText("Press R to Restart");
        GameOverPanel.SetActive(true);
        _state = GameState.Lost;
    }

    public void TriggerWin()
    {
        Debug.Log("Win");

        WinImage.enabled = true;
        LoseImage.enabled = false;
        InfoText.SetText("Press SPACE to Continue");
        GameOverPanel.SetActive(true);
        _state = GameState.Won;

    }

    /// <summary>
    /// Check status of all movable objects
    /// </summary>
    /// <returns>True no movable object is moving</returns>
    public bool CheckStatus()
    {
        return _state == GameState.Playing && _movables != null && _movables.All(x => !x.HasPositionChanged());
    }

    private bool noParticles()
    {
        int sum = 0;
        foreach (var blower in _blowers)
        {
            var ps = blower.GetComponentInChildren<ParticleSystem>();
            sum += ps.particleCount;
        }

        return sum < 100;
    }
}
