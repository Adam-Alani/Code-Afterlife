using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class CodeEditor : MonoBehaviour
{
    public InputField mainInputField;
    public GameObject textDisplay;
    
    // Start is called before the first frame update
    public void Start()
    {
        mainInputField.onValueChanged.AddListener(FormatText);

    }

    private void FormatText(string arg0)
    {
        string text = arg0;
        
        
        text = text.Replace("function", "<color=#E0E300>function</color>");
        text = text.Replace("{", "<color=#0095ff>{</color>");
        text = text.Replace("}", "<color=#0095ff>}</color>");

        /*string pattern = @"(?<=\bvar\b)(.*?)(?=\=)";
        text = Regex.Replace(text, pattern,  "<color=#0095ff>$&</color>"   , RegexOptions.IgnoreCase);
        */

        string[] initKeywords = {"int", "var", "bool"};
        text = Regex.Replace(text, "\\b" + string.Join("\\b|\\b", initKeywords) + "\\b", "<color=#ff00d4>$&</color>" );


        textDisplay.GetComponent<Text>().text = text;
        Debug.Log(arg0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
