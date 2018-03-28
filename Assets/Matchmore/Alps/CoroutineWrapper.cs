
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
    private List<string> _toDeleteActions = new List<string>();
    private object obj = new object();

    public void SetupContinuousRoutine(string id, Action action)
    {
        if (_actions.ContainsKey(id))
            _actions.Remove(id);
        _actions.Add(id, action);
    }

    public void StopContinuousRoutine(string id)
    {
        _toDeleteActions.Add(id);
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
            yield return ProcessActions();
            yield return ProcessOneTimeRoutines();

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator ProcessOneTimeRoutines()
    {
        var toDeleteRoutines = new List<string>();
        var keys = new List<string>(_routines.Keys);
        foreach (var key in keys)
        {
            StartCoroutine(_routines[key]);
            toDeleteRoutines.Add(key);
            yield return null;
        }

        _toDeleteActions = new List<string>();

        foreach (var key in toDeleteRoutines)
        {
            _routines.Remove(key);
        }

    }

    private IEnumerator ProcessActions()
    {
        var keys = new List<string>(_actions.Keys);

        foreach (var key in keys)
        {
            _actions[key]();
            yield return null;
        }

        foreach (var key in _toDeleteActions)
        {
            _actions.Remove(key);
        }
    }
}