using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public AntColony antColony;
    public List<GameObject> spheres; //here are spheres which are on lines
    public List<GameObject> availableSpheres; //here are spheres which we can add on line
    public Vector2 finishBase; //coordinates of finish base

    public GameObject clickOnLineEffect;

    public List<GameObject> conqueredBases; //all conquered bases


    private void Start()
    {
        if (spheres == null)
            spheres = new List<GameObject>();

        if (availableSpheres == null)
            availableSpheres = new List<GameObject>();

        if (conqueredBases == null)
            conqueredBases = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(1)) //righrt button click
            processRightButton();
        if (Input.GetMouseButtonUp(0)) //left button click
            processLeftButton();
    }

    //proces left button click (set point to line)
    void processLeftButton()
    {
        if (availableSpheres.Count == 0)
            return;

        //read mouse pos
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //and searching line with mouse coordinates
        foreach (var line in antColony.LineRenderersList)
        {
            if (new Assets.Scripts.triangulation.Line(line.GetPosition(0), line.GetPosition(1)).isPointOnLine(mousePos, line.startWidth)
                || new Assets.Scripts.triangulation.Line(line.GetPosition(1), line.GetPosition(0)).isPointOnLine(mousePos, line.startWidth))
            {
                for (int i = 0; i < conqueredBases.Count; ++i)
                {

                    //if we founded line 
                    if (conqueredBases[i].transform.position.x.Equals(line.GetPosition(0).x) && conqueredBases[i].transform.position.y.Equals(line.GetPosition(0).y)
                        || conqueredBases[i].transform.position.x.Equals(line.GetPosition(1).x) && conqueredBases[i].transform.position.y.Equals(line.GetPosition(1).y))
                    {

                        //set new values for point
                        availableSpheres[0].GetComponent<movePoint>().beginLine = new Vector2(conqueredBases[i].transform.position.x, conqueredBases[i].transform.position.y);

                        availableSpheres[0].GetComponent<Collider2D>().enabled = false;


                        if (availableSpheres[0].GetComponent<movePoint>().beginLine.x.Equals(line.GetPosition(0).x) && (availableSpheres[0].GetComponent<movePoint>().beginLine.y.Equals(line.GetPosition(0).y)))
                        {
                            availableSpheres[0].GetComponent<movePoint>().endLine = line.GetPosition(1);
                        }
                        else
                            availableSpheres[0].GetComponent<movePoint>().endLine = line.GetPosition(0);

                        availableSpheres[0].GetComponent<movePoint>().goal = availableSpheres[0].GetComponent<movePoint>().endLine;

                        availableSpheres[0].transform.position = conqueredBases[i].transform.position;

                        StartCoroutine(availableSpheres[0].GetComponent<movePoint>().turnOnCollider());

                        availableSpheres[0].SetActive(true);

                        spheres.Add(availableSpheres[0]);
                        availableSpheres.RemoveAt(0);
                        Destroy(Instantiate(clickOnLineEffect), 1);
                        break;
                    }
                }

                break;
            }
        }
    }

    //proces right button click (take point from line)
    void processRightButton()
    {
        //read mouse pos
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //and searching line with mouse coordinates
        foreach (var line in antColony.LineRenderersList)
        {
            if (new Assets.Scripts.triangulation.Line(line.GetPosition(0), line.GetPosition(1)).isPointOnLine(mousePos, line.startWidth)
                || new Assets.Scripts.triangulation.Line(line.GetPosition(1), line.GetPosition(0)).isPointOnLine(mousePos, line.startWidth))
            {
                for (int i = 0; i < spheres.Count; ++i)
                {
                    //if we founded line 
                    if (spheres[i].GetComponent<movePoint>().beginLine.x.Equals(line.GetPosition(0).x) && spheres[i].GetComponent<movePoint>().beginLine.y.Equals(line.GetPosition(0).y)
                        && spheres[i].GetComponent<movePoint>().endLine.x.Equals(line.GetPosition(1).x) && spheres[i].GetComponent<movePoint>().endLine.y.Equals(line.GetPosition(1).y)
                        || spheres[i].GetComponent<movePoint>().beginLine.x.Equals(line.GetPosition(1).x) && spheres[i].GetComponent<movePoint>().beginLine.y.Equals(line.GetPosition(1).y)
                        && spheres[i].GetComponent<movePoint>().endLine.x.Equals(line.GetPosition(0).x) && spheres[i].GetComponent<movePoint>().endLine.y.Equals(line.GetPosition(0).y))
                    {
                        //take point from line
                        spheres[i].SetActive(false);
                        availableSpheres.Add(spheres[i]);
                        spheres[i].GetComponent<Collider2D>().enabled = false;
                        spheres.RemoveAt(i);
                        Destroy(Instantiate(clickOnLineEffect), 1);
                        break;
                    }
                }
                break;
            }
        }
    }
}
