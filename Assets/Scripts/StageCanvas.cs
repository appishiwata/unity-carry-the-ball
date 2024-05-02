using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class StageCanvas : MonoBehaviour
{
    [SerializeField] Button _nextButton;
    [SerializeField] int _stageIndex;
    [SerializeField] HeaderCell _headerCell;
    [SerializeField] GameObject _clearPanel;

    void Start()
    {
        Instantiate(_headerCell, transform);
        
        _nextButton.OnClickAsObservable().Subscribe(_ =>
        {
            var nextStageIndex = _stageIndex + 1;
            var nextStageName = $"Stage{nextStageIndex}";
            // TODO 共通化
            Addressables.LoadSceneAsync($"Assets/Scenes/Stages/{nextStageName}.unity");
        });
    }
    
    public void ShowClearPanel()
    {
        _clearPanel.SetActive(true);
    }
}
