using UnityEngine;
using System.IO;
/// <summary>
/// Статическое расширение для сохранения и загрузки данных
/// </summary>
public static class DataManager
{
    /// <summary>
    ///  Загружает данные в виде GameData.
    /// </summary>
    /// <param name="value">Имя файла</param>
    /// <returns></returns>
    public static GameData LoadData(int value)
    {
        string path = Path.Combine(Application.dataPath, "Data_" + value.ToString());
        if (File.Exists(path))
        {
            string dataRead = File.ReadAllText(path);
            return JsonUtility.FromJson<GameData>(dataRead);
        }
        else return null;

    }
    /// <summary>
    /// Сохраняет данные.
    /// </summary>
    /// <param name="dataToSave">Контейнер данных в виде GameData</param>
    /// <param name="targetValue">Имя файла</param>
    public static void SaveData(GameData dataToSave, int targetValue)
    {
        string data = JsonUtility.ToJson(dataToSave);
        string path = Path.Combine(Application.dataPath, "Data_" + targetValue.ToString());
        File.WriteAllText(path, data);
    }
    public static void LoadLastGame()
    {

    }
}
