using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class MessagePanelScript : MonoBehaviour
{

    [SerializeField] private Text text;

    private void setPrefferedHeight()
    {
        float prefHeight = text.preferredHeight + 14;
        text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, prefHeight);
        transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, prefHeight);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setPrefferedHeight();
    }
}
