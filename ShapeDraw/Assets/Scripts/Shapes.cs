using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShapeDraw
{
    public class Bounds
    {
        public float X;
        public float Y;

        private float maxX;
        private float maxY;

        public float Width
        {
            get { return Mathf.Abs(maxX - X); }
        }

        public float Height
        {
            get { return Mathf.Abs(maxY - Y); }
        }

        public Bounds(float minX, float minY, float maxX, float maxY)
        {
            this.X = minX;
            this.Y = minY;
            this.maxX = maxX;
            this.maxY = maxY;
        }
    }

    public class Shape
    {
        private float equalDelta = 1f;


        public List<Vector3> vertices;

        public Bounds bounds;


        public Shape(List<Vector3> vertices)
        {
            this.vertices = vertices;

            float minX = vertices[0].x;
            float minY = vertices[0].y;
            float maxX = vertices[0].x;
            float maxY = vertices[0].y;
            CalcBounds(ref minX, ref minY, ref maxX, ref maxY);

            bounds = new Bounds(minX, minY, maxX, maxY);
        }


        public virtual float GetArea()
        {

            return 0f;
        }

        public virtual Vector3 GetCenter()
        {
            return Vector3.zero;
        }


        public override bool Equals(System.Object obj)
        {

            var drower = UIMainController.Instance.CurrentDrawer;
            if (obj == null)
                return false;

            Shape shape = obj as Shape;
            if ((System.Object)shape == null)
                return false;

            BringingShape(shape);

            var center = GetCenter();
            var points = Utils.GetPointsOnRadiusFromCenter(center);

            //drower.DrawShape(this);
            //drower.DrawShape(shape, Color.red, false);

            foreach (var p in points)
            {
                var intersections1 = Utils.GetIntersectionsLineAndShape(center, p, this);
                var intersections2 = Utils.GetIntersectionsLineAndShape(center, p, shape);

                if (intersections1.Count != intersections2.Count)
                {
                    //Debug.LogError("NOT == COUNT");
                    return false;
                }

                for (var i = 0; i < intersections1.Count; i++)
                {
                   // Debug.Log(Vector3.Distance(intersections1[i], intersections2[i]));
                    if (Vector3.Distance(intersections1[i], intersections2[i]) > equalDelta)
                    {
                        //Debug.LogError("Fail distance " + i);
                        return false;

                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return vertices.ToString().GetHashCode();
        }


        public void MatrixScale(float scale)
        {
            List<Vector3> result = new List<Vector3>();

            Matrix4x4 m = Matrix4x4.Scale(new Vector3(scale, scale, 1));

            foreach (var v in vertices)
            {
                result.Add(m.MultiplyPoint(v));
            }

            vertices = result;
        }

        public void MatrixTranslate(Vector3 translation)
        {
            List<Vector3> result = new List<Vector3>();

            Matrix4x4 m = Matrix4x4.TRS(translation, Quaternion.identity, Vector3.one);

            foreach (var v in vertices)
            {
                result.Add(m.MultiplyPoint(v));
            }

            vertices = result;
        }

        private void CalcBounds(ref float minX, ref float minY, ref float maxX, ref float maxY)
        {
            foreach (var v in vertices)
            {
                minX = Mathf.Min(minX, v.x);
                minY = Mathf.Min(minY, v.y);
                maxX = Mathf.Max(maxX, v.x);
                maxY = Mathf.Max(maxY, v.y);
            }
        }

        public void BringingShape(Shape templateShape)
        {
            var scaleW = templateShape.bounds.Width / this.bounds.Width;
            var scaleH = templateShape.bounds.Height / this.bounds.Height;
            var scale = (scaleW + scaleH) / 2;
            this.MatrixScale(scale);

            var center = GetCenter();
            var templCenter = templateShape.GetCenter();

            var dx = templCenter.x - center.x;
            var dy = templCenter.y - center.y;
            this.MatrixTranslate(new Vector3(dx, dy, 0));

        }
    }

    public class Line : Shape
    {

        private Vector3 a;
        private Vector3 b;


        public Line(Vector3 a, Vector3 b) : base(new List<Vector3>() { a, b })
        {
            this.a = a;
            this.b = b;
        }

        public Line(List<Vector3> vertices) : base(vertices)
        {
            if (vertices.Count > 2 && vertices.Count < 2) Debug.LogError("BAD DATA");

            this.a = vertices[0];
            this.b = vertices[1];

        }

        public override Vector3 GetCenter()
        {
            return new Vector3((a.x + b.x) / 2, (a.y + b.y) / 2, (a.z + b.z) / 2);
        }

        public override bool Equals(System.Object obj)
        {
            var drower = UIMainController.Instance.CurrentDrawer;
            if (obj == null)
                return false;

            Shape shape = obj as Shape;
            if ((System.Object)shape == null)
                return false;


            if (shape.vertices.Count != 2) return false;

            //drower.DrawShape(this);
            //drower.DrawShape(shape, Color.red, false);

            var ab = b - a;
            var cd = shape.vertices[1] - shape.vertices[0];

            var delta = Mathf.Abs(Mathf.Atan2(ab.x, ab.y) - Mathf.Atan2(cd.x, cd.y))  *Mathf.Rad2Deg;

            //  Debug.Log(delta);
            if (Utils.TestRange(delta, 0, 5) || Utils.TestRange(delta, 175, 185))
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return vertices.ToString().GetHashCode();
        }
    }

    public class Triangle : Shape
    {
        private Vector3 a;
        private Vector3 b;
        private Vector3 c;


        public Triangle(Vector3 a, Vector3 b, Vector3 c) : base(new List<Vector3>() { a, b, c })
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Triangle(List<Vector3> vertices) : base(vertices)
        {
            if (vertices.Count > 3 && vertices.Count < 3) Debug.LogError("BAD DATA");

            this.a = vertices[0];
            this.b = vertices[1];
            this.c = vertices[2];

        }

        public override float GetArea()
        {
            Vector3 side1 = b - a;
            Vector3 side2 = c - a;
            return Mathf.Abs(Vector3.Cross(side1, side2).magnitude) / 2;
        }

        public override Vector3 GetCenter()
        {
            return new Vector3((a.x + b.x + c.x) / 3, (a.y + b.y + c.y) / 3, (a.z + b.z + c.z) / 3);
        }        
    }

    public class Polygon : Shape
    {
        public Polygon(List<Vector3> vertices) : base(vertices) { }

        public override Vector3 GetCenter()
        {
            List<Triangle> triangles = Utils.PolygonToTriangles(this);

            float centrX = 0;
            float centrY = 0;
            float polygonArea = 0f;

            for (var i = 0; i < triangles.Count; i++)
            {
                var triangleCenter = triangles[i].GetCenter();
                var triangleArea = triangles[i].GetArea();
                centrX += triangleCenter.x * triangleArea;
                centrY += triangleCenter.y * triangleArea;
                polygonArea += triangleArea;
            }
            return new Vector3(centrX / polygonArea, centrY / polygonArea, 0);
        }
    }
}
