using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LoadStageButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _stageNameText;
    [SerializeField] Button _button;
    
    public string _stageName;
    
    public static readonly Subject<string> OnClicked = new();

    private void Start()
    {
        _button.OnClickAsObservable().Subscribe(_ =>
        {
            OnClicked.OnNext(_stageName);
        });
    }

    public void SetStageIndex(int stageIndex)
    {
        _stageName = $"Stage{stageIndex}";
        _stageNameText.text = _stageName;
    }
}
