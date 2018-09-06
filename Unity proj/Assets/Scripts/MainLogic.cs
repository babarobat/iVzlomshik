using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public static class MainLogic  {




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

    public static List<Unswer> FindAnUnswers(List<int> inputList, int target, bool chekForChar)
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
                        if (chekForChar)
                        {
                            if (!UnswerHistoryContainsCountOfChar(fullUns, '+', 3))
                            {
                                fullUnswers.Add(fullUns);
                            }
                            else
                            {
                                x = true;
                                
                                
                            }
                        }
                        else
                        {
                            fullUnswers.Add(fullUns);
                        }
                        
                        
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
        if (trueUnswers.Count > 0 && !x)
        {
            return GetUnique(trueUnswers);
        }
        else return null;

    }
    public static List<Unswer> FindAnUnswers(List<int> inputList, int target)
    {
        
        List<Unswer> fullUnswers = new List<Unswer>();
        
        
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
                    foreach (var uns in AllOperations(new Unswer { value = three, history = three.ToString() },withoutThree[0] ))
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
        if (trueUnswers.Count > 0)
        {
            return GetUnique(trueUnswers);
        }
        else return null;

    }
    static List<Unswer> GetUnique (List<Unswer> unswers)
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
     
}
