using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class PaperRoll : MonoBehaviour, IPointerDownHandler
{
    [Header("Settings")]
    [SerializeField] private float _paperPerSwipe = 0.2f;
    [SerializeField] private float _minSwipeLength = 50f;
    [SerializeField] private bool _development = false;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _paperCounterText;
    [SerializeField] private RectTransform _interactionArea;

    [Header("Paper Animation")]
    [SerializeField] private GameObject _whitePaperPrefab;
    [SerializeField] private float _paperSpacing = 100f; // Положительное значение, т.к. контейнер движется вниз
    [SerializeField] private int _initialPoolSize = 10;

    private float _paperLength;
    private Vector2 _startTouchPosition;
    private bool _isDragging = false;
    private bool _swipeCounted = false;

    private PaperAnimator _paperAnimator;
    private Queue<RectTransform> _paperPool = new Queue<RectTransform>();

    private void Start()
    {
        if (!_paperCounterText) Debug.LogError("PaperCounterText не назначен!");
        if (!_interactionArea) _interactionArea = GetComponent<RectTransform>();
        if (!_whitePaperPrefab) Debug.LogError("WhitePaperPrefab не назначен!");

        LoadPaperLength();
        UpdateUI();
        InitializePaperPool();
        _paperAnimator = new PaperAnimator(_interactionArea, _paperPool, _paperSpacing);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
            HandleInput(Input.GetTouch(0).position, Input.GetTouch(0).phase == TouchPhase.Ended);
        else if (_development && Input.GetMouseButton(0))
            HandleInput(Input.mousePosition, Input.GetMouseButtonUp(0));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsPointerOverThisOrChildren(eventData))
        {
            _startTouchPosition = eventData.position;
            _isDragging = true;
            _swipeCounted = false;
        }
    }

    private void HandleInput(Vector2 currentPosition, bool isInputEnded)
    {
        if (!_isDragging) return;

        if (isInputEnded && _swipeCounted)
        {
            UnrollPaper();
            if (AudioManager.Instance) AudioManager.Instance.PlayUnrollSound();
            _isDragging = false;
        }
        else if (!_swipeCounted)
        {
            Vector2 swipeDelta = currentPosition - _startTouchPosition;
            if (swipeDelta.y < -_minSwipeLength && Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x))
                _swipeCounted = true;
        }
    }

    private bool IsPointerOverThisOrChildren(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var result in results)
            if (result.gameObject == gameObject || result.gameObject.transform.IsChildOf(transform))
                return true;
        return false;
    }

    private void InitializePaperPool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            var paper = Instantiate(_whitePaperPrefab, _interactionArea).GetComponent<RectTransform>();
            paper.gameObject.SetActive(false);
            _paperPool.Enqueue(paper);
        }
    }

    private void UnrollPaper()
    {
        _paperLength += _paperPerSwipe;
        SavePaperLength();
        UpdateUI();
        _paperAnimator.AnimateUnroll();
    }

    private void UpdateUI() => _paperCounterText.text = $"{_paperLength:F1}";
    private void SavePaperLength() => PlayerPrefs.SetFloat("PaperLength", _paperLength);
    private void LoadPaperLength() => _paperLength = PlayerPrefs.GetFloat("PaperLength", 0);
}