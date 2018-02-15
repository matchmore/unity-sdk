using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Alps.Model;
using System.Collections.Generic;
using System;

public class CoroutineWrapperTest
{

    IEnumerator TestRoutine(string value, Action<string> callback)
    {
        callback(value);
        yield break;
    }

    [UnityTest]
    public IEnumerator Fires_once_a_coroutine()
    {
        var obj = new GameObject("coroutine_test");
        var coroutine = obj.AddComponent<CoroutineWrapper>();
            
        string expectedValue = "some random value";
        string value = null;

        coroutine.RunOnce("test_once", TestRoutine(expectedValue, returnedValue => value = returnedValue));

        int maxWait = 20;
        while (value != expectedValue && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            //print("Timed out");
            yield break;
        }

        Assert.AreEqual(expectedValue, value);
    }

}
