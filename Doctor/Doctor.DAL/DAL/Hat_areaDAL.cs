using Doctor.Model;
using System;
using System.Data;
using System.Data.SqlClient;
namespace Doctor.DAL
{
    public class Hat_areaDAL
    {
        public static Hat_areaModel GetById(int id)
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

        public static Hat_areaModel[] GetAll()
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from hat_area");
            Hat_areaModel[] hat_area = new Hat_areaModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                hat_area[i] = ToModel(table.Rows[i]);
            }
            return hat_area;
        }

        public static Hat_areaModel[] GetAllByCityId(int id)
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

        /// <summary>
        /// ͨ������ȡ�����ڳ���
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static Hat_cityModel GetFatherCity(Hat_areaModel area)
        {
            int id = (int)SqlHelper.ExecuteScalar("select id from hat_city where cityID = @cityID",
                new SqlParameter("@cityID", area.Father));

            return Hat_cityDAL.GetById(id);
        }

        /// <summary>
        /// ͨ������Area��ȡ�����ϼ��У�ʡ���ַ���
        /// e.g: ˫���ص�Hat_areaModel -> "�Ĵ�ʡ_�ɶ���_˫����"
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static string GetFullLocationString(Hat_areaModel area)
        {
            Hat_cityModel fatherCity = Hat_areaDAL.GetFatherCity(area);
            Hat_provinceModel fatherProvince = Hat_cityDAL.GetFatherProvince(fatherCity);

            string result = fatherProvince.Province + "_" + fatherCity.City + "_" + area.Area;
            return result;
        }
    }

}
