using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MainCanvas : MonoBehaviour
{
    private readonly List<string> _stageList = new List<string>();
    
    [SerializeField] LoadStageButton _buttonPrefab;
    [SerializeField] Transform _buttonParent;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    async void Start()
    {
        await LoadStageList();

        foreach (var stage in _stageList)
        {
            Debug.Log(stage);
            var button = Instantiate(_buttonPrefab, _buttonParent);
            button.SetStageIndex(ExtractStageNumber(stage));
        }

        LoadStageButton.OnClicked.Subscribe(stageName =>
        {
            LoadSceneFromAddressable(stageName);
        });
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
