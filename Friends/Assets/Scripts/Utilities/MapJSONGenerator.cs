using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapJSONGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string text = "{";
        foreach (var go in GetComponentsInChildren<Transform>())
        {
            if ((go.name.Length >= 6)&&(go.name.Substring(0, 6) == "Corall"))
            {
                text += ",\n\"" + go.name + "\":{\"x\":" + go.position.x + ",\"y\":" + go.position.z + ",\"type\":circle,\"radius\":" +go.GetComponent<CapsuleCollider>().radius+ "}";
            }
        }
        
        Debug.Log(text);
        
        using (FileStream fstream = new FileStream($"note.txt", FileMode.OpenOrCreate))
        {
            // преобразуем строку в байты
            byte[] array = System.Text.Encoding.Default.GetBytes(text);
            // запись массива байтов в файл
            fstream.Write(array, 0, array.Length);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
