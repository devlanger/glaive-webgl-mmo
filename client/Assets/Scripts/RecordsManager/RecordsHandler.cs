using System;
using System.Collections.Generic;
using System.Text;

public class RecordsHandler<T, D>
{
    public Dictionary<T, D> records = new Dictionary<T, D>();

    public event Action<T, D> OnRecordChanged = delegate { };

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

    public void SetRecord(T id, D val)
    {
        if (records.ContainsKey(id))
        {
            records[id] = val;
        }
        else
        {
            records.Add(id, val);
        }

        OnRecordChanged(id, val);
    }

    public void ReplaceRecord(T id, T id2)
    {
        D val1;
        D val2;

        GetRecord(id, out val1);
        GetRecord(id2, out val2);

        SetRecord(id, val2);
        SetRecord(id2, val1);

        OnRecordChanged(id, val2);
        OnRecordChanged(id2, val1);

    }
}
