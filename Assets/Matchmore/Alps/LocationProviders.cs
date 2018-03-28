using UnityEngine;
using System.Collections;
using Alps.Model;
using System;
using MatchmoreUtils;
using System.Threading;

namespace MatchmoreLocation
{
    public enum LocationServiceType
    {
        threaded, coroutine
    }

    public static class LocationServiceStarter
    {
        public static IEnumerator Start(Action callback)
        {
            // First, check if user has location service enabled
            if (!Input.location.isEnabledByUser)
            {
                MatchmoreLogger.Debug("Location service disabled by user");
#if !UNITY_IOS
                //https://docs.unity3d.com/ScriptReference/LocationService-isEnabledByUser.html
                //if it is IOS we do not break here
                yield break;
#endif
            }


            // Start service before querying location
            Input.location.Start();

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                yield break;
            }

            callback();
        }
    }

    public interface ILocationService
    {
        Location MockLocation { get; set; }

        Action<Location> OnUpdateLocation { get; }

        void Start();
        void Stop();
    }

    public class ThreadedLocationSercixe : ThreadedJob, ILocationService
    {
        private Action<Location> _onUpdate;
        private CoroutineWrapper _coroutine;
        private LocationInfo _locationInfo;
        private LocationServiceStatus _status;
        private bool _running;

        public Location MockLocation { get; set; }
        public Action<Location> OnUpdateLocation { get { return _onUpdate; } }

        public ThreadedLocationSercixe(CoroutineWrapper coroutine, Action<Location> onUpdate)
        {
            this._coroutine = coroutine;
            this._onUpdate = onUpdate;
        }

        public override void Start()
        {
            if (MockLocation != null)
            {
                _running = true;
                base.Start();
            }
            else
                _coroutine.RunOnce("location_service_start", LocationServiceStarter.Start(() =>
                {
                //setup a coroutine which will just copy data to this object so the thread actually can take it and send to matchmore
                _coroutine.SetupContinuousRoutine("data_update", () =>
                    {
                        _status = Input.location.status;
                        _locationInfo = Input.location.lastData;
                    });
                    _running = true;
                    base.Start();
                }));
        }

        protected override void ThreadFunction()
        {
            while (_running)
            {
                try
                {
                    if (MockLocation != null)
                        OnUpdateLocation(MockLocation);
                    else
                    if (_status == LocationServiceStatus.Running)
                    {
                        var location = _locationInfo;

                        OnUpdateLocation(new Location
                        {
                            Latitude = location.latitude,
                            Longitude = location.longitude,
                            Altitude = location.altitude
                        });
                    }

                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
                Thread.Sleep(6000);
            }
        }

        public void Stop()
        {
            _running = false;
        }
    }

    public class CoroutinedLocationService : ILocationService
    {
        private CoroutineWrapper _coroutine;
        private Action<Location> _onUpdate;

        public Location MockLocation { get; set; }

        public Action<Location> OnUpdateLocation { get { return _onUpdate; } }

        public CoroutinedLocationService(CoroutineWrapper coroutine, Action<Location> onUpdate)
        {
            this._coroutine = coroutine;
            this._onUpdate = onUpdate;
        }

        public void Start()
        {
            _coroutine.RunOnce("location_service_start", StartLocationServiceCoroutine());
        }

        public void Stop()
        {
            _coroutine.StopContinuousRoutine("location_service");
        }

        IEnumerator StartLocationServiceCoroutine()
        {
            if (MockLocation != null)
            {
                _coroutine.SetupContinuousRoutine("location_service", MockLocationUpdate);
                yield break;
            }
            _coroutine.RunOnce("location_service_start",
                               LocationServiceStarter.Start(
                                   () => _coroutine.SetupContinuousRoutine("location_service", UpdateLocation)));

        }

        private void MockLocationUpdate()
        {
            OnUpdateLocation(MockLocation);
        }

        private void UpdateLocation()
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                var location = Input.location.lastData;

                OnUpdateLocation(new Location
                {
                    Latitude = location.latitude,
                    Longitude = location.longitude,
                    Altitude = location.altitude
                });
            }
        }
    }
}