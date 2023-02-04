using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestGivingUI : MonoBehaviour
{
    public static QuestGivingUI Instance;
    
    [SerializeField] private GameObject questPanel; 
    [SerializeField] private TextMeshProUGUI title, description, reward;
    [SerializeField] private Button accept, later;

    private QuestData currentQuest;
    
    void Start()
    {
        if (Instance) Destroy(this);
        else Instance = this;

        accept.onClick.AddListener(AcceptQuest);
        later.onClick.AddListener(RefuseQuest);
    }

    public void SetupQuest(QuestData quest)
    {
        Time.timeScale = 0;
        currentQuest = quest;
        questPanel.SetActive(true);
        title.text = quest.title;
        //Setting up the text components of the UI
    }

    void AcceptQuest()
    {
        //Add to quest list
        //Check to NPC that quest is activated
        questPanel.SetActive(false);
        Time.timeScale = 1;
    }

    void RefuseQuest()
    {
        questPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
