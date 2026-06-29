using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //[SerializeField] private string mainMenuSceneName = "MainMenu";
    private GameObject backgroundContainer;
    private List<GameObject> menuContainers = new List<GameObject>();
    private int currentMenuIndex = 0;

    public static PauseMenu Instance { get; private set; }

    


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

    private void Start()
    {
        backgroundContainer = transform.GetChild(0).gameObject;
        Debug.Log("Background Container: " + backgroundContainer.name, backgroundContainer);
        for (int i = 0; i < backgroundContainer.transform.childCount; i++)
        {
            Debug.Log("Child " + i + ":" + backgroundContainer.transform.GetChild(i).gameObject.name);
            menuContainers.Add(backgroundContainer.transform.GetChild(i).gameObject);
            Debug.Log("Menu Container: " + menuContainers[i].name, menuContainers[i]);
        }

    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (currentMenuIndex == 0) //Nothing is open
        {
            EnablePauseMenu();
        }
        else if (currentMenuIndex == 1) //Pause Menu is open
        {
            DisablePauseMenu();
        }
        else if(currentMenuIndex > 1) //Submenu is open
        {
            DisableSublevelMenu(currentMenuIndex);
        }
    }

    private void EnablePauseMenu()
    {
        backgroundContainer.SetActive(true);
        menuContainers[currentMenuIndex].SetActive(true);
        if (GameOrchestrator.Instance != null) GameOrchestrator.Instance.DisableLocalPlayerInput();
        currentMenuIndex = 1;
    }
    private void DisablePauseMenu()
    {
        for (int i = 0; i < menuContainers.Count; i++)
        {
            menuContainers[i].SetActive(false);
        }
        backgroundContainer.SetActive(false);
        if (GameOrchestrator.Instance != null) GameOrchestrator.Instance.EnableLocalPlayerInput();
        currentMenuIndex = 0;
    }
    private void EnableSublevelMenu(int menuIndex)
    {
        menuContainers[0].SetActive(false);
        menuContainers[menuIndex].SetActive(true);
    }
    private void DisableSublevelMenu(int menuIndex)
    {
        menuContainers[menuIndex].SetActive(false);
        menuContainers[0].SetActive(true);
        currentMenuIndex = 0;
    }

    public void OnResumeGame()
    {
        DisablePauseMenu();
    }

    public void OnMainMenu()
    {
        if (GameOrchestrator.Instance != null) GameOrchestrator.Instance.LeaveGame();
        else
        {
            //Exit game
            Application.Quit();
            //exit 
        }
    }

    public void OnOptions()
    {
        EnableSublevelMenu(1);
        throw new NotImplementedException();
    }

}
