using System.Collections;
using System.Collections.Generic;
using Core.Manager;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instance.Init();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.Update(Time.deltaTime);
    }
}
