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
        public static bool Insert(UserModel user)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"insert into [User](password, date_of_birth, name)
				values(@password, @date_of_birth, @name)",
                    new SqlParameter("@password", user.Password),
                    new SqlParameter("@date_of_birth", SqlHelper.ToDBValue(user.Date_of_birth)),
                    new SqlParameter("@name", user.Name)
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
                SqlHelper.ExecuteNonQuery(@"delete from [User] where user_id = @id",
                    new SqlParameter("@id", id));
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool Update(UserModel user)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"update [User] set
				password = @password,
				date_of_birth = @date_of_birth,
				name = @name
				where user_id = @user_id",
                    new SqlParameter("@password", user.Password),
                    new SqlParameter("@date_of_birth", SqlHelper.ToDBValue(user.Date_of_birth)),
                    new SqlParameter("@name", user.Name),
                    new SqlParameter("@user_id", user.User_id)
                );
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static UserModel GetById(System.Int64 id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from [User] where user_id = @id",
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
            DataTable table = SqlHelper.ExecuteDataTable("select * from [User]");
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
            user.Date_of_birth = (System.DateTime?)SqlHelper.FromDBValue(row["date_of_birth"]);
            user.Name = (System.String)row["name"];
            return user;
        }
    }
}
