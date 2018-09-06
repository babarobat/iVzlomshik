using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BtnsManager : MonoBehaviour
{

    public Text outputField;
    private int _target;
    public Text[] btns;
    public Text helpFiled;
    public Text missionTextField;
    public string missionText;
    public Transform GGpannel;

    private bool _canClickNumber = false;
    private int? _currentNumber;
    private string _currentOperation;
    public List<Unswer> _currentUnswers;
    private Unswer _currentUnswer;
    private char[] _unswer;
    private List<int> _previous = new List<int>();
    Button[] buttons;
    MyInput tmpInput;

    private int _numberCounter;

    public InputContainer container;

    private int _stackOverflowCounter = 0;
    private void Start()
    {
        GGpannel.gameObject.SetActive(false);
        buttons = FindObjectsOfType<Button>();
        foreach (var btn in buttons)
        {
            if (btn.name != "NewGameBtn")
            {
                btn.interactable = false;
            }
        }
    }
    public void ResetGame()
    {
        _numberCounter = 0;
        _canClickNumber = true;
        _currentNumber = null;
        _currentOperation = string.Empty;
        outputField.text = _currentOperation;
        foreach (var item in btns)
        {
            item.GetComponentInParent<Button>().interactable = true;
        }
    }
    public void NewGame()
    {
        foreach (var btn in buttons)
        {
            if (btn.name != "NewGameBtn")
            {
                btn.interactable = true;
            }
        }

        _numberCounter = 0;
       
        _target = GetRandomExceptNumber(-20, 20,0);
        string path = Path.Combine(Application.dataPath, "Data_" + _target.ToString());
        if (File.Exists(path))
        {
            string dataRead = File.ReadAllText(path);
            InputContainer loadedContainer = JsonUtility.FromJson<InputContainer>(dataRead);
            container = new InputContainer(loadedContainer);
        }
        else
        {
            container = new InputContainer();
        }
        
        _canClickNumber = true;
        _currentNumber = null;
        _currentUnswer = new Unswer();
        _currentOperation = string.Empty;
        outputField.text = _currentOperation;
        helpFiled.text = string.Empty;
        tmpInput = new MyInput();
        _currentUnswers = new List<Unswer>();
        foreach (var item in btns)
        {
            item.GetComponentInParent<Button>().interactable = true;
            var value = UnityEngine.Random.Range(1, 9);
            tmpInput.input.Add(value);
            item.text = value.ToString();
        }

        _currentUnswers = MainLogic.FindAnUnswers(tmpInput.input, _target);
        tmpInput.input.Sort();

        bool proverka = _currentUnswers == null ||
            MainLogic.Contains(container.combinations, tmpInput.input) ||
            MainLogic.ArrayContainsCountOfChar(_currentUnswers, '+', 3);

        if (proverka)
        {
            if (_stackOverflowCounter < 100)
            {
                _stackOverflowCounter++;
                NewGame();
            }
            else
            {
                throw new Exception("GameOver");
            }
        }
        else
        {
            _stackOverflowCounter = 0;
            _currentUnswer = _currentUnswers[UnityEngine.Random.Range(0, _currentUnswers.Count - 1)];
            missionTextField.text = missionText + " " + _target.ToString();

        }
    }
    public void NumberClick(Button button)
    {
        
        var btnText = button.GetComponentInChildren<Text>();
        if (_canClickNumber)
        {
            
            if (_currentNumber == null)
            {
                button.interactable = false;
                _canClickNumber = false;
               
                outputField.text += btnText.text;
                _currentNumber = int.Parse(btnText.text);
                _numberCounter++;
            }
            else
            {
                int? tmpNumber = Operation((int)_currentNumber, _currentOperation, int.Parse(btnText.text));
                if (tmpNumber != null)
                {
                    _currentNumber = tmpNumber;
                    outputField.text = _currentNumber.ToString();
                    button.interactable = false;
                    _canClickNumber = false;
                    _numberCounter++;
                }
            }
            if (_numberCounter == 4 && _currentNumber == _target)
            {
                container.combinations.Add(tmpInput);
                string data = JsonUtility.ToJson(container);
                string path = Path.Combine(Application.dataPath, "Data_" + _target.ToString());
                File.WriteAllText(path, data);
                GGpannel.gameObject.SetActive(true);
                
            }
        }
        
    }
    public void OperationClick(Button button)
    {
        if (outputField.text != string.Empty)
        {
            var oper = button.GetComponentInChildren<Text>().text;
            _currentOperation = oper;
            outputField.text = _currentNumber.ToString() + _currentOperation;
            _canClickNumber = true;
        }
    }
    private int? Operation(int a, string op, int b)
    {
        if (op == "+")
        {
            return a + b;
        }
        if (op == "-")
        {
            return a - b;
        }
        if (op == "X")
        {
            return a * b;
        }
        if (op == "/" && b != 0 && a % b == 0)
        {
            return a / b;
        }
        else return null;
    }
    public void Help()
    {
        if (_currentUnswers != null && helpFiled.text.Length < _currentUnswer.history.Length)
        {
            helpFiled.text += _currentUnswer.history[helpFiled.text.Length];
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    int GetRandomExceptNumber(int a, int b , int exception)
    {
        int i = UnityEngine.Random.Range(a, b);
        if (i == exception)
        {
            i = GetRandomExceptNumber(a,b, exception);
        }
        return i;
    }


}
