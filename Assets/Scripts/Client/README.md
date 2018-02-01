# Alps - the C# library for the MATCHMORE ALPS Core REST API

## ALPS by [MATCHMORE](https://dev.matchmore.io)  The first version of the MATCHMORE API is an exciting step to allow developers use a context-aware pub/sub cloud service.  A lot of mobile applications and their use cases may be modeled using this approach and can therefore profit from using MATCHMORE as their backend service.  **Build something great with [ALPS by MATCHMORE](https://dev.matchmore.io)!**   Once you've [registered your client](https://dev.matchmore.io/account/register/) it's easy start using our awesome cloud based context-aware pub/sub (admitted, a lot of buzzwords).  ## RESTful API We do our best to have all our URLs be [RESTful](https://en.wikipedia.org/wiki/Representational_state_transfer). Every endpoint (URL) may support one of four different http verbs. GET requests fetch information about an object, POST requests create objects, PUT requests update objects, and finally DELETE requests will delete objects.  ## Domain Model  This is the current domain model extended by an ontology of devices and separation between the developer portal and the ALPS Core.      +- -- -- -- -- --+    +- -- -- -- -- -- --+     | Developer +- -- -+ Application |     +- -- -- -- -- --+    +- -- -- -+- -- -- -+                             |                        \"Developer Portal\"     ........................+..........................................                             |                        \"ALPS Core\"                         +- --+- --+                         | World |                         +- --+- --+                             |                           +- -- -- -- -- -- --+                             |                     +- -- --+ Publication |                             |                     |     +- -- -- -+- -- -- -+                             |                     |            |                             |                     |            |                             |                     |            |                             |                     |        +- --+- --+                        +- -- -+- --+- -- -- -- -- -- -- -- --+        | Match |                        | Device |                          +- --+- --+                        +- -- -+- --+- -- -- -- -- -- -- -- --+            |                             |                     |            |                             |                     |            |                             |                     |     +- -- -- -+- -- -- --+             +- -- -- -- -- -- -- --+- -- -- -- -- -- -- -+      +- -- --+ Subscription |             |               |              |            +- -- -- -- -- -- -- -+        +- -- -+- --+      +- -- -+- -- -+    +- -- -+- --+        |   Pin  |      | iBeacon |    | Mobile |        +- -- -+- --+      +- -- -- -- --+    +- -- -+- --+             |                              |             |         +- -- -- -- -- -+         |             +- -- -- -- --+ Location +- -- -- -- --+                       +- -- -- -- -- -+  1.  A **developer** is a mobile application developer registered in the     developer portal and allowed to use the **ALPS Developer Portal**.  A     developer might register one or more **applications** to use the     **ALPS Core cloud service**.  For developer/application pair a new     **world** is created in the **ALPS Core** and assigned an **API key** to     enable access to the ALPS Core cloud service **RESTful API**.  During     the registration, the developer needs to provide additional     configuration information for each application, e.g. its default     **push endpoint** URI for match delivery, etc. 2.  A [**device**](#tag/device) might be either *virtual* like a **pin device** or     *physical* like a **mobile device** or **iBeacon device**.  A [**pin     device**](#tag/device) is one that has geographical [**location**](#tag/location) associated with it     but is not represented by any object in the physical world; usually     it's location doesn't change frequently if at all.  A [**mobile     device**](#tag/device) is one that potentially moves together with its user and     therefore has a geographical location associated with it.  A mobile     device is typically a location-aware smartphone, which knows its     location thanks to GPS or to some other means like cell tower     triangulation, etc.  An [**iBeacon device**](#tag/device) represents an Apple     conform [iBeacon](https://developer.apple.com/ibeacon/) announcing its presence via Bluetooth LE     advertising packets which can be detected by a other mobile device.     It doesn't necessary has any location associated with it but it     serves to detect and announce its proximity to other **mobile     devices**. 3.  The hardware and software stack running on a given device is known     as its **platform**.  This include its hardware-related capabilities,     its operating systems, as well as the set of libraries (APIs)     offered to developers in order to program it. 4.  A devices may issue publications and subscriptions     at **any time**; it may also cancel publications and subscriptions     issued previously.  **Publications** and **subscriptions** do have a     definable, finite duration, after which they are deleted from the     ALPS Core cloud service and don't participate anymore in the     matching process. 5.  A [**publication**](#tag/publication) is similar to a Java Messaging Service (JMS)     publication extended with the notion of a **geographical zone**.  The     zone is defined as **circle** with a center at the given location and     a range around that location. 6.  A [**subscription**](#tag/subscription) is similar to a JMS subscription extended with the     notion of **geographical zone**. Again, the zone being defined as     **circle** with a center at the given location and a range around     that location. 7.  **Publications** and **subscriptions** which are associated with a     **mobile device**, e.g. user's mobile phone, potentially **follow the     movements** of the user carrying the device and therefore change     their associated location. 8.  A [**match**](#tag/match) between a publication and a subscription occurs when both     of the following two conditions hold:     1.  There is a **context match** occurs when for instance the         subscription zone overlaps with the publication zone or a         **proximity event** with an iBeacon device within the defined         range occurred.     2.  There is a **content match**: the publication and the subscription         match with respect to their JMS counterparts, i.e., they were         issued on the same topic and have compatible properties and the         evaluation of the selector against those properties returns true         value. 9.  A **push notification** is an asynchronous mechanism that allows an     application to receive matches for a subscription on his/her device.     Such a mechanism is clearly dependent on the device’s platform and     capabilities.  In order to use push notifications, an application must     first register a device (and possibly an application on that     device) with the ALPS core cloud service. 10. Whenever a **match** between a publication and a subscription     occurs, the device which owns the subscription receives that match     *asynchronously* via a push notification if there exists a     registered **push endpoint**.  A **push endpoint** is an URI which is     able to consume the matches for a particular device and     subscription.  The **push endpoint** doesn't necessary point to a     **mobile device** but is rather a very flexible mechanism to define     where the matches should be delivered. 11. Matches can also be retrieved by issuing a API call for a     particular device.   <a id=\"orgae4fb18\"></a>  ## Device Types                     +- -- -+- --+                    | Device |                    +- -- -- -- -+                    | id     |                    | name   |                    | group  |                    +- -- -+- --+                         |         +- -- -- -- -- -- -- --+- -- -- -- -- -- -- -- -+         |               |                |     +- --+- --+   +- -- -- --+- -- -- -+    +- -- -+- -- --+     |  Pin  |   | iBeacon      |    | Mobile   |     +- --+- --+   +- -- -- -- -- -- -- -+    +- -- -- -- -- -+         |       | proximityUUID|    | platform |         |       | major        |    | token    |         |       | minor        |    +- -- -+- -- --+         |       +- -- -- --+- -- -- -+         |         |               |                |         |               | <- -???         |         |          +- -- -+- -- --+          |         +- -- -- -- -- -+ Location +- -- -- -- -- -+                    +- -- -- -- -- -+   <a id=\"org68cc0d8\"></a>  ### Generic `Device`  -   id -   name -   group  <a id=\"orgc430925\"></a>  ### `PinDevice`  -   location   <a id=\"orgecaed9f\"></a>  ### `iBeaconDevice`  -   proximityUUID -   major -   minor   <a id=\"org7b09b62\"></a>  ### `MobileDevice`  -   platform -   deviceToken -   location 

