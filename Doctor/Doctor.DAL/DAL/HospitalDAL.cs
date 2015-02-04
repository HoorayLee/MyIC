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
        public static void Insert(HospitalModel hospital)
        {
            SqlHelper.ExecuteNonQuery(@"insert into HospitalModel(@hospital_id, @name, @address, @latitude, longitude)
			values(hospital_id, name, address, latitude, longitude)",
                new SqlParameter("@hospital_id", hospital.Hospital_id),
                new SqlParameter("@name", hospital.Name),
                new SqlParameter("@address", hospital.Address),
                new SqlParameter("@latitude", hospital.Latitude),
                new SqlParameter("@longitude", hospital.Longitude)
            );
        }

        public static void DeleteById(long id)
        {
            SqlHelper.ExecuteNonQuery(@"delete from Hospital where id = @id",
                new SqlParameter("@id", id));
        }

        public static void Update(HospitalModel hospital)
        {
            SqlHelper.ExecuteNonQuery(@"update Hospital set
			hospital_id = @hospital_id,
			name = @name,
			address = @address,
			latitude = @latitude,
			longitude = @longitude
			where id = @id",
                new SqlParameter("@hospital_id", hospital.Hospital_id),
                new SqlParameter("@name", hospital.Name),
                new SqlParameter("@address", hospital.Address),
                new SqlParameter("@latitude", hospital.Latitude),
                new SqlParameter("@longitude", hospital.Longitude)
            );
        }

        public static HospitalModel GetById(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Hospital where id = @id",
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
            return hospital;
        }
    }
}
