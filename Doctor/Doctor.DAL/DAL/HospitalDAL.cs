using Doctor.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Doctor.DAL
{
    public class HospitalDAL
    {
        public static bool Insert(HospitalModel hospital)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"insert into Hospital(name, address, latitude, longitude, area_id)
				values(@name, @address, @latitude, @longitude, @area_id)",
                    new SqlParameter("@name", hospital.Name),
                    new SqlParameter("@address", hospital.Address),
                    new SqlParameter("@latitude", hospital.Latitude),
                    new SqlParameter("@longitude", hospital.Longitude),
                    new SqlParameter("@area_id", hospital.Area_id)
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
                SqlHelper.ExecuteNonQuery(@"delete from Hospital where hospital_id = @id",
                    new SqlParameter("@id", id));
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool Update(HospitalModel hospital)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"update Hospital set
				name = @name,
				address = @address,
				latitude = @latitude,
				longitude = @longitude,
				area_id = @area_id
				where hospital_id = @hospital_id",
                    new SqlParameter("@name", hospital.Name),
                    new SqlParameter("@address", hospital.Address),
                    new SqlParameter("@latitude", hospital.Latitude),
                    new SqlParameter("@longitude", hospital.Longitude),
                    new SqlParameter("@area_id", hospital.Area_id),
                    new SqlParameter("@hospital_id", hospital.Hospital_id)
                );
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static HospitalModel GetById(System.Int64 id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Hospital where hospital_id = @id",
                new SqlParameter("@id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("Fatal Error: Duplicated Id in Table Hospital");
            }
            else
            {
                DataRow row = table.Rows[0];
                return ToModel(row);
            }
        }

        public static HospitalModel[] GetAll()
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from Hospital");
            HospitalModel[] hospital = new HospitalModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                hospital[i] = ToModel(table.Rows[i]);
            }
            return hospital;
        }

        private static HospitalModel ToModel(DataRow row)
        {
            HospitalModel hospital = new HospitalModel();
            hospital.Hospital_id = (System.Int64)row["hospital_id"];
            hospital.Name = (System.String)row["name"];
            hospital.Address = (System.String)row["address"];
            hospital.Latitude = (System.Double)row["latitude"];
            hospital.Longitude = (System.Double)row["longitude"];
            hospital.Area_id = (System.Int32)row["area_id"];
            return hospital;
        }

        /// <summary>
        /// 获取一个地区的所有医院
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static HospitalModel[] GetAllByAreaId(int id)
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from Hospital where area_id = @id",
                new SqlParameter("@id", id));
            HospitalModel[] hospitals = new HospitalModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                hospitals[i] = ToModel(table.Rows[i]);
            }
            return hospitals;
        }
    }
}
