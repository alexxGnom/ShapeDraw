using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace ShapeDraw
{
    public class StorageController : MonoSingleton<StorageController>
    {
        public static void SaveShapeToFile(string fileName, Shape shape)
        {
            var path = Application.dataPath + "/" + fileName + ".txt";
            StreamWriter sw = new StreamWriter(path);

            var vertices = shape.vertices;

            foreach (var v in vertices)
            {
                sw.WriteLine(v.ToString());
            }

            sw.Close();

            Debug.Log("SAVE FILE " + path);
        }

        public static Shape LoadShapeFromFile(string fileName)
        {
            var path = Application.dataPath + "/" + fileName + ".txt";
            if (File.Exists(path))
            {
                string[] rows = File.ReadAllLines(path);

                List<Vector3> result = new List<Vector3>();

                foreach (var r in rows)
                {
                    var v = Utils.StringToVector3(r);
                    if (v != null)
                        result.Add(v);
                }

                if (result.Count > 0)
                {
                    Debug.Log("LOAD FILE " + path);
                    return new Shape(result);
                }
            }

            Debug.LogError("File not loaded!");
            return null; 
        } 
    }
}
