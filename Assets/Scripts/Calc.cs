using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Calc : MonoBehaviour
{
    public Text output;
    public Text number1Variable;
    public Text number2Variable;
    public Text rememberNumVariable;
    public Text opVariable;
    public Text rememberOpVariable;
    public string input;
    public float timer = 0.0f;
    public float hoverDuration = 2.0f;
    public bool hovering = false, add = false;
    public Color buttonColor;
    public Color hoverColor;

    public void Start()
    {
        output = GameObject.Find("OutputText").GetComponentInChildren<Text>();
        number1Variable= GameObject.Find("number1Variable").GetComponentInChildren<Text>();
        number2Variable = GameObject.Find("number2Variable").GetComponentInChildren<Text>();
        rememberNumVariable = GameObject.Find("rememberNumVariable").GetComponentInChildren<Text>();
        opVariable = GameObject.Find("opVariable").GetComponentInChildren<Text>();
        rememberOpVariable = GameObject.Find("rememberOpVariable").GetComponentInChildren<Text>();
        output.text = "0";
        number1Variable.text = "";
        number2Variable.text = "";
        rememberNumVariable.text = "";
        opVariable.text = "";
        rememberOpVariable.text = "";
    }

    void Update()
    {
        ColorUtility.TryParseHtmlString("#D8F8EE", out buttonColor);
        ColorUtility.TryParseHtmlString("#D2668A", out hoverColor);

        if (hovering)
        {
            Color color = Color.Lerp(buttonColor, hoverColor, timer / hoverDuration);
            timer += Time.deltaTime;
            GetComponent<Image>().color = color;

            if (timer > hoverDuration)
            {
                //Debug.Log("CLICK");
                timer = 0.0f;
                add = true;
                onClick();
                add = false;
                //GetComponent<Image>().color = buttonColor;
            }
        }
    }
    
    public void onClick()
    {
        string buttonText;
        if (add)
        {
            buttonText = input;
        }
        else
        {
            timer = 0.0f;
            buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        }
        //string buttonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        string answer = output.text;
        string number1 = number1Variable.text;
        string number2 = number2Variable.text; //the empty string represents a zero or otherwise it would be harder to determine if a second number has actually been input in the case that the user inputs a first number then hits an operator and then the enter/submit/equals button; secondNum should never be zero unless the user explicitly assigns it zero
        string rememberNum = rememberNumVariable.text;
        string op = opVariable.text;
        string rememberOp = rememberOpVariable.text;
        if (buttonText == "ENTER") //could turn this into a larger switch statement but that could lead to less readability
        {
            if (number2 != "")
            {
                switch (op)
                {
                    case "+":
                        answer = (Convert.ToDouble(number1) + Convert.ToDouble(number2)).ToString();
                        break;
                    case "-":
                        answer = (Convert.ToDouble(number1) - Convert.ToDouble(number2)).ToString();
                        break;
                    case "x":
                        answer = (Convert.ToDouble(number1) * Convert.ToDouble(number2)).ToString();
                        break;
                    case "/":
                        answer = (Convert.ToDouble(number1) / Convert.ToDouble(number2)).ToString();
                        break;
                }
                rememberOp = op; //in case the user wants to continue performing the same operation on the new output value
                rememberNum = number2; //in case the user wants to continue performing the same operation on the new output value
                op = ""; //op gets no value after an expression is evaluated
                number2 = ""; //secondNum gets no value after an expression is evaluated
            }
            else //no second number input
            {
                if (op == "")
                {
                    if (rememberOp != "")
                    {
                        switch (rememberOp)
                        {
                            case "+":
                                answer = (Convert.ToDouble(number1) + Convert.ToDouble(rememberNum)).ToString();
                                break;
                            case "-":
                                answer = (Convert.ToDouble(number1) - Convert.ToDouble(rememberNum)).ToString();
                                break;
                            case "x":
                                answer = (Convert.ToDouble(number1) * Convert.ToDouble(rememberNum)).ToString();
                                break;
                            case "/":
                                answer = (Convert.ToDouble(number1) / Convert.ToDouble(rememberNum)).ToString();
                                break;
                            case "+/-":
                                answer = (Convert.ToDouble(number1) * -1).ToString();
                                break;
                        }
                        number1 = answer; //update the first input number
                    }
                    else
                    {
                        answer = number1; //no operator was ever input, so just return the first input number
                    }
                }
                else
                {
                    switch (op)
                    {
                        case "+":
                            answer = (Convert.ToDouble(number1) + Convert.ToDouble(number1)).ToString(); //aka 2*firstNum
                            break;
                        case "-":
                            answer = (Convert.ToDouble(number1) - Convert.ToDouble(number1)).ToString(); //aka 0
                            break;
                        case "x":
                            answer = (Convert.ToDouble(number1) * Convert.ToDouble(number1)).ToString();
                            break;
                        case "/":
                            answer = (Convert.ToDouble(number1) / Convert.ToDouble(number1)).ToString(); //aka 1
                            break;
                        case "+/-":
                            answer = (Convert.ToDouble(number1) * -1).ToString();
                            break;
                    }
                    number1 = answer;
                    rememberOp = op;
                    rememberNum = number1; //unique instance of actually remembering the first number input instead of the second
                    op = ""; //op gets no value after an expression is evaluated
                }
            }
        }
        else if (buttonText == "CLEAR") //aka clear everything
        {
            answer = "0";
            number1 = "";
            number2 = "";
            rememberNum = "";
            op = "";
            rememberOp = "";
        }
        else if (buttonText == "BACKSPACE")
        {
            //for possible future implementation
        }
        else
        {
            switch (buttonText)
            {
                case "+":
                case "-":
                case "x":
                case "/":
                case "+/-":
                    if (number1 == "") //must initialize firstNum if the user didn't
                    {
                        number1 = "0";
                    }
                    if (number2 != "") //must evaluate previous expression first; op has a value if secondNum does (and that value is not "+/-")
                    {
                        switch (op)
                        {
                            case "+":
                                answer = (Convert.ToDouble(number1) + Convert.ToDouble(number2)).ToString();
                                break;
                            case "-":
                                answer = (Convert.ToDouble(number1) - Convert.ToDouble(number2)).ToString();
                                break;
                            case "x":
                                answer = (Convert.ToDouble(number1) * Convert.ToDouble(number2)).ToString();
                                break;
                            case "/":
                                answer = (Convert.ToDouble(number1) / Convert.ToDouble(number2)).ToString();
                                break;
                        }
                        number1 = answer; //in case the user immediately presses a new operator
                        rememberNum = number2;
                        rememberOp = op;
                        number2 = "";
                        op = ""; //operator gets no value after an expression is evaluated (doesn't really matter as op is about to be assigned the value of the last button pressed)
                    }
                    if (buttonText == "+/-") //immediately resolve operation as only one input number is required
                    {
                        answer = (Convert.ToDouble(number1) * -1).ToString();
                        number1 = answer; //update first input number
                        rememberOp = op;
                        op = ""; //operator gets no value after an expression is evaluated
                        rememberNum = number2; //secondNum should be an empty string by this point
                        break;
                    }
                    op = buttonText;
                    break;
                case ".":
                    if (op == "")
                    {
                        if (number1 == "")
                        {
                            number1 += "0"; //can't forget leading zero
                        }
                        else if (number1.Contains(".")) //can't have two decimals in the same number
                        {
                            answer = number1;
                            break;
                        }
                        number1 += buttonText;
                        answer = number1;
                    }
                    else //an operator was selected, so new numbers refer to the second number input
                    {
                        if (number2 == "")
                        {
                            number2 += "0"; //can't forget leading zero
                        }
                        else if (number2.Contains(".")) //can't have two decimals in the same number
                        {
                            answer = number2;
                            break;
                        }
                        number2 += buttonText;
                        answer = number2;
                    }
                    break;
                case "0":
                    if (op == "")
                    {
                        if (number1 == "0" && buttonText == "0") //redundant zero clicked by the user
                        {
                            answer = number1; //should be unnecessary
                            break;
                        }
                        number1 += buttonText;
                        answer = number1;
                    }
                    else //an operator was selected, so new numbers refer to the second number input
                    {
                        if (number2 == "0" && buttonText == "0") //redundant zero clicked by the user
                        {
                            answer = number2; //should be unnecessary
                            break;
                        }
                        number2 += buttonText;
                        answer = number2;
                    }
                    break;
                //could replace all the following with simply default case or could reserve default case for error checking
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    if (op == "")
                    {
                        if (number1 == "0")
                        {
                            number1 = buttonText;
                        }
                        else
                        {
                            number1 += buttonText;
                        }
                        answer = number1;
                    }
                    else //an operator was selected, so new numbers refer to the second number input
                    {
                        if (number2 == "0")
                        {
                            number2 = buttonText;
                        }
                        else
                        {
                            number2 += buttonText;
                        }
                        answer = number2;
                    }
                    break;
            }
        }

        Debug.Log(answer);
        output.text = answer;
        this.number1Variable.text = number1;
        number2Variable.text = number2;
        rememberNumVariable.text = rememberNum;
        opVariable.text = op;
        rememberOpVariable.text = rememberOp;
    }

    public void MouseEnter(BaseEventData eventData)
    {
        input = name;
        //Debug.Log("Mouse is over GameObject " + name);
        timer = 0.0f;
        hovering = true;
    }

    public void MouseExit(BaseEventData eventData)
    {
        ColorUtility.TryParseHtmlString("#D8F8EE", out buttonColor);
        GetComponent<Image>().color = buttonColor;
        hovering = false;
        //Debug.Log("Mouse is exiting GameObject.");
    }
}