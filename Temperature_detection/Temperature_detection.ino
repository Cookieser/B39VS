
#include <Wire.h> //调用wire库
#include <LiquidCrystal_I2C.h> //调用LiquidCrystal_I2C库
// initialize the library with the numbers of the interface pins
LiquidCrystal_I2C lcd(0x27,16,2); //设置LCD设备地址 

//Initialize some useful constants
int SensorPin = 14;





class Motor{
private:    //私有成员，用来保存色彩的RGB分量
  int Pin1;
  int Pin2;
  int Pin3;
  int Pin4;
  

public:
  Motor(int pinFirst){
    Pin1 = pinFirst;
    Pin2 = pinFirst+1;
    Pin3 = pinFirst+2;
    Pin4 = pinFirst+3;
    };
  void clockwise(int num)
{
  for (int count = 0; count < num; count++)
  {
    for (int i = Pin1; i <= Pin4; i++)
    {
      digitalWrite(i, HIGH);
      delay(3);
      digitalWrite(i, LOW);
    }
  }
}
void anticlockwise(int num)
{
  for (int count = 0; count < num; count++)
  {
    for (int i = Pin4; i >= Pin1; i--)
    {
      digitalWrite(i, HIGH);
      delay(3);
      digitalWrite(i, LOW);
    }
  }
}
void init(){
    for (int i = Pin1; i <= Pin4; i++) {
      pinMode(i, OUTPUT);
    }
  }


};


//Initialize variables

String incomingByte = ""; //Declare variable with the type ofstring
int degree= 10;
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


Motor *motorA = new Motor(2);
Motor *motorB = new Motor(6);
Motor *motorC = new Motor(10);
void setup() {
  
  Serial.begin(9600); // 设置通信码率

  motorA->init();
  motorB->init();
  motorC->init();
  //The screen display of our LED
  
  lcd.init();                  // 初始化LCD_1602A
  lcd.backlight();             //设置LCD背景等亮
  lcd.setCursor(0,0);
  lcd.print("Welcome!!!");

  // The configuration of the pins used for stepper motors 
  
 
  
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
    lcd.setCursor(0,0);
    lcd.print("Initializing......");
    int num=degree/0.140;
    motorA->clockwise(num);
    motorB->clockwise(num);
    motorC->clockwise(num);
    lcd.setCursor(0,0);
    lcd.print("InitComplete      ");
    lcd.setCursor(0,1);
    lcd.print("Waiting......");
    
    
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
