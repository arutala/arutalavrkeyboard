using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AruKeyboardManager : MonoBehaviour
{
    #region Variables
    [Header("Button")]
    [SerializeField]
    private GameObject inputField;

    private string tempText = "";
    private bool capslockPressed;

    private GameObject aruKeyboard;
    private GameObject lowercaseKeyboard;
    private GameObject uppercaseKeyboard;
    private GameObject symbolKeyboard;

    #endregion

    #region State
    public enum KeyboardState
    {
        Uppercase,
        Lowercase,
        Symbol
    }

    [HideInInspector]
    public KeyboardState keyboardState;

    private enum ShiftState
    {
        ShiftSelected,
        KeySelected,
        Idle
    }

    private ShiftState shiftState;
#endregion

    void Start()
    {
        inputField.SetActive(true);
        ResetInputField();
        aruKeyboard = GameObject.Find("AruKeyboard");
        lowercaseKeyboard = aruKeyboard.transform.GetChild(1).GetChild(0).gameObject;
        uppercaseKeyboard = aruKeyboard.transform.GetChild(1).GetChild(1).gameObject;
        symbolKeyboard = aruKeyboard.transform.GetChild(1).GetChild(2).gameObject;
    }

    //Input the letters on the keyboard into the input field
    public void InputText(string text)
    {
        tempText += text;
        UpdateText();
        ResetShift();
    }

    //Get the keyed texts
    public string GetText()
    {
        return tempText;
    }

    //Update the input field
    public void UpdateText()
    {
        inputField.GetComponentInChildren<TMP_InputField>().text = tempText + "_";
    }

    //Input Field Deselected
    public void DeselectInput()
    {
        string text = inputField.GetComponentInChildren<TMP_InputField>().text;
        string textDeselect = text.Remove(text.Length - 1);
        inputField.GetComponentInChildren<TMP_InputField>().text = textDeselect;
    }

    //Reset the input field into default state
    public void ResetInputField()
    {
        tempText = "";
        inputField.GetComponentInChildren<TMP_InputField>().text = tempText;
    }

    //Function to reset keyboard to the default state
    public void ResetKeyboard()
    {
        tempText = "";
        inputField.GetComponentInChildren<TMP_InputField>().text = tempText;
        keyboardState = KeyboardState.Lowercase;
        shiftState = ShiftState.Idle;
        capslockPressed = false;
        SwitchKeyboard();
    }

    //Function to switch keyboard between state
    public void SwitchKeyboard()
    {
        switch (keyboardState)
        {
            case KeyboardState.Lowercase:
                lowercaseKeyboard.SetActive(true);
                uppercaseKeyboard.SetActive(false);
                symbolKeyboard.SetActive(false);
                break;
            case KeyboardState.Uppercase:
                lowercaseKeyboard.SetActive(false);
                uppercaseKeyboard.SetActive(true);
                symbolKeyboard.SetActive(false);
                break;
            case KeyboardState.Symbol:
                lowercaseKeyboard.SetActive(false);
                uppercaseKeyboard.SetActive(false);
                symbolKeyboard.SetActive(true);
                break;
        }
    }

    #region Button Function

    //Backspace button function
    public void DeleteText()
    {
        if (tempText.Length > 0)
        {
            string text;
            text = tempText.Remove(tempText.Length - 1);
            tempText = text;
            UpdateText();
        }
    }

    //Capslock button function
    public void CapslockSelected()
    {
        switch (keyboardState)
        {
            case KeyboardState.Uppercase:
                capslockPressed = false;
                keyboardState = KeyboardState.Lowercase;
                break;
            case KeyboardState.Lowercase:
                capslockPressed = true;
                keyboardState = KeyboardState.Uppercase;
                break;
        }
        SwitchKeyboard();
    }

    //Function of Tab button
    public void TabSelected()
    {
        InputText("\t");
    }

    //Enter button function
    public void EnterSelected()
    {
        if (inputField.GetComponentInChildren<TMP_InputField>().lineType == TMP_InputField.LineType.SingleLine)
        {
            SubmitSelected();
        }
        else
        {
            InputText("\n");
        }
    }

    //Shift Button Function
    public void ShiftSelected()
    {
        shiftState = ShiftState.ShiftSelected;
        if (keyboardState == KeyboardState.Lowercase)
        {
            keyboardState = KeyboardState.Uppercase;
            shiftState = ShiftState.KeySelected;
        }
        else if (keyboardState == KeyboardState.Uppercase)
        {
            keyboardState = KeyboardState.Lowercase;
            shiftState = ShiftState.KeySelected;
        }
        SwitchKeyboard();
    }

    //Return the keyboard back to normal after shift
    public void ResetShift()
    {
        if (shiftState == ShiftState.KeySelected)
        {
            if (keyboardState == KeyboardState.Lowercase)
            {
                keyboardState = KeyboardState.Uppercase;
                shiftState = ShiftState.Idle;
            }
            else if (keyboardState == KeyboardState.Uppercase)
            {
                keyboardState = KeyboardState.Lowercase;
                shiftState = ShiftState.Idle;
            }
            SwitchKeyboard();
        }
        else
        {
            shiftState = ShiftState.Idle;
        }
    }

    //Symbol button function
    public void SymbolSelected()
    {
        switch (keyboardState)
        {
            case KeyboardState.Lowercase:
                keyboardState = KeyboardState.Symbol;
                break;
            case KeyboardState.Uppercase:
                keyboardState = KeyboardState.Symbol;
                break;
            case KeyboardState.Symbol:
                if (capslockPressed)
                {
                    keyboardState = KeyboardState.Uppercase;
                }
                else
                {
                    keyboardState = KeyboardState.Lowercase;
                }
                break;
        }
        SwitchKeyboard();
    }

    public void SubmitSelected()
    {
        ResetKeyboard();
    }

    #endregion
}
