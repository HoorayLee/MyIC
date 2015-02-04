using Doctor.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Doctor.DAL
{
    public class UserDAL
    {
        public static void Insert(UserModel user)
        {
            SqlHelper.ExecuteNonQuery(@"insert into UserModel(@user_id, @password, @region, date_of_birth)
			values(user_id, password, region, date_of_birth)",
                new SqlParameter("@user_id", user.User_id),
                new SqlParameter("@password", user.Password),
                new SqlParameter("@region", user.Region),
                new SqlParameter("@date_of_birth", user.Date_of_birth)
            );
        }

        public static void DeleteById(long id)
        {
            SqlHelper.ExecuteNonQuery(@"delete from User where id = @id",
                new SqlParameter("@id", id));
        }

        public static void Update(UserModel user)
        {
            SqlHelper.ExecuteNonQuery(@"update User set
			user_id = @user_id,
			password = @password,
			region = @region,
			date_of_birth = @date_of_birth
			where id = @id",
                new SqlParameter("@user_id", user.User_id),
                new SqlParameter("@password", user.Password),
                new SqlParameter("@region", user.Region),
                new SqlParameter("@date_of_birth", user.Date_of_birth)
            );
        }

        public static UserModel GetById(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from User where id = @id",
                new SqlParameter("@id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("Fatal Error: Duplicated Id in Table User");
            }
            else
            {
                DataRow row = table.Rows[0];
                return ToModel(row);
            }
        }

        public static UserModel[] GetAll()
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from User");
            UserModel[] user = new UserModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                user[i] = ToModel(table.Rows[i]);
            }
            return user;
        }

        private static UserModel ToModel(DataRow row)
        {
            UserModel user = new UserModel();
            user.User_id = (System.Int64)row["user_id"];
            user.Password = (System.String)row["password"];
            user.Region = (System.String)row["region"];
            user.Date_of_birth = (System.DateTime)row["date_of_birth"];
            return user;
        }

        
    }

}
