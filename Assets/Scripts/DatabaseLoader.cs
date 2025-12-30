using System.IO;
using UnityEngine;

public static class DatabaseLoader
{
    public static string GetDatabasePath(string dbName)
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, dbName);
        string targetPath = Path.Combine(Application.persistentDataPath, dbName);

        if (!File.Exists(targetPath))
        {
#if UNITY_ANDROID
            // Android needs WWW / UnityWebRequest
            var www = new WWW(sourcePath);
            while (!www.isDone) { }
            File.WriteAllBytes(targetPath, www.bytes);
#else
            File.Copy(sourcePath, targetPath);
#endif
        }

        return targetPath;
    }
}
