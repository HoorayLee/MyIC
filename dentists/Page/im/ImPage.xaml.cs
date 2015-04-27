using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using dentists.Page.doctors;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using IMClient_WinForm;
using Model;
using dentists.Data.IMdata;
using dentists.Page.im.IMdata;

namespace dentists.Page.im
{
    public partial class ImPage : PhoneApplicationPage
    {
        ApplicationBarIconButton appBarButton;
        public string id;
        public ImPage()
        {
            InitializeComponent();
            appBarButton = new ApplicationBarIconButton(new Uri("/asset/doc.png", UriKind.Relative));
            appBarButton.Text = "send message";
            appBarButton.Click += appBarButton_Click;
            ClassList one = (ClassList)IsolatedStorageSettings.ApplicationSettings["s1"];
            this.name.Text = one.name;
            this.id = one.id;
            MyIMClient.running = this;
            WordList maglist = IsolatedStorageSettings.ApplicationSettings[one.id] as WordList;
            int num = 0;
            if (maglist.wordnum != 0)
            {
                num = maglist.wordnum;
            }
            else if (maglist.wordlist.Count < 5)
            {
                num = maglist.wordlist.Count;
            }
            else
            {
                num = 5;
            }

            int begin = maglist.wordlist.Count - num;
            DoctorList doclist = IsolatedStorageSettings.ApplicationSettings["DoctorList"] as DoctorList;

            for (int i = 0; i < num; i++)
            {
                string form = maglist.wordlist[begin + i].From;
                if(form.Equals(this.name))
                {
                    MessageBox box = new MessageBox();
                    box.message.text.Text = maglist.wordlist[begin + i].Content;
                    box.time.text.Text = maglist.wordlist[begin + i].Time.ToString("hh:mm");
                    this.imstack.Children.Add(box);
                }
                else
                {
                    Remassage box = new Remassage();
                    box.message.text.Text = maglist.wordlist[begin + i].Content;
                    box.time.text.Text = maglist.wordlist[begin + i].Time.ToString("hh:mm");
                    this.imstack.Children.Add(box);
                    maglist.wordnum--;
                    doclist.totalunread--;
                }
            }
        }

        public ImPage(ClassList a)
        {
            InitializeComponent();
            appBarButton = new ApplicationBarIconButton(new Uri("/asset/doc.png", UriKind.Relative));
            appBarButton.Text = "send message";
            appBarButton.Click += appBarButton_Click;
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (ApplicationBar == null) 
            {
                create_applicationbar();
            }
        }
        private void create_applicationbar() 
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Buttons.Add(appBarButton);
            if (this.message.Text == "") 
            {
                appBarButton.IsEnabled = false;
            }
        }

        private void appBarButton_Click(object sender, EventArgs e)
        {
            MyIMClient.SendMsg(new Msg(MyIMClient.Username,this.id, this.message.Text));
            uploadpage(0,this.message.Text);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            MyIMClient.running = null;
        }

        public void uploadpage(int state,string a) 
        {
            switch (state) 
            {
                case 0:
                    MessageBox box = new MessageBox();
                    box.message.text.Text = a;
                    box.time.text.Text = DateTime.Now.ToString("hh:mm");
                    this.imstack.Children.Add(box);
                    this.Focus();
                    this.message.Text = "";
                    this.message.Focus();
                    break;
                case 1:
                    Remassage message = new Remassage();
                    message.message.text.Text = a;
                    message.time.text.Text = DateTime.Now.ToString("hh:mm");
                    this.imstack.Children.Add(message);
                    break;
            }
            if(this.imstack.Children.Count>20)
            {
                for(int i=0;i<5;i++)
                {
                    this.imstack.Children.RemoveAt(0);
                }
            }
            this.imscrollview.ScrollToVerticalOffset(this.imscrollview.ActualHeight + this.imscrollview.ViewportHeight);
            //Imbox imbox = new Imbox();
            //imbox.text.Text = this.message.Text;
            //imbox.background.Background = new SolidColorBrush(Colors.LightGray); 
            //TimeBox timebox = new TimeBox();
            //timebox.text.Text = DateTime.Now.ToString("hh:mm");
            //this.imstack.Children.Add(timebox);
            //this.imstack.Children.Add(imbox);
        }

        private void message_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.message.Text == "")
            {
                appBarButton.IsEnabled = false;
            }
            else if(appBarButton.IsEnabled != true)
            {
                appBarButton.IsEnabled = true;
            }
        }

        private void message_LostFocus(object sender, RoutedEventArgs e)
        {
            ApplicationBar = null;
        }
    }
}