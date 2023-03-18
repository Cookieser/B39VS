module contrl(
	QR_in,     //It means there are some inputs from QR scanner When it is high level
	geo_in,    // The value of magnetic declination measured by Geomagnetic sensor
	gps_in,    // The value of the latitude obtained by the GPS module
	ro_motor,  // Geomagnetic sensor
	PWM,       // Drive the input of pulse : the pulse number determines the number of revolutions and pulse frequency determines the speed
   RST,  // Represents the status of the motor rotating at a constant speed
   EN,   // Represents the start signal(1: valid 0: off)
   M_OUT // Output of the stepper motor
);
/*
	Define some inputs and outputs
*/
input PWM,RST,EN,QR_in;
input [7:0] geo_in,gps_in;
output reg ro_motor;
output  [3:0] M_OUT;//【A+,A-,B+,B-】
reg [3:0]motor_ctl;

/* We split the 8-bit binary into two intervals to simulate the real positive and negative relationship 
 0-128--> negative  128-255 --> positive
 */
parameter threshold = 8'b1000_0000; // 128

initial begin
		motor_ctl=4'b0000;	
end


always@(posedge RST or posedge PWM)
begin
    if(RST)begin //Reset
        motor_ctl = 4'b0000;
		  ro_motor<=1'b0;
    end
    else begin
        if(EN)begin // The switch is on and the signal is valid	  
				//The logic realization of stepping motor		
            if(geo_in>threshold||gps_in||QR_in)begin
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
			
	 if(QR_in) 
		  begin
				ro_motor<=1'b1; //Start the motor rotating at a constant speed 
		  end
	 end
end

// Ouput of the stepper motor
assign M_OUT[3:0] = motor_ctl[3:0];

endmodule
