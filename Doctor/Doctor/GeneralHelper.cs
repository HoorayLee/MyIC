﻿using Doctor.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor
{
    class GeneralHelper
    {
        public static string DownloadPicFolder = Environment.CurrentDirectory + "\\DownloadFiles\\";
        public static string SelfCheckCache = Environment.CurrentDirectory + "\\SelfCheck.json";
        public static string ProvincesFileName = Environment.CurrentDirectory + "\\Provinces.json";
        public static string CitiesFileName = Environment.CurrentDirectory + "\\Cities.json";
        public static string AreasFileName = Environment.CurrentDirectory + "\\Areas.json";

        private static Hat_provinceModel[] provinces;
        private static Hat_cityModel[] cities;
        private static Hat_areaModel[] areas;

        public static Hat_provinceModel[] Provinces { get { return provinces; } }
        public static Hat_cityModel[] Cities { get { return cities; } }
        public static Hat_areaModel[] Areas { get { return areas; } }

        /// <summary>
        /// 通过省找到底下的城市
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        public static Hat_cityModel[] GetCitiesByProvince(Hat_provinceModel province)
        {
            LoadLocationData();

            List<Hat_cityModel> cityList = new List<Hat_cityModel>();
            string provinceID = province.ProvinceID;
            foreach (var city in cities)
            {
                if (city.Father.Equals(provinceID))
                {
                    cityList.Add(city);
                }
            }
            return cityList.ToArray();
        }

        /// <summary>
        /// 通过市找到底下的区县
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public static Hat_areaModel[] GetAreasByCity(Hat_cityModel city)
        {
            LoadLocationData();

            List<Hat_areaModel> areaList = new List<Hat_areaModel>();
            string cityID = city.CityID;
            foreach (var area in areas)
            {
                if (area.Father.Equals(cityID))
                {
                    areaList.Add(area);
                }
            }
            return areaList.ToArray();
        }

        public static Image GetPhoto(string fileName)
        {
            return Image.FromFile(Path.Combine(DownloadPicFolder, fileName));
        }

        /// <summary>
        /// 根据地区的主键id获取区域的名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetAreaName(int id)
        {
            Hat_areaModel area = areas[id - 1];

            //找到所在市
            var fatherCity = from city in cities
                             where city.CityID == area.Father
                             select city;

            //找到所在省
            var fatherProvince = from province in provinces
                                 where province.ProvinceID == fatherCity.ElementAt(0).Father
                                 select province;

            return fatherProvince.ElementAt(0).Province + fatherCity.ElementAt(0).City + areas[id - 1].Area;
        }


        /// <summary>
        /// 加载省市区数据
        /// </summary>
        public static void LoadLocationData()
        {
            //如果已经加载则不加载
            if (null != provinces)
            {
                return;
            }

            //优先从文件中读取
            if (File.Exists(ProvincesFileName))
            {
                //本地读取
                //省
                using (FileStream stream = new FileStream(ProvincesFileName, FileMode.Open))
                {
                    JArray jArr = JArray.Parse(stream.ToUTF8String());
                    int nbProvinces = jArr.Count;
                    provinces = new Hat_provinceModel[nbProvinces];
                    for (int i = 0; i < nbProvinces; i++)
                    {
                        provinces[i] = JsonConvert.DeserializeObject<Hat_provinceModel>(jArr[i].ToString());
                    }
                }

                //市
                using (FileStream stream = new FileStream(CitiesFileName, FileMode.Open))
                {
                    JArray jArr = JArray.Parse(stream.ToUTF8String());
                    int nbCities = jArr.Count;
                    cities = new Hat_cityModel[nbCities];
                    for (int i = 0; i < nbCities; i++)
                    {
                        cities[i] = JsonConvert.DeserializeObject<Hat_cityModel>(jArr[i].ToString());
                    }
                }

                //县区
                using (FileStream stream = new FileStream(AreasFileName, FileMode.Open))
                {
                    JArray jArr = JArray.Parse(stream.ToUTF8String());
                    int nbAreas = jArr.Count;
                    areas = new Hat_areaModel[nbAreas];
                    for (int i = 0; i < nbAreas; i++)
                    {
                        areas[i] = JsonConvert.DeserializeObject<Hat_areaModel>(jArr[i].ToString());
                    }
                }
            }
            //else
            //{
            //    //网络读取并保存到本地
            //    provinces = Hat_provinceDAL.GetAll();
            //    JArray jArr = new JArray();
            //    foreach (var province in provinces)
            //    {
            //        jArr.Add(JsonConvert.SerializeObject(province));
            //    }

            //    using (FileStream stream = new FileStream(ProvincesFileName, FileMode.Create))
            //    {
            //        string aa = jArr.ToString();
            //        stream.WriteUTF8String(jArr.ToString());
            //    }

            //    cities = Hat_cityDAL.GetAll();
            //    JArray jArr2 = new JArray();
            //    foreach (var city in cities)
            //    {
            //        jArr2.Add(JsonConvert.SerializeObject(city));
            //    }

            //    using (FileStream stream = new FileStream(CitiesFileName, FileMode.Create))
            //    {
            //        stream.WriteUTF8String(jArr2.ToString());
            //    }

            //    areas = Hat_areaDAL.GetAll();
            //    JArray jArr3 = new JArray();
            //    foreach (var area in areas)
            //    {
            //        jArr3.Add(JsonConvert.SerializeObject(area));
            //    }

            //    using (FileStream stream = new FileStream(AreasFileName, FileMode.Create))
            //    {
            //        stream.WriteUTF8String(jArr3.ToString());
            //    }
            //}
        }

        /// <summary>
        /// 通过区域的主键返回区域的编号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetAreaId(int id)
        {
            return areas[id - 1].AreaID;
        }
    }
}
