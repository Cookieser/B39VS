using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
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

           


            //Default settings
            comboBox1.Text = "COM1";
            comboBox2.Text = "115200";
            comboBox3.Text = "8";
            comboBox4.Text = "None";
            comboBox5.Text = "1";


            // Add the list of baud rates
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());

            // Add the list of baud rates
            for (int i = 300; i <= 38400; i = i * 2)
            {
                comboBox2.Items.Add(i.ToString());
            }



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

        private void button3_Click(object sender, EventArgs e)
        {
             
            
            try
            {
                //将可能产生异常的代码放置在try块中
                //根据当前串口属性来判断是否打开
                if (serialPort1.IsOpen)
                {
                    //串口已经处于打开状态
                    serialPort1.Close();    //关闭串口
                    button3.Text = "打开串口";
                    button3.BackColor = Color.ForestGreen;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    comboBox5.Enabled = true;
                    textReceive.Text = "";  //清空接收区
                    textSend.Text = "";     //清空发送区
                }
                else
                {
                    //串口已经处于关闭状态，则设置好串口属性后打开
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.DataBits = Convert.ToInt16(comboBox3.Text);

                    if (comboBox4.Text.Equals("None"))
                        serialPort1.Parity = System.IO.Ports.Parity.None;
                    else if (comboBox4.Text.Equals("Odd"))
                        serialPort1.Parity = System.IO.Ports.Parity.Odd;
                    else if (comboBox4.Text.Equals("Even"))
                        serialPort1.Parity = System.IO.Ports.Parity.Even;
                    else if (comboBox4.Text.Equals("Mark"))
                        serialPort1.Parity = System.IO.Ports.Parity.Mark;
                    else if (comboBox4.Text.Equals("Space"))
                        serialPort1.Parity = System.IO.Ports.Parity.Space;

                    if (comboBox5.Text.Equals("1"))
                        serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    else if (comboBox5.Text.Equals("1.5"))
                        serialPort1.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    else if (comboBox5.Text.Equals("2"))
                        serialPort1.StopBits = System.IO.Ports.StopBits.Two;

                    serialPort1.Open();     //打开串口
                    button3.Text = "关闭串口";
                    button3.BackColor = Color.Firebrick;
                }
            }
            catch (Exception ex)
            {
                //捕获可能发生的异常并进行处理

                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button3.Text = "打开串口";
                button3.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    //串口处于开启状态，将发送区文本发送
                    serialPort1.Write(textSend.Text);
                }
            }
            catch (Exception ex)
            {
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button1.Text = "打开串口";
                button1.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
            }


        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //因为要访问UI资源，所以需要使用invoke方式同步ui
                this.Invoke((EventHandler)(delegate
                {
                    textReceive.AppendText(serialPort1.ReadExisting());
                }
                   )
                );

            }
            catch (Exception ex)
            {
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(ex.Message);

            }
        }
    }
}
