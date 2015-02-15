using Doctor.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Doctor.DAL
{
    public class RecordDAL
    {
        public static bool Insert(RecordModel record)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"insert into Record(user_id, description, time, hat_area_id)
				values(@user_id, @description, @time, @hat_area_id)",
                    new SqlParameter("@user_id", record.User_id),
                    new SqlParameter("@description", record.Description),
                    new SqlParameter("@time", record.Time),
                    new SqlParameter("@hat_area_id", record.Hat_area_id)
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
                SqlHelper.ExecuteNonQuery(@"delete from Record where record_id = @id",
                    new SqlParameter("@id", id));
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool Update(RecordModel record)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"update Record set
				user_id = @user_id,
				description = @description,
				time = @time,
				hat_area_id = @hat_area_id
				where record_id = @record_id",
                    new SqlParameter("@user_id", record.User_id),
                    new SqlParameter("@description", record.Description),
                    new SqlParameter("@time", record.Time),
                    new SqlParameter("@hat_area_id", record.Hat_area_id),
                    new SqlParameter("@record_id", record.Record_id)
                );
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static RecordModel GetById(System.Int64 id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Record where record_id = @id",
                new SqlParameter("@id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("Fatal Error: Duplicated Id in Table Record");
            }
            else
            {
                DataRow row = table.Rows[0];
                return ToModel(row);
            }
        }

        public static RecordModel[] GetAll()
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from Record");
            RecordModel[] record = new RecordModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                record[i] = ToModel(table.Rows[i]);
            }
            return record;
        }

        private static RecordModel ToModel(DataRow row)
        {
            RecordModel record = new RecordModel();
            record.Record_id = (System.Int64)row["record_id"];
            record.User_id = (System.Int64)row["user_id"];
            record.Description = (System.String)row["description"];
            record.Time = (System.DateTime)row["time"];
            record.Hat_area_id = (System.Int32)row["hat_area_id"];
            return record;
        }
    }

}
