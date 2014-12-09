using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PanoramaApp1.Resources;

namespace PanoramaApp1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _Recommend = "No recommendation now";
        private string _DetectResult = "No record now";
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// ItemViewModel 对象的集合。
        /// </summary>
        /// 
   
        public ObservableCollection<ItemViewModel> Items { get; private set; }
        
        /// <summary>
        /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值
        /// </summary>
        /// <returns></returns>
        public string DetectResult
        {
            get
            {
                return _DetectResult;
            }
            set
            {
                if (value != _DetectResult)
                {
                    _DetectResult = value;
                    NotifyPropertyChanged("DetectResult");
                }
            }
        }

       
        /// <summary>
        /// 示例 ViewModel 属性；此属性在视图中用于使用绑定显示它的值
        /// </summary>
        /// <returns></returns>
        public string Recommend
        {
            get
            {
                return _Recommend;
            }
            set
            {
                if (value != _Recommend)
                {
                    _Recommend = value;
                    NotifyPropertyChanged("Recommend");
                }
            }
        }

        /// <summary>
        /// 返回本地化字符串的示例属性
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建一些 ItemViewModel 对象并将其添加到 Items 集合中。
        /// </summary>
        public void LoadData()
        {
            // 示例数据；替换为实际数据
            this.Items.Add(new ItemViewModel() { LineOne = "联系人一", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            this.Items.Add(new ItemViewModel() { LineOne = "联系人二", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            this.Items.Add(new ItemViewModel() { LineOne = "联系人三", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            this.Items.Add(new ItemViewModel() { LineOne = "联系人四", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            this.Items.Add(new ItemViewModel() { LineOne = "联系人五", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
           
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}