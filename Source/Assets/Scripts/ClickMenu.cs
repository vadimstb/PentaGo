using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ClickMenu : MonoBehaviour
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
            StartCoroutine(SwitchScene());
        }
    }

    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(0);
    }
}
