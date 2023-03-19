`timescale 1ns/1ns  // The time unit is 1ns, and the time precision is 1ns
`define clk_period 20 // The clock cycle is 20ns

module contrl_tb();

/*
	Define some inputs and outputs
*/
reg [7:0] geo_in,gps_in;// Input
reg PWM,RST,EN;
reg QR_in;
wire ro_motor;// Output
wire [3:0] M_OUT;

/*
	Instantiate the module to be tested
*/
contrl contrl(
.QR_in(QR_in),
.geo_in(geo_in),
.gps_in(gps_in),
.ro_motor(ro_motor),
.PWM(PWM),
.RST(RST),
.EN(EN),
.M_OUT(M_OUT)
);

always #(`clk_period/2) PWM=~PWM;// Common clock signal (duty cycle: 50%)

initial begin // initialization
PWM=1'b0;
RST=1'b1;
EN=1'b0;
geo_in=8'b0000_0000;
gps_in=8'b0000_0000;
QR_in=1'b0;

#(`clk_period*10) // Stage 1: Positive rotation due to the direction Angle

RST<=1'b0;
EN<=1'b1;
geo_in<=8'b0100_0110;

#(`clk_period*10) // Stop

RST<=1'b0;
EN<=1'b0;

#(`clk_period*10) // Stage 2: Negative rotation due to the direction Angle

RST<=1'b0;
EN<=1'b1;
geo_in<=8'b1100_0110;

#(`clk_period*10) // Stop

RST<=1'b0;
EN<=1'b0;

#(`clk_period*10)// Stage 3: clockwise rotation according to the latitude
RST<=1'b0;
EN<=1'b1;
gps_in<=8'b1100_0110;

#(`clk_period*10) // Stop

RST<=1'b0;
EN<=1'b0;

#(`clk_period*10) // Stage 4: Still going to rotate clockwise according to the latitude

RST<=1'b0;
EN<=1'b1;
gps_in<=8'b0100_0110;


#(`clk_period*10) // Stop

RST<=1'b0;
EN<=1'b0;

#(`clk_period*10)// Stage 5: There are some QR inputs and that motor will uniform rotation
RST<=1'b0;
EN<=1'b1;
QR_in<=1'b1;

#(`clk_period*10) // Stage 6: QR scanner has finished its work and that motor will still uniform rotation

RST<=1'b0;
EN<=1'b0;
QR_in<=1'b0;

#(`clk_period*10)
$stop;
end
endmodule
