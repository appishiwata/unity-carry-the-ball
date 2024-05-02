using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UnityEngine.AddressableAssets;

public class IngamePanel : MonoBehaviour
{
    [SerializeField] Button _menuButton;
    [SerializeField] GameObject _menuPanel;
    [SerializeField] Button _closeButton;

    [SerializeField] GameObject _clearPanel;
    [SerializeField] Button _nextButton;
    [SerializeField] TextMeshProUGUI _stageNameText;
    
    [SerializeField] TextMeshProUGUI _tutorialText1;
    [SerializeField] TextMeshProUGUI _tutorialText2;
    
    private int _stageIndex;
    
    void Start()
    {
        _stageNameText.text = $"Stage{_stageIndex}";
        
        _menuButton.OnClickAsObservable().Subscribe(_ =>
        {
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() =>
                {
                    _menuButton.transform.DOMoveY(100f, 0.1f).SetEase(Ease.OutBack).SetRelative(true);
                })
                .AppendInterval(0.1f)
                .AppendCallback(() =>
                {
                    _menuButton.transform.DOMoveY(-100f, 0.1f).SetEase(Ease.OutBack).SetRelative(true);
                })
                .AppendInterval(0.1f)
                .AppendCallback(() =>
                {
                    _menuButton.gameObject.SetActive(false);
                    _closeButton.gameObject.SetActive(true);
                    _menuPanel.SetActive(true);
                    Time.timeScale = 0f;
                });
        });
        
        _closeButton.OnClickAsObservable().Subscribe(_ =>
        {
            _menuPanel.SetActive(false);
            Time.timeScale = 1f;

            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() =>
                {
                    _menuButton.transform.DOMoveY(100f, 0.1f).SetEase(Ease.OutBack).SetRelative(true);
                })
                .AppendInterval(0.1f)
                .AppendCallback(() =>
                {
                    _menuButton.transform.DOMoveY(-100f, 0.1f).SetEase(Ease.OutBack).SetRelative(true);
                })
                .AppendCallback(() =>
                {
                    _closeButton.gameObject.SetActive(false);
                    _menuButton.gameObject.SetActive(true);
                });
        });

        _nextButton.OnClickAsObservable().Subscribe(_ =>
        {
            var nextStageIndex = _stageIndex + 1;
            var nextStageName = $"Stage{nextStageIndex}";
            // TODO 共通化
            Addressables.LoadSceneAsync($"Assets/Scenes/Stages/{nextStageName}.unity");
        });
        
        // チュートリアル用
        _tutorialText1.DOFade(0, 0);
        _tutorialText2.DOFade(0, 0);
        if (_stageIndex == 1)
        {
            _tutorialText1.DOFade(1, 1).SetDelay(3);
            _tutorialText2.DOFade(1, 1).SetDelay(6);
        }
    }
    
    public void SetStageIndex(int stageIndex)
    {
        _stageIndex = stageIndex;
    }
    
    public void ShowClearPanel()
    {
        if (_stageIndex == 1)
        {
            _tutorialText1.gameObject.SetActive(false);
            _tutorialText2.gameObject.SetActive(false);
        }
        
        _clearPanel.SetActive(true);
    }
}
