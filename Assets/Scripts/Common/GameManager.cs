using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    public static TimeManager timeManager;

    [HideInInspector] public static GameStatus gameStatus;
    public static LevelInfo currentlevelInfo;

    public static System.Action OnGameStarted, OnGameSuccess, OnGameFailed, OnGameEnded, OnLevelInitialized;

    public static int CurrentLevelCount { get => PlayerPrefs.GetInt("next_level", 1); }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        currentlevelInfo = FileUtilities.GetCurrentLevelInfo();
    }

    private void Start()
    {
        gameStatus = GameStatus.MENU;
    }

    public static void Fail(float delay = 0)
    {
        if (gameStatus == GameStatus.FAIL) return;
        DOVirtual.DelayedCall(delay, () =>
         {
             OnGameFailed?.Invoke();
             OnGameEnded?.Invoke();
             gameStatus = GameStatus.FAIL;
         });
    }

    public static void Success(float delay = 0)
    {
        if (gameStatus == GameStatus.SUCCESS) return;
        DOVirtual.DelayedCall(delay, () =>
        {
            // int nextLevelSc = PlayerPrefs.GetInt("next_levelSc", 1);

            // int nextLevelScFoo = nextLevelSc + 1;

            // if (nextLevelSc == 20) nextLevelScFoo = 1;

            // PlayerPrefs.SetInt("next_levelSc", nextLevelScFoo);

            // PlayerPrefs.SetInt("next_level", PlayerPrefs.GetInt("next_level", 1) + 1);

            OnGameSuccess?.Invoke();
            OnGameEnded?.Invoke();

            gameStatus = GameStatus.SUCCESS;
        });
    }

    public void NextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void StartGame()
    {
        Time.timeScale = 1;
        gameStatus = GameStatus.PLAY;
        OnGameStarted?.Invoke();
    }

    private void OnDestroy() => DOTween.KillAll();

    public static void PlayParticle(GameObject expObj, Transform objTransform, Vector3? pos = null, Vector3? scale = null, bool doParent = false)
    {
        var playPos = pos.HasValue ? pos.Value : objTransform.position;
        var playScale = scale.HasValue ? scale.Value : expObj.transform.localScale;

        if (doParent) expObj.transform.parent = objTransform;

        var particle = expObj.GetComponent<ParticleSystem>();
        expObj.transform.position = playPos;

        if (playScale != Vector3.zero) expObj.transform.localScale = playScale;

        particle.Play();

        var main = particle.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
    }

    public static void PlayParticle(Pooler particlePooler, Transform objTransform, Vector3? pos = null, Vector3? scale = null, bool doParent = false)
    {
        GameObject expObj = particlePooler.GetObject();

        var playPos = pos.HasValue ? pos.Value : objTransform.position;
        var playScale = scale.HasValue ? scale.Value : expObj.transform.localScale;

        if (doParent) expObj.transform.parent = objTransform;

        var particle = expObj.GetComponent<ParticleSystem>();
        expObj.transform.position = playPos;

        if (playScale != Vector3.zero)
            expObj.transform.localScale = playScale;

        particle.Play();
        var main = particle.main;
        DOVirtual.DelayedCall(main.duration + particle.GetComponentsInChildren<ParticleSystem>().Select(x => x.main).Max(x => x.startLifetime.constantMax), () =>
          {
              particle.Stop();
              DOVirtual.DelayedCall(main.duration, () => expObj.GetComponent<Poolable>().ClearMe());
          });
    }

    public enum GameStatus
    {
        MENU,
        PLAY,
        FAIL,
        SUCCESS,
    }
}