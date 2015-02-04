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
        public static void Insert(PhotoModel photo)
        {
            SqlHelper.ExecuteNonQuery(@"insert into PhotoModel(@photo_id, @record_id, path)
			values(photo_id, record_id, path)",
                new SqlParameter("@photo_id", photo.Photo_id),
                new SqlParameter("@record_id", photo.Record_id),
                new SqlParameter("@path", photo.Path)
            );
        }

        public static void DeleteById(long id)
        {
            SqlHelper.ExecuteNonQuery(@"delete from Photo where id = @id",
                new SqlParameter("@id", id));
        }

        public static void Update(PhotoModel photo)
        {
            SqlHelper.ExecuteNonQuery(@"update Photo set
			photo_id = @photo_id,
			record_id = @record_id,
			path = @path
			where id = @id",
                new SqlParameter("@photo_id", photo.Photo_id),
                new SqlParameter("@record_id", photo.Record_id),
                new SqlParameter("@path", photo.Path)
            );
        }

        public static PhotoModel GetById(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Photo where id = @id",
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
    }
}
