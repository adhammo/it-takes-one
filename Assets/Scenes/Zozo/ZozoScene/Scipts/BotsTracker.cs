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
        if ( DiedBotsCounter == 4)
        {
            manager.IdleStateBotsDied = true;

        }
        else if (DiedBotsCounter == 8)
        {
            manager.MiddleRingStateBotsDied = true;
        }
        else if (DiedBotsCounter == 14)
        {
            manager.InnerRingStateBotsDied = true;
        }
    }
}
