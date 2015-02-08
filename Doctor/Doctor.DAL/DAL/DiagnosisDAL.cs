using Doctor.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Doctor.DAL
{
    public class DiagnosisDAL
    {
        public static bool Insert(DiagnosisModel diagnosis)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"insert into Diagnosis(record_id, result, time)
				values(@record_id, @result, @time)",
                    new SqlParameter("@record_id", diagnosis.Record_id),
                    new SqlParameter("@result", diagnosis.Result),
                    new SqlParameter("@time", diagnosis.Time)
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
                SqlHelper.ExecuteNonQuery(@"delete from Diagnosis where diagnosis_id = @id",
                    new SqlParameter("@id", id));
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool Update(DiagnosisModel diagnosis)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"update Diagnosis set
				record_id = @record_id,
				result = @result,
				time = @time
				where diagnosis_id = @diagnosis_id",
                    new SqlParameter("@record_id", diagnosis.Record_id),
                    new SqlParameter("@result", diagnosis.Result),
                    new SqlParameter("@time", diagnosis.Time),
                    new SqlParameter("@diagnosis_id", diagnosis.Diagnosis_id)
                );
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static DiagnosisModel GetById(System.Int64 id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Diagnosis where diagnosis_id = @id",
                new SqlParameter("@id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("Fatal Error: Duplicated Id in Table Diagnosis");
            }
            else
            {
                DataRow row = table.Rows[0];
                return ToModel(row);
            }
        }

        public static DiagnosisModel[] GetAll()
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from Diagnosis");
            DiagnosisModel[] diagnosis = new DiagnosisModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                diagnosis[i] = ToModel(table.Rows[i]);
            }
            return diagnosis;
        }

        private static DiagnosisModel ToModel(DataRow row)
        {
            DiagnosisModel diagnosis = new DiagnosisModel();
            diagnosis.Diagnosis_id = (System.Int64)row["diagnosis_id"];
            diagnosis.Record_id = (System.Int64)row["record_id"];
            diagnosis.Result = (System.String)row["result"];
            diagnosis.Time = (System.DateTime)row["time"];
            return diagnosis;
        }
    }
}
