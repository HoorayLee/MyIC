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
        public static bool Insert(DoctorModel doctor)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"insert into Doctor(hospital_id, password, name, photoPath, licenseNo, licensePath, ifAuth)
				values(@hospital_id, @password, @name, @photoPath, @licenseNo, @licensePath, @ifAuth)",
                    new SqlParameter("@hospital_id", SqlHelper.ToDBValue(doctor.Hospital_id)),
                    new SqlParameter("@password", doctor.Password),
                    new SqlParameter("@name", doctor.Name),
                    new SqlParameter("@photoPath", SqlHelper.ToDBValue(doctor.PhotoPath)),
                    new SqlParameter("@licenseNo", SqlHelper.ToDBValue(doctor.LicenseNo)),
                    new SqlParameter("@licensePath", SqlHelper.ToDBValue(doctor.LicensePath)),
                    new SqlParameter("@ifAuth", doctor.IfAuth)
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
                SqlHelper.ExecuteNonQuery(@"delete from Doctor where doc_id = @id",
                    new SqlParameter("@id", id));
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool Update(DoctorModel doctor)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"update Doctor set
				hospital_id = @hospital_id,
				password = @password,
				name = @name,
				photoPath = @photoPath,
				licenseNo = @licenseNo,
				licensePath = @licensePath,
				ifAuth = @ifAuth
				where doc_id = @doc_id",
                    new SqlParameter("@hospital_id", SqlHelper.ToDBValue(doctor.Hospital_id)),
                    new SqlParameter("@password", doctor.Password),
                    new SqlParameter("@name", doctor.Name),
                    new SqlParameter("@photoPath", SqlHelper.ToDBValue(doctor.PhotoPath)),
                    new SqlParameter("@licenseNo", SqlHelper.ToDBValue(doctor.LicenseNo)),
                    new SqlParameter("@licensePath", SqlHelper.ToDBValue(doctor.LicensePath)),
                    new SqlParameter("@ifAuth", doctor.IfAuth),
                    new SqlParameter("@doc_id", doctor.Doc_id)
                );
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static DoctorModel GetById(System.Int64 id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Doctor where doc_id = @id",
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
            doctor.LicenseNo = (System.String)SqlHelper.FromDBValue(row["licenseNo"]);
            doctor.LicensePath = (System.String)SqlHelper.FromDBValue(row["licensePath"]);
            doctor.IfAuth = (System.Boolean)row["ifAuth"];
            return doctor;
        }

        /// <summary>
        /// 检查医生用户名是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CheckDoctorExist(string name)
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from Doctor where name = @name",
                new SqlParameter("@name", name));
            if (table.Rows.Count <= 0)
            {
                return false; 
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("数据库异常：存在相同用户名的医生");
            }
            else
            {
                return true;
            }
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
            if (!CheckDoctorExist(name))
            {
                state = "username not exist";
                return null;
            }
            else
            {

                DataTable table = SqlHelper.ExecuteDataTable("select * from Doctor where name = @name and password = @password",
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
