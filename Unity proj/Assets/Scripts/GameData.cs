using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Контейнер для хранения данных
/// </summary>
[System.Serializable]
public class GameData
{
    
    public List<InputPlusHistory> dataList;

    public GameData() { dataList = new List<InputPlusHistory>(); }

    public GameData(GameData gameData)
    {
        dataList = new List<InputPlusHistory>();
        foreach (var data in gameData.dataList)
        {
            var tmpInput = new InputPlusHistory(data.myInput, data.history);
            dataList.Add(tmpInput);
        }
        
    }
}
