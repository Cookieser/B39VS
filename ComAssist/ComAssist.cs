using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Helloworld
{

    public partial class ComAssist : Form
    {
        
       /*
        全局变量声明
        */
        private StringBuilder sb = new StringBuilder();     //为了避免在接收处理函数中反复调用，依然声明为一个全局变量
        private DateTime current_time = new DateTime();    //为了避免在接收处理函数中反复调用，依然声明为一个全局变量
        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        private long receive_count = 0; //接收字节计数, 作用相当于全局变量


        public ComAssist()

        {
            //初始化
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {


            /* 组件初始化使能设置 */
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton4.Enabled = false;
            radioButton3.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            numericUpDown1.Enabled = false;
            textReceive.Enabled = false;
            textSend.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;


            // Default settings

            comboBox1.Text = "COM1";
            comboBox2.Text = "115200";
            comboBox3.Text = "8";
            comboBox4.Text = "None";
            comboBox5.Text = "1";

            numericUpDown1.Maximum = 60000;
            numericUpDown1.Value = 1000;
            
            trackBar1.Value = 100;
            trackBar2.Value = 1;

            /*
             * Add some lists of constituent parts
             */
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames()); // Add the list of PortNames


            for (int i = 300; i <= 38400; i = i * 2)
            {
                comboBox2.Items.Add(i.ToString());// Add the list of baud rates
            }
            
            for (int i = 5; i <= 8; i++)
            {
                comboBox3.Items.Add(i.ToString());// 数据位
            }
            comboBox4.Items.Add("None");
            comboBox4.Items.Add("Even");
            comboBox4.Items.Add("Mark");
            comboBox4.Items.Add("Odd");
            comboBox4.Items.Add("Space");   // 校验位

            for (double i = 1; i <= 2; i=i+0.5)
            {
                comboBox5.Items.Add(i.ToString()); //停止位
            }


        }

        /*
         * Component concrete behavior and corresponding
         */




        // Panal Setting

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel2.ClientRectangle,
            Color.White, 1, ButtonBorderStyle.Solid, //左边
            Color.White, 1, ButtonBorderStyle.Solid, //上边
            Color.DimGray, 1, ButtonBorderStyle.Solid, //右边
            Color.DimGray, 1, ButtonBorderStyle.Solid);//底边
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel3.ClientRectangle,
            Color.White, 1, ButtonBorderStyle.Solid, //左边
            Color.White, 1, ButtonBorderStyle.Solid, //上边
            Color.DimGray, 1, ButtonBorderStyle.Solid, //右边
            Color.DimGray, 1, ButtonBorderStyle.Solid);//底边
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel4.ClientRectangle,
            Color.White, 1, ButtonBorderStyle.Solid, //左边
            Color.White, 1, ButtonBorderStyle.Solid, //上边
            Color.DimGray, 1, ButtonBorderStyle.Solid, //右边
            Color.DimGray, 1, ButtonBorderStyle.Solid);//底边
        }
        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }


        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel7.ClientRectangle,
            Color.White, 1, ButtonBorderStyle.Solid, //左边
            Color.White, 1, ButtonBorderStyle.Solid, //上边
            Color.DimGray, 1, ButtonBorderStyle.Solid, //右边
            Color.DimGray, 1, ButtonBorderStyle.Solid);//底边
        }
        // Setting of button

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
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


                    radioButton1.Enabled = true;
                    radioButton2.Enabled = true;
                    radioButton4.Enabled = true;
                    radioButton3.Enabled = true;
                    checkBox1.Enabled = true;
                    checkBox2.Enabled = true;
                    checkBox3.Enabled = true;
                    numericUpDown1.Enabled = true;
                    textReceive.Enabled = true;
                    textSend.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;



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
            /*
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
            */

            byte[] temp = new byte[1];
            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    int num = 0;   //获取本次发送字节数
                                   //串口处于开启状态，将发送区文本发送

                    //判断发送模式
                    if (radioButton3.Checked)
                    {
                        //以HEX模式发送
                        //首先需要用正则表达式将用户输入字符中的十六进制字符匹配出来
                        string buf = textSend.Text;
                        string pattern = @"\s";
                        string replacement = "";
                        Regex rgx = new Regex(pattern);
                        string send_data = rgx.Replace(buf, replacement);


                        //不发送新行
                        num = (send_data.Length - send_data.Length % 2) / 2;
                        for (int i = 0; i < num; i++)
                        {
                            temp[0] = Convert.ToByte(send_data.Substring(i * 2, 2), 16);
                            serialPort1.Write(temp, 0, 1);  //循环发送
                        }
                        //如果用户输入的字符是奇数，则单独处理
                        if (send_data.Length % 2 != 0)
                        {
                            temp[0] = Convert.ToByte(send_data.Substring(textSend.Text.Length - 1, 1), 16);
                            serialPort1.Write(temp, 0, 1);
                            num++;
                        }
                        //判断是否需要发送新行
                        if (checkBox2.Checked)
                        {
                            //自动发送新行
                            serialPort1.WriteLine("");
                        }
                    }
                    else
                    {
                        //以ASCII模式发送
                        //判断是否需要发送新行
                        if (checkBox2.Checked)
                        {
                            //自动发送新行
                            serialPort1.WriteLine(textSend.Text);
                            num = textSend.Text.Length + 2; //回车占两个字节
                        }
                        else
                        {
                            //不发送新行
                            serialPort1.Write(textSend.Text);
                            num = textSend.Text.Length;
                        }
                    }

                    int send_count = 0;
                    send_count += num;      //计数变量累加
                    label7.Text = "Tx:" + send_count.ToString() + "Bytes";   //刷新界面
                }
            }
            catch (Exception ex)
            {
                serialPort1.Close();
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

        private void button5_Click(object sender, EventArgs e)
        {
            textReceive.Text = "";  //清空接收文本框
            textSend.Text = "";     //清空发送文本框
            receive_count = 0;          //计数清零
            label6.Text = "Rx:" + receive_count.ToString() + "Bytes";   //刷新界面
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string message = "嘀嘀嘀嘀嘀嘀，我是语音信息！";

            speechSynthesizer.Volume = trackBar1.Value;
            speechSynthesizer.Rate = trackBar2.Value;
            speechSynthesizer.SpeakAsync(message);
            Console.ReadLine();
        }
        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            trackBar1.Enabled = false;
            trackBar2.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
        }
        
        
        
        
        // Configuration of Labels
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                //自动发送功能选中,开始自动发送
                numericUpDown1.Enabled = false;     //失能时间选择
                timer1.Interval = (int)numericUpDown1.Value;     //定时器赋初值
                timer1.Start();     //启动定时器
                label7.Text = "串口已打开" + " 自动发送中...";
            }
            else
            {
                //自动发送功能未选中,停止自动发送
                numericUpDown1.Enabled = true;     //使能时间选择
                timer1.Stop();     //停止定时器
                label7.Text = "串口已打开";

            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

           


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button4_Click(button4, new EventArgs());
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            

        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            int num = serialPort1.BytesToRead;      //获取接收缓冲区中的字节数
            byte[] received_buf = new byte[num];    //声明一个大小为num的字节数据用于存放读出的byte型数据

            receive_count += num;                   //接收字节计数变量增加nun
            serialPort1.Read(received_buf, 0, num);   //读取接收缓冲区中num个字节到byte数组中
            sb.Clear();     //防止出错,首先清空字符串构造器
                            //遍历数组进行字符串转化及拼接



            if (radioButton2.Checked)
            {
                //选中HEX模式显示
                foreach (byte b in received_buf)
                {
                    sb.Append(b.ToString("X2") + ' ');    //将byte型数据转化为2位16进制文本显示,用空格隔开
                }
            }
            else
            {
                //选中ASCII模式显示
                sb.Append(Encoding.ASCII.GetString(received_buf));  //将整个数组解码为ASCII数组
            }



            /*    
            foreach (byte b in received_buf)
                {
                sb.Append(b.ToString());
                
            }
            */
            // sb.Append(Encoding.ASCII.GetString(received_buf));  //将整个数组解码为ASCII数组
            try
            {

                //因为要访问UI资源，所以需要使用invoke方式同步ui
                Invoke((EventHandler)(delegate
                {
                    if (checkBox1.Checked)
                    {
                        //显示时间
                        current_time = System.DateTime.Now;     //获取当前时间
                        textReceive.AppendText("[" + current_time.ToString("u") + "]  " + sb.ToString());

                    }
                    else
                    {
                        //不显示时间 
                        textReceive.AppendText(sb.ToString());
                    }
                    label6.Text = "Rx:" + receive_count.ToString() + "Bytes";
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
