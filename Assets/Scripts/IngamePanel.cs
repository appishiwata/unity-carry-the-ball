using Cysharp.Threading.Tasks;
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
    [SerializeField] CanvasGroup _clearPanelCanvasGroup;
    [SerializeField] Button _nextButton;
    [SerializeField] TextMeshProUGUI _stageNameText;
    
    [SerializeField] TextMeshProUGUI _tutorialText1;
    [SerializeField] TextMeshProUGUI _tutorialText2;
    
    [SerializeField] Button _bgmOffButton;
    [SerializeField] Button _bgmOnButton;
    [SerializeField] Button _seOffButton;
    [SerializeField] Button _seOnButton;
    
    private int _stageIndex;
    
    void Start()
    {
        _stageNameText.text = $"Stage{_stageIndex}";
        
        _bgmOffButton.OnClickAsObservable().Subscribe(_ =>
        {
            _bgmOffButton.gameObject.SetActive(false);
            _bgmOnButton.gameObject.SetActive(true);
            Audio.Instance.SetBGMVolume(0f);
        });
        
        _bgmOnButton.OnClickAsObservable().Subscribe(_ =>
        {
            _bgmOffButton.gameObject.SetActive(true);
            _bgmOnButton.gameObject.SetActive(false);
            Audio.Instance.SetBGMVolume(1f);
        });
        
        _seOffButton.OnClickAsObservable().Subscribe(_ =>
        {
            _seOffButton.gameObject.SetActive(false);
            _seOnButton.gameObject.SetActive(true);
            Audio.Instance.SetSEVolume(0f);
        });
        
        _seOnButton.OnClickAsObservable().Subscribe(_ =>
        {
            Audio.Instance.PlaySE(Audio.Clip.ClickMenu);

            _seOffButton.gameObject.SetActive(true);
            _seOnButton.gameObject.SetActive(false);
            Audio.Instance.SetSEVolume(1f);
        });
        
        _menuButton.OnClickAsObservable().Subscribe(_ =>
        {
            Audio.Instance.PlaySE(Audio.Clip.ClickMenu);
            
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
            Audio.Instance.PlaySE(Audio.Clip.ClickMenu);
            
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

        _nextButton.OnClickAsObservable().Subscribe(async _ =>
        {
            /*
            var nextStageIndex = _stageIndex + 1;
            var nextStageName = $"Stage{nextStageIndex}";
            // TODO 共通化
            Addressables.LoadSceneAsync($"Assets/Scenes/Stages/{nextStageName}.unity");
            */

            var nextStageIndex = _stageIndex + 1;
            var nextStageName = $"Stage{nextStageIndex}";
            
            var sequence = DOTween.Sequence();
            
            await sequence
                .Append(_clearPanelCanvasGroup.DOFade(0, 0.3f))
                .Append(_stageNameText.DOFade(0, 0.3f))
                .AppendInterval(0.5f);
            
            Audio.Instance.PlaySE(Audio.Clip.MoveNext);

            Camera.main!.transform.DOMoveX(-12f, 0.5f).SetRelative();
            
            await UniTask.Delay(300);
            
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
