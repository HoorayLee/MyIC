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
        public static void Insert(RecordModel record)
        {
            SqlHelper.ExecuteNonQuery(@"insert into RecordModel(@record_id, @user_id, @description, time)
			values(record_id, user_id, description, time)",
                new SqlParameter("@record_id", record.Record_id),
                new SqlParameter("@user_id", record.User_id),
                new SqlParameter("@description", record.Description),
                new SqlParameter("@time", record.Time)
            );
        }

        public static void DeleteById(long id)
        {
            SqlHelper.ExecuteNonQuery(@"delete from Record where id = @id",
                new SqlParameter("@id", id));
        }

        public static void Update(RecordModel record)
        {
            SqlHelper.ExecuteNonQuery(@"update Record set
			record_id = @record_id,
			user_id = @user_id,
			description = @description,
			time = @time
			where id = @id",
                new SqlParameter("@record_id", record.Record_id),
                new SqlParameter("@user_id", record.User_id),
                new SqlParameter("@description", record.Description),
                new SqlParameter("@time", record.Time)
            );
        }

        public static RecordModel GetById(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Record where id = @id",
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
            return record;
        }
    }

}
