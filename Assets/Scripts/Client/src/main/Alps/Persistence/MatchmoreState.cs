using UnityEngine;
using System.Collections;
using System;
using Alps.Model;
using System.Collections.Generic;

[Serializable]
public class MatchmoreState
{
    public Device Device { get; set; }

    public List<Subscription> Subscription { get; set; }
    public List<Publication> Publications { get; set; }
}
