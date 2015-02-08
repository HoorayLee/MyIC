using Doctor.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Doctor.DAL
{
    public class GoodsDAL
    {
        public static bool Insert(GoodsModel goods)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"insert into Goods(name, introduction, price, type)
				values(@name, @introduction, @price, @type)",
                    new SqlParameter("@name", goods.Name),
                    new SqlParameter("@introduction", SqlHelper.ToDBValue(goods.Introduction)),
                    new SqlParameter("@price", goods.Price),
                    new SqlParameter("@type", goods.Type)
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
                SqlHelper.ExecuteNonQuery(@"delete from Goods where goods_id = @id",
                    new SqlParameter("@id", id));
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool Update(GoodsModel goods)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(@"update Goods set
				name = @name,
				introduction = @introduction,
				price = @price,
				type = @type
				where goods_id = @goods_id",
                    new SqlParameter("@name", goods.Name),
                    new SqlParameter("@introduction", SqlHelper.ToDBValue(goods.Introduction)),
                    new SqlParameter("@price", goods.Price),
                    new SqlParameter("@type", goods.Type),
                    new SqlParameter("@goods_id", goods.Goods_id)
                );
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static GoodsModel GetById(System.Int64 id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Goods where goods_id = @id",
                new SqlParameter("@id", id));
            if (table.Rows.Count <= 0)
            {
                return null;
            }
            else if (table.Rows.Count > 1)
            {
                throw new Exception("Fatal Error: Duplicated Id in Table Goods");
            }
            else
            {
                DataRow row = table.Rows[0];
                return ToModel(row);
            }
        }

        public static GoodsModel[] GetAll()
        {
            DataTable table = SqlHelper.ExecuteDataTable("select * from Goods");
            GoodsModel[] goods = new GoodsModel[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                goods[i] = ToModel(table.Rows[i]);
            }
            return goods;
        }

        private static GoodsModel ToModel(DataRow row)
        {
            GoodsModel goods = new GoodsModel();
            goods.Goods_id = (System.Int64)row["goods_id"];
            goods.Name = (System.String)row["name"];
            goods.Introduction = (System.String)SqlHelper.FromDBValue(row["introduction"]);
            goods.Price = (System.Decimal)row["price"];
            goods.Type = (System.String)row["type"];
            return goods;
        }
    }
}
