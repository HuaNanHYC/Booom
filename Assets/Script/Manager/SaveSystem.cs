using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem
{
    #region 普通数据保存：设置等

    /// <summary>
    /// 存档
    /// </summary>
    /// <param name="saveFileName"></param>
    /// <param name="data"></param>
    public static bool SaveByJson(string saveFileName, object data, string savePath)
    {
        var jason = JsonUtility.ToJson(data, true);
        var path = Path.Combine(savePath, saveFileName);
        try
        {
            File.WriteAllText(path, jason);
#if UNITY_EDITOR
            Debug.Log($"保存成功{path}");
#endif
            return true;
        }
        catch (System.Exception e)
        {
#if UNITY_EDITOR
            Debug.Log($"保存失败{path}.\n{e}");
#endif
            return false;
        }
    }

    /// <summary>
    /// 读档
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
            Debug.Log($"读取成功");
#endif
            return data;
        }
        catch (System.Exception e)
        {
#if UNITY_EDITOR
            Debug.Log($"读取失败.\n{e}");
#endif
            return default;
        }
    }

    /// <summary>
    /// 删除存档
    /// </summary>
    /// <param name="saveFileName"></param>
    /*public static bool DeleteSaveFile(string saveFileName, string savePath)
    {
        var path = Path.Combine(savePath, saveFileName);
        try
        {
            File.Delete(path);
            return true;
        }
        catch (System.Exception e)
        {
#if UNITY_EDITOR
            Debug.Log($"failed{e}");
#endif
            return false;
        }
    }*/
    #endregion
}