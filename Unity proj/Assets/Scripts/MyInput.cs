using System.Collections.Generic;
using System;
/// <summary>
/// Прослойка для сериализации данных. Сожердит комбинацию из цифр, с помощью которых будет найдено число
/// </summary>
[Serializable]
public class MyInput  {

    public List<int> input;

    public MyInput() {
        input = new List<int>();
    }

    public MyInput(MyInput newInput)
    {
        input = new List<int>();
        foreach (var numb in newInput.input)
        {
            input.Add(numb);
        }
    }
    public MyInput(List<int> newInput)
    {
        input = new List<int>();
        foreach (var numb in newInput)
        {
            input.Add(numb);
        }
    }
}
