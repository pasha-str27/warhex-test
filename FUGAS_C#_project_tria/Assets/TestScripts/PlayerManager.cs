using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public AntColony antColony;
    public List<GameObject> spheres;
    public List<GameObject> availableSpheres;
    public Vector2 finishBase;

    public GameObject clickOnLineEffect;

    //public static List<Assets.TestScripts.triangulation.Line> playerLines;

    public List<GameObject> conqueredBases;


    private void Start()
    {
        //playerLines = new List<Assets.TestScripts.triangulation.Line>();
        if (spheres == null)
            spheres = new List<GameObject>();
        //else
        //    if(ButtonManager.newGame)
        //        spheres.Clear();
        if (availableSpheres == null)
            availableSpheres = new List<GameObject>();
        //else
        //    availableSpheres.Clear();

        if (conqueredBases == null)
            conqueredBases = new List<GameObject>();
        //else
        //    if (ButtonManager.newGame)
        //        conqueredBases.Clear();
        //ButtonManager.newGame = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(1))
            processRightButton();
        if (Input.GetMouseButtonUp(0))
            processLeftButton();
    }

    void processLeftButton()
    {
        if (availableSpheres.Count == 0)
            return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (var line in antColony.LineRenderersList)
        {
            if (new Assets.TestScripts.triangulation.Line(line.GetPosition(0), line.GetPosition(1)).isPointOnLine(mousePos, line.startWidth)
                || new Assets.TestScripts.triangulation.Line(line.GetPosition(1), line.GetPosition(0)).isPointOnLine(mousePos, line.startWidth))
            {
                //print(line.GetPosition(0) + "____________________________" + line.GetPosition(1));
                //print(mousePos);
                for (int i = 0; i < conqueredBases.Count; ++i)
                {
                    //print(conqueredBases[i].transform.position);
                    //print(line.GetPosition(0) + "____________________________" + line.GetPosition(1));
                    if (conqueredBases[i].transform.position.x.Equals(line.GetPosition(0).x) && conqueredBases[i].transform.position.y.Equals(line.GetPosition(0).y)
                        || conqueredBases[i].transform.position.x.Equals(line.GetPosition(1).x) && conqueredBases[i].transform.position.y.Equals(line.GetPosition(1).y))
                    //|| spheres[i].GetComponent<movePoint>().beginLine.x.Equals(line.GetPosition(1).x) && spheres[i].GetComponent<movePoint>().beginLine.y.Equals(line.GetPosition(1).y)
                    //&& spheres[i].GetComponent<movePoint>().endLine.x.Equals(line.GetPosition(0).x) && spheres[i].GetComponent<movePoint>().endLine.y.Equals(line.GetPosition(0).y))
                    {
                        //print(line.GetPosition(0) + "____________________________" + line.GetPosition(1));
                        //додавання сфери на лінію
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

    void processRightButton()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (var line in antColony.LineRenderersList)
        {
            if (new Assets.TestScripts.triangulation.Line(line.GetPosition(0), line.GetPosition(1)).isPointOnLine(mousePos, line.startWidth)
                || new Assets.TestScripts.triangulation.Line(line.GetPosition(1), line.GetPosition(0)).isPointOnLine(mousePos, line.startWidth))
            {
                for (int i = 0; i < spheres.Count; ++i)
                {
                    if (spheres[i].GetComponent<movePoint>().beginLine.x.Equals(line.GetPosition(0).x) && spheres[i].GetComponent<movePoint>().beginLine.y.Equals(line.GetPosition(0).y)
                        && spheres[i].GetComponent<movePoint>().endLine.x.Equals(line.GetPosition(1).x) && spheres[i].GetComponent<movePoint>().endLine.y.Equals(line.GetPosition(1).y)
                        || spheres[i].GetComponent<movePoint>().beginLine.x.Equals(line.GetPosition(1).x) && spheres[i].GetComponent<movePoint>().beginLine.y.Equals(line.GetPosition(1).y)
                        && spheres[i].GetComponent<movePoint>().endLine.x.Equals(line.GetPosition(0).x) && spheres[i].GetComponent<movePoint>().endLine.y.Equals(line.GetPosition(0).y))
                    {
                        spheres[i].SetActive(false);
                        availableSpheres.Add(spheres[i]);
                        spheres[i].GetComponent<Collider2D>().enabled = false;
                        spheres.RemoveAt(i);
                        Destroy(Instantiate(clickOnLineEffect), 1);
                        break;
                    }
                }
                //print(line.GetPosition(0) + "____________________________" + line.GetPosition(1));
                break;
            }
        }
    }
}
