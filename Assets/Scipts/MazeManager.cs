using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{


    /*This Class Must Control:
     * 1- Maze Walls Rotation 
     * 2- Correct Positing for Maze Walls Doors    (Most Inner ) 09:03:06 (Most Outter)

     */


    /*Maze Walls Objects*/

    public GameObject MostOutterWall,MiddleWall,MostInnerWall ;

    void Start()
    {
        RotateRandom(MostOutterWall);
        RotateRandom(MiddleWall);
        RotateRandom(MostInnerWall);
    }




    void Update()
    {
        
    }


    void RotateRandom(GameObject Object)
    {
        Vector3 RotationDegrees = 90*  new Vector3 (0f, Random.Range(-3,4), 0f );

        Object.transform.Rotate(RotationDegrees) ;
    }
}
