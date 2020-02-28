using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class RecordsManager<T, D>
    {
        public Dictionary<int, RecordsHandler<T, D>> records = new Dictionary<int, RecordsHandler<T, D>>();

        public RecordsHandler<T, D> GetRecords(int id)
        {
            if (records.ContainsKey(id))
            {
                return records[id];
            }

            return null;
        }
    }
}
