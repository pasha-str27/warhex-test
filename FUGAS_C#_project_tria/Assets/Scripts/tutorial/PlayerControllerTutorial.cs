using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTutorial : MonoBehaviour
{
    public List<GameObject> spheres; //here are spheres which are on lines
    public List<GameObject> availableSpheres; //here are spheres which we can add on line

    public GameObject clickOnLineEffect;

    public List<LineRenderer> lines;

    public List<GameObject> conqueredBases; //all conquered bases

    public Vector2 finishBase;

    public TipsAnimationsController _animationController;

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
        if (Input.GetMouseButtonUp(1)) //right button click
            processRightButton();
        if (Input.GetMouseButtonUp(0)) //left button click
            processLeftButton();
    }

    //process left button click (set point to line)
    void processLeftButton()
    {
        if (availableSpheres.Count == 0||TipsAnimationsController.playingAnimation)
            return;

        //read mouse pos
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //and searching line with mouse coordinates
        foreach (var line in lines)
        {
            Vector3 pos0 = line.GetPosition(0);
            Vector3 pos1 = line.GetPosition(1);
            if (new Assets.Scripts.triangulation.Line(pos0, pos1).isPointOnLine(mousePos, line.startWidth)
                || new Assets.Scripts.triangulation.Line(pos1, pos0).isPointOnLine(mousePos, line.startWidth))
            {
                for (int i = 0; i < conqueredBases.Count; ++i)
                {
                    Vector3 basePosition = conqueredBases[i].transform.position;
                    //if we founded line 
                    if (basePosition.x.Equals(pos0.x) && basePosition.y.Equals(pos0.y)
                        || basePosition.x.Equals(pos1.x) && basePosition.y.Equals(pos1.y))
                    {
                        //set new values for point
                        var movePoint_ = availableSpheres[0].GetComponent<movePoint>();
                        movePoint_.beginLine = basePosition;

                        availableSpheres[0].GetComponent<Collider2D>().enabled = false;


                        if (movePoint_.beginLine.x.Equals(pos0.x) && (movePoint_.beginLine.y.Equals(pos0.y)))
                            movePoint_.endLine = pos1;
                        else
                            movePoint_.endLine = line.GetPosition(0);

                        movePoint_.goal = movePoint_.endLine;

                        availableSpheres[0].transform.position = basePosition;

                        StartCoroutine(movePoint_.turnOnCollider());

                        availableSpheres[0].SetActive(true);

                        spheres.Add(availableSpheres[0]);
                        availableSpheres.RemoveAt(0);
                        Destroy(Instantiate(clickOnLineEffect), 1);

                        if (TipsAnimationsController._currentAnimation == 7)
                        {
                            StartCoroutine(startAnimation());
                        }

                        break;
                    }
                }

                break;
            }
        }
    }

    //process right button click (take point from line)
    void processRightButton()
    {
        //read mouse pos
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //and searching line with mouse coordinates
        foreach (var line in lines)
        {
            Vector3 pos0 = line.GetPosition(0);
            Vector3 pos1 = line.GetPosition(1);
            if (new Assets.Scripts.triangulation.Line(pos0, pos1).isPointOnLine(mousePos, line.startWidth)
                || new Assets.Scripts.triangulation.Line(pos1, pos0).isPointOnLine(mousePos, line.startWidth))
            {
                for (int i = 0; i < spheres.Count; ++i)
                {
                    //if we founded line 
                    Vector2 beginLine = spheres[i].GetComponent<movePoint>().beginLine;
                    Vector2 endLine = spheres[i].GetComponent<movePoint>().endLine;

                    if (beginLine.x.Equals(pos0.x) && beginLine.y.Equals(pos0.y)
                        && endLine.x.Equals(pos1.x) && endLine.y.Equals(pos1.y)
                        || beginLine.x.Equals(pos1.x) && beginLine.y.Equals(pos1.y)
                        && endLine.x.Equals(pos0.x) && endLine.y.Equals(pos0.y))
                    {
                        //take point from line
                        spheres[i].SetActive(false);
                        availableSpheres.Add(spheres[i]);
                        spheres[i].GetComponent<Collider2D>().enabled = false;
                        spheres.RemoveAt(i);
                        Destroy(Instantiate(clickOnLineEffect), 1);

                        if(TipsAnimationsController._currentAnimation==5)
                        {
                            StartCoroutine(startAnimation());
                        }
                        break;
                    }
                }
                break;
            }
        }
    }

    IEnumerator startAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        TipsAnimationsController._currentAnimation++;
        StartCoroutine(_animationController.AnimationStart());
        _animationController._animations.SetTrigger("step" + TipsAnimationsController._currentAnimation.ToString());
        TipsAnimationsController.playingAnimation = true;
        yield return new WaitForSeconds(1.5f);
        TipsAnimationsController.playingAnimation = false;
        if (TipsAnimationsController._currentAnimation == 8)
            Destroy(transform.parent.gameObject);
    }

    public void ChangeLineColor(Vector2 start, Vector2 end, Color color)
    {
        List<LineRenderer> lines_ = lines.FindAll(x =>
        Vector2.Equals(new Vector2(x.GetPosition(0).x, x.GetPosition(0).y), start)
        && Vector2.Equals(new Vector2(x.GetPosition(1).x, x.GetPosition(1).y), end)
         || Vector2.Equals(new Vector2(x.GetPosition(0).x, x.GetPosition(0).y), end)
         && Vector2.Equals(new Vector2(x.GetPosition(1).x, x.GetPosition(1).y), start));

        for (int i = 0; i < lines_.Count; ++i)
        {
            lines_[i].startColor = color;
            lines_[i].endColor = color;
        }
    }
}
