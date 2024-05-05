using UnityEngine;

public class SaveManager
{
    private const string GameDataKey = "GameData";
    
    [System.Serializable]
    public class GameData
    {
        public float BgmVolume;
        public float SeVolume;
        public int StageCleared;
        public int StageOpened;
    }

    private GameData _gameData;
    
    // Singleton instance
    private static SaveManager _instance;
    public static SaveManager Instance => _instance ??= new SaveManager();
    
    private SaveManager()
    {
        LoadData();
    }

    private void SaveData()
    {
        string jsonData = JsonUtility.ToJson(_gameData);
        PlayerPrefs.SetString(GameDataKey, jsonData);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        string jsonData = PlayerPrefs.GetString(GameDataKey, "");
        if (!string.IsNullOrEmpty(jsonData))
        {
            _gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            // Set default values
            _gameData = new GameData()
            {
                BgmVolume = 1f,
                SeVolume = 1f,
                StageCleared = 0,
                StageOpened = 5,
            };
        }
    }
    
    public float BgmVolume 
    {
        get => _gameData.BgmVolume;
        set 
        {
            _gameData.BgmVolume = value;
            SaveData();
        }
    }
    
    public float SeVolume 
    {
        get => _gameData.SeVolume;
        set 
        {
            _gameData.SeVolume = value;
            SaveData();
        }
    }
    
    public int StageCleared 
    {
        get => _gameData.StageCleared;
        set 
        {
            _gameData.StageCleared = value;
            SaveData();
        }
    }

    public int StageOpened 
    {
        get => _gameData.StageOpened;
        set 
        {
            _gameData.StageOpened = value;
            SaveData();
        }
    }
    
    public int CurrentStageIndex => _gameData.StageCleared + 1;
    
    public string CurrentStageName => $"Stage{CurrentStageIndex}";
    
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        LoadData();
    }
}