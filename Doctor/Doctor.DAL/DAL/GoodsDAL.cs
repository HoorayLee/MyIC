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
        public static void Insert(GoodsModel goods)
        {
            SqlHelper.ExecuteNonQuery(@"insert into GoodsModel(@goods_id, @name, @introduction, @price, type)
			values(goods_id, name, introduction, price, type)",
                new SqlParameter("@goods_id", goods.Goods_id),
                new SqlParameter("@name", goods.Name),
                new SqlParameter("@introduction", goods.Introduction),
                new SqlParameter("@price", goods.Price),
                new SqlParameter("@type", goods.Type)
            );
        }

        public static void DeleteById(long id)
        {
            SqlHelper.ExecuteNonQuery(@"delete from Goods where id = @id",
                new SqlParameter("@id", id));
        }

        public static void Update(GoodsModel goods)
        {
            SqlHelper.ExecuteNonQuery(@"update Goods set
			goods_id = @goods_id,
			name = @name,
			introduction = @introduction,
			price = @price,
			type = @type
			where id = @id",
                new SqlParameter("@goods_id", goods.Goods_id),
                new SqlParameter("@name", goods.Name),
                new SqlParameter("@introduction", goods.Introduction),
                new SqlParameter("@price", goods.Price),
                new SqlParameter("@type", goods.Type)
            );
        }

        public static GoodsModel GetById(long id)
        {
            DataTable table = SqlHelper.ExecuteDataTable(@"select * from Goods where id = @id",
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
            goods.Introduction = (System.String)row["introduction"];
            goods.Price = (System.Decimal)row["price"];
            goods.Type = (System.String)row["type"];
            return goods;
        }
    }
}
