using System;
using System.Collections.Generic;
using System.Text;

public class RecordsManager<T, D>
{
    public Dictionary<int, RecordsHandler<int, D>> records = new Dictionary<int, RecordsHandler<int, D>>();

    public RecordsHandler<int, D> GetRecords(int id)
    {
        if (records.ContainsKey(id))
        {
            return records[id];
        }

        return null;
    }
}
