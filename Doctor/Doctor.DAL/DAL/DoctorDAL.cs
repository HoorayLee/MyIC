using Doctor.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Doctor.DAL
{
    public class DoctorDAL
    {
        public static void Insert(DoctorModel doctor)
        {
            SqlHelper.ExecuteNonQuery(@"insert into Doctor(@doc_id, @hospital_id, @password, @name, @photoPath, @hospital, @licenseNo, @licensePath, ifAuth)
			values(doc_id, hospital_id, password, name, photoPath, hospital, licenseNo, licensePath, ifAuth)",
                new SqlParameter("@doc_id", doctor.Doc_id),
                new SqlParameter("@hospital_id", doctor.Hospital_id),
                new SqlParameter("@password", doctor.Password),
                new SqlParameter("@name", doctor.Name),
                new SqlParameter("@photoPath", doctor.PhotoPath),
                new SqlParameter("@hospital", doctor.Hospital),
                new SqlParameter("@licenseNo", doctor.LicenseNo),
                new SqlParameter("@licensePath", doctor.LicensePath),
                new SqlParameter("@ifAuth", doctor.IfAuth)
            );
        }

        public static void DeleteById(long id)
        {
            SqlHelper.ExecuteNonQuery(@"delete from Doctor where id = @id",
                new SqlParameter("@id", id));
        }

        public static void Update(DoctorModel doctor)
        {
            SqlHelper.ExecuteNonQuery(@"update Doctor set
			doc_id = @doc_id,
			hospital_id = @hospital_id,
			password = @password,
			name = @name,
			photoPath = @photoPath,
			hospital = @hospital,
			licenseNo = @licenseNo,
			licensePath = @licensePath,
			ifAuth = @ifAuth
			where id = @id",
                new SqlParameter("@doc_id", doctor.Doc_id),
                new SqlParameter("@hospital_id", doctor.Hospital_id),
                new SqlParameter("@password", doctor.Password),
                new SqlParameter("@name", doctor.Name),
                new SqlParameter("@photoPath", doctor.PhotoPath),
                new SqlParameter("@hospital", doctor.Hospital),
                new SqlParameter("@licenseNo", doctor.LicenseNo),
                new SqlParameter("@licensePath", doctor.LicensePath),
                new SqlParameter("@ifAuth", doctor.IfAuth)
            );
        }

        public static DoctorModel GetById(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Doctor where id = @id",
                new SqlParameter("@id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("Fatal Error: Duplicated Id in Table Doctor");
            }
            else
            {
                DataRow row = table.Rows[0];
                return ToModel(row);
            }
        }

        public static DoctorModel[] GetAll()
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from Doctor");
            DoctorModel[] doctor = new DoctorModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                doctor[i] = ToModel(table.Rows[i]);
            }
            return doctor;
        }

        private static DoctorModel ToModel(DataRow row)
        {
            DoctorModel doctor = new DoctorModel();
            doctor.Doc_id = (System.Int64)row["doc_id"];
            doctor.Hospital_id = (System.Int64?)SqlHelper.FromDBValue(row["hospital_id"]);
            doctor.Password = (System.String)row["password"];
            doctor.Name = (System.String)row["name"];
            doctor.PhotoPath = (System.String)SqlHelper.FromDBValue(row["photoPath"]);
            doctor.Hospital = (System.String)SqlHelper.FromDBValue(row["hospital"]);
            doctor.LicenseNo = (System.String)SqlHelper.FromDBValue(row["licenseNo"]);
            doctor.LicensePath = (System.String)SqlHelper.FromDBValue(row["licensePath"]);
            doctor.IfAuth = (System.Boolean)row["ifAuth"];
            return doctor;
        }

        /// <summary>
        /// 检查用户名和密码是否正确
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="state">返回的状态:username not exist;password error;success</param>
        /// <returns>正确返回用户所有信息</returns>
        public static DoctorModel CheckPassword(string name, string password, ref string state)
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from Doctor where name = @name",
                new SqlParameter("@name", name));
            if (table.Rows.Count <= 0)
            {
                state = "username not exist";
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("数据库异常：存在相同用户名的医生");
            }
            else
            {

                table = SqlHelper.ExecuteDataTable("select * from Doctor where name = @name and password = @password",
                    new SqlParameter("@name", name),
                    new SqlParameter("@password", password));

                if (table.Rows.Count == 1)
                {
                    state = "success";
                    DoctorModel doctorModel = DoctorDAL.ToModel(table.Rows[0]);
                    return doctorModel;
                }
                else
                {
                    state = "password error";
                    return null;
                }
            }
        }
    }

}
