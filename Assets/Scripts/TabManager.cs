using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<ComputerTab> tabs = new List<ComputerTab>();
    [SerializeField] private GameObject browser;
    [SerializeField] private int newsTabIndex = 1;
    void Start()
    {
        //for each tab, add all listeners

        foreach (ComputerTab thistab in tabs)
        {
            foreach(Button button in thistab.toggleButtons)
            {
                button.onClick.AddListener(thistab.ToggleTab);
            }

            foreach (Button button in thistab.openButtons)
            {
                button.onClick.AddListener(thistab.OpenTab);
            }

            foreach (Button button in thistab.closeButtons)
            {
                button.onClick.AddListener(thistab.CloseTab);
            }

        }

        DayManager.Instance.onDayEnd.AddListener(StartOnNews);


    }
    public void ToggleBrowser()
    {
        browser.SetActive(!browser.activeSelf);
    }
    public void OpenBrowser()
    {
        browser.SetActive(true);
    }
    public void CloseBrowser()
    {
        browser.SetActive(false);
    }



    // make it the news site 
    public void StartOnNews()
    {
        OpenBrowser();
        for (int i = 0; i < tabs.Count; i++)
        {
            ComputerTab thistab = tabs[i];
            thistab.CloseTab();
            if(i == newsTabIndex)
            {
                thistab.OpenTab();
            }
        }
    }

}
[System.Serializable]
public class ComputerTab
{
    public GameObject tab;
    public List<Button> toggleButtons; //buttons that will open the tab 
    public List<Button> closeButtons; //buttons that will close the tab
    public List<Button> openButtons; //buttons that will open the tab

    public void ToggleTab()
    {
        tab.SetActive(!tab.activeSelf);
    }
    public void OpenTab()
    {
        tab.SetActive(true);
    }
    public void CloseTab()
    {
        tab.SetActive(false);
    }

}