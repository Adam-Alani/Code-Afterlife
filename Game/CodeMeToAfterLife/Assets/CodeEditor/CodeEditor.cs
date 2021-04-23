using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using AfterlifeInterpretor;

public class CodeEditor : MonoBehaviour
{
    
    public Button outputButton;
    public TMPro.TMP_Text lineNumbers;
    public TMPro.TMP_Text codeUI;
    public Text outputText;
    public Image caret;
    public string code { get; set; }
    public int lineIndex;
    public int charIndex;
    public float blinkRate = 1;
    float lastInputTime;
    float timer;
    const string indentString = "  ";

    private Interpretor interpretor; 
    

    // Start is called before the first frame update
    public void Start()
    {

        outputButton.onClick.AddListener(executeCode);

        if (code == null) {
            code = "";
        }
        charIndex = code.Length;

        CustomInput.instance.RegisterKey (KeyCode.Backspace);
        CustomInput.instance.RegisterKey (KeyCode.LeftArrow);
        CustomInput.instance.RegisterKey (KeyCode.RightArrow);
        CustomInput.instance.RegisterKey (KeyCode.UpArrow);
        CustomInput.instance.RegisterKey (KeyCode.DownArrow);

        interpretor = new Interpretor();
    }

    
    void HandleTextInput () {
        string input = Input.inputString;
        if (!Input.GetKey (KeyCode.LeftControl) && !Input.GetKey (KeyCode.LeftCommand)) {
            foreach (char c in input) {
                lastInputTime = Time.time;
                if (string.IsNullOrEmpty (code) || charIndex == code.Length) {
                    code += c;
                } else {
                    code = code.Insert (charIndex, c.ToString ());
                }
                charIndex++;
            }
        }
    }

    void HandleSpecialInput () {
        if (Input.GetKeyDown (KeyCode.Return)) {
            lastInputTime = Time.time;
            if (string.IsNullOrEmpty (code) || charIndex == code.Length) {
                code += "\n";

            } else {
                code = code.Insert (charIndex, "\n");
            }
            charIndex++;
            lineIndex++;
        }

        if (CustomInput.instance.GetKeyPress (KeyCode.Backspace)) {
            if (charIndex > 0) {
                lastInputTime = Time.time;
                char deletedChar = code[charIndex - 1];
                string start = code.Substring (0, charIndex - 1);
                string end = code.Substring (charIndex, code.Length - charIndex);
                code = start + end;
                charIndex--;
                if (deletedChar == '\n') {
                    lineIndex--;
                }
            }
        }

        if (CustomInput.instance.GetKeyPress (KeyCode.LeftArrow)) {
            lastInputTime = Time.time;
            if (code.Length > 0 && charIndex > 0) {
                if (code[charIndex - 1] == '\n') {
                    lineIndex--;
                }
            }
            charIndex = Mathf.Max (0, charIndex - 1);
        }
        if (CustomInput.instance.GetKeyPress (KeyCode.RightArrow)) {
            lastInputTime = Time.time;
            if (code.Length > charIndex) {
                if (code[charIndex] == '\n') {
                    lineIndex++;
                }
            }
            charIndex = Mathf.Min (code.Length, charIndex + 1);
        }
        if (CustomInput.instance.GetKeyPress (KeyCode.UpArrow)) {
            if (lineIndex > 0) {
                lastInputTime = Time.time;
                string[] lines = code.Split ('\n');
                int numCharsInPreviousLines = 0;
                for (int i = 0; i < lineIndex; i++) {
                    numCharsInPreviousLines += lines[i].Length + 1;
                }
                charIndex = numCharsInPreviousLines - 1;
                lineIndex--;
            }
        }

        if (CustomInput.instance.GetKeyPress (KeyCode.DownArrow)) {
            string[] lines = code.Split ('\n');

            if (lineIndex < lines.Length - 1) {
                lastInputTime = Time.time;

                int numCharsInPreviousLines = lines[0].Length;
                for (int i = 1; i <= lineIndex + 1; i++) {
                    numCharsInPreviousLines += lines[i].Length + 1;
                }
                charIndex = numCharsInPreviousLines;
                lineIndex++;
            }
        }
    }
    
