using Doctor.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Doctor.DAL
{
    public class TelDAL
    {
        public static void Insert(TelModel tel)
        {
            SqlHelper.ExecuteNonQuery(@"insert into TelModel(@hospital_id, TelNo)
			values(hospital_id, TelNo)",
                new SqlParameter("@hospital_id", tel.Hospital_id),
                new SqlParameter("@TelNo", tel.TelNo)
            );
        }

        public static void DeleteById(long id)
        {
            SqlHelper.ExecuteNonQuery(@"delete from Tel where id = @id",
                new SqlParameter("@id", id));
        }

        public static void Update(TelModel tel)
        {
            SqlHelper.ExecuteNonQuery(@"update Tel set
			hospital_id = @hospital_id,
			TelNo = @TelNo
			where id = @id",
                new SqlParameter("@hospital_id", tel.Hospital_id),
                new SqlParameter("@TelNo", tel.TelNo)
            );
        }

        public static TelModel GetById(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Tel where id = @id",
                new SqlParameter("@id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("Fatal Error: Duplicated Id in Table Tel");
            }
            else
            {
                DataRow row = table.Rows[0];
                return ToModel(row);
            }
        }

        public static TelModel[] GetAll()
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from Tel");
            TelModel[] tel = new TelModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                tel[i] = ToModel(table.Rows[i]);
            }
            return tel;
        }

        private static TelModel ToModel(DataRow row)
        {
            TelModel tel = new TelModel();
            tel.Hospital_id = (System.Int64)row["hospital_id"];
            tel.TelNo = (System.String)row["telNo"];
            return tel;
        }
    }
}
