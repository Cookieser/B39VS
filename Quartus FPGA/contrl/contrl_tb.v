`timescale 1ns/1ns  //时间单位1ns，时间精度1ns
`define clk_period 20 //时钟周期20ns

module contrl_tb();//测试仿真文件
reg clk;
reg QR_in;
reg [7:0] geo_in,gps_in;//输入
reg PWM,RST,DIR,EN;
wire ro_motor;//输出
wire [3:0] M_OUT;



//待测试的模块例化
contrl contrl(
.clk(clk),
.QR_in(QR_in),
.geo_in(geo_in),
.gps_in(gps_in),
.ro_motor(ro_motor),
.PWM(PWM),
.RST(RST),
.DIR(DIR),
.EN(EN),
.M_OUT(M_OUT)
);

initial clk=1;
always #(`clk_period/2) PWM=~PWM;//普通时钟信号，占空比50%

initial begin // initial
PWM=1'b0;
RST=1'b1;
EN=1'b0;

#(`clk_period*10) // 方向角正传

RST<=1'b0;
EN<=1'b1;
geo_in<=8'b0100_0110;

#(`clk_period*10) // 停止

RST<=1'b0;
EN<=1'b0;

#(`clk_period*10) // 方向角反传

RST<=1'b0;
EN<=1'b1;
geo_in<=8'b1100_0110;

#(`clk_period*10) // 停止

RST<=1'b0;
EN<=1'b0;

#(`clk_period*10)// gps正传
RST<=1'b0;
EN<=1'b1;
gps_in<=8'b1100_0110;

#(`clk_period*10) // 停止

RST<=1'b0;
EN<=1'b0;

#(`clk_period*10) // gps仍正传

RST<=1'b0;
EN<=1'b1;
gps_in<=8'b0100_0110;


#(`clk_period*10) // 停止

RST<=1'b0;
EN<=1'b0;

#(`clk_period*10)// QR输入，电机恒定转动
RST<=1'b0;
EN<=1'b1;
QR_in<=1'b1;
#(`clk_period*10) // 停止

RST<=1'b0;
EN<=1'b0;
QR_in<=1'b0;

#(`clk_period*10)
$stop;
end
endmodule