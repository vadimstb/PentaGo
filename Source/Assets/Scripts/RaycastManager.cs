using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaycastManager : MonoBehaviour
{
    [SerializeField] private raycast[] raycasts;

    void Start()
    {
        raycasts = FindObjectsOfType<raycast>();
    }

    public void CheckForNoWinners()
    {
        bool hasEmpty = false;

        foreach (var raycast in raycasts)
        {

            foreach (var ball in raycast.ballArray)
            {
                if (ball == 0)
                {
                    hasEmpty = true;
                    break;
                }
            }

            if (hasEmpty)
            {
                break;
            }
        }

        if (!hasEmpty)
        {
            Invoke("NoWinners", 1.0f);
        }
    }

    void NoWinners()
    {
        SceneManager.LoadScene(5);
    }
}
