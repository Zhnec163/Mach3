using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class RecordTable : MonoBehaviour
{
    public GameObject[] rows;

    private List<Row> rowsList = new List<Row>();

    private void Start()
    {
        if (!PlayerPrefs.HasKey("FirstStart") || PlayerPrefs.GetInt("FirstStart") != 0)
        {
            PlayerPrefs.SetInt("FirstStart", 0);
            TextAsset scoreFile = Resources.Load<TextAsset>("Records");
            string[] data = scoreFile.text.Split(new char[] { '\n' });

            for (int i = 1; i < data.Length; i++)
            {
                var elementsCSV = data[i].Split(new char[] { ',' });
                rowsList.Add(new Row(elementsCSV[0], elementsCSV[1], elementsCSV[2], elementsCSV[3]));
            }

            var sort = rowsList.OrderByDescending(x => Convert.ToInt32(x.GetScore())).ToList();

            for (int i = 0; i < rows.Length; i++)
            {
                string namePref = Constants.NamePref + (i + 1).ToString();
                PlayerPrefs.SetString(namePref, sort[i].GetYear() + "," + sort[i].GetMonth() + "," + sort[i].GetDay() + "," + sort[i].GetScore());
            }
            rowsList.Clear();
        }

        for (int i = 0; i < rows.Length; i++)
        {
            string namePrefs = Constants.NamePref + (i + 1).ToString();

            var elements = PlayerPrefs.GetString(namePrefs).Split(new char[] { ',' });
            rowsList.Add(new Row(elements[0], elements[1], elements[2], elements[3]));
        }

        if (PlayerPrefs.GetString("newScore") == "")
        {
            for (int i = 0; i < rows.Length; i++)
            {
                var element = rows[i].GetComponent<Row>();
                element.SetDateText(rowsList[i].GetYear() + " - " + rowsList[i].GetMonth() + " - " + rowsList[i].GetDay());
                element.SetScoreText(rowsList[i].GetScore());
            }
        }
        else
        {
            var newResultElements = PlayerPrefs.GetString("newScore").Split(new char[] { ',' });
            rowsList.Last().SetResult(newResultElements[0], newResultElements[1], newResultElements[2], newResultElements[3]);

            var sort = rowsList.OrderByDescending(x => Convert.ToInt32(x.GetScore())).ToList();

            for (int i = 0; i < rows.Length; i++)
            {
                var element = rows[i].GetComponent<Row>();
                element.SetDateText(sort[i].GetYear() + " - " + sort[i].GetMonth() + " - " + sort[i].GetDay());
                element.SetScoreText(sort[i].GetScore());
                element.SetResult(sort[i].GetYear(), sort[i].GetMonth(), sort[i].GetDay(), sort[i].GetScore());
                if (element.ToString() == PlayerPrefs.GetString("newScore").ToString()) element.HighlightResult();
            }

            for (int i = 0; i < rows.Length; i++)
            {
                string namePrefs = Constants.NamePref + (i + 1).ToString();
                PlayerPrefs.SetString(namePrefs, sort[i].GetYear() + "," + sort[i].GetMonth() + "," + sort[i].GetDay() + "," + sort[i].GetScore());
            }
            PlayerPrefs.SetString("newScore", "");
        }
    }
}

