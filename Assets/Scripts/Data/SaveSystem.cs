using UnityEngine;
using System.IO;

public static class SaveSystem
{
    /// <summary>
    /// ��������ͨ��Json
    /// </summary>
    /// <param name="fileName"> �ļ�������Ϊkey </param>
    /// <param name="data"></param>
    public static void SaveData(string fileName, object data)
    {
        var jsonData = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllText(path, jsonData);

#if UNITY_EDITOR
        Debug.Log($"save the data to {path} successly.");
        Debug.Log(jsonData.ToString());
#endif
    }

    /// <summary>
    /// ��ȡ����ͨ��Json
    /// </summary>
    /// <param name="fileName"> ���ݱ�����ļ��� </param>
    /// <returns></returns>
    public static T LoadData<T>(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            var jsonData = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(jsonData);
            return data;
        }
        catch(System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Failed to load data form {path},\n {exception}");
#endif
            return default;
        }
    }

    public static void DeleteData(string fileName)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.Delete(path);
        }
        catch(System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Failed to delete data form {path},\n {exception}");
#endif
        }
    }
}
