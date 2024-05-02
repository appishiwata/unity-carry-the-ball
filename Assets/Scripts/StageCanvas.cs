using UnityEngine;

public class StageCanvas : MonoBehaviour
{
    [SerializeField] int _stageIndex;
    [SerializeField] IngamePanel _ingamePanel;
    
    void Start()
    {
        var ingamePanel = Instantiate(_ingamePanel, transform);
        
        ingamePanel.SetStageIndex(_stageIndex);
    }
}
