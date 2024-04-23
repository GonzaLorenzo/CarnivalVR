using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsDisplay : MonoBehaviour
{
    private int _points;
    private bool _isReloading = false;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private GameManager _gameManager;

    void Start()
    {
        _gameManager.onPointsUpdate += UpdatePoints;
    }

    private void UpdatePoints(int points)
    {
        _points = points;
        _pointsText.text = points + " PTS";
    }

    public void ReloadText()
    {
        if(!_isReloading)
        {
            _pointsText.text = "RELOAD!";
        }   
        else
        {
            _pointsText.text = _points + " PTS";
        }
    }
}
