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
        public static void RecordInDB(string path, string title, string hashSum, string typeError)
        {
            // create db context 
            using (HashContext db = new HashContext())
            {
                HashSum hash = new HashSum { Path = path, Title = title, Hash = hashSum, TypeError = typeError };

                db.HashSums.Add(hash);

                db.SaveChanges();
            }
        }
    }
}
