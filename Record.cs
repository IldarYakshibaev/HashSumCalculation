using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashSumCalculation
{
    public class Record
    {
        public static void RecordInDB(HashSum hashSum)
        {
            // create db context 
            using (HashContext db = new HashContext())
            {
                db.HashSums.Add(hashSum);

                db.SaveChanges();
            }
        }
    }
}
