using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeGen
{
    public partial class MainForm : Form
    {
        private static readonly string connStr = ConfigurationManager.ConnectionStrings["Geek"].ConnectionString;


        private void MainForm_Load(object sender, EventArgs e)
        {
            tb_connStr.Text = connStr;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击事件：连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string connStr = tb_connStr.Text;
            string[] tableNames = GetTableNames(connStr);
            if (tableNames != null)
            {
                cb_tables.Items.AddRange(tableNames);
                cb_tables.Enabled = true;
                btn_gen.Enabled = true;
                btn_genAll.Enabled = true;
            }
        }

        /// <summary>
        /// 点击事件：生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_gen_Click(object sender, EventArgs e)
        {
            if (cb_tables.SelectedIndex < 0)
            {
                MessageBox.Show("请选择数据库表名");
                return;
            }

            string tableName = cb_tables.SelectedItem.ToString();
            string nameSpace = tb_namespace.Text;

            tb_model.Text = GenModels(tableName, nameSpace);
            tb_DAL.Text = GenDAL(tableName, nameSpace);
        }

        private string GenDAL(string tableName, string nameSpace)
        {
            StringBuilder builder = new StringBuilder();

            string className = FirstLetterUpper(tableName) + "DAL";
            string modelClassName = FirstLetterUpper(tableName) + "Model";
            string objName = FirstLetterLower(tableName);

            builder.Append("public class ").AppendLine(className);
            builder.AppendLine("{");


            DataTable table = SqlHelper.ExecuteDataTable("select top 0 * from [" + tableName + "]");
            string primaryKey = GetPrimaryKey(table);
            string primaryKeyType = table.PrimaryKey[0].DataType.ToString();

            //1.Insert
            builder.AppendFormat("\tpublic static bool Insert({0} {1})", modelClassName, objName).AppendLine();
            builder.AppendLine("\t{");
            builder.AppendLine("\t\ttry");
            builder.AppendLine("\t\t{");
            builder.Append("\t\t\tSqlHelper.ExecuteNonQuery(@\"insert into ").Append(tableName).
                AppendLine(GetColsString(table, false));
            builder.Append("\t\t\t\tvalues").Append(GetColsString(table, true)).AppendLine("\",");
            builder.Append(GetParamsString(table, objName, false));
            builder.AppendLine("\t\t\t);");
            builder.AppendLine("\t\t\treturn true;");
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t\tcatch(SqlException)");
            builder.AppendLine("\t\t{");
            builder.AppendLine("\t\t\treturn false;");
            builder.AppendLine("\t\t}");;
            builder.AppendLine("\t}");
            builder.AppendLine();

            //2.DeleteById
            builder.AppendFormat("\tpublic static bool DeleteById({0} id)", primaryKeyType).AppendLine();
            builder.AppendLine("\t{");
            builder.AppendLine("\t\ttry");
            builder.AppendLine("\t\t{");
            builder.AppendFormat("\t\t\tSqlHelper.ExecuteNonQuery(@\"delete from {0} where {1} = @id\",", tableName, primaryKey).AppendLine();
            builder.AppendLine("\t\t\t\tnew SqlParameter(\"@id\", id));");
            builder.AppendLine("\t\t\treturn true;");
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t\tcatch(SqlException)");
            builder.AppendLine("\t\t{");
            builder.AppendLine("\t\t\treturn false;");
            builder.AppendLine("\t\t}"); ;
            builder.AppendLine("\t}");
            builder.AppendLine();

            //3.Update
            builder.AppendFormat("\tpublic static bool Update({0} {1})", modelClassName, objName).AppendLine();
            builder.AppendLine("\t{");
            builder.AppendLine("\t\ttry");
            builder.AppendLine("\t\t{");
            builder.AppendFormat("\t\t\tSqlHelper.ExecuteNonQuery(@\"update {0} set", tableName).AppendLine();
            builder.Append(GetColsStringForSet(table));
            builder.AppendFormat("\t\t\t\twhere {0} = @{0}\",", primaryKey).AppendLine();
            builder.Append(GetParamsString(table, objName, true));
            builder.AppendLine("\t\t\t);");
            builder.AppendLine("\t\t\treturn true;");
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t\tcatch(SqlException)");
            builder.AppendLine("\t\t{");
            builder.AppendLine("\t\t\treturn false;");
            builder.AppendLine("\t\t}"); ;
            builder.AppendLine("\t}");
            builder.AppendLine();

            //4.GetById
            builder.AppendFormat("\tpublic static {0} GetById({1} id)", modelClassName, primaryKeyType).
                AppendLine();
            builder.AppendLine("\t{");
            builder.Append("\t\tDataTable table = SqlHelper.ExecuteDataTable(@\"select * from ").Append(tableName).
                AppendFormat(" where {0} = @id\",", primaryKey).AppendLine();
            builder.AppendLine("\t\t\tnew SqlParameter(\"@id\", id));");
            builder.AppendLine("\t\tif (table.Rows.Count <= 0)");
            builder.AppendLine("\t\t{");
            builder.AppendLine("\t\t\treturn null;");
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t\telse if (table.Rows.Count > 1)");
            builder.AppendLine("\t\t{");
            builder.Append("\t\t\tthrow new Exception(\"Fatal Error: Duplicated Id in Table ").Append(tableName).
                AppendLine("\");");
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t\telse");
            builder.AppendLine("\t\t{");
            builder.AppendLine("\t\t\tDataRow row = table.Rows[0];");
            builder.AppendLine("\t\t\treturn ToModel(row);");
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t}");
            builder.AppendLine();

            //5.GetAll()
            builder.Append("\tpublic static ").Append(modelClassName).AppendLine("[] GetAll()");
            builder.AppendLine("\t{");
            builder.Append("\t\tDataTable table = SqlHelper.ExecuteDataTable(\"select * from ").Append(tableName).
                AppendLine("\");");
            builder.Append("\t\t").Append(modelClassName).Append("[] ").Append(objName).Append(" = new ").
                Append(modelClassName).AppendLine("[table.Rows.Count];");
            builder.AppendLine("\t\tfor (int i = 0; i < table.Rows.Count; i++)");
            builder.AppendLine("\t\t{");
            builder.Append("\t\t\t").Append(objName).AppendLine("[i] = ToModel(table.Rows[i]);");
            builder.AppendLine("\t\t}");
            builder.Append("\t\treturn ").Append(objName).AppendLine(";");
            builder.AppendLine("\t}");
            builder.AppendLine();

            //6.ToModel
            builder.Append("\tprivate static ").Append(modelClassName).AppendLine(" ToModel(DataRow row)");
            builder.AppendLine("\t{");
            builder.AppendFormat("\t\t{0} {1} = new {0}();", modelClassName, objName).AppendLine();
            foreach (DataColumn col in table.Columns)
            {
                if (!col.AllowDBNull)
                {
                    builder.AppendFormat("\t\t{0}.{1} = ({2})row[\"{3}\"];", objName, FirstLetterUpper(col.ColumnName), 
                        GetColumnType(col), FirstLetterLower(col.ColumnName)).AppendLine();
                }
                else
                {
                    builder.AppendFormat("\t\t{0}.{1} = ({2})SqlHelper.FromDBValue(row[\"{3}\"]);",
                        objName, FirstLetterUpper(col.ColumnName), GetColumnType(col), FirstLetterLower(col.ColumnName)).AppendLine();
                }
            }

            builder.AppendFormat("\t\treturn {0};", objName).AppendLine();
            builder.AppendLine("\t}");

            builder.AppendLine("}");

            //如果输入命名空间
            if (!string.IsNullOrWhiteSpace(tb_namespace.Text))
            {
                builder = AddNameSpaceToCode(builder, nameSpace);
            }

            return builder.ToString();
        }


        /// <summary>
        /// 生成数据模型
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        private string GenModels(string tableName, string nameSpace)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("public class ").AppendLine(FirstLetterUpper(tableName) + "Model");
            builder.AppendLine("{");

            DataTable table = SqlHelper.ExecuteDataTable("select top 0 * from [" + tableName + "]");

            foreach (DataColumn col in table.Columns)
            {
                builder.Append("\tpublic ").Append(GetColumnType(col) + " ").Append(FirstLetterUpper(col.ColumnName)).AppendLine("{ get; set; }");
            }



            //结尾大括号
            builder.AppendLine("}");

            //如果输入命名空间
            if (!string.IsNullOrWhiteSpace(tb_namespace.Text))
            {
                builder = AddNameSpaceToCode(builder, nameSpace);
            }

            return builder.ToString(); 
        }

        private StringBuilder AddNameSpaceToCode(StringBuilder builder, string nameSpace)
        {
            builder = builder.InsertTabForEveryLine();
            builder.Insert(0, "{" + ClassEx.newLine);
            builder.Insert(0, "namespace " + nameSpace + ClassEx.newLine);
            builder.AppendLine("}");
            return builder;
        }

        /// <summary>
        /// 获取一个表的主键（字符串）（认定主键只有一列）
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private string GetPrimaryKey(DataTable table)
        {
            return table.PrimaryKey[0].ColumnName;
        }

        private string GetColumnType(DataColumn col)
        {
            return col.DataType + ((col.AllowDBNull && col.DataType.IsValueType) ? "?" : "");
        }

        private string FirstLetterUpper(string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        private string FirstLetterLower(string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        /// <summary>
        /// Insert中需要用到各列的名字，不含自增长的
        /// </summary>
        /// <param name="table"></param>
        /// <param name="withAt"></param>
        /// <returns></returns>
        private string GetColsString(DataTable table, bool withAt)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("(");
            int i;
            DataColumn col;
            DataColumn primaryKey = table.PrimaryKey[0];
            if (withAt)
            {
                for (i = 0; i < table.Columns.Count - 1; i++)
                {
                    col = table.Columns[i];
                    if (!col.Equals(primaryKey))
                    {
                        builder.Append("@").Append(table.Columns[i].ColumnName).Append(", "); 
                    }
                }
                col = table.Columns[i];
                if (!col.Equals(primaryKey))
                {
                    builder.Append("@").Append(table.Columns[i].ColumnName);
                }
            }
            else
            {
                for (i = 0; i < table.Columns.Count - 1; i++)
                {
                    col = table.Columns[i];
                    if (!col.Equals(primaryKey))
                    {
                        builder.Append(table.Columns[i].ColumnName).Append(", ");
                    }
                }
                col = table.Columns[i];
                if (!col.Equals(primaryKey))
                {
                    builder.Append(table.Columns[i].ColumnName);
                }
            }

            
            builder.Append(")");

            return builder.ToString();
        }

        private string GetColsStringForSet(DataTable table)
        {
            StringBuilder builder = new StringBuilder();
            int i;
            DataColumn col;
            DataColumn primaryKey = table.PrimaryKey[0];
            string colName;
            for (i = 0; i < table.Columns.Count - 1; i++)
            {
                col = table.Columns[i];
                colName = col.ColumnName;
                if (!col.Equals(primaryKey))
                {
                    builder.Append("\t\t\t\t").Append(colName).Append(" = @").Append(colName).AppendLine(",");
                }
            } 
            col = table.Columns[i];
            colName = col.ColumnName;
            if (!col.Equals(primaryKey))
            {
                builder.Append("\t\t\t\t").Append(colName).Append(" = @").Append(colName).AppendLine();
            }

            return builder.ToString();
        }

        private string GetParamsString(DataTable table, string objName, bool primaryKeyAtEnd)
        {
            StringBuilder builder = new StringBuilder();
            int i;
            DataColumn col;
            DataColumn primaryKey = table.PrimaryKey[0];
            for (i = 0; i < table.Columns.Count; i++)
            {
                col = table.Columns[i];
                if (!col.Equals(primaryKey))
                {
                    if (!col.AllowDBNull)
                    {
                        builder.AppendFormat("\t\t\t\tnew SqlParameter(\"@{0}\", {1}.{2}),",
                        col.ColumnName, objName, FirstLetterUpper(col.ColumnName)).AppendLine();
                    }
                    else
                    {
                        builder.AppendFormat("\t\t\t\tnew SqlParameter(\"@{0}\", SqlHelper.ToDBValue({1}.{2})),",
                        col.ColumnName, objName, FirstLetterUpper(col.ColumnName)).AppendLine();
                    }
                }
            }

            if (primaryKeyAtEnd)
            {
                builder.AppendFormat("\t\t\t\tnew SqlParameter(\"@{0}\", {1}.{2})",
                    primaryKey.ColumnName, objName, FirstLetterUpper(primaryKey.ColumnName)).AppendLine();
            }
            else
            {
                builder = builder.RemoveCRLFAtEnd().Remove(builder.Length - 1, 1).AppendLine();
            }

            return builder.ToString();
        }

        /// <summary>
        /// 点击事件：生成所有文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_genAll_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string dirPath = folderBrowserDialog.SelectedPath;
                //System.IO.Path.Combine()
                string connStr = tb_connStr.Text;
                string[] tableNames = GetTableNames(connStr);
                string nameSpace = tb_namespace.Text.Trim();

                //1.生成Model代码文件
                int i;
                string modelDirPath = Path.Combine(dirPath, "Model\\");
                if (!Directory.Exists(modelDirPath))
                {
                    Directory.CreateDirectory(modelDirPath);
                }

                for (i = 0; i < tableNames.Length; i++)
                {
                    string tableName = tableNames[i];
                    string modelStr = GenModels(tableName, nameSpace);
                    string className = FirstLetterUpper(tableName) + "Model.cs";
                    File.WriteAllText(Path.Combine(modelDirPath, className), modelStr, Encoding.Default);
                }

                //2.生成DAL的代码文件
                string DALDirPath = Path.Combine(dirPath, "DAL\\");
                if (!Directory.Exists(DALDirPath))
                {
                    Directory.CreateDirectory(DALDirPath);
                }

                for (i = 0; i < tableNames.Length; i++)
                {
                    string tableName = tableNames[i];
                    string DALStr = GenDAL(tableName, nameSpace);
                    string className = FirstLetterUpper(tableName) + "DAL.cs";
                    File.WriteAllText(Path.Combine(DALDirPath, className), DALStr, Encoding.Default);
                }

                MessageBox.Show("生成完成");
            }
        }

        private string[] GetTableNames(string connStr)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;
                }
                
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select table_name from information_schema.tables where table_type = 'base table'";
                    DataSet dataSet = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.FillSchema(dataSet, SchemaType.Source);
                    adapter.Fill(dataSet);
                    DataTable table = dataSet.Tables[0];
                    string[] tableNames = new string[table.Rows.Count];
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        tableNames[i] = (string)table.Rows[i]["table_name"];
                    }
                    return tableNames;
                }
            }
        }
    }
}
