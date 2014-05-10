//  
//  Project: SOAPI
//  http://soapics.codeplex.com
//  http://stackapps.com/questions/386
//  
//  Copyright 2010, Sky Sanders
//  Licensed under the GPL Version 2 license.
//  http://soapi.codeplex.com/license
//  

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using POEApi.Infrastructure.Events;

namespace POEApi.Transport
{
    /// <summary>
    ///   This is a fully configurable, thread-safe web request throttle that fully complies with the 
    ///   published usage guidelines. In addition to compliance with the letter of the law, testing
    ///   has exposed further limitations that are compensated. See code comments for more detail.
    /// 
    ///   Simply route all WebRequest.Create calls through RequestThrottle.Instance.Create();
    /// 
    ///   Upon completion of an issued request, regardless of status, you must call 
    ///   RequestThrottle.Instance.Complete() to decrement the outstanding request count.
    /// 
    ///   NOTE: you can use this as a global throttle using WebRequest.RegisterPrefix
    ///   http://msdn.microsoft.com/en-us/library/system.net.webrequest.registerprefix.aspx
    ///   but this is not a viable option for silverlight so in Soapi, where requests
    ///   are created in one place, we just call it explicitly.
    /// </summary>
    /// <remarks>
    /// Throttling conversation here: http://stackapps.com/questions/1143/request-throttling-limits
    /// </remarks>
    public sealed class RequestThrottle
    {
        private int _outstandingRequests;

        private readonly Queue<DateTime> _requestTimes = new Queue<DateTime>();

        public event ThottledEventHandler Throttled;

        private RequestThrottle()
        {
            ThrottleWindowTime = new TimeSpan(0, 0, 1, 0);
            ThrottleWindowCount = 42;
            MaxPendingRequests = 42;
        }

        public static RequestThrottle Instance
        {
            get { return Nested.instance; }
        }

        /// <summary>
        ///   The maximum number of allowed pending request.
        /// 
        ///   The throttle window will keep us in compliance with the 
        ///   letter of the law, but testing has shown that a large 
        ///   number of outstanding requests result in a cascade of 
        ///   (500) errors that does not stop. 
        /// 
        ///   So we will block while there are > MaxPendingRequests 
        ///   regardless of throttle window.
        /// 
        ///   Defaults to 15 which has proven to be reliable.
        /// </summary>
        public int MaxPendingRequests { get; set; }

        /// <summary>
        ///   If you are interested in monitoring
        /// </summary>
        public int OutstandingRequests
        {
            get { return _outstandingRequests; }
        }

        /// <summary>
        ///   The quantitive portion (xxx) of the of 30 requests per 5 seconds
        ///   Defaults to published guidelines of 5 seconds
        /// </summary>
        public int ThrottleWindowCount { get; set; }

        /// <summary>
        ///   The temporal portion (yyy) of the of 30 requests per 5 seconds
        ///   Defaults to the published guidelines of 30
        /// </summary>
        public TimeSpan ThrottleWindowTime { get; set; }


        /// <summary>
        ///   This decrements the outstanding request count.
        /// 
        ///   This MUST MUST MUST be called when a request has 
        ///   completed regardless of status.
        /// 
        ///   If a request fails, it may be wise to delay calling 
        ///   this, e.g. cool down, for a few seconds, before 
        ///   reissuing the request.
        /// </summary>
        public void Complete()
        {
            _outstandingRequests--;
        }

        /// <summary>
        ///   Create a WebRequest. This method will block if too many
        ///   outstanding requests are pending or the throttle window
        ///   threshold has been reached.
        /// </summary>
        /// <param name = "uri"></param>
        /// <returns></returns>
        public WebRequest Create(Uri uri)
        {
            lock (typeof(ThrottleLock))
            {
                // note: we could use a list of WeakReferences and 
                // may do so at a later date, but for now, this
                // works just fine as long as you call .Complete
                _outstandingRequests++;

                while (_outstandingRequests > MaxPendingRequests)
                {
                    using (var throttleGate = new AutoResetEvent(false))
                    {
                        Debug.WriteLine("Max number requests reached, waiting");
                        throttleGate.WaitOne(100);
                    }
                }

                if (_requestTimes.Count == ThrottleWindowCount)
                {
                    // pull the earliest request of the bottom
                    DateTime tail = _requestTimes.Dequeue();
                    // calculate the interval between now (head) and tail
                    // to determine if we need to chill out for a few millisecons

                    TimeSpan waitTime = (ThrottleWindowTime - (DateTime.Now - tail));

                    if (waitTime.TotalMilliseconds > 0)
                    {
                        Trace.WriteLine("waiting:\t" + waitTime + "\t" + uri.AbsoluteUri);
                        using (var throttleGate = new AutoResetEvent(false))
                        {
                            if (Throttled != null)
                                Throttled(this, new ThottledEventArgs(waitTime));

                            Debug.WriteLine("Approaching Threshold, Just Chillin for a few seconds " + waitTime.TotalSeconds);
                            throttleGate.WaitOne(waitTime);
                        }
                    }
                }

                // good to go. 
                _requestTimes.Enqueue(DateTime.Now);
                return WebRequest.Create(uri);
            }
        }


        public WebRequest Create(string url)
        {
            return Create(new Uri(url));
        }

        private class ThrottleLock
        {
        }

        class Nested
        {
            static Nested()
            {
            }

            internal static readonly RequestThrottle instance = new RequestThrottle();
        }

    }
}