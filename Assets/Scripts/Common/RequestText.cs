using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class RequestText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI requestText;
    [SerializeField] private float duration = 4;
    [SerializeField] private Ease inEase = Ease.OutBack, outEase = Ease.InBack;
    [SerializeField] private float scaleTweenDuration = .5f;

    private Sequence _seq;

    private HorizontalLayoutGroup _horizontalLayoutGroup;

    private void Awake()
    {
        _horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
    }

    public void CreateText(string text)
    {
        requestText.text = text;
        _horizontalLayoutGroup.UpdateLayout();
        if (_seq.IsActive())
        {
            _seq.Kill();
            transform.localScale = Vector3.zero;
        }
        Canvas.ForceUpdateCanvases();
        _seq = DOTween.Sequence()
        .Append(transform.DOScale(Vector3.one, scaleTweenDuration).SetEase(inEase))
        .AppendInterval(duration)
        .Append(transform.DOScale(Vector3.zero, scaleTweenDuration).SetEase(outEase));
    }

    public void CloseCurrentText(System.Action action = null)
    {
        if (transform.localScale == Vector3.zero)
        {
            action?.Invoke();
            return;
        }
        _seq.Kill();
        transform.DOScale(Vector3.zero, scaleTweenDuration).SetEase(outEase).OnComplete(() => action?.Invoke());
    }
}
