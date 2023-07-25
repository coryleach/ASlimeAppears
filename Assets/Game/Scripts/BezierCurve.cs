using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public static class BezierCurve
    {
        private static readonly float[] Factorial = new float[]
        {
            1.0f,
            1.0f,
            2.0f,
            6.0f,
            24.0f,
            120.0f,
            720.0f,
            5040.0f,
            40320.0f,
            362880.0f,
            3628800.0f,
            39916800.0f,
            479001600.0f,
            6227020800.0f,
            87178291200.0f,
            1307674368000.0f,
            20922789888000.0f,
        };

        private static float Binomial(int n, int i)
        {
            var a1 = Factorial[n];
            var a2 = Factorial[i];
            var a3 = Factorial[n - 1];
            return (a1 / (a2 * a3));
        }

        private static float Bernstein(int n, int i, float t)
        {
            var t_i = Mathf.Pow(t, i);
            var t_n_minus_i = Mathf.Pow((1 - t), (n - i));
            var basis = Binomial(n, i) * t_i * t_n_minus_i;
            return basis;
        }

        public static Vector3 Point(float t, IReadOnlyList<Vector3> controlPoints)
        {
            var N = controlPoints.Count - 1;
            if (N >= Factorial.Length)
            {
                throw new ArgumentOutOfRangeException($"The maximum control points allowed is {Factorial.Length}.");
            }

            if (t <= 0)
            {
                return controlPoints[0];
            }

            if (t >= 1)
            {
                return controlPoints[controlPoints.Count - 1];
            }

            var p = new Vector3();

            for (var i = 0; i < controlPoints.Count; ++i)
            {
                var bn = Bernstein(N, i, t) * controlPoints[i];
                p += bn;
            }

            return p;
        }

        public static Vector2 Point(float t, IReadOnlyList<Vector2> controlPoints)
        {
            var N = controlPoints.Count - 1;
            if (N >= Factorial.Length)
            {
                throw new ArgumentOutOfRangeException($"The maximum control points allowed is {Factorial.Length}.");
            }

            if (t <= 0)
            {
                return controlPoints[0];
            }

            if (t >= 1)
            {
                return controlPoints[controlPoints.Count - 1];
            }

            var p = new Vector2();

            for (var i = 0; i < controlPoints.Count; ++i)
            {
                var bn = Bernstein(N, i, t) * controlPoints[i];
                p += bn;
            }

            return p;
        }
    }
}
