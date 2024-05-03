using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] Button _startButton;
    
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] CanvasGroup _startButtonCanvasGroup;
    
    [SerializeField] ParticleSystem _backgroundParticle;
    
    [SerializeField] BallController _ballController;
    
    [SerializeField] Audio _audioPrefab;
    private Audio _audio;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        _audio = Instantiate(_audioPrefab, transform);
        _audio.PlayBGM();
        
        _startButton.OnClickAsObservable().Subscribe(async _ =>
        {
            _backgroundParticle.Stop();
            _audio.PlaySE(Audio.Clip.SelectStage);
            _audio.FadeOutBGM();
            
            await UniTask.Delay(1000);

            _ballController.ResetBall();
            
            var sequence = DOTween.Sequence();
            await sequence
                .Append(_titleText.DOFade(0, 0.2f))
                .Append(_startButtonCanvasGroup.DOFade(0, 0.2f));
            
            await UniTask.Delay(1000);

            Camera.main!.transform.DOMoveX(-12f, 0.5f).SetRelative();
            
            await UniTask.Delay(300);
            
            var stageName = "Stage1";
            Addressables.LoadSceneAsync($"Assets/Scenes/Stages/{stageName}.unity");
        });
    }
}
