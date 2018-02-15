
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
    private Dictionary<string, Action> _actions = new Dictionary<string, Action>();
    private Dictionary<string, IEnumerator> _routines = new Dictionary<string, IEnumerator>();

    public void Setup(string id, Action action)
    {
        if (_actions.ContainsKey(id))
            _actions.Remove(id);
        _actions.Add(id, action);
    }

    public void RunOnce(string id, IEnumerator coroutine)
    {
        if (_routines.ContainsKey(id))
            _routines.Remove(id);
        _routines.Add(id, coroutine);
    }

    private IEnumerator _rountine;


    public void Start()
    {
        _rountine = Run();
        StartCoroutine(_rountine);
    }

    private IEnumerator Run()
    {
        while (true)
        {
            foreach (KeyValuePair<string, Action> entry in _actions)
            {
                entry.Value();
            }

            var toRemove = new List<string>();

            foreach (KeyValuePair<string, IEnumerator> entry in _routines)
            {
                StartCoroutine(entry.Value);
                toRemove.Add(entry.Key);
            }

            foreach (var key in toRemove)
            {
                _routines.Remove(key);
            }

            yield return new WaitForSeconds(1);
        }
    }
}