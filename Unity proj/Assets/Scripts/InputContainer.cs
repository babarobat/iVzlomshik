using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InputContainer
{

    public List<MyInput> combinations;
    
    public InputContainer()
    {
        combinations = new List<MyInput>();
    }

    public InputContainer(InputContainer container)
    {
        
        combinations = new List<MyInput>();
        foreach (var input in container.combinations)
        {
            MyInput tmpInp = new MyInput(input);
            combinations.Add(tmpInp);
        }
    }
}
