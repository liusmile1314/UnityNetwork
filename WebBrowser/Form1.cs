using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WebBrowser
{
    
    public partial class FormObj : Form
    {
        public FormObj()
        {
            InitializeComponent();

            for (int i = 1; i <= 30; i++)
            {
                list.Add((20 * i - 1 ).ToString());
            }
        }


        List<string> list = new List<string>();

        private void button1_Click(object sender, EventArgs e)
        {
            //webBrowser1.Navigate("http://www.baidu.com/");
            webBrowser1.ScriptErrorsSuppressed = true; onceFlag = true; finish = true;
            webBrowser1.Navigate("http://group.cnblogs.com/topic/72424.html");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true; onceFlag = true; finish = true;
            //MessageBox.Show( webBrowser1.Document.GetElementById("thread_comment_list").GetElementsByTagName("li").Count.ToString());
            
        }

        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();
        private void button3_Click(object sender, EventArgs e)
        {

            timer1.Interval = Convert.ToInt32(textBox1.Text);
            IntPtr pHandle = GetCurrentProcess();
            SetProcessWorkingSetSize(pHandle, -1, -1);

            //MessageBox.Show(webBrowser1.Document.Title);
            //webBrowser1.Document.GetElementById("kw").SetAttribute("value","hi");
            //webBrowser1.Document.GetElementById("su").InvokeMember("click");

            //webBrowser1.Document.GetElementById("txtContent").SetAttribute("value", "hi");
            //webBrowser1.Document.GetElementById("app_ing").InvokeMember("click");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string info = webBrowser1.Document.GetElementById("thread_comment_list").GetElementsByTagName("li").Count.ToString();
            //if (info == "19" || info == "39" || info == "59" || info == "79")
            //if (info == "24")


            //string count = webBrowser1.Document.GetElementById("panel_comment").NextSibling == null ? "null" : webBrowser1.Document.GetElementById("panel_comment").NextSibling.ToString(); //Children.GetElementsByName("a").Count.ToString();
            label1.Text = info + " " + (Convert.ToInt32(info) + 50*Convert.ToInt32(textBox2.Text));//+ count;
           
            //if(list.Contains(info))
            if(true)
            {
                if (onceFlag)
                {
                    webBrowser1.Document.GetElementById("txtContent").SetAttribute("value", "呵呵");//
                    webBrowser1.Document.GetElementById("btnReply").InvokeMember("click");
                }
                onceFlag = false;
            }
            else
            {
                finish = true;
            }
            
        }

        public bool onceFlag = false,finish =false;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (finish)
            {
                onceFlag = true;
                webBrowser1.ScriptErrorsSuppressed = true;
                webBrowser1.Navigate("http://group.cnblogs.com/topic/72424-1.html");
                finish = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
            }
            else
            {
                timer1.Start();
            }
        }

        private void FormObj_Load(object sender, EventArgs e)
        {

        }
    }
}
