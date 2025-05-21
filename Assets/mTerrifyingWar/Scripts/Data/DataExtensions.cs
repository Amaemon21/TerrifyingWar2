using Newtonsoft.Json;
using UnityEngine;

public static class DataExtensions
{
    public static Vector3Data AsVectorData(this Vector3 vector3)
    {
        return new Vector3Data(vector3.x, vector3.y, vector3.z);
    }

    public static Vector3 AsUnityVector(this Vector3Data vector3Data)
    {
        return new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);
    }

    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public static T ToDeserialized<T>(this string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static Vector3 AddY(this Vector3 vector3, float y)
    {
        vector3.y += y;
        return vector3;
    }

    public static float SqrMagnitudeTo(this Vector3 from, Vector3 to)
    {
        return Vector3.SqrMagnitude(to - from);
    }
}