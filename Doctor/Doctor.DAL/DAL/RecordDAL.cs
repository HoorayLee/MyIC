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

//        public static RecordModel[] GetAllByProvinceName(string province)
//        {
//            try
//            {
//                //获取省的编号
//                DataTable table = SqlHelper.ExecuteDataTable(@"select * from hat_province where province like '@province%'",
//                    new SqlParameter("@province", province));
//                DataRow provinceRow = table.Rows[0];

//                SqlHelper.ExecuteDataTable(@"select * from Record where hat_area_")
//            }
//            catch (SqlException)
//            {
//                return null;
//            }
//        }

//        public static RecordModel[] GetAllCityFirst(string city)
//        {
//            try
//            {
//                DataTable cityTable = SqlHelper.ExecuteDataTable(@"select * from hat_city where city like '@city%'",
//                    new SqlParameter("@city", city));
//                DataRow cityRow = cityTable.Rows[0];

//                //通过city找所有底下的区域
//                Hat_areaModel[] areas = Hat_areaDAL.GetAllByCityId((int)cityRow["id"]);

//                //通过区域找相关联的自检
//                List<RecordModel> recordList = new List<RecordModel>();
//                foreach (var area in areas)
//                {
//                    DataTable table = SqlHelper.ExecuteDataTable(@"select * from Record where hat_area_id = @id 
//                        union select * from Record where hat_area_id != @id",
//                        new SqlParameter("@id", area.Id));
//                    foreach (DataRow row in table.Rows)
//                    {
//                        recordList.Add(ToModel(row));
//                    }
//                }
//                return recordList.ToArray();
//            }
//            catch (SqlException)
//            {
//                return null;
//            }
//        }

//        public static RecordModel[] GetAllAreaFirst(string area)
//        {
//            try
//            {
//                //获取地区编号
//                int area_id = (int)SqlHelper.ExecuteScalar(@"select id from hat_area where area like '@area'",
//                    new SqlParameter("@area", area));

//                //根据地区编号获取所有记录
//                DataTable table = SqlHelper.ExecuteDataTable(@"select * from Record where hat_area_id = @id 
//                    union select * from Record where hat_area_id != @id",
//                    new SqlParameter("@id", area_id));

//                int nbRows = table.Rows.Count;
//                RecordModel[] models = new RecordModel[nbRows];
//                for (int i = 0; i < nbRows; i++)
//                {
//                    models[i] = ToModel(table.Rows[i]);
//                }
//                return models;
//            }
//            catch (SqlException)
//            {
//                return null;
//            }
//        }
    }

}
