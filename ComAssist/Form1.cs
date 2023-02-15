using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Helloworld
{

    /// <summary>

    /// 各种语言的枚举

    /// </summary>

    public enum LanguageType : int

    {

        /// <summary>

        /// 中文-简体

        /// </summary>

        ZH_HANS = 0,

     

        EN_GB = 1



    }
    



    public partial class Form1 : Form
    {
        private string _directory = AppDomain.CurrentDomain.BaseDirectory; //当前程序所属路径

        public static LanguageType Language { get; set; }

        public static string LanguageName

        {

            get

            {

                switch (Form1.Language)

                {

                    case LanguageType.EN_GB:

                        return "en-GB";

                    case LanguageType.ZH_HANS:

                        return "zh-Hans";

                    default:

                        return "zh-Hans";

                }

            }

        }

        public Form1()

        {

            string path = Path.Combine(_directory, "LanguageConfig.txt");

            if (!File.Exists(path))

                Form1.Language = LanguageType.ZH_HANS;

            else

            {

                StreamReader sr = new StreamReader(path, Encoding.Default);

                String line;

                while ((line = sr.ReadLine()) != null)

                {

                    line = line.ToString();

                    if (line == "zh-Hans")

                        Form1.Language = LanguageType.ZH_HANS;

                    else if (line == "en-GB")

                        Form1.Language = LanguageType.EN_GB;

                    else

                        Form1.Language = LanguageType.ZH_HANS;

                }

                sr.Close();

            }



            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Form1.LanguageName);

            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

            InitializeComponent();



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i;
            for (i = 300; i <= 38400; i = i*2)
            {
                comboBox2.Items.Add(i.ToString());  //添加波特率列表
            }

            //批量添加波特率列表
            //string[] baud = { "43000", "56000", "57600", "115200", "128000", "230400", "256000", "460800" };
            //comboBox2.Items.AddRange(baud);

            //设置默认值
            comboBox1.Text = "COM1";
            comboBox2.Text = "115200";
            comboBox3.Text = "8";
            comboBox4.Text = "None";
            comboBox5.Text = "1";
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1.Language = LanguageType.ZH_HANS;

            byte[] data = Encoding.Default.GetBytes(Form1.LanguageName);



            FileStream fs = new FileStream(Path.Combine(_directory, "LanguageConfig.txt"), FileMode.Create);

            fs.Write(data, 0, data.Length);

            //清空缓冲区、关闭流

            fs.Flush();

            fs.Close();

            Application.Restart();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.Language = LanguageType.EN_GB;

            byte[] data = Encoding.Default.GetBytes(Form1.LanguageName);



            FileStream fs = new FileStream(Path.Combine(_directory, "LanguageConfig.txt"), FileMode.Create);

            fs.Write(data, 0, data.Length);

            //清空缓冲区、关闭流

            fs.Flush();
            Application.Restart();


        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
