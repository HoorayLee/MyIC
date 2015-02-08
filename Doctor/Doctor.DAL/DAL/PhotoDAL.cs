using Doctor.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Doctor.DAL
{
    public class PhotoDAL
    {
        public static bool Insert(PhotoModel photo)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"insert into Photo(record_id, path)
				values(@record_id, @path)",
                    new SqlParameter("@record_id", photo.Record_id),
                    new SqlParameter("@path", photo.Path)
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
                SqlHelper.ExecuteNonQuery(@"delete from Photo where photo_id = @id",
                    new SqlParameter("@id", id));
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool Update(PhotoModel photo)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"update Photo set
				record_id = @record_id,
				path = @path
				where photo_id = @photo_id",
                    new SqlParameter("@record_id", photo.Record_id),
                    new SqlParameter("@path", photo.Path),
                    new SqlParameter("@photo_id", photo.Photo_id)
                );
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static PhotoModel GetById(System.Int64 id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Photo where photo_id = @id",
                new SqlParameter("@id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("Fatal Error: Duplicated Id in Table Photo");
            }
            else
            {
                DataRow row = table.Rows[0];
                return ToModel(row);
            }
        }

        public static PhotoModel[] GetAll()
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from Photo");
            PhotoModel[] photo = new PhotoModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                photo[i] = ToModel(table.Rows[i]);
            }
            return photo;
        }

        private static PhotoModel ToModel(DataRow row)
        {
            PhotoModel photo = new PhotoModel();
            photo.Photo_id = (System.Int64)row["photo_id"];
            photo.Record_id = (System.Int64)row["record_id"];
            photo.Path = (System.String)row["path"];
            return photo;
        }

        public static PhotoModel[] GetAllByRecordId(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from Photo where record_id = @id",
                new SqlParameter("@id", id));
            PhotoModel[] photo = new PhotoModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                photo[i] = ToModel(table.Rows[i]);
            }
            return photo;
        }
    }
}
