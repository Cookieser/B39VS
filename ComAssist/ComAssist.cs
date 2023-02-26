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
using System.Runtime.InteropServices;

namespace Helloworld
{

    public partial class ComAssist : Form
    {

        /* 
         * Global variable declaration
         *(To avoid repeated calls in the receive handler)
         */
        private StringBuilder sb = new StringBuilder();     
        private DateTime current_time = new DateTime();    // The object about time and date
        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer(); // Speech function object
        private long receive_count = 0; // Count the received data 

        public ComAssist()

        {
            //Initialization
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {


            /* Enablement Settings of components in the initial stage */
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


            // Default settings of options

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
                comboBox3.Items.Add(i.ToString());// Add the list of data
            }
            comboBox4.Items.Add("None");
            comboBox4.Items.Add("Even");
            comboBox4.Items.Add("Mark");
            comboBox4.Items.Add("Odd");
            comboBox4.Items.Add("Space");   // Add the list of parity

            for (double i = 1; i <= 2; i=i+0.5)
            {
                comboBox5.Items.Add(i.ToString()); // Add the list of stop
            }


        }

        /*
         * Component concrete behavior and corresponding
         */



        // Panal Setting
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel2.ClientRectangle,
            Color.White, 1, ButtonBorderStyle.Solid, // Left
            Color.White, 1, ButtonBorderStyle.Solid, // Up
            Color.DimGray, 1, ButtonBorderStyle.Solid, // Right
            Color.DimGray, 1, ButtonBorderStyle.Solid);// Down
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel3.ClientRectangle,
            Color.White, 1, ButtonBorderStyle.Solid,
            Color.White, 1, ButtonBorderStyle.Solid,
            Color.DimGray, 1, ButtonBorderStyle.Solid,
            Color.DimGray, 1, ButtonBorderStyle.Solid);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel4.ClientRectangle,
            Color.White, 1, ButtonBorderStyle.Solid, 
            Color.White, 1, ButtonBorderStyle.Solid, 
            Color.DimGray, 1, ButtonBorderStyle.Solid, 
            Color.DimGray, 1, ButtonBorderStyle.Solid);
        }
        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }


        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel7.ClientRectangle,
            Color.White, 1, ButtonBorderStyle.Solid, 
            Color.White, 1, ButtonBorderStyle.Solid, 
            Color.DimGray, 1, ButtonBorderStyle.Solid, 
            Color.DimGray, 1, ButtonBorderStyle.Solid);
        }
        // Setting of button

        //Switch language to English 
        private void button1_Click(object sender, EventArgs e)
        {
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(typeof(ComAssist)); //New a ResourceManager Object
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB"); // Switch to EN

            label1.Text = rm.GetString("label1.Text");
            label2.Text = rm.GetString("label2.Text");
            label3.Text = rm.GetString("label3.Text");
            label4.Text = rm.GetString("label4.Text");
            label5.Text = rm.GetString("label5.Text");
            label8.Text = rm.GetString("label8.Text");
            label9.Text = rm.GetString("label9.Text");
            // Logical processing of switching serial ports
            if (button3.Text == "打开串口")
            {
                button3.Text = rm.GetString("button3.Text");
            }
            if (button3.Text == "关闭串口")
            {
                button3.Text = "Close";
            }
            button4.Text = rm.GetString("button4.Text");
            button5.Text = rm.GetString("button4.Text");
            button6.Text = rm.GetString("button6.Text");
            button7.Text = rm.GetString("button7.Text");
            checkBox1.Text=rm.GetString("checkBox1.Text");
            checkBox2.Text = rm.GetString("checkBox2.Text");
            checkBox3.Text = rm.GetString("checkBox3.Text");

        }

        //Switch language to Chinese 
        private void button2_Click(object sender, EventArgs e)
        {
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(typeof(ComAssist));//New a ResourceManager Object
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-Hans");// Switch to ZH

            label1.Text = rm.GetString("label1.Text");
            label2.Text = rm.GetString("label2.Text");
            label3.Text = rm.GetString("label3.Text");
            label4.Text = rm.GetString("label4.Text");
            label5.Text = rm.GetString("label5.Text");
            label8.Text = rm.GetString("label8.Text");
            label9.Text = rm.GetString("label9.Text");

            // Logical processing of switching serial ports
            if (button3.Text == "Open")
            {
                button3.Text = rm.GetString("button3.Text");
            }
            if (button3.Text == "Close")
            {
                button3.Text = "关闭串口";
            }
            
            button4.Text = rm.GetString("button4.Text");
            button5.Text = rm.GetString("button4.Text");
            button6.Text = rm.GetString("button6.Text");
            button7.Text = rm.GetString("button7.Text");
            checkBox1.Text = rm.GetString("checkBox1.Text");
            checkBox2.Text = rm.GetString("checkBox2.Text");
            checkBox3.Text = rm.GetString("checkBox3.Text");
        }


        private void button3_Click(object sender, EventArgs e)
        {


            try
            {
                if (serialPort1.IsOpen)
                {
                    // The serial port is enabled
                    serialPort1.Close();    // Close this serial port
                    if (button4.Text == "发送")
                    {
                        button3.Text = "打开串口";
                    }
                    else {
                        button3.Text = "Open";
                    }
                    button3.BackColor = Color.ForestGreen;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    comboBox5.Enabled = true;
                    textReceive.Text = "";  // Clear receiving data
                    textSend.Text = "";     // Clear sending data
                }
                else
                {
                    //If the serial port is closed, set the properties of the serial port and enable it
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

                    serialPort1.Open();     // Open it
                    // Logical processing of switching serial ports
                    if (button4.Text == "发送")
                    {
                        button3.Text = "关闭串口";
                    }
                    else
                    {
                        button3.Text = "Close";
                    }
                    button3.BackColor = Color.Firebrick;

                    // Enablement Settings of components
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

                /* Catch and handle possible exceptions */

                // Create a new object that can no longer be used
                serialPort1 = new System.IO.Ports.SerialPort();
                //Refresh the options of COM
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                // Ring the bell and display the exception to the user
                System.Media.SystemSounds.Beep.Play();
                
                button3.BackColor = Color.Firebrick;
                if (button4.Text == "发送")
                {
                    button3.Text = "打开串口";
                }
                else
                {
                    button3.Text = "Open";
                }
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
            
            byte[] temp = new byte[1];
            try
            {
                // Check whether the serial port is enabled at first
                if (serialPort1.IsOpen)
                {
                    int num = 0;   

                    // Judge the sending mode
                    if (radioButton3.Checked)
                    {
                        /*
                         * Send through HEX
                         * First, we need to use regular expressions to match hexadecimal characters
                         */
                        string buf = textSend.Text;
                        string pattern = @"\s";
                        string replacement = "";
                        Regex rgx = new Regex(pattern);
                        string send_data = rgx.Replace(buf, replacement);


                        // No new lines
                        num = (send_data.Length - send_data.Length % 2) / 2;
                        for (int i = 0; i < num; i++)
                        {
                            temp[0] = Convert.ToByte(send_data.Substring(i * 2, 2), 16);
                            serialPort1.Write(temp, 0, 1);  //Cyclic transmission
                        }
                        // If the user enters an odd number of characters, it is processed separately
                        if (send_data.Length % 2 != 0)
                        {
                            temp[0] = Convert.ToByte(send_data.Substring(textSend.Text.Length - 1, 1), 16);
                            serialPort1.Write(temp, 0, 1);
                            num++;
                        }
                        // Determine whether a new line needs to be sent
                        if (checkBox2.Checked)
                        {
                            // Send a new line automatically 
                            serialPort1.WriteLine("");
                        }
                    }
                    else
                    {
                        // Send through ASCII
                        // Determine whether a new line needs to be sent
                        if (checkBox2.Checked)
                        {
                            // Send a new line automatically 
                            serialPort1.WriteLine(textSend.Text);
                            num = textSend.Text.Length + 2; // Carriage return takes two bytes
                        }
                        else
                        {
                            // No new lines
                            serialPort1.Write(textSend.Text);
                            num = textSend.Text.Length;
                        }
                        current_time = System.DateTime.Now;
                        textReceive.AppendText("Send:[" + current_time.ToString("u") + "]  " + textSend.Text);
                        textReceive.AppendText(System.Environment.NewLine);
                    }

                    int send_count = 0;
                    send_count += num;      // Counting variables add up
                    label7.Text = "Tx:" + send_count.ToString() + "Bytes";   // Refresh data 
                }
            }
            catch (Exception ex)
            {
                serialPort1.Close();
                // Create a new object that can no longer be used
                serialPort1 = new System.IO.Ports.SerialPort();
                //Refresh the options of COM
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                // Ring the bell and display the exception to the user
                System.Media.SystemSounds.Beep.Play();
                if (button4.Text == "发送")
                {
                    button3.Text = "打开串口";
                }
                else
                {
                    button3.Text = "Open";
                }

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
            textReceive.Text = "";  //Clear Receive data
            textSend.Text = "";     
            receive_count = 0;        
            label6.Text = "Rx:" + receive_count.ToString() + "Bytes";   //Refresh data
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string message = "DI,DI,DI,DI,this is a test message！";
            //Control the Volume and Rate of the voice
            speechSynthesizer.Volume = trackBar1.Value;
            speechSynthesizer.Rate = trackBar2.Value;
            // Speak something
            speechSynthesizer.SpeakAsync(message);
        }
        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            // Lock the settings after saving
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
                // Select this option to start automatic sending
                numericUpDown1.Enabled = false;     // Disabling time selection
                timer1.Interval = (int)numericUpDown1.Value;     // The timer is assigned an initial value
                timer1.Start();    
                label7.Text = " Auto...";
            }
            else
            {
                // If the automatic sendingis not selected, it stops
                numericUpDown1.Enabled = true;     //Enabling time selection
                timer1.Stop();     
                label7.Text = "Open";

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
            // Clicking the Send button will trigger this
            button4_Click(button4, new EventArgs());
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            

        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            int num = serialPort1.BytesToRead;      // Gets the number of bytes in the receive buffer
            byte[] received_buf = new byte[num];    // Declare enough space to hold received data

            receive_count += num;                   
            serialPort1.Read(received_buf, 0, num);   //Read data from the receive buffer
            sb.Clear();     
                            
            if (radioButton2.Checked)
            {
                // HEX show
                foreach (byte b in received_buf)
                {
                    sb.Append(b.ToString("X2") + ' ');    // Convert the data into 2 - bit hexadecimal display
                }
            }
            else
            {
                // ASCII show
                sb.Append(Encoding.ASCII.GetString(received_buf));  // Decode the entire array as an ASCII array
            }
            try
            {

                //Because we want to access UI resources, we need to use the invoke method to synchronize the UI
                Invoke((EventHandler)(delegate
                {
                    if (checkBox1.Checked)
                    {
                        // Show the time
                        current_time = System.DateTime.Now;     //Get time at present
                        textReceive.AppendText("[" + current_time.ToString("u") + "]  " + sb.ToString());

                    }
                    else
                    {
                        //No time 
                        textReceive.AppendText(sb.ToString());
                    }
                    label6.Text = "Rx:" + receive_count.ToString() + "Bytes";
                }
  )
);

            }
            catch (Exception ex)
            {
                // Ring the bell and display the exception to the user
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(ex.Message);

            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textReceive_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
