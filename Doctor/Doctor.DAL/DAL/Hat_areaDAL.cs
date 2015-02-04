using Doctor.Model;
using System;
using System.Data;
using System.Data.SqlClient;
namespace Doctor.DAL
{
    public class Hat_areaDAL
    {
        public static void Insert(Hat_areaModel hat_area)
        {
            SqlHelper.ExecuteNonQuery(@"insert into Hat_areaModel(@id, @areaID, @area, father)
			values(id, areaID, area, father)",
                new SqlParameter("@id", hat_area.Id),
                new SqlParameter("@areaID", hat_area.AreaID),
                new SqlParameter("@area", hat_area.Area),
                new SqlParameter("@father", hat_area.Father)
            );
        }

        public static void DeleteById(long id)
        {
            SqlHelper.ExecuteNonQuery(@"delete from hat_area where id = @id",
                new SqlParameter("@id", id));
        }

        public static void Update(Hat_areaModel hat_area)
        {
            SqlHelper.ExecuteNonQuery(@"update hat_area set
			id = @id,
			areaID = @areaID,
			area = @area,
			father = @father
			where id = @id",
                new SqlParameter("@id", hat_area.Id),
                new SqlParameter("@areaID", hat_area.AreaID),
                new SqlParameter("@area", hat_area.Area),
                new SqlParameter("@father", hat_area.Father)
            );
        }
        
        public static Hat_areaModel GetById(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from hat_area where id = @id",
                new SqlParameter("@id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("Fatal Error: Duplicated Id in Table hat_area");
            }
            else
            {
                DataRow row = table.Rows[0];
                return ToModel(row);
            }
        }
        
        public static Hat_areaModel[] GetAllByCityId(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from hat_area where hat_area.father = (select cityID from hat_city where hat_city.id = @id)",
                new SqlParameter("@id", id));
            Hat_areaModel[] hat_area = new Hat_areaModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                hat_area[i] = ToModel(table.Rows[i]);
            }
            return hat_area;
        }

        private static Hat_areaModel ToModel(DataRow row)
        {
            Hat_areaModel hat_area = new Hat_areaModel();
            hat_area.Id = (System.Int32)row["id"];
            hat_area.AreaID = (System.String)row["areaID"];
            hat_area.Area = (System.String)row["area"];
            hat_area.Father = (System.String)row["father"];
            return hat_area;
        }
    }

}
