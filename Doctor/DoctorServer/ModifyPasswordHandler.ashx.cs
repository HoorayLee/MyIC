using Doctor.DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DoctorServer
{
    /// <summary>
    /// ModifyPasswordHandler 的摘要说明
    /// 修改密码
    /// </summary>
    public class ModifyPasswordHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            StreamReader reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
            string requestStr = reader.ReadToEnd();

            JObject jObj = JObject.Parse(requestStr);
            long doc_id = (long)jObj.Property("doc_id");
            string oldPwd = (string)jObj.Property("old_password");
            string newPwd = (string)jObj.Property("new_password");

            string state = null;
            DoctorDAL.ModifyPassword(doc_id, oldPwd, newPwd, ref state);

            //返回处理结果（成功，失败，旧密码错误）
            JObject jResponseObj = new JObject();
            jResponseObj.Add("state", state);

            byte[] buf = Encoding.UTF8.GetBytes(jResponseObj.ToString());
            context.Response.OutputStream.Write(buf, 0, buf.Length);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}