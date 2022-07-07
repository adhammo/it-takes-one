using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotsTracker : MonoBehaviour
{
    public int DiedBotsCounter = 0;
    public MazeManager manager;


    void Update()
    {
        TrackBots();
    }
    
    public void TrackBots()
    {
        if ( DiedBotsCounter == 6 )                  /*First Wave Died */
        {
            manager.IdleStateBotsDied = true;

        }
        else if (DiedBotsCounter == 12 )             /* Second Wave Died */
        {
            manager.MiddleRingStateBotsDied = true;
        }
        else if (DiedBotsCounter == 22)            /* Third Wave Died */
        {
            manager.InnerRingStateBotsDied = true;
        }
    }
}
