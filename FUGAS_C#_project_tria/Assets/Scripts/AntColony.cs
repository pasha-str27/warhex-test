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
    private Assets.Scripts.triangulation.triangulation _triangulation;
    private List<LineRenderer> _lineRenderersList;
    private List<GameObject> _pointsList;
    private List<Assets.Scripts.triangulation.Line> _linesList;
    private List<List<Assets.Scripts.triangulation.Line>> _antColonyWay;
    private List<float> _antColonyWayLength;

    private GameObject _startBase;
    private GameObject _endBase;

    private float _sceneWidth = 10;

    private float _evaporationLevel = 0.01f;
    private int _iterationsNumber = 50;
    private int _tryNumber = 200;

    public GameObject enemyPoint;
    public GameObject playerPoint;

    static PlayerManager _playerManager;
    static AIManager _AIManager;

    public GameObject tip;

    public static bool wasClick;

    void Start()
    {
        if (_playerManager == null)
            _playerManager = GameObject.FindGameObjectWithTag("playerController").GetComponent<PlayerManager>();

        if (_AIManager == null)
            _AIManager = GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>();

        _pointsList = new List<GameObject>();
        _lineRenderersList = new List<LineRenderer>();
        _linesList = new List<Assets.Scripts.triangulation.Line>();
        _antColonyWay = new List<List<Assets.Scripts.triangulation.Line>>();
        _antColonyWayLength = new List<float>();
        _triangulation = _triangulationObject.GetComponent<Assets.Scripts.triangulation.triangulation>();

        _triangulation.Start_();

        _startBase = Instantiate(playerStartBasePrefab, new Vector3(-_sceneWidth - 1, 0, 0), transform.rotation);
        _endBase = Instantiate(enemyStartBasePrefab, new Vector3(_sceneWidth + 1, 0, 0), transform.rotation);

        GeneratePoints();
        GenerateLines();

        foreach (var i in _linesList)
            DrawLine(i, Color.gray);

        for (int i = 0; i < _iterationsNumber; ++i)
            FindAntColonyWay();

        Debug.LogWarning("Best way: " + _antColonyWayLength.Min());

        StartCoroutine(WaitOnClick());
    }

    IEnumerator WaitOnClick()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Destroy(tip);
                wasClick = true;
                break;
            }
        }

        createPoints();
    }

    void createPoints()
    {
        GameObject enemyPointFirst = Instantiate(enemyPoint, _endBase.transform.position, Quaternion.identity);

        var movePoint_ = enemyPointFirst.GetComponent<movePoint>();

        movePoint_.endLine = _antColonyWay[_antColonyWayLength.IndexOf(_antColonyWayLength.Min())][_antColonyWay[_antColonyWayLength.IndexOf(_antColonyWayLength.Min())].Count - 1].Origin;
        movePoint_.goal = movePoint_.endLine;
        movePoint_.speed *= PlayerPrefs.GetInt("difficulty");
        movePoint_.beginLine = _endBase.transform.position;

        _playerManager.finishBase = enemyPointFirst.GetComponent<movePoint>().beginLine;

        if (_playerManager.conqueredBases == null)
            _playerManager.conqueredBases = new List<GameObject>();

        _playerManager.conqueredBases.Add(_startBase);

        if (_AIManager.conqueredBases == null)
            _AIManager.conqueredBases = new List<GameObject>();

        _AIManager.conqueredBases.Add(_endBase);
        StartCoroutine(createPlayerPoints());
    }

    IEnumerator createPlayerPoints()
    {
        if (_playerManager.spheres == null)
            _playerManager.spheres = new List<GameObject>();

        Vector2 goal = _antColonyWay[_antColonyWayLength.IndexOf(_antColonyWayLength.Min())][1].Origin;

        for (int i = 0; i < 3; ++i)
        {
            GameObject playerPointFirst = Instantiate(playerPoint, _startBase.transform.position, Quaternion.identity);

            var movePoint_ = playerPointFirst.GetComponent<movePoint>();

            movePoint_.endLine = goal;
            movePoint_.goal = goal;
            movePoint_.speed *= PlayerPrefs.GetInt("difficulty");
            _playerManager.spheres.Add(playerPointFirst);

            yield return new WaitForSeconds(0.2f * PlayerPrefs.GetInt("difficulty"));
        }

        _AIManager.finishBase = _playerManager.spheres[0].GetComponent<movePoint>().beginLine;
    }

    public Assets.Scripts.triangulation.Line ColonyWay(int index)
    {
        return _antColonyWay[_antColonyWayLength.IndexOf(_antColonyWayLength.Min())][_antColonyWay[_antColonyWayLength.IndexOf(_antColonyWayLength.Min())].Count - index - 1];
    }

    void GeneratePoints()
    {
        foreach (var i in _triangulation.GetPointsList())
            _pointsList.Add(Instantiate(basePrefab, new Vector2(i.x, i.y), transform.rotation));
    }

    public ref List<LineRenderer> LineRenderersList
    {
        get
        {
            return ref _lineRenderersList;
        }
    }

    public void ChangeLineColor(Vector2 start, Vector2 end, Color color)
    {
        List<LineRenderer> lines = _lineRenderersList.FindAll(x =>
        Vector2.Equals(new Vector2(x.GetPosition(0).x, x.GetPosition(0).y), start)
        && Vector2.Equals(new Vector2(x.GetPosition(1).x, x.GetPosition(1).y), end)
         || Vector2.Equals(new Vector2(x.GetPosition(0).x, x.GetPosition(0).y), end)
         && Vector2.Equals(new Vector2(x.GetPosition(1).x, x.GetPosition(1).y), start));

        for (int i = 0; i < lines.Count; ++i)
        {
            lines[i].startColor = color;
            lines[i].endColor = color;
        }
    }

    void AddLineToLinesList(Assets.Scripts.triangulation.Line line)
    {
        if (_linesList.Find(x => (x.Origin.Equals(line.Origin) || x.Origin.Equals(line.Destination)) &&
        (x.Destination.Equals(line.Origin) || x.Destination.Equals(line.Destination))) == null)
            _linesList.Add(line);
    }

    Vector2 FindNearestPointPosition(Transform point)
    {
        List<float> minDis = new List<float>();

        for (int i = 0; i < _pointsList.Count; ++i)
            minDis.Add(Vector3.Distance(point.position, _pointsList[i].transform.position));

        var nearestPointPosition = _pointsList[minDis.IndexOf(minDis.Min())].transform.position;
        return new Vector2(nearestPointPosition.x, nearestPointPosition.y);
    }
    void GenerateLines()
    {
        if (_linesList.Count == 0)
        {
            var pointsPos = _triangulation.GetPointsList();
            var triangles = _triangulation.GetTriangles();

            if (triangles == null)
                Debug.Log("There are no triangles");
            if (pointsPos == null)
                Debug.Log("There are no points");

            foreach (var i in triangles)
            {
                AddLineToLinesList(new Assets.Scripts.triangulation.Line(i.A.x, i.A.y, i.B.x, i.B.y));
                AddLineToLinesList(new Assets.Scripts.triangulation.Line(i.B.x, i.B.y, i.C.x, i.C.y));
                AddLineToLinesList(new Assets.Scripts.triangulation.Line(i.A.x, i.A.y, i.C.x, i.C.y));
            }

            var startBasePosition = _startBase.transform.position;
            var endBasePosition = _endBase.transform.position;

            var nearestPoint = FindNearestPointPosition(_startBase.transform);

            _linesList.Insert(0, new Assets.Scripts.triangulation.Line
                        (startBasePosition.x, startBasePosition.y,
                            _linesList.Find(x => x.Origin.Equals(nearestPoint)).Origin.x,
                            _linesList.Find(x => x.Origin.Equals(nearestPoint)).Origin.y));

            nearestPoint = FindNearestPointPosition(_endBase.transform);

            _linesList.Add(new Assets.Scripts.triangulation.Line
                        (_linesList.Find(x => x.Origin.Equals(nearestPoint)).Origin.x,
                            _linesList.Find(x => x.Origin.Equals(nearestPoint)).Origin.y,
                            endBasePosition.x, endBasePosition.y));
        }
    }

    void DrawLine(Assets.Scripts.triangulation.Line line, Color lineColor)
    {
        var tempLine = Instantiate(linePrefab, transform);

        var lineRenderer = tempLine.GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.25f;
        lineRenderer.endWidth = 0.25f;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.positionCount = 2;


        lineRenderer.SetPosition(0, new Vector3(line.Origin.x, line.Origin.y, -1));
        lineRenderer.SetPosition(1, new Vector3(line.Destination.x, line.Destination.y, -1));

        _lineRenderersList.Add(lineRenderer);
    }

    void SwapLine(Assets.Scripts.triangulation.Line line)
    {
        Vector2 pointSwapper;
        pointSwapper = line.Origin;
        line.Origin = line.Destination;
        line.Destination = pointSwapper;
    }

    void SortLinesForPoint(Vector2 point)
    {
        foreach (var i in _linesList)
            if ((i.Origin.Equals(point) ||
                i.Destination.Equals(point)) &&
                i.Origin.x > i.Destination.x)
                SwapLine(i);
    }

    public void FindAntColonyWay()
    {
        _pointsList = _pointsList.OrderBy(x => x.transform.position.x).ToList();

        List<Assets.Scripts.triangulation.Line> tempAntColonyWay = new List<Assets.Scripts.triangulation.Line>();

        var tempPointsList = _pointsList;
        tempPointsList.Insert(0, _startBase);
        tempPointsList.Add(_endBase);

        var currentPoint = new Vector2(_startBase.transform.position.x, _startBase.transform.position.y);

        for (int index = 0; index < _tryNumber; ++index)
        {
            if (currentPoint.Equals(new Vector2(_endBase.transform.position.x, _endBase.transform.position.y)))
            {
                float wayLength = 0f;

                foreach (var i in tempAntColonyWay)
                    wayLength += i.Length;

                _antColonyWayLength.Add(wayLength);

                foreach (var i in _linesList)
                    foreach (var j in tempAntColonyWay)
                        if (i.Equals(j))
                            i.Pheromone = (1 - _evaporationLevel) * i.Pheromone + 1 / wayLength;

                _antColonyWay.Add(tempAntColonyWay);

                break;
            }

            SortLinesForPoint(currentPoint);
            var accessableLines = _linesList.FindAll(x => x.Origin.Equals(currentPoint)).ToList();

            List<float> probability = new List<float>();

            if (accessableLines.Count != 0)
            {
                if (accessableLines.Count != 1)
                {
                    for (int i = 0; i < accessableLines.Count; ++i)
                    {
                        float sum = 0f;

                        for (int j = 0; j < accessableLines.Count; ++j)
                            sum += accessableLines[j].Length * accessableLines[j].Pheromone;

                        probability.Add((accessableLines[i].Length * accessableLines[i].Pheromone) / sum);
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
