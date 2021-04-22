using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTerminal : MonoBehaviour
{
    void CloseTerminal()
    {
        SceneManager.LoadScene("Assets/Scenes/Test Terminal.unity");
        Debug.Log("Terminal is closed");
    }
}
