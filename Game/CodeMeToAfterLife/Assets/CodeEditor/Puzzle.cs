using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AfterlifeInterpretor;
using System;
public class Puzzle : MonoBehaviour
{

    public List<Func<Interpretor, (bool, string)>> Functions = new List<Func<Interpretor, (bool, string)>>();


    void Start()
    {
        Functions.Add(Fibo);
        Functions.Add(Multiply);
    }


    public (bool, string) Evaluate(Interpretor interpretor, int index)
    {
        return Functions[index](interpretor);
    }

    public (bool, string) Fibo(Interpretor interpretor)
    {
        string er = interpretor.Interpret("fibo 42").ToString();
        return (er == "267914296", er); 
    }

    public (bool, string) Multiply(Interpretor interpretor)
    {
        string er = interpretor.Interpret("multiply 4, 2").ToString();
        return (er == "8", er); 
    }
}
