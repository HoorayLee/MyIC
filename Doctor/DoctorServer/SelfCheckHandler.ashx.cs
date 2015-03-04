﻿using Doctor.DAL;
using Doctor.Model;
using Newtonsoft.Json;
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
    /// SelfCheckHandler 的摘要说明
    /// </summary>
    public class SelfCheckHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            StreamReader reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
            string requestStr = reader.ReadToEnd();

            //返回所有自检结果，其中本地区的置前
            if ("ListAll".Equals(requestStr))
            {
                ////获取IP地理信息
                //string hostIP = context.Request.UserHostAddress;
                //IPRecord ip = HttpHelper.GetIPRecord(hostIP);

                ////通过IP地理信息获取所在区域的编号
                //RecordModel[] recordModels = null;
                //if (string.IsNullOrEmpty(ip.Province))
                //{
                //    //返回所有的自检
                //    recordModels = RecordDAL.GetAll();
                //}
                //else if (string.IsNullOrEmpty(ip.City))
                //{
                //    //返回指定省份的自检
                //    recordModels = RecordDAL.GetAllProvinceFirst(ip.Province);
                //}
                //else if (string.IsNullOrEmpty(ip.District))
                //{
                //    //返回指定城市的自检
                //    recordModels = RecordDAL.GetAllCityFirst(ip.City);
                //}
                //else
                //{
                //    //返回指定区域的自检
                //    recordModels = RecordDAL.GetAllAreaFirst(ip.District);
                //}

                RecordModel[] recordModels = RecordDAL.GetAll();
                JObject jObj = new JObject();
                jObj.Add("count", recordModels.Length);
                JArray jArr = new JArray();
                foreach (RecordModel recordModel in recordModels)
                {
                    jArr.Add(JsonConvert.SerializeObject(recordModel));
                }
                jObj.Add("content", jArr);

                byte[] bytes = Encoding.UTF8.GetBytes(jObj.ToString());
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                //返回指定id对应的自检图片
                long id = long.Parse(requestStr);
                PhotoModel[] photos = PhotoDAL.GetAllByRecordId(id);
                JObject jObj = new JObject();
                jObj.Add("count", photos.Length);
                JArray jArr = new JArray();
                foreach (PhotoModel photo in photos)
                {
                    jArr.Add(JsonConvert.SerializeObject(photo));
                }
                jObj.Add("content", jArr);

                byte[] bytes = Encoding.UTF8.GetBytes(jObj.ToString());
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
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