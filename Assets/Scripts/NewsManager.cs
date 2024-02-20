using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsManager : MonoBehaviour
{
    // this script has a reference to all the days'news. Order is the same as the days. Depending on the day, it changes the correct prefab to display.
    [SerializeField] private List<GameObject> newsArticles= new List<GameObject>();
    private GameObject activeArticle;
    void Start()
    {
        DayManager.Instance.onDayEnd.AddListener(UpdateNews);
        activeArticle = newsArticles[DayManager.Instance.dayCount];
        UpdateNews();
    }

    // Update is called once per frame
    public void UpdateNews() //subscribed to day end
    {
        activeArticle.SetActive(false);
        Debug.Log("new news");
        //the daymanager can't go outside its own bounds
        if(DayManager.Instance.dayCount < newsArticles.Count)
        {
            activeArticle = newsArticles[DayManager.Instance.dayCount]; 
            activeArticle.SetActive(true);
        }



    }
}
