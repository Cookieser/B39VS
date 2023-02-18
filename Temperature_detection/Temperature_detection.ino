#include <LiquidCrystal.h>
// initialize the library with the numbers of the interface pins
LiquidCrystal lcd1602(12, 11, 5, 4, 3, 2);//The configuration of the pins used by LED

//Initialize some useful constants


int SensorPin = 14;




//Initialize variables

String incomingByte = ""; //Declare variable with the type ofstring
int degree= 100;
int TempValue;




//Implement the clockwise rotation of the motor
void clockwise(int num)
{
  for (int count = 0; count < num; count++)
  {
    for (int i = 6; i < 10; i++)
    {
      digitalWrite(i, HIGH);
      delay(3);
      digitalWrite(i, LOW);
    }
  }
}

//Realize the counterclockwise rotation of the motor
void anticlockwise(int num)
{
  for (int count = 0; count < num; count++)
  {
    for (int i = 9; i > 5; i--)
    {
      digitalWrite(i, HIGH);
      delay(3);
      digitalWrite(i, LOW);
    }
  }
}


void setup() {
  
  Serial.begin(9600); // 设置通信码率
  
  //The screen display of our LED
  lcd1602.begin(16, 2);
  lcd1602.print("Nothing: ");


 
  // The configuration of the pins used for stepper motors 
  for (int i = 6; i < 10; i++) {
    pinMode(i, OUTPUT);
  }
  
}



void loop() {

  while (Serial.available() > 0)
 {
     incomingByte += char(Serial.read());//读取单个字符值，转换为字符，并按顺序一个个赋值给incomingByte
     delay(10);//不能省略，因为读取缓冲区数据需要时间
 }

//The response of the specific component accordingly to the prescribed command  

 if ( incomingByte.length() > 0 ) 
 { 
   if( incomingByte == "i") {
    lcd1602.setCursor(0,1);
    lcd1602.print("Welcome!The system is initializing......");
    int num=degree/0.140;
    clockwise(num);
    delay(500000);
    }
   else
   
  {
     incomingByte = "";//清空变量，准备下次输入
   }
  
  
  
  
  }

  delay(500);
  //anticlockwise(512);
}
