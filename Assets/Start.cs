using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    public void Click()
    {
        SceneManager.LoadScene(1);
    }
    public void Click2()
    {
        SceneManager.LoadScene(0);
    }
}
