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
        public static void Insert(DiagnosisModel diagnosis)
        {
            SqlHelper.ExecuteNonQuery(@"insert into DiagnosisModel(@diagnosis_id, @record_id, @result, time)
			values(diagnosis_id, record_id, result, time)",
                new SqlParameter("@diagnosis_id", diagnosis.Diagnosis_id),
                new SqlParameter("@record_id", diagnosis.Record_id),
                new SqlParameter("@result", diagnosis.Result),
                new SqlParameter("@time", diagnosis.Time)
            );
        }

        public static void DeleteById(long id)
        {
            SqlHelper.ExecuteNonQuery(@"delete from Diagnosis where id = @id",
                new SqlParameter("@id", id));
        }

        public static void Update(DiagnosisModel diagnosis)
        {
            SqlHelper.ExecuteNonQuery(@"update Diagnosis set
			diagnosis_id = @diagnosis_id,
			record_id = @record_id,
			result = @result,
			time = @time
			where id = @id",
                new SqlParameter("@diagnosis_id", diagnosis.Diagnosis_id),
                new SqlParameter("@record_id", diagnosis.Record_id),
                new SqlParameter("@result", diagnosis.Result),
                new SqlParameter("@time", diagnosis.Time)
            );
        }

        public static DiagnosisModel GetById(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Diagnosis where id = @id",
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