This C# SDK is automatically generated by the [Swagger Codegen](https://github.com/swagger-api/swagger-codegen) project:

- API version: 0.5.0
- SDK version: 1.0.0
- Build date: 2018-02-01T14:07:41.931+01:00
- Build package: io.swagger.codegen.languages.CsharpDotNet2ClientCodegen
    For more information, please visit [https://matchmore.com](https://matchmore.com)

<a name="frameworks-supported"></a>
## Frameworks supported
- .NET 2.0

<a name="dependencies"></a>
## Dependencies
- Mono compiler
- Newtonsoft.Json.7.0.1
- RestSharp.Net2.1.1.11

Note: NuGet is downloaded by the mono compilation script and packages are installed with it. No dependency DLLs are bundled with this generator

<a name="installation"></a>
## Installation
Run the following command to generate the DLL
- [Mac/Linux] `/bin/sh compile-mono.sh`
- [Windows] TODO

Then include the DLL (under the `bin` folder) in the C# project, and use the namespaces:
```csharp
using Alps.Api;
using Alps.Client;
using Alps.Model;
```
<a name="getting-started"></a>
## Getting Started

```csharp
using System;
using System.Diagnostics;
using Alps.Api;
using Alps.Client;
using Alps.Model;

namespace Example
{
    public class Example
    {
        public void main()
        {
            
            var apiInstance = new DeviceApi();
            var device = new Device(); // Device | The device to be created.

            try
            {
                // Create a device
                Device result = apiInstance.CreateDevice(device);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DeviceApi.CreateDevice: " + e.Message );
            }
        }
    }
}
```

<a name="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://api.matchmore.io/v5*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*DeviceApi* | [**CreateDevice**](docs/DeviceApi.md#createdevice) | **POST** /devices | Create a device
*DeviceApi* | [**CreateLocation**](docs/DeviceApi.md#createlocation) | **POST** /devices/{deviceId}/locations | Create a new location for a device
*DeviceApi* | [**CreatePublication**](docs/DeviceApi.md#createpublication) | **POST** /devices/{deviceId}/publications | Create a publication for a device
*DeviceApi* | [**CreateSubscription**](docs/DeviceApi.md#createsubscription) | **POST** /devices/{deviceId}/subscriptions | Create a subscription for a device
*DeviceApi* | [**DeleteDevice**](docs/DeviceApi.md#deletedevice) | **DELETE** /devices/{deviceId} | Delete an existing device
*DeviceApi* | [**DeletePublication**](docs/DeviceApi.md#deletepublication) | **DELETE** /devices/{deviceId}/publications/{publicationId} | Delete a Publication
*DeviceApi* | [**DeleteSubscription**](docs/DeviceApi.md#deletesubscription) | **DELETE** /devices/{deviceId}/subscriptions/{subscriptionId} | Delete a Subscription
*DeviceApi* | [**GetDevice**](docs/DeviceApi.md#getdevice) | **GET** /devices/{deviceId} | Info about a device
*DeviceApi* | [**GetIBeaconTriples**](docs/DeviceApi.md#getibeacontriples) | **GET** /devices/IBeaconTriples | Get IBeacons triples for all registered devices
*DeviceApi* | [**GetMatch**](docs/DeviceApi.md#getmatch) | **GET** /devices/{deviceId}/matches/{matchId} | Get match for the device by its id
*DeviceApi* | [**GetMatches**](docs/DeviceApi.md#getmatches) | **GET** /devices/{deviceId}/matches | Get matches for the device
*DeviceApi* | [**GetPublication**](docs/DeviceApi.md#getpublication) | **GET** /devices/{deviceId}/publications/{publicationId} | Info about a publication on a device
*DeviceApi* | [**GetPublications**](docs/DeviceApi.md#getpublications) | **GET** /devices/{deviceId}/publications | Get all publications for a device
*DeviceApi* | [**GetSubscription**](docs/DeviceApi.md#getsubscription) | **GET** /devices/{deviceId}/subscriptions/{subscriptionId} | Info about a subscription on a device
*DeviceApi* | [**GetSubscriptions**](docs/DeviceApi.md#getsubscriptions) | **GET** /devices/{deviceId}/subscriptions | Get all subscriptions for a device
*DeviceApi* | [**TriggerProximityEvents**](docs/DeviceApi.md#triggerproximityevents) | **POST** /devices/{deviceId}/proximityEvents | Trigger the proximity event between a device and a ranged BLE iBeacon
*DeviceApi* | [**UpdateDevice**](docs/DeviceApi.md#updatedevice) | **PATCH** /devices/{deviceId} | Updates name or/and device token for existing device
*LocationApi* | [**CreateLocation**](docs/LocationApi.md#createlocation) | **POST** /devices/{deviceId}/locations | Create a new location for a device
*MatchesApi* | [**GetMatch**](docs/MatchesApi.md#getmatch) | **GET** /devices/{deviceId}/matches/{matchId} | Get match for the device by its id
*MatchesApi* | [**GetMatches**](docs/MatchesApi.md#getmatches) | **GET** /devices/{deviceId}/matches | Get matches for the device
*PublicationApi* | [**CreatePublication**](docs/PublicationApi.md#createpublication) | **POST** /devices/{deviceId}/publications | Create a publication for a device
*PublicationApi* | [**DeletePublication**](docs/PublicationApi.md#deletepublication) | **DELETE** /devices/{deviceId}/publications/{publicationId} | Delete a Publication
*PublicationApi* | [**GetPublication**](docs/PublicationApi.md#getpublication) | **GET** /devices/{deviceId}/publications/{publicationId} | Info about a publication on a device
*PublicationApi* | [**GetPublications**](docs/PublicationApi.md#getpublications) | **GET** /devices/{deviceId}/publications | Get all publications for a device
*SubscriptionApi* | [**CreateSubscription**](docs/SubscriptionApi.md#createsubscription) | **POST** /devices/{deviceId}/subscriptions | Create a subscription for a device
*SubscriptionApi* | [**DeleteSubscription**](docs/SubscriptionApi.md#deletesubscription) | **DELETE** /devices/{deviceId}/subscriptions/{subscriptionId} | Delete a Subscription
*SubscriptionApi* | [**GetSubscription**](docs/SubscriptionApi.md#getsubscription) | **GET** /devices/{deviceId}/subscriptions/{subscriptionId} | Info about a subscription on a device
*SubscriptionApi* | [**GetSubscriptions**](docs/SubscriptionApi.md#getsubscriptions) | **GET** /devices/{deviceId}/subscriptions | Get all subscriptions for a device


<a name="documentation-for-models"></a>
## Documentation for Models

 - [Alps.Model.APIError](docs/APIError.md)
 - [Alps.Model.Device](docs/Device.md)
 - [Alps.Model.DeviceType](docs/DeviceType.md)
 - [Alps.Model.DeviceUpdate](docs/DeviceUpdate.md)
 - [Alps.Model.Devices](docs/Devices.md)
 - [Alps.Model.IBeaconTriple](docs/IBeaconTriple.md)
 - [Alps.Model.IBeaconTriples](docs/IBeaconTriples.md)
 - [Alps.Model.Location](docs/Location.md)
 - [Alps.Model.Match](docs/Match.md)
 - [Alps.Model.Matches](docs/Matches.md)
 - [Alps.Model.ProximityEvent](docs/ProximityEvent.md)
 - [Alps.Model.Publication](docs/Publication.md)
 - [Alps.Model.Publications](docs/Publications.md)
 - [Alps.Model.Subscription](docs/Subscription.md)
 - [Alps.Model.Subscriptions](docs/Subscriptions.md)
 - [Alps.Model.IBeaconDevice](docs/IBeaconDevice.md)
 - [Alps.Model.MobileDevice](docs/MobileDevice.md)
 - [Alps.Model.PinDevice](docs/PinDevice.md)


<a name="documentation-for-authorization"></a>
## Documentation for Authorization

<a name="api-key"></a>
### api-key

- **Type**: API key
- **API key parameter name**: api-key
- **Location**: HTTP header

