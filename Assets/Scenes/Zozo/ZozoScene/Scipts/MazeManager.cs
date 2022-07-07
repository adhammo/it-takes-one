using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{

    /*General Variable*/


    /* Idle State Varibles*/
    public GameObject MostOutterWall, MostOutterButton,MostOutterDoor;
    public bool IdleStateBotsDied = false;

    /* Middle Ring State Variables */
    public GameObject MiddleWall , MiddleButton , MiddleDoor ;
    public bool MiddleRingStateBotsDied = false;

    /* Inner Ring State Variables */
    public GameObject MostInnerWall, MostInnerButton, MostInnerDoor;
    public bool InnerRingStateBotsDied = false;

    /* Travelling Ring State Variables */
    public GameObject SpaceTimeGate;
    public int CanTravelCounter = 0;

    /* EndLevel Ring State Variables */
    public bool ReturnToMaze = false;
    public bool BossisDied = false;
    public Animator MostInnerMazeAnimator;
    public GameObject Boss;

    /* States Definition*/
    public enum State
    {
        Idle,
        MiddleRing,
        InnerRing,
        Travelling,
        EndLevel,
    }

    public State CurrentState;

    void Start()
    {
        /* Define Initial State*/
        CurrentState = State.Idle;

        /* Show Doors and Hide Buttons and SpaceTime Gate*/
        MostOutterButton.SetActive(false);
        MiddleButton.SetActive(false);
        MostInnerButton.SetActive(false); 
        SpaceTimeGate.SetActive(false);    

        MostOutterDoor.SetActive(true);  
        MiddleDoor.SetActive(true);
        MostInnerDoor.SetActive(true);                         
    }


    void Update()
    {
        switch (CurrentState)
        {
            default:

            case State.Idle:       
                                    CheckIdleStateBots();
                                    break;
            case State.MiddleRing:
                                    CheckMiddleRingStateBots();
                                    break;
            case State.InnerRing:
                                    CheckInnerRingStateBots();
                                    break;
            case State.Travelling:
                                    CheckTravelling();
                                    break;
            case State.EndLevel:
                                    CheckReturn();
                                    break;                                                                                                                                              
        }
    }

                                                              /* Functions Used in Script*/

    /* General Functions */


    /* Idle State Functions */
    private void CheckIdleStateBots()
    {
        if (IdleStateBotsDied)
        {
            MostOutterButton.SetActive(true);   /* Show Idle State Button */
            Destroy(MostOutterDoor);            /* Open the Ring Door*/
            IdleStateBotsDied = false;          /* Reset Flag*/
            CurrentState = State.MiddleRing;    /* Change Current State to Next State */
        }
    }

    /* Middle Ring State Functions */
    private void CheckMiddleRingStateBots()
    {
        if (MiddleRingStateBotsDied)
        {
            MiddleButton.SetActive(true);       /* Show Idle State Button */
            Destroy(MiddleDoor);                /* Open the Ring Door*/
            MiddleRingStateBotsDied = false;    /* Reset Flag*/
            CurrentState = State.InnerRing;    /* Change Current State to Next State */
        }
    }

    /* Inner Ring State Functions */
    private void CheckInnerRingStateBots()
    {
        if (InnerRingStateBotsDied)
        {
            MostInnerButton.SetActive(true);       /* Show Idle State Button */
            MostInnerDoor.SetActive(false);        /* Open the Ring Door*/
            InnerRingStateBotsDied = false;        /* Reset Flag*/
            CurrentState = State.Travelling;       /* Change Current State to Next State */
        }
    }

    /* Travelling State Functions */
    public void CheckTravelling()
    {
        if(CanTravelCounter==3)
        {
            SpaceTimeGate.SetActive(true);     /* Show Travelling Gate */

            /* Key has been taken*/

            CurrentState = State.EndLevel;    /* Change Current State to Next State */           
        }
    }

    /* End Level State Functions */
    public void CheckReturn()
    {
        if (PlayerPrefs.GetInt("Level")==3)
        {
            /* DO SMTH!*/
            MostInnerDoor.SetActive(true);                        /* Close the Door */
            MostInnerMazeAnimator.SetBool("Stand",false);         /* Start Rotating the Maze */
            SpaceTimeGate.SetActive(false);                       /* Disappear Travelling Gate */

            /* Start Fighting the Boss */
            Boss.SetActive(true);

            /* After Boss is Died */

            if (BossisDied)
            {
                MostInnerDoor.SetActive(false);                        /* Open the Door */
                MostInnerMazeAnimator.SetBool("Stand",true);           /* Stop Rotating the Maze */
                ReturnToMaze = false;

                /*Game Ends Here*/ 
                PlayerPrefs.SetInt("Level" , 4 );

                /* Add Some UI  ( IT TAKES ONE !) */ 
            }  

            

        }
    }                          
}
