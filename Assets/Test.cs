using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alps.Client;
using Alps.Api;
using Alps.Model;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var client = new ApiClient ("http://35.201.116.232/v5");
		client.AddDefaultHeader ("api-key", "eyJ0eXAiOiJKV1QiLCJhbGciOiJFUzI1NiJ9.eyJpc3MiOiJhbHBzIiwic3ViIjoiZTcxMmE1YjEtMDNkMi00NmFlLWE1NDEtOGFjZmFiMGJjM2M0IiwiYXVkIjpbIlB1YmxpYyJdLCJuYmYiOjE1MTY4MjA4MzIsImlhdCI6MTUxNjgyMDgzMiwianRpIjoiMSJ9.SyRjVl-4yss3oUUiZ1GPwl9uEt76H3npwDiuISSCmbcu-qCDUmnzfpMOXG7I7hqJUcCZoFxRINDMDFUdTACKQw");
		var deviceApi = new DeviceApi (client); 
		var result = deviceApi.CreateDevice (new PinDevice{
			Name = "Test Unity Device",
            DeviceType = Alps.Model.DeviceType.Pin,
            Location = new Location{
                Latitude = 0,
                Longitude = 0,
                Altitude = 0
            }
		});
		Debug.Log (result);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