    private string autoIndentation()
    {
        string formattedCode = "";
        string[] lines = code.Split ('\n');

        int indentLevel = 0;
        for (int i = 0; i < lines.Length; i++) {
            string line = lines[i];
            if (line.Contains ("}")) {
                indentLevel--;
            }
            

            for (int j = 0; j < indentLevel; j++) {
                line = indentString + line;
            }

            formattedCode += line;
            if (i < lines.Length - 1) {
                formattedCode += "\n";
            }

            if (line.Contains ("{")) {
                indentLevel++;
            }
        }
        return formattedCode;
        
    }
    
    void formatLines () {
        string numbers = "";

        int numLines = code.Split ('\n').Length;
        for (int i = 0; i < numLines; i++) {
            numbers += (i + 1) + "\n";
        }
        lineNumbers.text = numbers;
    }
    
    public void CaretTimer()
    {
        timer += Time.deltaTime;
        if (Time.time - lastInputTime < blinkRate / 2) {
            caret.enabled = true;
            timer = 0;
        } else {
            caret.enabled = (timer % blinkRate < blinkRate / 2);
        }
    }

    public void CaretPosition(string text)
    {
        
        string stopChar = ".";

        // Get single line height, and height of code up to charIndex
        codeUI.text = stopChar;
        float stopCharWidth = codeUI.preferredWidth;
        float singleLineHeight = codeUI.preferredHeight;

        string codeUpToCharIndex = text.Substring (0, charIndex);
        codeUI.text = codeUpToCharIndex + stopChar;
        float height = codeUI.preferredHeight - singleLineHeight;

        // Get indent level
        int indentLevel = 0;
        string[] lines = text.Split ('\n');
        bool startIndentationNextLine = false;
        for (int i = 0; i <= lineIndex; i++) {
            if (startIndentationNextLine) {
                startIndentationNextLine = false;
                indentLevel++;
            }
            if (lines[i].Contains ("{")) {
                startIndentationNextLine = true;
            }
            if (lines[i].Contains ("}")) {
                if (startIndentationNextLine) {
                    startIndentationNextLine = false;
                } else {
                    indentLevel--;
                }
            }
        }

        // Get string from start of current line up to caret
        string textUpToCaretOnCurrentLine = "";
        for (int i = charIndex - 1; i >= 0; i--) {
            if (code[i] == '\n' || i == 0) {
                textUpToCaretOnCurrentLine = text.Substring (i, charIndex - i);
                break;
            }
        }
        textUpToCaretOnCurrentLine = textUpToCaretOnCurrentLine.Replace ("\n", "");
        for (int i = 0; i < indentLevel; i++) {
            textUpToCaretOnCurrentLine = indentString + textUpToCaretOnCurrentLine;
        }

        codeUI.text = textUpToCaretOnCurrentLine + stopChar;
        float width = codeUI.preferredWidth - stopCharWidth;

        caret.rectTransform.position = codeUI.rectTransform.position;
        caret.rectTransform.localPosition += Vector3.right * (width + caret.rectTransform.rect.width / 2f);
        caret.rectTransform.localPosition += Vector3.down * (caret.rectTransform.rect.height / 2 + height);
   
    
    }
    
    private string textFormatter(string arg0)
    {
        string text = arg0;
        
        string pattern = @"(?<=\bfunction\b)(.*?)(?=\{)";
        text = Regex.Replace(text, pattern,  "<color=#03fc17>$&</color>"   , RegexOptions.IgnoreCase);
        
        string[] initKeywords = {"int", "var", "bool","return"};
        text = Regex.Replace(text, "\\b" + string.Join("\\b|\\b", initKeywords) + "\\b", "<color=#ff00d4>$&</color>" );

        string[] expKeywords = {"for", "if", "else", "while"};
        text = Regex.Replace(text, "\\b" + string.Join("\\b|\\b", expKeywords) + "\\b", "<color=#0014ed>$&</color>" );
        

        
        
        text = text.Replace("function", "<color=#E0E300>function</color>");
        
        return text;
    }

    public void executeCode() {

        interpretor = new Interpretor();
        Debug.Log(code);
        EvaluationResults er = interpretor.Interpret(code.ToLower());
        outputText.GetComponent<Text>().text = er.ToString();
        
    }

    
    
    // Update is called once per frame
    void Update()
    {

        

        HandleTextInput ();
        HandleSpecialInput ();

        formatLines();
        
        if (codeUI != null && caret != null) {
            CaretTimer();
            CaretPosition(code);
        }
        
        string formattedCode = autoIndentation();
        codeUI.text = textFormatter(formattedCode);


    }


}

    
