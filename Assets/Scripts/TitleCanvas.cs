using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] Button _startButton;
    
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] CanvasGroup _startButtonCanvasGroup;
    [SerializeField] TextMeshProUGUI _startButtonText;
    
    [SerializeField] ParticleSystem _backgroundParticle;
    
    [SerializeField] BallController _ballController;
    
    [SerializeField] Audio _audio;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        Audio.Instance.PlayBGM();
        
        _startButton.OnClickAsObservable().Subscribe(async _ =>
        {
            _startButtonCanvasGroup.interactable = false;
            _startButton.transform.DOMoveY(50f, 0.5f).SetRelative();
            _startButtonText.transform.DOScale(2f, 0.5f);
            _startButtonCanvasGroup.DOFade(0, 0.5f);
            
            _backgroundParticle.Stop();
            Audio.Instance.PlaySE(Audio.Clip.SelectStage);
            
            await UniTask.Delay(1000);

            _ballController.ResetBall();
            
            var sequence = DOTween.Sequence();
            await sequence
                .Append(_titleText.DOFade(0, 0.2f))
                .Append(_startButtonCanvasGroup.DOFade(0, 0.2f));
            
            await UniTask.Delay(1000);
            
            Audio.Instance.PlaySE(Audio.Clip.MoveNext);

            Camera.main!.transform.DOMoveX(-12f, 0.5f).SetRelative();
            
            await UniTask.Delay(300);
            
            var stageName = "Stage1";
            Addressables.LoadSceneAsync($"Assets/Scenes/Stages/{stageName}.unity");
        });
    }
}
