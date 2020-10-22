using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;


public class MapJSONGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string text = "{";
        foreach (var go in GetComponentsInChildren<Transform>())
        {
            if (go.name.Length >= 7 && go.name.Substring(0, 7)=="Terrain")
            {
                Texture[] maps = go.GetComponent<Terrain>().terrainData.alphamapTextures;
                int i = 0;
                foreach (var map in maps)
                {
                    Debug.Log("MAP " + i + " " + map.GetNativeTexturePtr());

                    // For testing purposes, also write to a file in the project folder
                    // File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
                }
                
            }
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
        foreach (var go in GetComponentsInChildren<Transform>())
        {
            if (go.tag == "Terrain")
            {
                Debug.Log("MAP TERRAIN");
                Texture[] maps = go.GetComponent<Terrain>().terrainData.alphamapTextures;
                int i = 0;
                foreach (var map in maps)
                {
                    //map.graphicsFormat = GraphicsFormat.A10R10G10B10_XRSRGBPack32;
                    //Debug.Log("MAP " + i + " " + ());

                    // For testing purposes, also write to a file in the project folder
                    // File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
                }
                
            }
            /*if ((go.name.Length >= 6)&&(go.name.Substring(0, 6) == "Corall"))
            {
                text += ",\n\"" + go.name + "\":{\"x\":" + go.position.x + ",\"y\":" + go.position.z + ",\"type\":circle,\"radius\":" +go.GetComponent<CapsuleCollider>().radius+ "}";
            }*/
        }
    }
}
