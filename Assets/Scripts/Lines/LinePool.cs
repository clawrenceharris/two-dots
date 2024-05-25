using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePool : MonoBehaviour
{
    public static LinePool Instance { get; private set; }
    private readonly Queue<ConnectorLine> linePool = new();
    
    

    public void FillPool(int size){
        for(int i = 0; i < size; i++){
            ConnectorLine line = Instantiate(GameAssets.Instance.Line, Vector2.one, Quaternion.identity);
            line.transform.parent = transform;

            linePool.Enqueue(line);

            line.gameObject.SetActive(false);

        }
    }
    void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }

    
    public ConnectorLine Get(){
        if (linePool.Count > 0){
            ConnectorLine line = linePool.Dequeue();
            line.gameObject.SetActive(true);
            return line;

        }


        else{
            Debug.Log("HIUYGTFF");
            ConnectorLine line = Instantiate(GameAssets.Instance.Line, Vector2.one, Quaternion.identity);
            line.transform.parent = transform;

            linePool.Enqueue(line);

            line.gameObject.SetActive(false);
            return line;
        }
    }


    public void Return(ConnectorLine line){
        linePool.Enqueue(line);
        line.gameObject.SetActive(false);
    }

    
}
