using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public List<GameObject> spheres; //all available spheres

    public List<GameObject> conqueredBases;

    public Vector2 finishBase;


    private void Start()
    {
        if (spheres == null)
            spheres = new List<GameObject>();

        if (conqueredBases == null)
            conqueredBases = new List<GameObject>();
    }
}
