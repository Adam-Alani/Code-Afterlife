using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doors : MonoBehaviour
{
    public Animator animator;
    public GameObject codeEditor;

    public GameObject greenWire;
    public GameObject redWire;




    // Start is called before the first frame update
    void Start()
    {
       animator.SetBool("Open", false);
    }

    // Update is called once per frame
    void Update()
    {
        bool res = codeEditor.GetComponent<CodeEditor>().Solved;

        if (res)
        {
            animator.SetBool("Open", res);
            redWire.SetActive(false);
            greenWire.SetActive(true);

        }
    }
}
