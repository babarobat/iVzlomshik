using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
}
