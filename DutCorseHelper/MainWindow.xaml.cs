using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace DutCorseHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            progressBar1.Value = 0;
            string username = txtUser.Text;
            string pwd = txtPwd.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pwd))
            {
                wc_MessageRecieved("学号和密码均不能为空", EventArgs.Empty);
                return;
            }
            int a;
            if (!int.TryParse(username, out a))
            {
                wc_MessageRecieved("输入的学号不正确", EventArgs.Empty);
                return;
            }


            Thread th1 = new Thread(() =>
            {
                try
                {
                    this.Dispatcher.Invoke((Action)(() => { progressBar1.IsIndeterminate = true; }));
                    wc_MessageRecieved("正在读取数据文件...", EventArgs.Empty);
                    DataReader dr = new DataReader();

                    if (dr.Corses.Count <= 0)//||
                    // string.IsNullOrEmpty(dr.StuNum)||
                    // string.IsNullOrEmpty(dr.Password))
                    {
                        throw new Exception("数据文件格式错误！");
                    }
                    wc_MessageRecieved("正在获取选课服务器列表...", EventArgs.Empty);
                    ServerAnalyzer sa = new ServerAnalyzer();
                    var svrs=sa.GetServerList();
                   
                    WebController wc = new WebController();
                    
                    //wc.UserName = "201292012";
                    //wc.Password = "19001X";
                    wc.UserName = username;
                    wc.Password = pwd;
                    //wc.Courses = dr.Courses;
                    //wc.SeqNos = dr.SeqNos;
                    wc.Corses = dr.Corses;
                    wc.StatusChanged += new EventHandler(wc_StatusChanged);
                    wc.MessageRecieved += new EventHandler(wc_MessageRecieved);
                    bool succeed = false;
                    for (int i=0;i<svrs.Count;i++)
                    {
                        var host = svrs[i];
                        wc_MessageRecieved(string.Format("正在连接服务器({0})...",i+1), EventArgs.Empty);
                       // string host = sa.GetUsableServer();
                        //if (string.IsNullOrEmpty(host))
                       // {
                       //     throw new Exception("网络忙！请稍后重试！");
                        //}
                        if (!sa.TestServer(host))
                        {
                            continue;
                        }
                        wc.Host = host;
                        if (wc.BeginRequest())
                        {

                            succeed = true;
                            break;
                        }
                       
                    }
                    if (!succeed)
                    {
                        wc_MessageRecieved("选课失败", EventArgs.Empty);
                    }

                }
                catch (Exception ex)
                {
                    wc_MessageRecieved(ex.Message, EventArgs.Empty);
                }
                finally
                {
                    this.Dispatcher.Invoke((Action)(() => { progressBar1.IsIndeterminate = false; }));
                }
            });

            th1.Start();

            progressBar1.Value = 100;
        }

        void wc_MessageRecieved(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(
                 (Action)(() =>
                     {
                         try
                         {

                             txtResult.Text += "[" + DateTime.Now.ToLongTimeString() + "]"
                                 + (string)sender + Environment.NewLine;
                             if (txtResult.Text.Length > 3000)
                             {
                                 txtResult.Text = txtResult.Text.Substring(txtResult.Text.Length - 3000);
                             }
                             txtResult.ScrollToEnd();

                         }
                         catch
                         {
                         }

                     }));

        }


        void wc_StatusChanged(object sender, EventArgs e)
        {
            int status = (int)sender;
            string result = "";
            string[] statusTexts = 
            { 
                "正在连接...", 
                "正在登录...", 
                "正在选课...", 
                "选课成功..." 
            };
            string[] errorTexts =
            {
                "连接超时！",
                "登录失败！",
                "选课失败！"
            };
            if (status > 0)
            {
                result = statusTexts[status - 1];
            }
            else
            {
                result = errorTexts[-status - 1];
            }

            //txtStatus.Dispatcher.Invoke(
            //   (Action)(()=>
            //{
            //    txtStatus.Text = result;
            //}));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtResult.Text += "◇◇◇大连理工大学选课辅助工具◇◇◇" + Environment.NewLine +
                "ver 1.4 " + "(c) socona, duttic  me@socona.me" + Environment.NewLine +
                "【注意】工具作者不对使用本工具的造成任何后果负责" + Environment.NewLine +
                "【使用方法】\r\n\t修改data.txt文件，每一行的格式为" + Environment.NewLine +
                "\t【课程号】【空格】【课序号】" + Environment.NewLine + "\t每行一个课程" + Environment.NewLine + "";

            try
            {
                UserReader ur = new UserReader();
                txtUser.Text = ur.StuNum;
                txtPwd.Password = ur.Password;
            }
            catch (Exception)
            { }
        }


    }
}
