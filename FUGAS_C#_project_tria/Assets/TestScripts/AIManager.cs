using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIManager : MonoBehaviour
{
    public List<GameObject> spheres;

    //public List<Assets.TestScripts.triangulation.Line> playerLines;

    public List<GameObject> conqueredBases;

    public Vector2 finishBase;

    public int maxPointCount;


    private void Start()
    {
        //playerLines = new List<Assets.TestScripts.triangulation.Line>();
        if (spheres==null)
            spheres = new List<GameObject>();

        if (conqueredBases == null)
            conqueredBases = new List<GameObject>();
    }
}
