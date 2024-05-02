using DG.Tweening;
using TMPro;
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
    
    [SerializeField] TextMeshProUGUI _tutorialText1;
    [SerializeField] TextMeshProUGUI _tutorialText2;

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
        
        // チュートリアル用
        if (_stageIndex == 1)
        {
            _tutorialText1.DOFade(0, 0);
            _tutorialText2.DOFade(0, 0);
            _tutorialText1.DOFade(1, 1).SetDelay(2);
            _tutorialText2.DOFade(1, 1).SetDelay(5);
        }
    }
    
    public void ShowClearPanel()
    {
        _clearPanel.SetActive(true);
    }
}
