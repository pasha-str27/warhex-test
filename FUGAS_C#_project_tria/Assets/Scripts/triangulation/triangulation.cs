using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.UI;

namespace Assets.Scripts.triangulation
{
    public class triangulation : MonoBehaviour
    {
        public List<Vector2> points;

        private List<Triangle> _triangulation;

        public static int level;
        public static bool loadingFromLevelsMenu;

        public Text levelText;

        public void Start_()
        {
            //reset count of computer`s and player`s points
            movePoint.EnemyNumber = 0;
            movePoint.PlayerNumber = 0;

            //get current level
            if(level==0&&!PlayerPrefs.HasKey("currentLevel"))
            {
                level = 1;
                PlayerPrefs.SetInt("currentLevel",1);
            }
            else
                if(!loadingFromLevelsMenu)
                    level = PlayerPrefs.GetInt("currentLevel");
            loadingFromLevelsMenu = false;

            //set image to level frame
            levelText.text = level.ToString();

            //load points and triangulate theirs
            loadPoints();
            _triangulation = Triangulate(points);
        }


        //Load list of points from file
        void loadPoints()
        {
            StreamReader fileWithLevels = new StreamReader(Application.dataPath+"/levels.txt");
            string pointsString ="";
            for (int i = 0; i < level; ++i)
                pointsString = fileWithLevels.ReadLine();
            points.Clear();
            for(int i = 0; i < pointsString.Split(' ').Length - 1; i += 2)
                points.Add(new Vector2(float.Parse(pointsString.Split(' ')[i]), float.Parse(pointsString.Split(' ')[i + 1])));

            fileWithLevels.Close();
        }

        public List<Triangle> GetTriangles()
        {
            return _triangulation;
        }

        public List<Vector2> GetPointsList()
        {
            return points;
        }

        public List<Triangle> Triangulate(List<Vector2> points)
        {
            this.points = points;
            var res = new List<Triangle>();
            var frontier = new List<Line> { FindFirstEdge() };

            while (frontier.Count != 0)
            {
                // find and exctract min edge
                var minEdge = frontier.Min();
                var point = FindMate(minEdge);
                if (point != null)
                {
                    // add new edges to frontier. But if frontier already contains it -- then delete. It's dead edge.
                    updateFrontier(frontier, (Vector2)point, minEdge.Origin);
                    updateFrontier(frontier, minEdge.Destination, (Vector2)point);
                    // add triangle in triangulaton result
                    res.Add(new Triangle(minEdge.Origin, minEdge.Destination, (Vector2)point));
                }
                // delete edge from frontier
                frontier.Remove(minEdge);
            }
            return res;
        }

        private void updateFrontier(List<Line> frontier, Vector2 a, Vector2 b)
        {
            var e = new Line(a, b);
            if (frontier.Contains(e)) // doesnt work appropriate
            {
                frontier.Remove(e);
            }
            else
            {
                e.Flip();
                frontier.Add(e);
            }
        }

        // Find first edge following to Jarvis gift-wrapping algorithm
        private Line FindFirstEdge()
        {
            if (points?.Count == 0)
                return null;

            Vector2 startPoint = points[0];

            // the most left point
            foreach (var point in points)
            {
                if (point.x < startPoint.x || (point.x == startPoint.x && point.y < startPoint.y))
                    startPoint = point;
            }

            // find the pint with max angle (min cos) 
            var minCos = 1.0;
            var endPoint = startPoint;

            var srtartVector = new Vector2(0, startPoint.y + 10);

            foreach (var point in points)
            {
                if (point == startPoint)
                    continue;

                var cos = CosBetween(srtartVector, new Vector2(point.x - startPoint.x, point.y - startPoint.y));

                if (cos < minCos)
                {
                    minCos = cos;
                    endPoint = point;
                }
            }

            return new Line(startPoint, endPoint);
        }

        private double CosBetween(Vector2 a, Vector2 b)
        {
            var dotProduct = a.DotProduct(b);
            var lenA = a.RadiusVector();
            var lenB = b.RadiusVector();
            return dotProduct / (lenA * lenB);
        }

        private Vector2? FindMate(Line edge)
        {
            Vector2? mate = null;
            // parameter for vectors intersection
            var t = double.MaxValue;
            var bestT = t;
            // get normal to the edge
            Line normal = new Line(edge);
            normal.Rotate();
            foreach (var point in points)
            {
                if (IsRightPoint(edge, point))
                {
                    Line g = new Line(edge.Destination, point);
                    // normal to edge g
                    g.Rotate();
                    // find point of intersections 2 normals
                    t = -normal.Intersect(g);
                    if (t < bestT)
                    {
                        bestT = t;
                        mate = point;
                    }
                }
            }

            return mate;
        }

        private bool IsRightPoint(Line e, Vector2 point)
        {
            Vector2 a = e.Destination.Substract(e.Origin);
            Vector2 b = point.Substract(e.Origin);
            var sa = a.x * b.y - b.x * a.y;
            return sa > 0;
        }


        public class Triangle
        {
            public Vector2 A { get; private set; }
            public Vector2 B { get; private set; }
            public Vector2 C { get; private set; }

            public Triangle(Vector2 A, Vector2 B, Vector2 C)
            {
                this.A = A;
                this.B = B;
                this.C = C;
            }
        }
    }
}