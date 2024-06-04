using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class ObjectClickAnimation : MonoBehaviour
{
    public Animator animator;
    public string animationTriggerName = "StartAnimation";

    private bool isAnimationPlayed = false;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void OnMouseDown()
    {
        if (!isAnimationPlayed)
        {
            isAnimationPlayed = true;
            animator.SetTrigger(animationTriggerName);
            StartCoroutine(RestartScene());
        }
    }

    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
