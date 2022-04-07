using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LerpManager : Singleton<LerpManager>
{
    [SerializeField] private AnimationCurve[] animationCurves;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        var instance = GameObject.FindGameObjectWithTag("LerpManager");
        if (instance && instance != gameObject)
        {
            Destroy(gameObject);
        }

        SceneManager.activeSceneChanged += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene current, Scene next)
    {
        if (this != null) StopAllCoroutines();
    }

    public static void WaitForFrames(int howManyFrames, System.Action action) => Instance.StartCoroutine(Instance.WaitForFramesRoutine(howManyFrames, action));

    private IEnumerator WaitForFramesRoutine(int howManyFrames, System.Action action)
    {
        for (int i = 0; i < howManyFrames; i++)
        {
            yield return null;
        }
        action.Invoke();
    }

    public static IEnumerator LoopWait<T>(T[] array, float timeBetweenElements, System.Action<T> elementAction, System.Action finishedAction = null)
    {
        for (int i = 0; i < array.Length; i++)
        {
            elementAction.Invoke(array[i]);
            yield return new WaitForSeconds(timeBetweenElements);
        }
        finishedAction?.Invoke();
    }

    public static AnimationCurve PresetToAnimationCurve(PresetAnimationCurves curve) => Instance.animationCurves[(int)curve];

    public static AnimationCurve CreateAnimationCurve(Keyframe start, Keyframe end, Keyframe[] extraKeys = null)
    {
        List<Keyframe> ks = new List<Keyframe>();
        ks.Add(start);
        ks.AddRange(extraKeys);
        if (ks.FirstOrDefault(x => x.time == 1).time == 0) ks.Add(end);
        return new AnimationCurve(ks.ToArray());
    }
}

public enum PresetAnimationCurves
{
    DECREASING,
    INCREASE,
    SMOOTH,
    LINEER,
    BOUNCE,
}
