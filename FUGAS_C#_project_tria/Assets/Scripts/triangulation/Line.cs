using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.triangulation
{
    public class Line :IComparable<Line>, IEquatable<Line>
    {
        public Vector2 Origin { get;  set; }
        public Vector2 Destination { get;  set; }

        public float Pheromon { get; set; }

        public int CirclesOnLine { get; set; }

        public Line(Vector2 origin, Vector2 destination)
        {
            Origin = origin;
            Destination = destination;
            Pheromon = 1;
            CirclesOnLine = 4;
        }


        public Line(float x1, float y1, float x2, float y2)
        {
            Origin = new Vector2(x1, y1);
            Destination = new Vector2(x2, y2);
            Pheromon = 1;
            CirclesOnLine = 4;
        }

        public Line(Line other)
        {
            Origin = other.Origin;
            Destination = other.Destination;
            Pheromon = 1;
            CirclesOnLine = 4;
        }

        public float Lenght
        {
            get
            {
                return Vector2.Distance(Origin, Destination);
            }
        }

        public int CompareTo(Line other)
        {
            if (Origin.LessThen(other.Origin)) return -1;
            if (Origin.GreaterThen(other.Origin)) return 1;
            if (Destination.LessThen(other.Destination)) return -1;
            if (Destination.GreaterThen(other.Destination)) return 1;
            return 0;
        }

        public void Rotate()
        {
            // rotate on 90
            var mid = (Origin.Add(Destination)).Multiply(0.5f);
            var v = Destination.Substract(Origin);
            var n = new Vector2(v.y, -v.x);
            Origin = mid.Substract(n.Multiply(0.5f));
            Destination = mid.Add(n.Multiply(0.5f));
        }

        public void Flip()
        {
            Rotate();
            Rotate();
        }

        // returns paramter of intersection equation. NaN if no intersection
        public double Intersect(Line other)
        {
            var t = Double.NaN;
            Vector2 a = Origin;
            Vector2 b = Destination;
            Vector2 c = other.Origin;
            Vector2 d = other.Destination;
            Vector2 n = new Vector2(d.Substract(c).y, c.Substract(d).x);

            var denom = n.DotProduct(b.Substract(a));
            if (denom != .0)
            {
                var num = n.DotProduct(a.Substract(c));
                t = -num / denom;
            }

            return t;
        }

        public bool Equals(Line other)
        {
            return Point.EqualsPoints(Origin, other.Origin) && Point.EqualsPoints(Destination, other.Destination);
        }

        //find coordinate y by given x
        float findYbyX(float x)
        {
            return ((x - Origin.x) * (Destination.y - Origin.y)) / (Destination.x - Origin.x) + Origin.y;
        }

        //find coordinate x by given y
        float findXbyY(float y)
        {
            return ((y-Origin.y)*(Destination.x-Origin.x))/(Destination.y-Origin.y) + Origin.x;
        }


        //is point on line including offset delta
        public bool isPointOnLine(Vector2 point,float delta)
        {
            if ((point.x < Origin.x || point.x > Destination.x)&&(point.y < Origin.y || point.y > Destination.y))
                return false;

            if (Origin.x.Equals(Destination.x) && (point.y >= Origin.y && point.y <= Destination.y
                || point.y >= Destination.y && point.y <= Origin.y)
                && point.x >= Origin.x - delta && point.x <= Origin.x + delta)
                return true;

            if (Origin.y.Equals(Destination.y) && (point.x >= Origin.x && point.x <= Destination.x
                || point.x >= Destination.x && point.x <= Origin.x) &&
                (Origin.y - delta <= point.y && Origin.y + delta >= point.y))
                return true;

            if ((findXbyY(point.y) + delta >= point.x && findXbyY(point.y) - delta <= point.x)
                && (findYbyX(point.x) - delta <= point.y && findYbyX(point.x) + delta >= point.y))
                return true;

            return false;
        }
    }
}
