using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    public void SetupHotKey(KeyCode _myHotKey)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myText.text = _myHotKey.ToString();
        myHotKey = _myHotKey;
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            Debug.Log("Hot Key is" + myHotKey);
        }
    }
}
