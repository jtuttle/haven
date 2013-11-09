using UnityEngine;
using System;

public class UnityUtils {
    public static T LoadResource<T>(string path, bool instantiate = false) where T : UnityEngine.Object {
        try {
            T resource = (T)Resources.Load(path, typeof(T));
            return (instantiate ? (T)GameObject.Instantiate(resource) : resource);
        } catch(Exception) {
            throw new ArgumentException("Unable to load resource: " + path);
        }
    }
}
