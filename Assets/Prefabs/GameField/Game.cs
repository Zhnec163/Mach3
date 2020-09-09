using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject ball;
    public Transform spawnerBalls;

    private int score = 0;
    private int movesLeft = 5;

    private List<Ball> verticalElementOther = new List<Ball>();
    private List<Ball> verticalSet = new List<Ball>();
    private List<Ball> horizontalSet = new List<Ball>();

    private Ball[,] ballsField = new Ball[Constants.BallsInHeight, Constants.BallsInWidth]; 

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI movesLeftText;

    private bool isFreezeAction;

    private GameObject gameoverMessage;

    private void Awake()
    {
        scoreText = Init.ScoreText();
        movesLeftText = Init.MovesLeftText();
        gameoverMessage = Init.GameoverMessage();
        gameoverMessage.SetActive(false);

        for (int i = 0; i < Constants.BallsInHeight; i++)
        {
            for (int j = 0; j < Constants.BallsInWidth; j++)
            {
                var _ball = Instantiate(ball,
                    new Vector3(spawnerBalls.position.x + j, spawnerBalls.position.y - i),
                    Quaternion.identity,
                    spawnerBalls.transform);

                ballsField[i, j] = _ball.GetComponent<Ball>();

                ballsField[i, j].SetIndexI(i);
                ballsField[i, j].SetIndexJ(j);
                ballsField[i, j].SetCoordinates();
                ballsField[i, j].SetRandomColor();
            }
        }
        Training();
    }
    private void Training() 
    {
        int bestMoveIndexI = 0;
        int bestMoveIndexJ = 0;
        int series = 0;

        foreach (var item in ballsField)
        {
            CountingBalls(item.GetIndexI(), item.GetIndexJ(), item.color);
            if (verticalSet.Count + horizontalSet.Count > series)
            {
                series = verticalSet.Count + horizontalSet.Count;
                bestMoveIndexI = item.GetIndexI();
                bestMoveIndexJ = item.GetIndexJ();
            }
        }
        ballsField[bestMoveIndexI, bestMoveIndexJ].AnimationFlashing();
    }

    public void Action(int indexI, int indexJ, Color color)
    {
        if (!isFreezeAction)
        {
            isFreezeAction = true;

            CountingBalls(indexI, indexJ, color);

            int result = verticalSet.Count + horizontalSet.Count;

            AddScore(result);

            if (result > 2) AddMoves(result - 1);
            else SubtractMoves();

            StartCoroutine(Reinstall(indexI, indexJ));
        }
    }
    private IEnumerator Reinstall(int indexI, int indexJ) 
    {
        foreach (var item in verticalSet) item.AnimationCollapse();
        foreach (var item in horizontalSet) item.AnimationCollapse();

        yield return new WaitForSeconds(0.5f);

        ReinstallBallsField(indexJ, verticalSet, horizontalSet, verticalElementOther);

        if (indexI == 0) yield return new WaitForSeconds(Constants.SpeedFall);
        else yield return new WaitForSeconds(Constants.SpeedFall * indexI);

        if (NotMovesPossible() || movesLeft == 0) 
        {
            GameOver();
        }

        isFreezeAction = false;
    }
    public bool InTop()
    {
        List<Row> rowsList = new List<Row>();

        for (int i = 0; i < Constants.PlacesInTheTop; i++)
        {
            string namePref = Constants.NamePref + (i + 1).ToString();
            var elements = PlayerPrefs.GetString(namePref).Split(new char[] { ',' });

            rowsList.Add(new Row(elements[0], elements[1], elements[2], elements[3]));
        }

        foreach (var item in rowsList)
        {
            if (score > Convert.ToInt32(item.GetScore())) return true;
        }
        return false;
    }
    private bool NotMovesPossible()
    {
        bool move = false;
        foreach (var item in ballsField)
        {
            CountingBalls(item.GetIndexI(), item.GetIndexJ(), item.color);
            if (verticalSet.Count + horizontalSet.Count > Constants.MinCombinationLength) move = true;
        }
        if (move) return false;
        return true;
    }
    private void CountingBalls(int indexI, int indexJ, Color color)
    {
        verticalElementOther.Clear();
        horizontalSet.Clear();
        verticalSet.Clear();

        verticalSet.Add(ballsField[indexI, indexJ]);

        int counter = 1;
        if (ballsField[indexI, indexJ].GetIndexI() != 0)
        {
            bool verticalSetEnd = false;

            while (ballsField[indexI - counter, indexJ].GetIndexI() != 0)
            {
                if (!verticalSetEnd && ballsField[indexI - counter, indexJ].color == color)
                {
                    verticalSet.Add(ballsField[indexI - counter, indexJ]);
                }
                else
                {
                    verticalElementOther.Add(ballsField[indexI - counter, indexJ]);
                    verticalSetEnd = true;
                }
                counter++;
            }

            if (ballsField[0, indexJ].color == color && !verticalSetEnd) verticalSet.Add(ballsField[0, indexJ]);
            else verticalElementOther.Add(ballsField[0, indexJ]);
        }
        counter = 1;
        if (ballsField[indexI, indexJ].GetIndexI() != Constants.BallsInHeight - 1)
        {
            while (ballsField[indexI + counter, indexJ].color == color)
            {
                verticalSet.Insert(0, ballsField[indexI + counter, indexJ]);

                if (ballsField[indexI + counter, indexJ].GetIndexI() == Constants.BallsInHeight - 1) break;
                counter++;
            }
        }
        counter = 1;
        if (ballsField[indexI, indexJ].GetIndexJ() != 0)
        {
            while (ballsField[indexI, indexJ - counter].color == color)
            {
                horizontalSet.Add(ballsField[indexI, indexJ - counter]);

                if (ballsField[indexI, indexJ - counter].GetIndexJ() == 0) break;
                counter++;
            }
        }
        counter = 1;
        if (ballsField[indexI, indexJ].GetIndexJ() != Constants.BallsInWidth - 1)
        {
            while (ballsField[indexI, indexJ + counter].color == color)
            {
                horizontalSet.Add(ballsField[indexI, indexJ + counter]);

                if (ballsField[indexI, indexJ + counter].GetIndexJ() == Constants.BallsInWidth - 1) break;
                counter++;
            }
        }
    }
    private void GameOver()
    {
        if (InTop())
        {
            string newResult = DateTime.Now.Year + "," + DateTime.Now.Month + "," + DateTime.Now.Day + "," + score;
            PlayerPrefs.SetString("newScore", newResult);
            SceneManager.LoadScene("Score");
        }
        else gameoverMessage.SetActive(true);
    }
    private void AddMoves(int addMoves)
    {
        movesLeft += addMoves;
        movesLeftText.SetText(movesLeft.ToString());
    }
    private void AddScore(int addScore)
    {
        score += addScore;
        scoreText.SetText(score.ToString());
    }
    private void SubtractMoves() 
    {
        movesLeft--;
        movesLeftText.SetText((movesLeft).ToString());
    }
    private void ReinstallBallsField(int indexJ, List<Ball> verticalSet, List<Ball> horizontalSet, List<Ball> verticalElementOther)
    {
        for (int i = 0; i < verticalSet.Count; i++)
        {
            verticalSet[i].transform.localPosition = new Vector3(verticalSet[i].GetIndexJ(), i + 1, 0);
            verticalSet[i].SetRandomColor();
        }

        var allElement = verticalSet.Union(verticalElementOther).ToList();
        var allElementInverted = verticalElementOther.Union(verticalSet).ToList();

        var cloneCollection = new List<Ball>();

        foreach (var item in allElement)
        {
            var clonedBall = new Ball();

            clonedBall.SetPositionY(item.GetPositionY());
            clonedBall.SetIndexI(item.GetIndexI());
            cloneCollection.Add(clonedBall);
        }

        int bottomElement = allElement[0].GetIndexI();

        for (int i = 0; i < allElement.Count; i++)
        {
            ballsField[bottomElement - i, indexJ] = allElementInverted[i];

            ballsField[bottomElement - i, indexJ].SetPositionY(cloneCollection[i].GetPositionY());
            ballsField[bottomElement - i, indexJ].SetIndexI(cloneCollection[i].GetIndexI());
        }

        foreach (var item in horizontalSet)
        {
            item.transform.localPosition = new Vector3(item.GetIndexJ(), 1, 0);
            var upperElement = ballsField[item.GetIndexI(), item.GetIndexJ()];

            for (int i = 0; i < item.GetIndexI(); i++)
            {
                ballsField[item.GetIndexI() - i, item.GetIndexJ()] = ballsField[item.GetIndexI() - i - 1, item.GetIndexJ()];
                ballsField[item.GetIndexI() - i, item.GetIndexJ()].SetIndexI(item.GetIndexI() - i);
                ballsField[item.GetIndexI() - i, item.GetIndexJ()].SetPositionY(item.GetPositionY() + i);
            }

            upperElement.SetIndexI(0);
            upperElement.SetPositionY(0);
            upperElement.SetRandomColor();
            ballsField[0, item.GetIndexJ()] = upperElement;
        }
        foreach (var item in ballsField) item.MotionBegin();
    }
}
