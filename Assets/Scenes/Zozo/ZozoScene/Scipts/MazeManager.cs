using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{

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
            Destroy(MostInnerDoor);                /* Open the Ring Door*/
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

            /* Show Some Partical Systems for the Gate */

            CurrentState = State.EndLevel;    /* Change Current State to Next State */           
        }
    }

    /* End Level State Functions */
                                   

   /* void RotateRandom(GameObject Object)
    {
        Vector3 RotationDegrees = 90*  new Vector3 (0f, Random.Range(-3,4), 0f );

        Object.transform.Rotate(RotationDegrees) ;
    }
    */
}
