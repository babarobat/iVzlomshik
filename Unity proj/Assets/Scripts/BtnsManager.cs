using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BtnsManager : MonoBehaviour
{



    private GameData _currentGameData;
    private InputPlusHistory _currentInputPlusHistory;
    ///////

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
    private int _getRandomCombinationIndex;
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


        _target = GetRandomExceptNumber(1, 20, _target);
        missionTextField.text = missionText + _target.ToString();



        foreach (var btn in buttons)
        {
            if (btn.name != "NewGameBtn")
            {
                btn.interactable = true;
            }
        }

        _numberCounter = 0;
        _currentGameData = new GameData();
        _currentGameData = DataManager.LoadData(_target);
        if ( _currentGameData == null || _currentGameData.dataList.Count == 0 )
        {
            _currentGameData = ListCreator.CreateDataForTarget(_target);
            DataManager.SaveData(_currentGameData, _target);
        }

        _canClickNumber = true;
        _currentNumber = null;
        _currentUnswer = new Unswer();
        _currentOperation = string.Empty;
        outputField.text = _currentOperation;
        helpFiled.text = string.Empty;

        _currentInputPlusHistory = _currentGameData.dataList[UnityEngine.Random.Range(0, _currentGameData.dataList.Count)];
        _currentUnswer.history = _currentInputPlusHistory.history;
        _currentUnswer.value = _target;
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].GetComponentInParent<Button>().interactable = true;
            btns[i].text = _currentInputPlusHistory.myInput.input[i].ToString();
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
                _currentGameData.dataList.Remove(_currentInputPlusHistory);
                DataManager.SaveData(_currentGameData, _target);
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
