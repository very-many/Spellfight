using Mirror;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimation;
    [SerializeField] private float transitionDelay = 0.5f;
    private static readonly int EndHash = Animator.StringToHash("End");
    private static readonly int StartHash = Animator.StringToHash("Start");

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transitionAnimation.SetTrigger(StartHash);
    }

    public void StartTransitionAndDelay(Action onTransitionFinished)
    {
        transitionAnimation.ResetTrigger(StartHash);
        transitionAnimation.SetTrigger(EndHash);
        StartCoroutine(LoadSceneAfterTransition(onTransitionFinished));
    }

    private System.Collections.IEnumerator LoadSceneAfterTransition(Action onTransitionFinished)
    {
        yield return new WaitForSeconds(transitionDelay);
        onTransitionFinished?.Invoke();
    }
}
