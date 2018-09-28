using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Используется для нашей игры. Хранит набор цифр и историю получения из них ответа(тобишь таргета)
/// </summary>
[System.Serializable]
public class InputPlusHistory
{
    /// <summary>
    /// Комбинация цифр
    /// </summary>
    public MyInput myInput;
    /// <summary>
    /// Представление ответа в виде строки(история)
    /// </summary>
    public string history;
    public InputPlusHistory()
    {
    }
    public InputPlusHistory(MyInput input, string history)
    {
        this.myInput = new MyInput(input);
        this.history = history;
    }
    public InputPlusHistory(List <int> input, string history)
    {
        this.myInput = new MyInput(input);
        this.history = history;
    }
}
