using System.Collections.Generic;
using System;
/// <summary>
/// Расширение для формирования списков из комбинаций и истории по заданному таргету(тобишь параметру)
/// </summary>
public static class ListCreator
{
    #region Private
    static int GetMinValue(int numberLenght)
    {
        string tmp = string.Empty;
        for (int i = 0; i < numberLenght; i++)
        {
            tmp += "1";
        }
        return Int32.Parse(tmp);
    }
    static int GetMaxValue(int numberLenght)
    {
        string tmp = string.Empty;
        for (int i = 0; i < numberLenght; i++)
        {
            tmp += "9";
        }
        return Int32.Parse(tmp);
    }
    /// <summary>
    /// Возвращает список уникальных комбинаций из цифр с количеством знаков numberLenght.
    /// В этих комбинациях отсутсвует цифра 0.
    /// </summary>
    /// <param name="numberLenght">Количество знаков в числе</param>
    /// <returns>ебливый список</returns>
    static List<List<int>> GetUniqueListWithoutZero(int numberLenght)
    {

        List<List<int>> tmpMainList = new List<List<int>>();
        int a = GetMinValue(numberLenght);
        int b = GetMaxValue(numberLenght);

        for (int i = a; i <= b; i++)
        {

            List<int> tmpList = new List<int>();

            string tmpString = i.ToString();



            for (int j = 0; j < tmpString.Length; j++)
            {
                var tmpNumb = (int)Char.GetNumericValue(tmpString[j]);
                if (tmpNumb == 0)
                {
                    continue;

                }
                tmpList.Add(tmpNumb);

            }
            if (tmpList.Count < 4)
            {
                continue;
            }
            tmpList.Sort();

            if (!MainLogic.Contains(tmpMainList, tmpList))
            {

                tmpMainList.Add(tmpList);
            }

        }

        return tmpMainList;


    }
    #endregion
    #region Public
    /// <summary>
    /// Создает обьект класса GameData для числа target
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static GameData CreateDataForTarget(int target)
    {
        GameData tmpData = new GameData();

        foreach (List<int> input in GetUniqueListWithoutZero(4))
        {
            List<Unswer> tmpUnsw = new List<Unswer>();
            tmpUnsw = MainLogic.FindAnUnswers(input, target, true);
            if (tmpUnsw != null)
            {
                InputPlusHistory tmpInput = new InputPlusHistory(input, tmpUnsw[0].history);

                tmpData.dataList.Add(tmpInput);
            }
        }

        return tmpData;

    }
    #endregion








}
