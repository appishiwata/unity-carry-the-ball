using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class IngamePanel : MonoBehaviour
{
    [SerializeField] Image _bgImage;
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

    [SerializeField] Button _titleButton;
    [SerializeField] Button _resetButton;
    
    private int _stageIndex;
    
    void Start()
    {
        _stageNameText.text = $"Stage{_stageIndex}";
        
        _bgmOffButton.gameObject.SetActive(SaveManager.Instance.BgmVolume == 1f);
        _bgmOnButton.gameObject.SetActive(SaveManager.Instance.BgmVolume == 0f);
        _seOffButton.gameObject.SetActive(SaveManager.Instance.SeVolume == 1f);
        _seOnButton.gameObject.SetActive(SaveManager.Instance.SeVolume == 0f);

        _titleButton.OnClickAsObservable().Subscribe(_ =>
        {
            Time.timeScale = 1f;
            Audio.Instance.PlaySE(Audio.Clip.ClickMenu);
            SceneManager.LoadSceneAsync("TitleScene");
        }).AddTo(this);
        
        _resetButton.OnClickAsObservable().Subscribe(_ =>
        {
            Time.timeScale = 1f;
            Audio.Instance.PlaySE(Audio.Clip.ClickMenu);
            SaveManager.Instance.ResetData();
            SceneManager.LoadSceneAsync("TitleScene");
        }).AddTo(this);
        
        _bgmOffButton.OnClickAsObservable().Subscribe(_ =>
        {
            _bgmOffButton.gameObject.SetActive(false);
            _bgmOnButton.gameObject.SetActive(true);
            Audio.Instance.SetBGMVolume(0f);
            SaveManager.Instance.BgmVolume = 0f;
        }).AddTo(this);
        
        _bgmOnButton.OnClickAsObservable().Subscribe(_ =>
        {
            _bgmOffButton.gameObject.SetActive(true);
            _bgmOnButton.gameObject.SetActive(false);
            Audio.Instance.SetBGMVolume(1f);
            SaveManager.Instance.BgmVolume = 1f;
        }).AddTo(this);
        
        _seOffButton.OnClickAsObservable().Subscribe(_ =>
        {
            _seOffButton.gameObject.SetActive(false);
            _seOnButton.gameObject.SetActive(true);
            Audio.Instance.SetSEVolume(0f);
            SaveManager.Instance.SeVolume = 0f;
        }).AddTo(this);
        
        _seOnButton.OnClickAsObservable().Subscribe(_ =>
        {
            Audio.Instance.PlaySE(Audio.Clip.ClickMenu);

            _seOffButton.gameObject.SetActive(true);
            _seOnButton.gameObject.SetActive(false);
            Audio.Instance.SetSEVolume(1f);
            SaveManager.Instance.SeVolume = 1f;
        }).AddTo(this);

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
                    _bgImage.gameObject.SetActive(true);
                    Time.timeScale = 0f;
                });
        }).AddTo(this);
        
        _closeButton.OnClickAsObservable().Subscribe(_ =>
        {
            Audio.Instance.PlaySE(Audio.Clip.ClickMenu);
            
            _menuPanel.SetActive(false);
            _bgImage.gameObject.SetActive(false);
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
        }).AddTo(this);

        _nextButton.OnClickAsObservable().Subscribe(async _ =>
        {
            _nextButton.interactable = false;
            
            var nextStageIndex = _stageIndex + 1;
            var nextStageName = $"Stage{nextStageIndex}";
            
            var sequence = DOTween.Sequence();
            
            await sequence
                .Append(_clearPanelCanvasGroup.DOFade(0, 0.3f))
                .Append(_stageNameText.DOFade(0, 0.3f))
                .Append(_bgImage.DOFade(0, 0.3f))
                .AppendInterval(0.5f);
            
            Audio.Instance.PlaySE(Audio.Clip.MoveNext);

            Camera.main!.transform.DOMoveX(-12f, 0.5f).SetRelative();
            
            await UniTask.Delay(300);
            
            Addressables.LoadSceneAsync($"Assets/Scenes/Stages/{nextStageName}.unity");
        }).AddTo(this);
        
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
        _bgImage.gameObject.SetActive(true);
    }
}
