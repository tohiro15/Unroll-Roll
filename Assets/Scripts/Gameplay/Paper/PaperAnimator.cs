using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PaperAnimator
{
    private RectTransform _interactionArea;
    private GameObject _paperPrefab;
    private float _paperSpacing;
    private int _maxPaperCount;

    private Queue<RectTransform> _paperQueue = new Queue<RectTransform>();

    public PaperAnimator(RectTransform interactionArea, GameObject paperPrefab, float paperSpacing, int maxPaperCount)
    {
        _interactionArea = interactionArea;
        _paperPrefab = paperPrefab;
        _paperSpacing = paperSpacing;
        _maxPaperCount = maxPaperCount;
    }

    public void AnimatePaper()
    {
        if (_paperQueue.Count >= _maxPaperCount)
        {
            // ���������� ������ ������ �����, ���� ��� ������ ���������
            RectTransform oldPaper = _paperQueue.Dequeue();
            oldPaper.DOAnchorPosY(_interactionArea.anchoredPosition.y + _paperSpacing, 0.3f);
            oldPaper.SetAsLastSibling(); // ���������� � �����

            // ���������� ������ ��� ���������� �������������
            oldPaper.gameObject.SetActive(false);
            oldPaper.anchoredPosition = new Vector2(0f, -(_paperQueue.Count * _paperSpacing));  // ���������� ����
            oldPaper.gameObject.SetActive(true);
        }

        // ������� ����� ������
        RectTransform newPaper = GameObject.Instantiate(_paperPrefab, _interactionArea).GetComponent<RectTransform>();
        newPaper.anchoredPosition = new Vector2(0f, -(_paperQueue.Count * _paperSpacing));

        // ��������� � �������
        _paperQueue.Enqueue(newPaper);

        // ������ ����������� ������ ������
        newPaper.DOSizeDelta(new Vector2(newPaper.sizeDelta.x, 200f), 0.5f);
    }
}
