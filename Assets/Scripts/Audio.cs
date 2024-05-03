using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] AudioSource _audioSourceBGM;
    [SerializeField] AudioSource _audioSourceSE;
    
    public enum Clip
    {
        SelectStage,
        ClickMenu,
    }
    
    [SerializeField] AudioClip[] _clips;
    
    public void PlaySE(Clip clip)
    {
        _audioSourceSE.PlayOneShot(_clips[(int)clip]);
    }
    
    public void PlayBGM()
    {
        _audioSourceBGM.Play();
    }
    
    public async UniTask FadeOutBGM()
    {
        await _audioSourceBGM.DOFade(0f, 1.5f);
    }
}
