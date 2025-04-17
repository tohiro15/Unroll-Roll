using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PaperRoll : MonoBehaviour, IPointerDownHandler
{
    #region Serialized Fields

    [Header("Swipe Settings")]
    [SerializeField] private float _paperPerSwipe = 0.2f;
    [SerializeField] private float _minSwipeLength = 50f;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _paperCounterText;
    [SerializeField] private RectTransform _interactionArea;

    [Header("Animation Unroll")]
    [SerializeField] private ScrollRect _scrollRect;

    #endregion

    #region Private Fields

    private Queue<GameObject> _activePapers = new Queue<GameObject>();
    private Vector2 _startTouchPosition;
    private bool _isDragging = false;
    private bool _swipeCounted = false;

    #endregion

    #region Unity Methods

    private void Start()
    {
        UpdateUI();
        _interactionArea ??= GetComponent<RectTransform>();
    }

    private void Update()
    {
        HandleTouchInput();
    }

    #endregion

    #region Input Handling

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsPointerOverThisOrChildren(eventData))
        {
            _startTouchPosition = eventData.position;
            _isDragging = true;
            _swipeCounted = false;
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended) ProcessSwipe();
            else if (touch.phase == TouchPhase.Moved) CheckSwipe(touch.position);
        }
    }

    private void CheckSwipe(Vector2 currentPosition)
    {
        if (!_swipeCounted && (currentPosition - _startTouchPosition).y < -_minSwipeLength)
            _swipeCounted = true;
    }

    private void ProcessSwipe()
    {
        if (_isDragging && _swipeCounted)
        {
            UnrollPaper();
            AudioManager.Instance?.PlayUnrollSound();
        }
        _isDragging = false;
    }

    #endregion

    #region Paper Management

    private void AnimateUnroll()
    {
        GameObject paperGO;

        if (_activePapers.Count < PaperPool.Instance.InitialSize)
        {
            paperGO = PaperPool.Instance.GetPaper();
            _activePapers.Enqueue(paperGO);
        }
        else
        {
            paperGO = _activePapers.Dequeue();
            _activePapers.Enqueue(paperGO);
        }

        RectTransform paperRect = paperGO.GetComponent<RectTransform>();

        paperRect.SetAsLastSibling();
        paperRect.sizeDelta = new Vector2(paperRect.sizeDelta.x, 0f);
        paperRect.anchoredPosition = new Vector2(0, 0);

        paperRect.DOSizeDelta(new Vector2(paperRect.sizeDelta.x, 100f), 0.5f).SetEase(Ease.OutSine);

        StartCoroutine(ScrollToBottomNextFrame());
    }

    private IEnumerator ScrollToBottomNextFrame()
    {
        yield return null;

        float start = _scrollRect.verticalNormalizedPosition;
        float duration = 0.3f;
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            _scrollRect.verticalNormalizedPosition = Mathf.Lerp(start, 0f, time / duration);
            yield return null;
        }
        _scrollRect.verticalNormalizedPosition = 0f;
    }

    #endregion

    #region Utility Methods

    private bool IsPointerOverThisOrChildren(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject == gameObject || result.gameObject.transform.IsChildOf(transform))
                return true;
        }
        return false;
    }

    private void UnrollPaper()
    {
        PaperCurrencyManager.Instance.AddPaper(_paperPerSwipe);
        UpdateUI();
        AnimateUnroll();
    }

    private void UpdateUI()
    {
        _paperCounterText.text = $"{PaperCurrencyManager.Instance.PaperLength:F1}";
    }

    #endregion
}
