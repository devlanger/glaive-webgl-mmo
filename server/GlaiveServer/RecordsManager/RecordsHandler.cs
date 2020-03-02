using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class RecordsHandler<T, D>
    {
        public Dictionary<T, D> records = new Dictionary<T, D>();
        public ushort maxSize = 64;

        public event Action<T, D> OnRecordAdded = delegate { };
        public event Action<T> OnRecordRemoved = delegate { };

        public bool GetFreeSlot(out dynamic id)
        {
            for (ushort i = 0; i < maxSize; i++)
            {
                dynamic x = i;
                if (!records.ContainsKey(x))
                {
                    id = x;
                    return true;
                }
            }

            id = 0;
            return false;
        }

        public bool GetRecord(T id, out D val)
        {
            if (!records.TryGetValue(id, out val))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ClearRecord(T id)
        {
            if (records.ContainsKey(id))
            {
                records.Remove(id);
            }

            OnRecordRemoved(id);
        }

        public void SetRecord(T id, D val)
        {
            if (val != null)
            {
                if (records.ContainsKey(id))
                {
                    records[id] = val;
                }
                else
                {
                    records.Add(id, val);
                }

                OnRecordAdded(id, val);
            }
        }

        public void ReplaceRecord(T id, T id2)
        {
            D val1;
            D val2;

            GetRecord(id, out val1);
            GetRecord(id2, out val2);

            ClearRecord(id);
            ClearRecord(id2);

            SetRecord(id, val2);
            SetRecord(id2, val1);
        }
    }
}
