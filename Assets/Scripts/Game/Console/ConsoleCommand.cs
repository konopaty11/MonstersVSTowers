using UnityEngine;
using UnityEngine.Events;

public class ConsoleCommand
{
    public string Name { get; }
    public UnityAction<string[]> Execute { get; }

    public ConsoleCommand(string _name, UnityAction<string[]> _execute)
    {
        Name = _name;
        Execute = _execute;
    }
}
