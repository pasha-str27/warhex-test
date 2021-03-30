using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.triangulation
{
    public static class Point
    {
        public static bool LessThen(this Vector2 point, Vector2 other)
        {
            return RadiusVector(point) < RadiusVector(other);
        }

        public static bool EqualsPoints(this Vector2 point, Vector2 other)
        {
            return Mathf.Abs(point.x- other.x) <0.1&& Mathf.Abs(point.y - other.y) < 0.01;
        }

        public static bool GreaterThen(this Vector2 point, Vector2 other)
        {
            return RadiusVector(point) > RadiusVector(other);
        }

        public static double RadiusVector(this Vector2 point)
        {
            return Math.Sqrt(Math.Pow(point.x, 2) + Math.Pow(point.y, 2));
        }

        public static double DotProduct(this Vector2 a, Vector2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static Vector2 Substract(this Vector2 point, Vector2 other)
        {
            return new Vector2(point.x - other.x, point.y - other.y);
        }

        public static Vector2 Add(this Vector2 point, Vector2 other)
        {
            return new Vector2(point.x + other.x, point.y + other.y);
        }

        public static Vector2 Multiply(this Vector2 point, float x)
        {
            return new Vector2(point.x * x, point.y * x);
        }
    }
}
