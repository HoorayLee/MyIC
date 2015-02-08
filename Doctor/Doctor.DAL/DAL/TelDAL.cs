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
        public static bool Insert(TelModel tel)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"insert into Tel(TelNo, hospital_id)
				values(@TelNo, @hospital_id)",
                    new SqlParameter("@TelNo", tel.TelNo),
                    new SqlParameter("@hospital_id", tel.Hospital_id)
                );
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool DeleteById(System.Int64 id)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"delete from Tel where tel_id = @id",
                    new SqlParameter("@id", id));
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool Update(TelModel tel)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"update Tel set
				TelNo = @TelNo,
				hospital_id = @hospital_id
				where tel_id = @tel_id",
                    new SqlParameter("@TelNo", tel.TelNo),
                    new SqlParameter("@hospital_id", tel.Hospital_id),
                    new SqlParameter("@tel_id", tel.Tel_id)
                );
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static TelModel GetById(System.Int64 id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Tel where tel_id = @id",
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
            tel.Tel_id = (System.Int64)row["tel_id"];
            tel.TelNo = (System.String)row["telNo"];
            tel.Hospital_id = (System.Int64)row["hospital_id"];
            return tel;
        }
    }
}
