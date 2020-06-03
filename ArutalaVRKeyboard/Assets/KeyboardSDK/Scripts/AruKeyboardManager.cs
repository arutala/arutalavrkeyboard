using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AruKeyboardManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject inputField;

    private string tempText = "";
    private bool capslockPressed;

    //Keyboards
    private GameObject aruKeyboard;
    private GameObject lowercaseKeyboard;
    private GameObject uppercaseKeyboard;
    private GameObject symbolKeyboard;

    //Buttons
    private GameObject capslockLC;
    private GameObject capslockUC;
    private GameObject shiftLC;
    private GameObject shiftUC;

    //Colors
    private Color32 normalColor = new Color32(17, 17, 19, 0);
    private Color32 selectedColor = new Color32(122, 6, 242, 192);

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

        //Buttons
        capslockLC = lowercaseKeyboard.transform.GetChild(1).GetChild(0).gameObject;
        capslockUC = uppercaseKeyboard.transform.GetChild(1).GetChild(0).gameObject;
        shiftLC = lowercaseKeyboard.transform.GetChild(2).GetChild(0).gameObject;
        shiftUC = uppercaseKeyboard.transform.GetChild(2).GetChild(0).gameObject;

        capslockPressed = false;
        keyboardState = KeyboardState.Lowercase;
        shiftState = ShiftState.Idle;
    }

    private void FixedUpdate()
    {
        if(capslockPressed == true)
        {
            ChangeNormalButtonColor(capslockLC, selectedColor);
            ChangeNormalButtonColor(capslockUC, selectedColor);
        }
        else
        {
            ChangeNormalButtonColor(capslockLC, normalColor);
            ChangeNormalButtonColor(capslockUC, normalColor);
        }
    }

    //Input the letters on the keyboard into the input field
    public void InputText(string text)
    {
        tempText += text;
        UpdateText();
        ResetShift();
        StartCoroutine(Haptics());
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
        StartCoroutine(Haptics());
        tempText = "";
        inputField.GetComponentInChildren<TMP_InputField>().text = tempText;
    }

    //Function to reset keyboard to the default state
    public void ResetKeyboard()
    {
        StartCoroutine(Haptics());
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
        StartCoroutine(Haptics());
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
        StartCoroutine(Haptics());

        if(capslockPressed == false)
        {
            capslockPressed = true;
            keyboardState = KeyboardState.Uppercase;
            if(shiftState == ShiftState.KeySelected)
            {
                shiftState = ShiftState.Idle;
                ChangeNormalButtonColor(shiftLC, normalColor);
                ChangeNormalButtonColor(shiftUC, normalColor);
            }
        }
        else if(capslockPressed == true)
        {
            capslockPressed = false;
            keyboardState = KeyboardState.Lowercase;
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
            StartCoroutine(Haptics());
            InputText("\n");
        }
    }

    //Shift Button Function
    public void ShiftSelected()
    {
        StartCoroutine(Haptics());
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
        ChangeNormalButtonColor(shiftLC, selectedColor);
        ChangeNormalButtonColor(shiftUC, selectedColor);
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
        ChangeNormalButtonColor(shiftLC, normalColor);
        ChangeNormalButtonColor(shiftUC, normalColor);
    }

    //Symbol button function
    public void SymbolSelected()
    {
        StartCoroutine(Haptics());
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

    private void ChangeNormalButtonColor(GameObject button, Color32 color32)
    {
        ColorBlock colorBlock = button.GetComponent<Button>().colors;
        colorBlock.normalColor = color32;
        button.GetComponent<Button>().colors = colorBlock;
    }

    private IEnumerator Haptics()
    {
        OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);
        yield return new WaitForSeconds(0.15f);
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
    }
}
