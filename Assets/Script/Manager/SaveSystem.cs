using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem
{
    #region ÆÕÍ¨Êý¾Ý±£´æ£ºÉèÖÃµÈ
    /// <summary>
    /// ´æµµ
    /// </summary>
    /// <param name="saveFileName"></param>
    /// <param name="data"></param>
    public static bool SaveByJson(string saveFileName, object data,string savePath)
    {
        var jason = JsonUtility.ToJson(data,true);
        var path = Path.Combine(savePath, saveFileName);
        try
        {
            File.WriteAllText(path, jason);
        #if UNITY_EDITOR
            Debug.Log($"Save Success{path}");
#endif
            return true;

        }
        catch (System.Exception e)
        {
        #if UNITY_EDITOR
            Debug.Log($"Save Failed{path}.\n{e}");
        #endif
            return false;

             
        }

    }
    /// <summary>
    /// ¶Áµµ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="saveFileName"></param>
    /// <returns></returns>
    public static T LoadFromJson<T>(string saveFileName, string savePath)
    {

        try
        {
            var path = Path.Combine(savePath, saveFileName);

            var json = File.ReadAllText(path);

            var data = JsonUtility.FromJson<T>(json);
#if UNITY_EDITOR
            Debug.Log($"Load Success");
#endif
            return data;


        }
        catch (System.Exception e)
        {
#if UNITY_EDITOR
            Debug.Log($"Load Failed.\n{e}");
#endif
            return default;
        }

    }
    /// <summary>
    /// É¾³ý´æµµ
    /// </summary>
    /// <param name="saveFileName"></param>
    public static bool DeleteSaveFile(string saveFileName, string savePath)
    {
        var path = Path.Combine(savePath, saveFileName);
        try { File.Delete(path);return true; }
        catch (System.Exception e)
        {
#if UNITY_EDITOR
            Debug.Log($"failed{e}");
            return false;
#endif
        }
    }
    #endregion
}
