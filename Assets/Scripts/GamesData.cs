using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GamesData : ScriptableObject
{
    #if UNITY_EDITOR
    [MenuItem("Tools/Data/GamesData")]
    static void CreateScriptableObject()
    {
        var obj = CreateInstance<GamesData>();
        AssetDatabase.CreateAsset(obj, "Assets/Resources/GamesData.asset");
    }
    #endif

    public GameData[] Items = Array.Empty<GameData>();

    static GamesData m_Instance;

    public static GamesData Instance => m_Instance ??
                                        (m_Instance = Resources.Load<GamesData>("GamesData"));
    
    public GameData GetFirst()
    {
        return Items.FirstOrDefault();
    }

    
    [Preserve]
    [Serializable]
    public class GameData
    {
        public string アプリ名_日本語;
        public string アプリ名_英語;
        public string 広告ID;
    }
}