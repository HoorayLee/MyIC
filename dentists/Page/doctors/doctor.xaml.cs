using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using dentists.Page.doctors;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using Com.AMap.Api.Maps;
using dentists.Data.IMdata;
namespace dentists.Page.doctors
{
    public partial class docotor : PhoneApplicationPage
    {
        public docotor()
        {
            InitializeComponent();
            List<ClassList> doctor = new List<ClassList>();
            DoctorList doctorlist = IsolatedStorageSettings.ApplicationSettings["DoctorList"] as DoctorList;
            foreach(ClassList a in doctorlist.doclist)
            {
                doctor.Add(a);
            }
            
            lls.ItemsSource = Group<ClassList>.GetTypeGroups(doctor);
        }
        public class Group<T> : List<T>
        {
            public Group(string name, IEnumerable<T> items) : base(items)
            {
                this.Title = name;
            }

            public string Title
            {
                get;
                set;
            }

            public static List<Group<ClassList>> GetTypeGroups(List<ClassList> classList)
            {
                IEnumerable<ClassList> typeList = classList;
                return GetItemGroups(typeList, c => c.hospital);
            }
            public static List<Group<T>> GetItemGroups<T>(IEnumerable<T> itemList, Func<T, string> getKeyFunc)
            {
                IEnumerable<Group<T>> groupList = from item in itemList
                                                  group item by getKeyFunc(item) into g
                                                  select new Group<T>(g.Key, g);
                return groupList.ToList();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) 
        {
            this.lls.SelectedItem = null;
        }

        private void lls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.lls.SelectedItem != null)
            {
                ClassList one = (ClassList)e.AddedItems[0];
                IsolatedStorageSettings.ApplicationSettings["s1"] = one;
                NavigationService.Navigate(new Uri("/Page/im/ImPage.xaml", UriKind.Relative));
            }
        }

        private void Button_Click_Map(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(new Uri("/Page/doctors/DoctorMap.xaml", UriKind.Relative));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

