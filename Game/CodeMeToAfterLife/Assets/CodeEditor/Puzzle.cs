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
        // Tuto level
        Functions.Add(SetIsActivated); // 0

        Functions.Add(SetRange); // 1

        Functions.Add(Factorial); // 2

        Functions.Add(Syra); // 3 

        Functions.Add(Fibo); // 4
        // Level 1
        Functions.Add(Sorted); // 5

        Functions.Add(SetTarget); // 6        

        Functions.Add(Search); // 7
        
        // Level 2
        Functions.Add(Delete); // 8

        Functions.Add(IndexOf); // 9

        Functions.Add(FinishGame); // 10

    }


    public (bool, string) Evaluate(Interpretor interpretor, int index)
    {
        return Functions[index](interpretor);
    }

    public (bool, string) SetIsActivated(Interpretor interpretor)
    {
        AfterlifeInterpretor.EvaluationResults er = interpretor.Interpret("isActivated");
        return (er.Value is bool && (bool)er.Value == false, er.ToString()); 
    }

    public (bool, string) SetRange(Interpretor interpretor)
    {
        string er = interpretor.Interpret("range").ToString();
        return (er == "0", er); 
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

    public (bool, string) Syra(Interpretor interpretor)
    {
        string er = interpretor.Interpret("syra 1").ToString();
        if (er != "0")
            return (false, er); 
        
        er = interpretor.Interpret("syra 4").ToString();
        if (er != "2")
            return (false, er); 
        
        er = interpretor.Interpret("syra 17").ToString();
        if (er != "12")
            return (false, er); 
        
        er = interpretor.Interpret("syra 18").ToString();
        if (er != "20")
            return (false, er); 

        er = interpretor.Interpret("syra 19").ToString();
        return (er == "20", er); 
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

    public (bool, string) Sorted(Interpretor interpretor)
    {
        string er = interpretor.Interpret("sorted (0, 1, 2, 3, 4, 5)").ToString();
        if (er != "true")
            return (false, er); 

        er = interpretor.Interpret("sorted  (5, 1, 2, 3, 4, 5)").ToString();
        if (er != "false")
            return (false, er);
        
        er = interpretor.Interpret("sorted (42, 42, 69)").ToString();
        return (er == "true", er); 
    }

    public (bool, string) Search(Interpretor interpretor)
    {
        string er = interpretor.Interpret("search (0, 1, 2, 3, 4, 5) 0").ToString();
        if (er != "True")
            return (false, er); 

        er = interpretor.Interpret("search (0, 1, 2, 3, 4, 5) 6").ToString();
        if (er != "False")
            return (false, er); 
        
        er = interpretor.Interpret("search (0, 1, 2, 3, 4, 5) 4").ToString();
        if (er != "True")
            return (false, er); 
        
        er = interpretor.Interpret("search (0, 1, 2, 3, 4, 5, 42) 42").ToString();
        return (er == "True", er); 
    }
    public (bool, string) SetTarget(Interpretor interpretor)
    {
        string er = interpretor.Interpret("target").ToString();
        return (er == "me", er); 
    }

    public (bool, string) Delete(Interpretor interpretor)
    {
        string er = interpretor.Interpret("delete (0, 1, 2, 3, 4, 5) 0").ToString();
        if (er != "(1, 2, 3, 4, 5)")
            return (false, er); 

        er = interpretor.Interpret("delete  (0, 1, 2, 3, 4, 5) 1").ToString();
        if (er != "(0, 2, 3, 4, 5)")
            return (false, er);
        
        er = interpretor.Interpret("delete (0, 1, 2, 3, 4, 5) 4").ToString();
        if (er != "(0, 1, 2, 3, 5)")
            return (false, er); 
        
        er = interpretor.Interpret("delete (0, 1, 2, 3, 4, 5) 5").ToString();
        return (er == "(0, 1, 2, 3, 4)", er); 
    }

    public (bool, string) IndexOf(Interpretor interpretor)
    {
        string er = interpretor.Interpret("indexof (0, 1, 2, 3, 4, 5) 0").ToString();
        if (er != "(1, 2, 3, 4, 5)")
            return (false, er); 

        er = interpretor.Interpret("indexof  (0, 1, 2, 3, 4, 5) 1").ToString();
        if (er != "1")
            return (false, er);
        
        er = interpretor.Interpret("indexof (0, 1, 2, 3, 4, 5) 4").ToString();
        if (er != "4")
            return (false, er); 
        
        er = interpretor.Interpret("indexof (0, 1, 2, 3, 4, 5) 5").ToString();
        return (er == "5", er); 
    }

    public (bool, string) FinishGame(Interpretor interpretor)
    {
        string er = interpretor.Interpret("finishGame").ToString();
        return (er == "True", er); 
    }

}
