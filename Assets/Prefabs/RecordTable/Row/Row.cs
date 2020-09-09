using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Row : MonoBehaviour
{
    public TextMeshProUGUI _dateText;
    public TextMeshProUGUI _scoreText;
    public Image _currentResult;

    private string _year, _month, _day, _score;
    public Row(string yaer, string month, string day, string score)
    {
        _year = yaer;
        _month = month;
        _day = day;
        _score = score;
    }
    public string GetYear()
    {
        return _year;
    }
    public string GetMonth()
    {
        return _month;
    }
    public string GetDay()
    {
        return _day;
    }
    public string GetScore()
    {
        return _score;
    }
    public void SetResult(string year, string month, string day, string score)
    {
        _year = year;
        _month = month;
        _day = day;
        _score = score;
    }
    public void SetDateText(string text)
    {
        _dateText.text = text;
    }
    public void SetScoreText(string text)
    {
        _scoreText.text = text;
    }
    public void HighlightResult() 
    {
        _currentResult.color = new Color(_currentResult.color.r, _currentResult.color.g, _currentResult.color.b, 1f);

    }
    public override string ToString()
    {
        return _year + "," + _month + "," + _day + "," + _score; 
    }
}