using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Вспомогательный статический класс для математических рассчетов
/// </summary>
public static class MainLogic
{

    #region Public
    /// <summary>
    /// Магическим образом возвращает тебя в детство
    /// </summary>
    /// <param name="inputList"></param>
    /// <param name="target"></param>
    /// <param name="chekAllPlus"></param>
    /// <returns></returns>
    public static List<Unswer> FindAnUnswers(List<int> inputList, int target, bool chekAllPlus)
    {
        List<Unswer> fullUnswers = new List<Unswer>();
        bool x = false;
        foreach (var one in inputList)
        {
            List<int> withoutOne = RemoveFromArray(inputList, one);
            List<Unswer> extUnswers = new List<Unswer>();
            foreach (var two in withoutOne)
            {
                List<int> withoutTwo = RemoveFromArray(withoutOne, two);
                List<Unswer> unswers = new List<Unswer>();
                foreach (var three in withoutTwo)
                {
                    List<int> withoutThree = RemoveFromArray(withoutTwo, three);
                    foreach (var uns in AllOperations(new Unswer { value = three, history = three.ToString() }, withoutThree[0]))
                    {
                        unswers.Add(uns);
                    }
                }
                foreach (var uns in unswers)
                {
                    foreach (var extUns in AllOperations(uns, two))
                    {
                        extUnswers.Add(extUns);
                    }
                }
                foreach (var uns in extUnswers)
                {
                    foreach (var fullUns in AllOperations(uns, one))
                    {
                        fullUnswers.Add(fullUns);
                    }
                }
            }
        }
        List<Unswer> trueUnswers = new List<Unswer>();

        foreach (var item in fullUnswers)
        {
            if (item.value == target)
            {
                trueUnswers.Add(item);
            }
        }

        if (chekAllPlus) //проверка
        {
            foreach (var item in trueUnswers)
            {
                if (UnswerHistoryContainsCountOfChar(item, '+', 3))
                    x = true;
            }
        }
        if (trueUnswers.Count > 0 && !x)
        {
            return GetUnique(trueUnswers);
        }
        else return null;

    }
    /// <summary>
    /// Возвращает true, если массив содержит заданное число указанных симолов
    /// </summary>
    /// <param name="unswers"></param>
    /// <param name="chr"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static bool ArrayContainsCountOfChar(List<Unswer> unswers, char chr, int count)
    {
        int i = 0;
        foreach (Unswer uns in unswers)
        {
            foreach (char item in uns.history)
            {
                if (item == chr)
                {
                    i++;
                }
            }
        }
        if (i >= count)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    /// <summary>
    /// Возвращает true, если Unswer.history содержит заданное число указанных симолов
    /// </summary>
    /// <param name="unswer"></param>
    /// <param name="chr"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static bool UnswerHistoryContainsCountOfChar(Unswer unswer, char chr, int count)
    {
        int i = 0;

        foreach (char item in unswer.history)
        {
            if (item == chr)
            {
                i++;
            }
        }

        if (i >= count)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    /// <summary>
    /// Содержит ли массив MyInput заданный список int
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static bool Contains(List<MyInput> x, List<int> y)
    {

        var a = false;
        foreach (var item in x)
        {
            if (item.input.SequenceEqual(y))
            {
                a = true;

                break;
            }
        }
        return a;
    }
    /// <summary>
    /// Содержит ли массив List(int) заданный список int
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static bool Contains(List<List<int>> x, List<int> y)
    {

        var a = false;

        foreach (var item in x)
        {
            if (item.SequenceEqual(y))
            {
                a = true;

                break;
            }
        }
        return a;
    }
    #endregion
    #region Private
    /// <summary>
    /// производит все мат оперции с Unswer и числом item
    /// </summary>
    /// <param name="arg"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    static List<Unswer> AllOperations(Unswer arg, int item)
    {
        List<Unswer> newArray = new List<Unswer>();
        var tmp = arg;
        newArray.Add(Sum(tmp, item));
        newArray.Add(Div(tmp, item));
        if (tmp.value % item == 0 && item != 0)
        {
            newArray.Add(Del(tmp, item));
        }
        newArray.Add(Mul(tmp, item));
        return newArray;
    }

    static Unswer Sum(Unswer unswer, int item)
    {
        unswer.value += item;
        unswer.history += "+" + item.ToString();
        return unswer;
    }
    static Unswer Div(Unswer unswer, int item)
    {
        unswer.value -= item;
        unswer.history += "-" + item.ToString();
        return unswer;
    }
    static Unswer Mul(Unswer unswer, int item)
    {
        unswer.value *= item;
        unswer.history += "*" + item.ToString();
        return unswer;
    }
    static Unswer Del(Unswer unswer, int item)
    {
        unswer.value /= item;
        unswer.history += "/" + item.ToString();
        return unswer;
    }
    static List<int> RemoveFromArray(List<int> inputList, int value)
    {
        List<int> newArray = new List<int>();
        foreach (var item in inputList)
        {
            newArray.Add(item);
        }
        newArray.Remove(value);
        return newArray;
    }
    
    /// <summary>
    /// Удаляет повторяющиеся элементы массива
    /// </summary>
    /// <param name="unswers"></param>
    /// <returns></returns>
    static List<Unswer> GetUnique(List<Unswer> unswers)
    {
        List<Unswer> uniques = new List<Unswer>();

        foreach (var item in unswers)
        {
            if (!uniques.Contains(item))
            {
                uniques.Add(item);
            }
        }
        return uniques;
    }
    
    #endregion




}
