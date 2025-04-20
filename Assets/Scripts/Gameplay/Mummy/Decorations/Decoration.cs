using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Decoration : MonoBehaviour
{
    [SerializeField] private Image _iconImage; 
    [SerializeField] private Transform _targetPosition;

    private MummyWrap _mummyWrap;

    private void Start()
    {
        _mummyWrap = FindFirstObjectByType<MummyWrap>();
    }

    public void Init(MummyWrap mummyWrap, Transform target)
    {
        _mummyWrap = mummyWrap;
        _targetPosition = target;
    }

    public void BuyDecoration()
    {
        if (_mummyWrap == null)
        {
            Debug.LogError("MummyWrap не был инициализирован!");
            return;
        }

        _iconImage.gameObject.SetActive(true);

        if (_mummyWrap != null)
        {
            _mummyWrap.AddDecoration(this);
        }
        else
        {
            Debug.LogWarning("MummyWrap is null!");
        }

        gameObject.SetActive(false);
    }

}
