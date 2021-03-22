using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AntColony : MonoBehaviour
{
    public GameObject playerStartBasePrefab;
    public GameObject enemyStartBasePrefab;
    public GameObject basePrefab;
    public GameObject linePrefab;
    public GameObject _triangulationObject;
    private Assets.TestScripts.triangulation.triangulation _triangulation;
    private List<LineRenderer> _lineRenderersList;
    private List<GameObject> _pointsList;
    private List<Assets.TestScripts.triangulation.Line> _linesList;
    private List<List<Assets.TestScripts.triangulation.Line>> _antColonyWay;
    private List<float> _antColontWaysLenght;

    private GameObject _startBase;
    private GameObject _endBase;
    
    private float _sceneWidth = 10;

    private float _evaporationLevel = 0.01f;
    private int _iterationsNumber = 125;

    public GameObject enemyPoint;
    public GameObject playerPoint;

    void Start()
    {
        _pointsList = new List<GameObject>();
        _lineRenderersList = new List<LineRenderer>();
        _linesList = new List<Assets.TestScripts.triangulation.Line>();
        _antColonyWay = new List<List<Assets.TestScripts.triangulation.Line>>();
        _antColontWaysLenght = new List<float>();
        _triangulation = _triangulationObject.GetComponent<Assets.TestScripts.triangulation.triangulation>();
        
        _triangulation.Start();

        _startBase = Instantiate(playerStartBasePrefab, new Vector3(-_sceneWidth - 1, 0, 0), transform.rotation);
        _endBase = Instantiate(enemyStartBasePrefab, new Vector3(_sceneWidth + 1, 0, 0), transform.rotation);

        _startBase.GetComponent<SpriteRenderer>().color = Color.blue;
        _endBase.GetComponent<SpriteRenderer>().color = Color.red;
        basePrefab.GetComponent<SpriteRenderer>().color = Color.white;

        GeneratePoints();
        GenerateLines();

        foreach (var i in _linesList)
            DrawLine(i, Color.gray);

        for (int i = 0; i < _iterationsNumber; ++i)
            FindAntColonyWay();

        //foreach (var i in _antColonyWay[_antColontWaysLenght.IndexOf(_antColontWaysLenght.Min())])
        //    DrawLine(i, Color.red);

        Debug.LogWarning("Best way: " + _antColontWaysLenght.Min());

        //if (_pointsList.Count == 0)
        //    Debug.Log("there is no points on scene!");

        createPoint();
    }

    void createPoint()
    {
        GameObject enemyPointFirst = Instantiate(enemyPoint, _endBase.transform.position,Quaternion.identity);
        enemyPointFirst.GetComponent<movePoint>().endLine = _antColonyWay[_antColontWaysLenght.IndexOf(_antColontWaysLenght.Min())][_antColonyWay[_antColontWaysLenght.IndexOf(_antColontWaysLenght.Min())].Count-1].Origin;

        if (PlayerManager.conqueredBases == null)
            PlayerManager.conqueredBases = new List<GameObject>();

        StartCoroutine(createPlayerPoints());

        PlayerManager.conqueredBases.Add(_startBase);
        if (AIManager.conqueredBases == null)
            AIManager.conqueredBases = new List<GameObject>();
        AIManager.conqueredBases.Add(_endBase);
        //print(PlayerManager.spheres.Count);
    }

    IEnumerator createPlayerPoints()
    {
        for(int i=0;i<3;++i)
        {
            GameObject playerPointFirst = Instantiate(playerPoint, _startBase.transform.position, Quaternion.identity);
            playerPointFirst.GetComponent<movePoint>().endLine = _antColonyWay[_antColontWaysLenght.IndexOf(_antColontWaysLenght.Min())][1].Origin; //ColonyWay(1).Origin;
            if (PlayerManager.spheres == null)
                PlayerManager.spheres = new List<GameObject>();
            PlayerManager.spheres.Add(playerPointFirst);
            yield return new WaitForSeconds(0.25f);
        }
        AIManager.finishBase = PlayerManager.spheres[0].GetComponent<movePoint>().endLine;
    }

    public Assets.TestScripts.triangulation.Line ColonyWay(int index)
    {
         return _antColonyWay[_antColontWaysLenght.IndexOf(_antColontWaysLenght.Min())][_antColonyWay[_antColontWaysLenght.IndexOf(_antColontWaysLenght.Min())].Count - index-1];
    }

    void GeneratePoints()
    {
        foreach (var i in _triangulation.GetPointsList())
            _pointsList.Add(Instantiate(basePrefab, new Vector2(i.x, i.y), transform.rotation));
    }

    public List<LineRenderer> LineRenderersList
    {
        get
        {
            return _lineRenderersList;
        }
    }

    public void ChangeLineColor(Vector2 start, Vector2 end,Color color)
    {
        List<LineRenderer> lines = _lineRenderersList.FindAll(x => Vector2.Equals(new Vector2(x.GetPosition(0).x, x.GetPosition(0).y), start) && Vector2.Equals(new Vector2(x.GetPosition(1).x, x.GetPosition(1).y), end)
         || Vector2.Equals(new Vector2(x.GetPosition(0).x, x.GetPosition(0).y), end) && Vector2.Equals(new Vector2(x.GetPosition(1).x, x.GetPosition(1).y), start));
        for(int i=0;i< lines.Count;++i)
        {
            lines[i].startColor = color;
            lines[i].endColor = color;
        }
    }

    void AddLineToLinesList(Assets.TestScripts.triangulation.Line line)
    {
        if (_linesList.Find(x => (x.Origin.Equals(line.Origin) || x.Origin.Equals(line.Destination)) &&
        (x.Destination.Equals(line.Origin) || x.Destination.Equals(line.Destination))) == null)
            _linesList.Add(line);
    }

    void GenerateLines()
    {
        if (_linesList.Count == 0)
        {
            var pointsPos = _triangulation.GetPointsList();
            var accessablePoints = _triangulation.GetAllAccessablePoints();
            var triangles = _triangulation.GetTriangles();

            if (triangles == null)
                Debug.Log("There are no triangles");
            if (accessablePoints == null)
                Debug.Log("There are no accessable points");
            if (pointsPos == null)
                Debug.Log("There are no points");

            foreach (var i in triangles)
            {
                AddLineToLinesList(new Assets.TestScripts.triangulation.Line(i.A.x, i.A.y, i.B.x, i.B.y));
                AddLineToLinesList(new Assets.TestScripts.triangulation.Line(i.B.x, i.B.y, i.C.x, i.C.y));
                AddLineToLinesList(new Assets.TestScripts.triangulation.Line(i.A.x, i.A.y, i.C.x, i.C.y));
            }

            //for (int i = 0; i < accessablePoints.Count; ++i)
            //    for (int j = 0; j < accessablePoints[i].Count; ++j)
            //        _linesList.Add(new Assets.TestScripts.triangulation.Line(pointsPos[i].x, pointsPos[i].y, accessablePoints[i][j].x, accessablePoints[i][j].y));

            List<float> minDis = new List<float>();

            for (int i = 0; i < _pointsList.Count; ++i)
                minDis.Add(Vector3.Distance(_startBase.transform.position, _pointsList[i].transform.position));

            _linesList.Insert(0, new Assets.TestScripts.triangulation.Line
                        (_startBase.transform.position.x,
                            _startBase.transform.position.y,
                            _linesList.Find(x => x.Origin.Equals(new Vector2(_pointsList[minDis.IndexOf(minDis.Min())].transform.position.x, _pointsList[minDis.IndexOf(minDis.Min())].transform.position.y))).Origin.x,
                             _linesList.Find(x => x.Origin.Equals(new Vector2(_pointsList[minDis.IndexOf(minDis.Min())].transform.position.x, _pointsList[minDis.IndexOf(minDis.Min())].transform.position.y))).Origin.y));

            minDis.Clear();

            for (int i = 0; i < _pointsList.Count; ++i)
                minDis.Add(Vector3.Distance(_endBase.transform.position, _pointsList[i].transform.position));

            _linesList.Add(new Assets.TestScripts.triangulation.Line
                        (_linesList.Find(x => x.Origin.Equals(new Vector2(_pointsList[minDis.IndexOf(minDis.Min())].transform.position.x, _pointsList[minDis.IndexOf(minDis.Min())].transform.position.y))).Origin.x,
                            _linesList.Find(x => x.Origin.Equals(new Vector2(_pointsList[minDis.IndexOf(minDis.Min())].transform.position.x, _pointsList[minDis.IndexOf(minDis.Min())].transform.position.y))).Origin.y,
                            _endBase.transform.position.x,
                            _endBase.transform.position.y));
        }
    }

    void DrawLine(Assets.TestScripts.triangulation.Line line, Color lineColor)
    {
        var tempLine = Instantiate(linePrefab, transform);

        var lineRenderer = tempLine.GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.positionCount = 2;


        lineRenderer.SetPosition(0, new Vector3(line.Origin.x, line.Origin.y, -1));
        lineRenderer.SetPosition(1, new Vector3(line.Destination.x, line.Destination.y, -1));

        _lineRenderersList.Add(lineRenderer);
    }

    void SortLines()
    {
        _pointsList = _pointsList.OrderBy(x => x.transform.position.y).ToList();
        _pointsList = _pointsList.OrderBy(x => x.transform.position.x).ToList();

        var pointSwapper = _linesList[0].Origin;

        foreach (var i in _linesList)
            if ((i.Origin.x > i.Destination.x))
            {
                pointSwapper = i.Origin;
                i.Origin = i.Destination;
                i.Destination = pointSwapper;
            }

    }

    public void FindAntColonyWay()
    {
        SortLines();

        List<Assets.TestScripts.triangulation.Line> tempAntColonyWay = new List<Assets.TestScripts.triangulation.Line>();

        //Debug.LogError(_pointsList.Count);
        //Debug.LogError(_linesList.Count);

        var tempPointsList = _pointsList;
        tempPointsList.Insert(0, _startBase);
        tempPointsList.Add(_endBase);

        //foreach (var i in tempPointsList)
        //    Debug.Log(i.transform.position.x + " " + i.transform.position.y);

        var currentPoint = new Vector2(_startBase.transform.position.x, _startBase.transform.position.y);
        var previousPoint = currentPoint;

        for (int index = 0; index < 200; ++index)
        {
            if (currentPoint.Equals(new Vector2(_endBase.transform.position.x, _endBase.transform.position.y)))
            {
                float wayLenght = 0f;

                foreach (var i in tempAntColonyWay)
                    wayLenght += i.Lenght;

                _antColontWaysLenght.Add(wayLenght);
                Debug.Log("Way lenght: " + wayLenght);

                foreach (var i in _linesList)
                    foreach (var j in tempAntColonyWay)
                        if (i.Equals(j))
                            i.Pheromon = (1 - _evaporationLevel) * i.Pheromon + 1 / wayLenght;

                _antColonyWay.Add(tempAntColonyWay);

                break;
            }

            var accessableLines = _linesList.FindAll(x => x.Origin.Equals(currentPoint)).ToList();

            if (index > 0)
                accessableLines = _linesList.FindAll(x => x.Origin.Equals(currentPoint) ||
                (x.Destination.Equals(currentPoint)) &&
                !x.Origin.Equals(previousPoint)).ToList();


            List<float> probability = new List<float>();

            //Debug.LogWarning(accessableLines.Count);

            if (accessableLines.Count != 0)
            {
                if (accessableLines.Count > 1)
                {
                    for (int i = 0; i < accessableLines.Count; ++i)
                    {
                        float sum = 0f;

                        for (int j = 0; j < accessableLines.Count; ++j)
                            sum += accessableLines[j].Lenght * accessableLines[j].Pheromon;

                        probability.Add((accessableLines[i].Lenght * accessableLines[i].Pheromon) / sum);
                    }

                    var randomSeed = Random.value;
                    float cumulativeSum = 0.0f;
                    bool newLineFounded = false;

                    for (int j = 0; j < probability.Count; ++j)
                    {
                        if (probability[j] > 1)
                            Debug.LogError("Probability is bigger than 1");

                        cumulativeSum += probability[j];

                        if (cumulativeSum > 1.0001f)
                        {
                            Debug.LogError("Cumulative sum is bigger than 1");
                            Debug.LogError(cumulativeSum);
                        }

                        if (randomSeed <= cumulativeSum)
                        {
                            newLineFounded = true;
                            tempAntColonyWay.Add(accessableLines[j]);
                            previousPoint = currentPoint;
                            currentPoint = accessableLines[j].Destination;
                            break;
                        }
                    }

                    if (!newLineFounded)
                        Debug.LogError("Can't find new point");
                }
                else
                {
                    tempAntColonyWay.Add(accessableLines[0]);
                    currentPoint = accessableLines[0].Destination;
                }
            }
            else
                Debug.Log("There are no accessable lines");
        }
    }
}
