using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Crystals crystals;
    [SerializeField] MonstersSpawn spawn;
    [SerializeField] GameObject consoleObject;

    Vector2 _startPosition;
    Vector2 _finishPosition;

    float _distanceThreshold = 100f;
    float _diagonalThreshold = 0.4f;

    Dictionary<string, ConsoleCommand> _commands = new();

    void Start()
    {
        Init();
    }

    void Update()
    {
        ReadTouch();
    }

    void Init()
    {
        AddCommand(new("get", HandleGet));
        AddCommand(new("spawn", HandleSpawn));
        AddCommand(new("console", HandleConsole));
    }

    

    void ReadTouch()
    {
        if (Touchscreen.current == null) return;

        TouchControl _touch = Touchscreen.current.primaryTouch;

        if (_touch.press.wasPressedThisFrame)
        {
            _startPosition = _touch.ReadValue().position;
        }

        if (_touch.press.wasReleasedThisFrame)
        {
            _finishPosition = _touch.ReadValue().position;
            DetectDiagonalSwipe();
        }
    }

    void DetectDiagonalSwipe()
    {
        Vector2 _swipeVector = _finishPosition - _startPosition;
        if (_swipeVector.magnitude < _distanceThreshold) return;

        Vector2 _swipeVectorNormalized = _swipeVector.normalized;
        if (_swipeVectorNormalized.x < _diagonalThreshold || 
            _swipeVectorNormalized.y < _diagonalThreshold) return;

        OpenConsole();
    }

    void OpenConsole()
    {
        consoleObject.SetActive(true);
    }

    void CloseConsole()
    {
        consoleObject.SetActive(false);
    }

    public void AddCommand(ConsoleCommand _command)
    {
        _commands[_command.Name] = _command;
    }

    public void ExecuteCommand()
    {
        string[] _command = inputField.text.Split();
        _commands[_command[0]].Execute(_command[1..]);
    }

    public void HandleGet(string[]  _args)
    {
        var (_item, _count) = (_args[0].ToLower(), int.Parse(_args[1]));

        switch (_item)
        {
            case "crystals":
                crystals.AddCrystals(_count);
                break;
        }
    }

    public void HandleSpawn(string[] _args)
    {
        string _monster = _args[0].ToLower();

        switch (_monster)
        {
            case "grox":
                spawn.SpawnMonster(MonsterType.Grox);
                break;
            case "minion":
                spawn.SpawnMonster(MonsterType.Minion);
                break;
            case "zombie":
                spawn.SpawnMonster(MonsterType.Zombie);
                break;
            case "brut":
                spawn.SpawnMonster(MonsterType.Brut);
                break;
        }
    }

    public void HandleConsole(string[] _args)
    {
        string _action = _args[0].ToLower();

        switch (_action)
        {
            case "close":
                CloseConsole(); 
                break;
        }
    }
}
