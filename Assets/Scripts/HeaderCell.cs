using System.Collections;
using System.Collections.Generic;
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
        });
        
        _closeButton.onClick.AddListener(() =>
        {
            _menuPanel.SetActive(false);
        });
    }
}
