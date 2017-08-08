using UnityEngine;

public class CResourceLoad
{
    private CResourceLoad(){}

    public static GameObject LoadGameObjPerfab(string path)
    {
        return (GameObject)(Resources.Load(path, typeof(GameObject))); 
    }

    public static TextAsset LoadTextAsset(string path)
    {
        return (TextAsset)(Resources.Load(path, typeof(TextAsset)));
    }

    public static GameObject LoadAndInstancePerfab(string path, Vector3 postion, Quaternion rotation)
    {

        GameObject perfab= (GameObject)(Resources.Load(path, typeof(GameObject)));
        return Object.Instantiate(perfab, postion, rotation) as GameObject;
    }

    public static GameObject LoadAndInstancePerfab(string path)
    {

        GameObject perfab = (GameObject)(Resources.Load(path, typeof(GameObject)));
        return Object.Instantiate(perfab) as GameObject;
    }

}
