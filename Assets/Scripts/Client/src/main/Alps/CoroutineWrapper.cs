
using UnityEngine;
using Alps.Client;
using Alps.Model;
using Alps.Api;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class CoroutineWrapper : MonoBehaviour
{
    Dictionary<string, Action> Actions = new Dictionary<string, Action>();

    public void Setup(string id, Action action)
    {
        Actions.Add(id, action);
    }

    private IEnumerator _rountine;


    public void Start()
    {
        _rountine = Run();
        StartCoroutine(_rountine);
    }

    private IEnumerator Run()
    {

        foreach (KeyValuePair<string, Action> entry in Actions)
        {
            entry.Value();
        }
        yield return new WaitForSeconds(3);
        yield return Run();
    }
}