using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Data
{
    public static class DataExtensions
    {
        public static Vector3Data AsVectorData(this Vector3 vector) => 
            new Vector3Data(vector.x, vector.y, vector.z);
    
        public static Vector3 AsUnityVector(this Vector3Data vector3Data) => 
            new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);

        public static float SqrMagnitudeTo(this Vector3 from, Vector3 to)
        {
            return Vector3.SqrMagnitude(to - from);
        }

        public static string ToJson(this object obj) => 
            JsonUtility.ToJson(obj);

        public static T ToDeserialized<T>(this string json) =>
            JsonUtility.FromJson<T>(json);
        public static List<T> Shuffle<T>(this List<T> _list)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                T temp = _list[i];
                int randomIndex = Random.Range(i, _list.Count);
                _list[i] = _list[randomIndex];
                _list[randomIndex] = temp;
            }

            return _list;
        }
    }
}