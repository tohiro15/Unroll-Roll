using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class MummyBurialChamber : MonoBehaviour
{
    #region UI References

    [Header("UI Components")]
    [SerializeField] private GameObject _contentContainer;
    [SerializeField] private TextMeshProUGUI _mummyCount;
    [SerializeField] private GameObject _mummyUIPrefab;

    #endregion

    #region Data

    private List<MummyData> _mummyDataList = new List<MummyData>();
    private const string SaveKey = "SavedMummiesV2";

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        MummyWrap.OnMummyCreated += AddMummyToUI;
    }

    private void OnDisable()
    {
        MummyWrap.OnMummyCreated -= AddMummyToUI;
    }

    private void Start()
    {
        LoadMummies();
    }

    #endregion

    #region Mummy Logic

    private void AddMummyToUI(string json)
    {
        MummyData mummy = JsonUtility.FromJson<MummyData>(json);

        if (ColorUtility.TryParseHtmlString("#" + mummy.ColorHex, out Color color))
        {
            CreateMummyUI(mummy.Name, color);
        }
        else
        {
            CreateMummyUI(mummy.Name, Color.white);
        }

        _mummyDataList.Add(mummy);
        SaveMummies();
    }

    private void CreateMummyUI(string name, Color color)
    {
        GameObject newMummyUI = Instantiate(_mummyUIPrefab, _contentContainer.transform);

        TextMeshProUGUI nameText = newMummyUI.GetComponentInChildren<TextMeshProUGUI>();
        if (nameText != null)
        {
            nameText.text = name;
        }

        Image[] images = newMummyUI.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            if (img.gameObject.name == "Mummy")
            {
                img.color = color;
                break;
            }
        }

        _mummyCount.text = _contentContainer.transform.childCount.ToString();
    }

    #endregion

    #region Save & Load

    private void SaveMummies()
    {
        string json = JsonUtility.ToJson(new MummyDataList { Mummies = _mummyDataList });
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    private void LoadMummies()
    {
        _mummyDataList.Clear();

        if (PlayerPrefs.HasKey(SaveKey))
        {
            string data = PlayerPrefs.GetString(SaveKey);
            MummyDataList list = JsonUtility.FromJson<MummyDataList>(data);

            if (list != null && list.Mummies != null)
            {
                foreach (var mummy in list.Mummies)
                {
                    if (ColorUtility.TryParseHtmlString("#" + mummy.ColorHex, out Color color))
                    {
                        CreateMummyUI(mummy.Name, color);
                    }
                    else
                    {
                        CreateMummyUI(mummy.Name, Color.white);
                    }

                    _mummyDataList.Add(mummy);
                }
            }
        }
    }

    #endregion

    #region Serializable Class

    [System.Serializable]
    public class MummyDataList
    {
        public List<MummyData> Mummies;
    }

    #endregion
}
