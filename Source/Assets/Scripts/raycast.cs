using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class raycast : MonoBehaviour
{
    public int[] ballArray;
    public int whiteBallCount = 0;
    public int blackBallCount = 0;
    public float rayLength = 10f;

    public static bool whiteWinsOnAnyRay = false;
    public static bool blackWinsOnAnyRay = false;
    public static bool checkForDraw = false;

    private bool isCheckingRay = false;


    void Start()
    {
        ResetWinFlags();
    }

    void Update()
    {
        if (isCheckingRay)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit[] hits;
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green);

            hits = Physics.RaycastAll(ray, rayLength);

            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            List<int> tempBallList = new List<int>();

            whiteBallCount = 0;
            blackBallCount = 0;

            for (int i = 0; i < hits.Length; i++)
            {
                GameObject hitObject = hits[i].collider.gameObject;

                if (hitObject.CompareTag("WhiteBall"))
                {
                    tempBallList.Add(1);
                    whiteBallCount++;
                }
                else if (hitObject.CompareTag("BlackBall"))
                {
                    tempBallList.Add(2);
                    blackBallCount++;
                }
                else
                {
                    tempBallList.Add(0);
                }
            }

            ballArray = tempBallList.ToArray();

            bool whiteWins = CheckForConsecutive(ballArray, 1, 5);
            bool blackWins = CheckForConsecutive(ballArray, 2, 5);

            if (whiteWins)
            {
                whiteWinsOnAnyRay = true;
            }

            if (blackWins)
            {
                blackWinsOnAnyRay = true;
            }

            checkForDraw = true;
            isCheckingRay = false;
        }
    }

    void LateUpdate()
    {
        if (checkForDraw)
        {
            if (whiteWinsOnAnyRay && blackWinsOnAnyRay)
            {
                Invoke("LoadDrawScene", 0.5f);
            }
            else if (whiteWinsOnAnyRay)
            {
                Invoke("LoadWinWhiteScene", 0.5f);
            }
            else if (blackWinsOnAnyRay)
            {
                Invoke("LoadWinBlackScene", 0.5f);
            }
            checkForDraw = false;
        }

    }

    public bool CheckForConsecutive(int[] array, int value, int count)
    {
        int consecutiveCount = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == value)
            {
                consecutiveCount++;
                if (consecutiveCount == count)
                {
                    return true;
                }
            }
            else
            {
                consecutiveCount = 0;
            }
        }
        return false;
    }

    void LoadWinWhiteScene()
    {
        SceneManager.LoadScene(3);
    }

    void LoadWinBlackScene()
    {
        SceneManager.LoadScene(2);
    }

    void LoadDrawScene()
    {
        SceneManager.LoadScene(4);
    }

    void ResetWinFlags()
    {
        whiteWinsOnAnyRay = false;
        blackWinsOnAnyRay = false;
        checkForDraw = false;
    }

    public void ActivateRayCheck(float duration)
    {
        isCheckingRay = true;
        Invoke("DeactivateRayCheck", duration);
        VisualizeRay();
    }

    void DeactivateRayCheck()
    {
        isCheckingRay = false;
    }

    void VisualizeRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green, 0.2f);
    }
}
