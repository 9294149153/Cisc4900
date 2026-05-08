using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CanvasManager : MonoBehaviour
{
   [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private TextMeshProUGUI healthDisplay;

   
    private bool isPaused;


    [Header("TransferOnLoad")]
    [SerializeField] private List<GameObject> imageToLoad=  new List<GameObject>();
    [SerializeField] private float transferTime;


    [Header("PlayMenu")]
    [SerializeField ]private GameObject playMenuPanel;
    private bool isPlayMenuActive;
    private void Start()
    {
        isPlayMenuActive = false;
        isPaused = isPlayMenuActive;
       SetPlayMenuPanelActive(false);
        isPaused = false;



        if (imageToLoad.Count > 0)
        {
            StartCoroutine(TransferImageWaitTimer(transferTime));
        }


    }


    private void Update()
    {
        if (healthDisplay != null && playerHealth != null)
        {
            healthDisplay.text = playerHealth.PlayerCurrentHealth.ToString();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPlayMenuActive)
            {
                SetPlayMenuPanelActive(true);
                PuaseAndStartGame();
            }
            else if(isPlayMenuActive)
            {
                SetPlayMenuPanelActive(false);
                PuaseAndStartGame();
            }
            else
            {

            }
        }
    }

    public void SetPlayMenuPanelActive(bool value)
    {
        if (playMenuPanel != null)
        {
            playMenuPanel.SetActive(value);
            isPlayMenuActive=value;
        }
    }



    public void PuaseAndStartGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause
        }
        else
        {
            Time.timeScale = 1f; // Resume
        }
    }

    private void DeactiveImageAtBegining()
    {
        if (imageToLoad == null ||imageToLoad.Count==0) return;

        foreach (var image in imageToLoad)
        {
            if (image.activeInHierarchy)
            {
                image.SetActive(false);
            }
        }


    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");//load to the scene
    }
    public void ExitGame()
    {
        //Game Exit
        Application.Quit();
    }
    private IEnumerator TransferImageWaitTimer(float value)
    {
        DeactiveImageAtBegining();

        for (int i = 0; i < imageToLoad.Count; i++)
        {
            imageToLoad[i].SetActive(true);
            yield return new WaitForSeconds(value);
            imageToLoad[i].SetActive(false);
        }

        Debug.Log("TransferImageWaitTimer Called");
    }
}
