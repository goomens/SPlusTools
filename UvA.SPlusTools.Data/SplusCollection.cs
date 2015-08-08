using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.SPlusTools.Data.Entities;

namespace UvA.SPlusTools.Data
{
    public class SPlusCollection<T> : ICollection<T> where T : SPlusObject
    {
        public dynamic Source { get; private set; }
        College College;

        public SPlusCollection(College college, dynamic source)
        {
            College = college;
            Source = source;
        }

        public T FindByName(string name)
        {
            var col = Source.Find(Name: name);
            if (col.Count == 0)
                return null;
            return College.GetObject<T>(col[1]);
        }

        public T FindByHostKey(string hostkey)
        {
            var col = Source.Find(HostKey: hostkey);
            if (col.Count == 0)
                return null;
            return College.GetObject<T>(col[1]);
        }

        public T FindByObjectId(string objectId)
        {
            var col = Source.Find(ObjectId: objectId);
            if (col.Count == 0)
                return null;
            return College.GetObject<T>(col[1]);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SPlusEnumerator<T>(College, Source);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(T item)
        {
            Source.Add(item.Object);
        }

        public void Clear()
        {
            int c = Count;
            for (int z = 0; z < c; z++)
                Source.Remove(this[0].Object);
        }

        public bool Contains(T item)
        {
            return ((IEnumerable<T>)this).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return Source.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            Source.Remove(item.Object);
            return true;
        }

        public T this[int index]
        {
            get { return College.GetObject<T>(Source[index + 1]); }
        }
        
    }

    public class SPlusEnumerator<T> : IEnumerator<T> where T : SPlusObject
    {
        dynamic Source;
        College College;
        int Index, Count;

        public SPlusEnumerator(College college, dynamic source)
        {
            College = college;
            Source = source;
            Count = source.Count;
        }

        public T Current
        {
            get { return College.GetObject<T>(Source[Index]); }
        }

        public void Dispose()
        {
            
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            Index++;
            return Index <= Count;
        }

        public void Reset()
        {
            Index = 0;
        }
    }
}
