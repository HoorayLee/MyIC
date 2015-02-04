using Doctor.Model;
using System;
using System.Data;
using System.Data.SqlClient;
namespace Doctor.DAL
{

    public class Hat_cityDAL
    {
        public static void Insert(Hat_cityModel hat_city)
        {
            SqlHelper.ExecuteNonQuery(@"insert into Hat_cityModel(@id, @cityID, @city, father)
			values(id, cityID, city, father)",
                new SqlParameter("@id", hat_city.Id),
                new SqlParameter("@cityID", hat_city.CityID),
                new SqlParameter("@city", hat_city.City),
                new SqlParameter("@father", hat_city.Father)
            );
        }

        public static void DeleteById(long id)
        {
            SqlHelper.ExecuteNonQuery(@"delete from hat_city where id = @id",
                new SqlParameter("@id", id));
        }

        public static void Update(Hat_cityModel hat_city)
        {
            SqlHelper.ExecuteNonQuery(@"update hat_city set
			id = @id,
			cityID = @cityID,
			city = @city,
			father = @father
			where id = @id",
                new SqlParameter("@id", hat_city.Id),
                new SqlParameter("@cityID", hat_city.CityID),
                new SqlParameter("@city", hat_city.City),
                new SqlParameter("@father", hat_city.Father)
            );
        }

        public static Hat_cityModel GetById(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from hat_city where id = @id",
                new SqlParameter("@id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("Fatal Error: Duplicated Id in Table hat_city");
            }
            else
            {
                DataRow row = table.Rows[0];
                return ToModel(row);
            }
        }

        public static Hat_cityModel[] GetAllByProvinceId(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from hat_city where hat_city.father = (select provinceID from hat_province where hat_province.id = @id)", 
                new SqlParameter("@id", id));
            Hat_cityModel[] hat_city = new Hat_cityModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                hat_city[i] = ToModel(table.Rows[i]);
            }
            return hat_city;
        }

        private static Hat_cityModel ToModel(DataRow row)
        {
            Hat_cityModel hat_city = new Hat_cityModel();
            hat_city.Id = (System.Int32)row["id"];
            hat_city.CityID = (System.String)row["cityID"];
            hat_city.City = (System.String)row["city"];
            hat_city.Father = (System.String)row["father"];
            return hat_city;
        }
    }
}
