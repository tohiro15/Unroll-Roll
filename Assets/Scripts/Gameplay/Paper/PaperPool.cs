using System.Collections.Generic;
using UnityEngine;

public class PaperPool : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private GameObject _paperPrefab;
    [SerializeField] private RectTransform _contentArea;
    [SerializeField] private int _initialSize = 5;
    #endregion

    #region Private Fields
    private Queue<GameObject> _pool = new Queue<GameObject>();
    public static PaperPool Instance { get; private set; }
    public int InitialSize => _initialSize;  // Доступ к _initialSize
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < _initialSize; i++)
            AddNewObjectToPool();
    }
    #endregion

    #region Pool Management
    private GameObject AddNewObjectToPool()
    {
        GameObject obj = Instantiate(_paperPrefab, _contentArea);
        obj.SetActive(false);
        _pool.Enqueue(obj);
        return obj;
    }

    public GameObject GetPaper()
    {
        if (_pool.Count == 0) AddNewObjectToPool();
        GameObject obj = _pool.Dequeue();
        obj.transform.SetParent(_contentArea);
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.transform.SetParent(transform);
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
    #endregion
}
