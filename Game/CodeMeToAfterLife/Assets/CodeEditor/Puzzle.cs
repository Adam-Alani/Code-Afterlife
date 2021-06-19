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
        Functions.Add(SetIsActivated);

        Functions.Add(SetRange);

        Functions.Add(Multiply);
        Functions.Add(Divide);

        Functions.Add(Factorial);
        Functions.Add(Fibo);
    }


    public (bool, string) Evaluate(Interpretor interpretor, int index)
    {
        return Functions[index](interpretor);
    }

    public (bool, string) SetIsActivated(Interpretor interpretor)
    {
        AfterlifeInterpretor.EvaluationResults er = interpretor.Interpret("isActivated");
        if (er is null)
            Debug.Log("NULLLLL JEAN - Pierre NULLLLLLLLLLLLLLLL");
        else
            Debug.Log(er.ToString());
        return ((bool)er.Value == false, er.ToString()); 
    }

    public (bool, string) SetRange(Interpretor interpretor)
    {
        string er = interpretor.Interpret("range").ToString();
        return (er == "0", er); 
    }


    public (bool, string) Multiply(Interpretor interpretor)
    {
        string er = interpretor.Interpret("multiply 4, 2").ToString();
        if (er != "8")
            return (false, er); 

        er = interpretor.Interpret("multiply 4, 4").ToString();
        if (er != "16")
            return (false, er); 
        
        er = interpretor.Interpret("multiply 10, 4").ToString();
        if (er != "40")
            return (false, er); 
        
        er = interpretor.Interpret("multiply 10, 10").ToString();
        return (er == "100", er); 
    }

    public (bool, string) Divide(Interpretor interpretor)
    {
        string er = interpretor.Interpret("divide 4, 2").ToString();
        if (er != "2")
            return (false, er); 

        er = interpretor.Interpret("divide 4, 4").ToString();
        if (er != "1")
            return (false, er); 
        
        er = interpretor.Interpret("divide 12, 4").ToString();
        if (er != "3")
            return (false, er); 
        
        er = interpretor.Interpret("divide 100, 10").ToString();
        return (er == "10", er); 
    }


    public (bool, string) Factorial(Interpretor interpretor)
    {
        string er = interpretor.Interpret("facto 1").ToString();
        if (er != "1")
            return (false, er); 

        er = interpretor.Interpret("facto 2").ToString();
        if (er != "2")
            return (false, er); 
        
        er = interpretor.Interpret("facto 3").ToString();
        if (er != "6")
            return (false, er); 
        
        er = interpretor.Interpret("facto 10").ToString();
        return (er == "3628800", er); 
    }


    public (bool, string) Fibo(Interpretor interpretor)
    {
        string er = interpretor.Interpret("fibo 0").ToString();
        if (er != "0")
            return (false, er); 

        er = interpretor.Interpret("fibo 1").ToString();
        if (er != "1")
            return (false, er); 
        
        er = interpretor.Interpret("fibo 4").ToString();
        if (er != "3")
            return (false, er); 
        
        er = interpretor.Interpret("fibo 42").ToString();
        return (er == "267914296", er); 
    }




}
