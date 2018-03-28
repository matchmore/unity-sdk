using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MatchmoreUtils
{
    public class Utils
    {
        private class ApiKeyObject
        {
            public string Sub { get; set; }
        }

        public static string ExtractWorldId(string apiKey)
        {
            try
            {
                var subjectData = Convert.FromBase64String(apiKey.Split('.')[1]);
                var subject = Encoding.UTF8.GetString(subjectData);
                var deserializedApiKey = JsonConvert.DeserializeObject<ApiKeyObject>(subject);

                return deserializedApiKey.Sub;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Api key was invalid", e);
            }
        }
    }

    public class MTuple<T1, T2>
    {
        public T1 First { get; private set; }
        public T2 Second { get; private set; }
        internal MTuple(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }

    public static class MTuple
    {
        public static MTuple<T1, T2> New<T1, T2>(T1 first, T2 second)
        {
            var tuple = new MTuple<T1, T2>(first, second);
            return tuple;
        }
    }


    public class ThreadedJob
    {
        private bool m_IsDone = false;
        private object m_Handle = new object();
        private System.Threading.Thread m_Thread = null;
        public bool IsDone
        {
            get
            {
                bool tmp;
                lock (m_Handle)
                {
                    tmp = m_IsDone;
                }
                return tmp;
            }
            set
            {
                lock (m_Handle)
                {
                    m_IsDone = value;
                }
            }
        }

        public virtual void Start()
        {
            m_Thread = new System.Threading.Thread(Run);
            m_Thread.Start();
        }
        public virtual void Abort()
        {
            m_Thread.Abort();
        }

        protected virtual void ThreadFunction() { }

        protected virtual void OnFinished() { }

        public virtual bool Update()
        {
            if (IsDone)
            {
                OnFinished();
                return true;
            }
            return false;
        }
        public IEnumerator WaitFor()
        {
            while (!Update())
            {
                yield return null;
            }
        }
        private void Run()
        {
            ThreadFunction();
            IsDone = true;
        }
    }

}