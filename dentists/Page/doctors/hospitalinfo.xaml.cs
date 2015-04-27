using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using dentists.helper;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using Model;
using dentists.Page.im.IMdata;
using dentists.Data.IMdata;
using System.Windows.Media.Imaging;

namespace dentists.Page.doctors
{
    public partial class hospitalinfo : UserControl
    {
        private ObservableCollection<ClassList> doclist;
        private PhoneApplicationPage map;
        public hospitalinfo(string hospital, string citycode, PhoneApplicationPage a)
        {
            this.map = a;
            InitializeComponent();
            this.Title.Text = hospital;
            doclist = new ObservableCollection<ClassList>();
            getdoclist(hospital, citycode);
        }

        private void getdoclist(string hospital, string citycode)
        {
            if (citycode != "")
            {
                if (hospital != "我的位置")
                {
                    JObject jO = new JObject();
                    jO.Add("name", hospital);
                    jO.Add("citycode", citycode);
                    var task = Reqdoc.postreq("http://121.42.136.178:19710/DoctorListHandler.ashx", jO.ToString());
                        task.ContinueWith(async (a) =>
                        {
                            string b = await a;
                            if (b != "error")
                            {
                                JObject jo = JObject.Parse(b);
                                JArray jarray = jo.GetValue("content") as JArray;
                                int num = (int)jo.GetValue("count");
                                if (num > 0)
                                {
                                    for (int i = 0; i < num; i++)
                                    {
                                        ClassList one = new ClassList();
                                        one.hospital = (string)jarray[i]["hospital"];
                                        one.id = (string)jarray[i]["id"];
                                        one.name = (string)jarray[i]["name"];
                                        this.doclist.Add(one);
                                    }

                                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        this.Doclist.ItemTemplate = this.template;
                                        this.loading.Visibility = System.Windows.Visibility.Collapsed;
                                        this.Doclist.Visibility = System.Windows.Visibility.Visible;
                                        this.Doclist.Items.Clear();
                                        //this.Doclist.DisplayMemberPath = "name";
                                        this.Doclist.Foreground = new SolidColorBrush(Colors.Black);
                                        this.Doclist.ItemsSource = this.doclist;
                                    });
                                }
                                else
                                {
                                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                                    {
                                        this.loading.Text = "并没有医生";
                                    });
                                }
                            }
                            else 
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    this.loading.Text = "网络环境不好，稍后重试";
                                });
                            }
                        }
                    );
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        this.loading.Text = "";
                    });
                }
            }
            else 
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.loading.Text = "网络环境不好，稍后重试";
                });
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox box = sender as ListBox;
            ClassList one = box.SelectedItem as ClassList;
            IsolatedStorageSettings.ApplicationSettings[one.id] = new WordList(new List<Msg>());
            DoctorList doctorlist = IsolatedStorageSettings.ApplicationSettings["DoctorList"] as DoctorList;
            ClassList doc1 = new ClassList()
            {
                name = one.name,
                hospital = one.hospital,
                img = new BitmapImage(new Uri("/asset/doc.png", UriKind.RelativeOrAbsolute)),
                id = one.id
            };
            this.Doclist.ItemTemplate = null;
            doctorlist.doclist.Add(doc1);
            IsolatedStorageSettings.ApplicationSettings["s1"] = doc1;
            IsolatedStorageSettings.ApplicationSettings[one.id] = new WordList(new List<Msg>());
            this.map.NavigationService.Navigate(new Uri("/Page/im/ImPage.xaml", UriKind.Relative));
        }
    }
}
