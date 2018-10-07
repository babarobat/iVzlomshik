using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// Менеджер кнопок
/// </summary>
public class BtnsManager : MonoBehaviour
{
    #region HelpButtns
    void SetImageAndValueForHelpButtons()
    {
        for (int i = _helpButtons.Length - 1; i >= 0; i--)
        {
            var tmpImage = _helpButtons[i].GetComponent<Image>();
            var tmptext = _helpButtons[i].GetComponentInChildren<Text>();
            tmpImage.enabled = true;
            tmptext.text = _currentUnswer.history[i].ToString();
            tmptext.enabled = false;
        }

    }
    void ResetHelpButtnsValues()
    {
        foreach (var btn in _helpButtons)
        {
            btn.GetComponentInChildren<Text>().text = string.Empty;
        }
    }
    public void OpenHelpButton(Button button)
    {
        button.GetComponent<Image>().enabled = false;
        button.GetComponentInChildren<Text>().enabled = true;
    }
    #endregion

    #region Публичные текстовые элементы
    /// <summary>
    /// ссылка на текстовый эелемент поля вывода решения
    /// </summary>
    [Header("Текстовые поля")]
    [Tooltip("ссылка на текстовый эелемент поля вывода решения")]
    public Text outputField;

    /// <summary>
    /// ссылка на текстовый эелемент поля таймер
    /// </summary>
    [Tooltip("ссылка на текстовый эелемент поля таймер")]
    public Text timerText;
    /// <summary>
    /// ссылка на текстовый эелемент поля текста задания
    /// </summary>
    [Tooltip("ссылка на текстовый эелемент поля текста задания")]
    public Text missionTextField;
    /// <summary>
    /// Такст задания. В конце к нему прибавится переменная _target
    /// </summary>
    [Tooltip("Такст задания. В конце к нему прибавится переменная _target")]
    public string missionText;
    #endregion
    #region Кнопки
    /// <summary>
    /// массив кнопок-подсказок
    /// </summary>
    [Space(10)]
    [Header("Кнопки")]
    [Tooltip("массив кнопок-подсказок")]
    public Button[] _helpButtons;
    /// <summary>
    /// массив кнопок с цифрами
    /// </summary>
    [Tooltip("массив кнопок с цифрами")]
    public Text[] NumberButtons;
    #endregion
    #region Панели
    /// <summary>
    /// Панель, вызывается при правильном ответе
    /// </summary>
    [Space(10)]
    [Header("Панели")]
    [Tooltip("Вызывается при правильном ответе")]
    public Transform GoodGamePannel;
    #endregion


    /// <summary>
    /// Можно кликать по кнопке с числом?
    /// </summary>
    private bool _canClickNumber = false;
    /// <summary>
    /// Последнее нажатое число либо результат последней операции. Может принимать значение null
    /// </summary>
    private int? _currentNumber;
    /// <summary>
    /// текущая операция
    /// </summary>
    //private string _currentOperation;
    private Operations _currentOperation;

    float _currentCount = 60;
    private Unswer _currentUnswer;
    private char[] _unswer;
    private List<int> _previous = new List<int>();
    Button[] buttons;
    MyInput tmpInput;

    private int _numberCounter;
    private int _getRandomCombinationIndex;

    private GameData _currentGameData;
    private InputPlusHistory _currentInputPlusHistory;
    private int _stackOverflowCounter = 0;
    private int _target;

    TimeCounter tc = new TimeCounter();


    void CountingAndDisplay()
    {
        timerText.text = _currentCount.ToString();
        _currentCount -= 1;
        if (_currentCount<10)
        {
            timerText.color = Color.red;
            if (_currentCount==0)
            {
                StopCounting();
                timerText.text = "LOOSER!";
                timerText.color = Color.red;

            }
        }
    }
    void ResetCouning()
    {
        _currentCount = 60;
        timerText.color = Color.white;
    }
    void StartCounting()
    {
        InvokeRepeating("CountingAndDisplay", 0, 1f);
    }
    void StopCounting()
    {
        CancelInvoke("CountingAndDisplay");
        ResetCouning();
    }

    private void Start()
    {
        
        ResetHelpButtnsValues();
        GoodGamePannel.gameObject.SetActive(false);
        buttons = FindObjectsOfType<Button>();
        foreach (var btn in buttons)
        {
            if (btn.name != "NewGameBtn")
            {
                btn.interactable = false;
            }
        }
        NewGame();
    }
    public void ResetGame()
    {
        
        _numberCounter = 0;
        _canClickNumber = true;
        _currentNumber = null;
        _currentOperation = Operations.none;
        outputField.text = MainLogic.OperationToString(_currentOperation);
        foreach (var item in NumberButtons)
        {
            item.GetComponentInParent<Button>().interactable = true;
        }
    }


    private void Update()
    {
        tc.Update();
        timerText.text = tc.GetSecMilisec();
    }
    public void NewGame()
    {
        
        tc.Start(10);
        //StopCounting();
        //StartCounting();
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
        _currentOperation = Operations.none;
        outputField.text = MainLogic.OperationToString(_currentOperation);
        //helpFiled.text = string.Empty;

        _currentInputPlusHistory = _currentGameData.dataList[UnityEngine.Random.Range(0, _currentGameData.dataList.Count)];
        _currentUnswer.history = _currentInputPlusHistory.history;
        
        _currentUnswer.value = _target;
        for (int i = 0; i < NumberButtons.Length; i++)
        {
            NumberButtons[i].GetComponentInParent<Button>().interactable = true;
            
            NumberButtons[i].text = _currentInputPlusHistory.myInput.input[i].ToString();
        }
        SetImageAndValueForHelpButtons();
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
                //int? tmpNumber = Operation((int)_currentNumber, _currentOperation, int.Parse(btnText.text));
                int? tmpNumber = MainLogic.Operation( (int)_currentNumber, _currentOperation, int.Parse(btnText.text));
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
                GoodGamePannel.gameObject.SetActive(true);                
            }
        }        
    }
    public void OperationClick(string oper)
    {
        if (outputField.text != string.Empty)
        {
            ///string oper = string.Empty;
            _currentOperation = MainLogic.StringToOperationParsing( oper);
            outputField.text = _currentNumber.ToString() + MainLogic.OperationToString(_currentOperation);
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
    
    public void QuitGame()
    {
        StopCounting();
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
