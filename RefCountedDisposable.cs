using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading;

namespace RefCounted
{

    // -----------------------------------------------------------------
    public class Reference<T> : IDisposable where T : IDisposable
    {
        private RefCounter<T> _rc;
        public class RefCounter<T> : IDisposable where T : IDisposable
        {
            bool isObjDisposed = false;
            int cnt = 1;
            T obj;
            string name = null;
            public T Instance { get => obj; }
            public string Name => name;
            public int RefCount => cnt;
            public bool IsDisposed => isObjDisposed;

            public RefCounter(T o, string name, string who)
            {
                obj = o;
                cnt = 0;
                this.name = name;
                IncRef(who);
            }

            ~RefCounter()
            {
                //if (cnt > 0)
                //    ReleaseRef();
            }

            public void Dispose()
            {
                if (!isObjDisposed)
                {
                    obj?.Dispose();
                    isObjDisposed = true;
                }
            }

            public void IncRef(string who)
            {
                lock (this)
                {
                    Interlocked.Increment(ref cnt);
                    if (isObjDisposed)
                        throw new Exception($"Something wrong, trying to increment reference counter for disposed object!");
                }
            }

            public void ReleaseRef(string who)
            {
                lock (this)
                {
                    Interlocked.Decrement(ref cnt);
                    if (cnt == 0)
                    {
                        obj?.Dispose();
                        isObjDisposed = true;
                        name += "(disposed)";
                        this.Dispose();
                    }
                }
            }
        }
        public T Instance { get { lock (this) { return _rc.Instance; } } }
        public string Name { get { lock (this) { return _rc.Name; } } }
        public bool IsInstance => _rc.Instance != null;



        // -----------------------------------------------------------------
        public Reference(T o, string name, string who)
        {
            _rc = new RefCounter<T>(o, name, who);
        }


        // -----------------------------------------------------------------
        public Reference(Reference<T> refer, string who)
        {
            lock (refer._rc)
            {
                this._rc = refer._rc;
                this._rc.IncRef(who);
            }
        }


        // -----------------------------------------------------------------
        ~Reference() 
        {
            //this._rc?.Dispose();
        }



        // -----------------------------------------------------------------
        public void Dispose()
        {
            lock (this._rc)
            {
                _rc.ReleaseRef("Reference.Dispose");
            }
        }



        // -----------------------------------------------------------------
        public virtual Reference<T> GetReference4Using()
        {
            lock (this)
                lock (this._rc)
                {
                    string s = _rc.Name;
                    return new Reference<T>(this, "GetReference4Using");
                }
        }



        // -----------------------------------------------------------------
        public void Change(T newItem, string name)
        {
            lock (this)
                lock (this._rc)
                {
                    string oldName = Name;
                    _rc.ReleaseRef("Change");
                    _rc = new RefCounter<T>(newItem, name, "Change");
                }
        }



        // -----------------------------------------------------------------
        public void Change(Reference<T> reference)
        {
            lock (this._rc)
                lock (reference)
                {
                    reference._rc.IncRef("Change");
                    _rc.ReleaseRef("Change");
                    _rc = reference._rc;
                }
        }
    }





    // -----------------------------------------------------------------
    public class RefCountedObjectList<T> : List<Reference<T>> where T : IDisposable
    {
        public T Add(T item, string name)
        {
            this.Add(new Reference<T>(item, name, ""));
            return item;
        }

    }



}
