using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{

    public MazeManager Manager;

    public void TravellingIncreament()
    {
        Manager.CanTravelCounter++;
    }
}
