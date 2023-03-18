module contrl(
clk,
QR_in,
geo_in,
gps_in,
ro_motor,
	PWM,  //驱动脉冲输入，脉冲数决定转的圈数，脉冲频率决定转速
    RST,//高复位
    DIR,//方向
    EN,//代表启动信号，1：有效   0：关闭
    M_OUT//输出
);
input clk;
input PWM,RST,DIR,EN,QR_in;
input [7:0] geo_in,gps_in;
output reg ro_motor;
output  [3:0] M_OUT;//【A+,A-,B+,B-】
reg [3:0]motor_ctl;
parameter threshold = 8'b1000_0000; //128

initial begin

		motor_ctl=4'b0000;	
end


always@(posedge RST or posedge PWM)
begin
    if(RST)begin
        motor_ctl = 4'b0000;
		  ro_motor<=1'b0;
    end
    else begin
        if(EN)begin//开关打开，信号有效
            if(geo_in>threshold||DIR||gps_in||QR_in)begin
                case(motor_ctl)
                    4'b0000: motor_ctl = 4'b0001;
                    4'b0001: motor_ctl = 4'b0010;
                    4'b0010: motor_ctl = 4'b0100;
                    4'b0100: motor_ctl = 4'b1000;
                    4'b1000: motor_ctl = 4'b0001;
                    default:motor_ctl = 4'b0000;
                endcase    
            end
            else begin 
                case(motor_ctl)
                    4'b0000: motor_ctl = 4'b1000;
                    4'b1000: motor_ctl = 4'b0100;
                    4'b0100: motor_ctl = 4'b0010;
                    4'b0010: motor_ctl = 4'b0001;
                    4'b0001: motor_ctl = 4'b1000;
                    default:motor_ctl = 4'b0000;
                endcase            
            end
        
		  end
			
    
	 if(QR_in)begin
		  ro_motor<=1'b1;
		  end
	else begin
	ro_motor<=1'b0;
	     end
	 end
end


assign M_OUT[3:0] = motor_ctl[3:0];

/*
input clk,rst_n;
input [7:0]tem_in,hum_in,sun_in,water_in;
output reg pump,blow,beep,hum_low,led,water_low;
parameter tem_high_nor = 8'b0001_1110, //30
          tem_low_nor  = 8'b0000_1010; //10
always@(posedge clk or negedge RST)
if(!rst_n)begin
blow<=1'b0;
beep<=1'b0;
end
else if(tem_in>=tem_high_nor)begin
blow<=1'b1;
beep<=1'b0;
end
else if(tem_in<=tem_low_nor)begin
blow<=1'b0;
beep<=1'b1;
end
else begin
blow<=1'b0;
beep<=1'b0;
end

parameter hum_in_nor=8'b0011_1100;//60
always@(posedge clk or negedge rst_n)
if(!rst_n)
hum_low<=1'b0;
else if(hum_in<=hum_in_nor)
hum_low<=1'b1;
else
hum_low<=1'b0;

parameter sun_in_nor=8'b0001_0100;//20
always@(posedge clk or negedge rst_n)
if(!rst_n)
led<=1'b0;
else if(sun_in<=sun_in_nor)
led<=1'b1;
else
led<=1'b0;

parameter water_in_nor=8'b0001_0100;//20
always@(posedge clk or negedge rst_n)
if(!rst_n)
water_low<=1'b0;
else if(water_in<=water_in_nor)
water_low<=1'b1;
else
water_low<=1'b0;

wire [1:0]contrl_en;
assign contrl_en={hum_low,water_low};

always@(posedge clk or negedge rst_n)
if(!rst_n)
pump<=1'b0;
else if(contrl_en==2'b10)
pump<=1'b1;
else
pump<=1'b0;
*/
endmodule