using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace ShapeDraw
{
    public class Utils : MonoBehaviour
    {
        public static void SetStringToTextObject(Text textObject, string text)
        {
            if (textObject != null)
                textObject.text = text;
        }

        public static Vector3 StringToVector3(string sVector)
        {
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            string[] sArray = sVector.Split(',');

            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }

        public static List<Triangle> PolygonToTriangles(Polygon polygon)
        {
            var count = polygon.vertices.Count;
            if (count < 3) return null;

            if (polygon.vertices[0].Equals(polygon.vertices[count - 1]))
                count--;

            List<Triangle> result = new List<Triangle>();

            for (var a = 0; a < count - 2 ; a++)
            {
                for (var b = a + 1; b < count; b++)
                {
                    for (var c = b + 1; c < count; c++)
                    {
                        result.Add(new Triangle(polygon.vertices[a], polygon.vertices[b], polygon.vertices[c]));
                    } 
                }
            }

            return result;
        }

        public static Polygon ShapeToPoligon(Shape shape)
        {
            return new Polygon(shape.vertices);
        }

        public static Line ShapeToLine(Shape shape)
        {
            return new Line(shape.vertices);
        }

        public static List<Vector3> GetPointsOnRadiusFromCenter(Vector3 center, int count = 12, float radius = 10f)
        {
            List<Vector3> result = new List<Vector3>();
            for (var i = 0; i <= count; i++)
            {
                float alpha = (360f * i / count) * Mathf.Deg2Rad;
                var x1 = center.x - radius * Mathf.Sin(alpha);
                var y1 = center.y + radius * Mathf.Cos(alpha);

                result.Add(new Vector3(x1, y1, 0));
            }

            return result;
        }

        public static Vector3 GetIntersectionTwoLine(Vector3 l1p1, Vector3 l1p2, Vector3 l2p1, Vector3 l2p2)
        {
            Vector3 result;
            var d = (l2p2.y - l2p1.y) * (l1p2.x - l1p1.x) - (l2p2.x - l2p1.x) * (l1p2.y - l1p1.y);

            if (d == 0) return Vector3.back;

            var a = (l2p2.x - l2p1.x) * (l1p1.y - l2p1.y) - (l2p2.y - l2p1.y) * (l1p1.x - l2p1.x);

            var x = l1p1.x + a * (l1p2.x - l1p1.x) / d;
            var y = l1p1.y + a * (l1p2.y - l1p1.y) / d;

            result = new Vector3(x, y, 0);

            var ml1p1 = l1p1 - result;
            var ml1p2 = l1p2 - result;
            var dot1 = Vector3.Dot(ml1p1, ml1p2);

            var ml2p1 = l2p1 - result;
            var ml2p2 = l2p2 - result;
            var dot2 = Vector3.Dot(ml2p1, ml2p2);

            if (dot1 > 0 || dot2 > 0) return Vector3.back;

            return result;
        }

        public static List<Vector3> GetIntersectionsLineAndShape(Vector3 linePoint1, Vector3 linePoint2, Shape shape )
        {
            List<Vector3> result = new List<Vector3>();

            for (var i = 0; i < shape.vertices.Count - 1; i++)
            {
                var point = GetIntersectionTwoLine(linePoint1, linePoint2, shape.vertices[i], shape.vertices[i + 1]);
                if(point != Vector3.back)
                    result.Add(point);
            }

            return result;
        }

        public static bool TestRange(float numberToCheck, float bottom, float top)
        {
            return (numberToCheck >= bottom && numberToCheck <= top);
        }

        public static bool TestRange(int numberToCheck, int bottom, int top)
        {
            return (numberToCheck >= bottom && numberToCheck <= top);
        }

        

#if UNITY_EDITOR
        public static void ClearLog()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
            var type = assembly.GetType("UnityEditorInternal.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
#endif
    }  
}
