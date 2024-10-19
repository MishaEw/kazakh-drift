using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour
{
    [SerializeField] Camera Camera1;
    [SerializeField] Camera Camera2;
    [SerializeField] Camera Camera3;
    [SerializeField] Camera Camera4;

    private void Start()
    {
        Camera1.gameObject.SetActive(true);
        Camera2.gameObject.SetActive(false);
        Camera3.gameObject.SetActive(false);
        Camera4.gameObject.SetActive(false);
    }
    public void restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
    public void button1()
    {
        Time.timeScale = 1.0f;
        Camera1.gameObject.SetActive(true);
        Camera2.gameObject.SetActive(false);
        Camera3.gameObject.SetActive(false);
        Camera4.gameObject.SetActive(false);
    }
    public void button2()
    {
        Time.timeScale = 1.0f;
        Camera1.gameObject.SetActive(false);
        Camera2.gameObject.SetActive(true);
        Camera3.gameObject.SetActive(false);
        Camera4.gameObject.SetActive(false);
    }
    public void button3()
    {
        Time.timeScale = 1.0f;
        Camera1.gameObject.SetActive(false);
        Camera2.gameObject.SetActive(false);
        Camera3.gameObject.SetActive(true);
        Camera4.gameObject.SetActive(false);
    }
    public void button4()
    {
        Time.timeScale = 1.0f;
        Camera1.gameObject.SetActive(false);
        Camera2.gameObject.SetActive(false);
        Camera3.gameObject.SetActive(false);
        Camera4.gameObject.SetActive(true);
    }
}
