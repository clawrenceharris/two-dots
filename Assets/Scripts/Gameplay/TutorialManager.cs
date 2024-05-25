using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TutorialManager : MonoBehaviour
{

   private TextMeshProUGUI topText;
   private TextMeshProUGUI bottomText;
    public static event Action OnTutorialComplete;
    private int index;
    public bool isTutorial { get; private set; }
    private TutorialData[] tutorialSteps;
    private Board board;
    private Dot[] connection;


    private void Awake()
    {

        topText = GameObject.Find("Top Text").GetComponent<TextMeshProUGUI>();
        bottomText = GameObject.Find("Bottom Text").GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {
        LevelManager.OnLevelRestart += OnLevelRestart;
       

    }

    public void StartTutorial(TutorialData[] tutorialSteps,Board board)
    {
        
        this.tutorialSteps = tutorialSteps;
        this.board = board;
        isTutorial = true;
        index = 0;
        
        NextTutorialStep();
    }

   

    private void HighlightDots()
    {
        
    }

    private void DisableBlobs()
    {
        

    }

    private void OnLevelRestart()
    {
        if (isTutorial)
        {
            //reset index to zero
            index = 0;
            NextTutorialStep();
        }
        
    }


    private void OnMoveUndone()
    {
        if (isTutorial)
        {
            //go back one step
            index--;

            NextTutorialStep();
        }


    }
    private void OnMoveComplete()
    {
        if (isTutorial)
        {
            //move forward one step
            index++;

            NextTutorialStep();
        }



    }

    private void NextTutorialStep()
    {
        

        DisableBlobs();
        SetMessages();

        if (index < tutorialSteps.Length)
        {
            HighlightDots();
        }
       


    }
    private void SetMessages()
    {
        if(index < tutorialSteps.Length)
        {
            topText.text = tutorialSteps[index].topText;
            bottomText.text = tutorialSteps[index].bottomText;

        }
        else if(index == tutorialSteps.Length)
        {
            topText.text = "";
            bottomText.text = "";
        }

    }

}


