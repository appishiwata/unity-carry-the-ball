using UnityEngine;
using UnityEngine.UI;

public class HeaderCell : MonoBehaviour
{
    [SerializeField] Button _menuButton;
    [SerializeField] Button _closeButton;
    [SerializeField] GameObject _menuPanel;
    
    void Start()
    {
        _menuButton.onClick.AddListener(() =>
        {
            _menuPanel.SetActive(true);
            Time.timeScale = 0f;
        });
        
        _closeButton.onClick.AddListener(() =>
        {
            _menuPanel.SetActive(false);
            Time.timeScale = 1f;
        });
    }
}
