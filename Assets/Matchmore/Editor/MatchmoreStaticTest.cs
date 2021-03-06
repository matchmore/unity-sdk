﻿using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using MatchmorePersistence;
using Alps.Model;
using System.Collections.Generic;
using System;

public class MatchmoreStaticTest
{

    [TearDown]
    public void Reset()
    {
        Matchmore.Reset();
    }

    [Test]
    public void Configure_static_instance()
    {
        var apiKey = "eyJ0eXAiOiJKV1QiLCJhbGciOiJFUzI1NiJ9.eyJpc3MiOiJhbHBzIiwic3ViIjoiMzU2OGRhMWMtM2YxYS00MzdiLWFiNjYtN2JlNmU4Y2IzODg2IiwiYXVkIjpbIlB1YmxpYyJdLCJuYmYiOjE1MTg1MjEwNzMsImlhdCI6MTUxODUyMTA3MywianRpIjoiMSJ9.Jt4FtCApf5xHxwgmsT1xrZuRK53krIP886TptVn-7QRqZYpwX1RE5svrfUmn1XUcuVxWum-qwDIi_BvoVmykyg";
        Matchmore.Configure(Matchmore.Config.WithApiKey(apiKey));
        Assert.NotNull(Matchmore.Instance);
        Assert.AreEqual("https://" + Matchmore.PRODUCTION + "/v5", Matchmore.Instance.ApiUrl);
    }

    [Test]
    public void Throws_on_invalid_key()
    {
        var apiKey = "invalid key";
        Assert.Throws<ArgumentException>(() => Matchmore.Configure(apiKey));

        Assert.Null(Matchmore.Instance);
    }
}
