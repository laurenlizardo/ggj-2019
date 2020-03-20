using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    public int loadBranchIndexForNextScene;
    public int loadChoiceIndexForNextScene;
    
    private AudioSource audSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "SC00_MainMenu")
        {
            LoadSC01();
        }

        //audSource = GetComponent<AudioSource>();
        //audSource.Play();
    }

    public void LoadSC01()
    {
        SceneManager.LoadScene("SC01_Montana");
    }
}
