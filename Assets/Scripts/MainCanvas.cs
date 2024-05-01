using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] Button[] _buttons;
    
    private readonly List<string> _stageList = new List<string>();
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    async void Start()
    {
        _buttons[0].onClick.AddListener(() =>
        {
            LoadSceneFromAddressable("Stage1");
        });
        _buttons[1].onClick.AddListener(() =>
        {
            LoadSceneFromAddressable("Stage2");
        });
        _buttons[2].onClick.AddListener(() =>
        {
            LoadSceneFromAddressable("Stage3");
        });
        
        await LoadStageList();
        
        // DEBUG: ステージ一覧を出力
        foreach (var stage in _stageList)
        {
            Debug.Log(stage);
        }
    }
    
    void LoadSceneFromAddressable(string stageName)
    {
        Addressables.LoadSceneAsync($"Assets/Scenes/Stages/{stageName}.unity");
    }
    
    private async UniTask LoadStageList()
    {
        var locations = await Addressables.LoadResourceLocationsAsync("Stages").Task;

        foreach (var location in locations)
        {
            _stageList.Add(location.PrimaryKey);
        }
        
        _stageList.Sort((x, y) => ExtractStageNumber(x).CompareTo(ExtractStageNumber(y)));
    }
    
    int ExtractStageNumber(string path)
    {
        // 文字列から数字部分を抽出して整数に変換する関数
        string numStr = new string(path.Where(char.IsDigit).ToArray());
        return string.IsNullOrEmpty(numStr) ? int.MaxValue : int.Parse(numStr);
    }
}
