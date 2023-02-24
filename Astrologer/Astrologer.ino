#include <TinyGPS++.h>
#include <SoftwareSerial.h>
#include <Wire.h> //调用wire库
#include <LiquidCrystal_I2C.h> //调用LiquidCrystal_I2C库
LiquidCrystal_I2C lcd(0x27,20,4); //设置LCD设备地址 

String incomingByte = ""; //Declare variable with the type ofstring
int degree= 10;
int SensorPin = A0;
int deflectionAngle;
int elevation;
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



Motor *motorA = new Motor(10);
Motor *motorB = new Motor(6);
Motor *motorC = new Motor(2);


/*
void setup() {
  
  Serial.begin(9600); // 设置通信码率

  motorA->init();
  motorB->init();
  motorC->init();
 
  //The screen display of our LED
  


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
    
    
    
  deflectionAngle = analogRead(SensorPin);   // Getting LM35 value and saving it in variable
  float deflectionValue = ( deflectionAngle/1024.0)*500;   // Getting the celsius value from 10 bit analog value
  //lcd.print(TempCel);
  int num=deflectionValue/0.140;
  motorA->clockwise(num);


  //elevation = 
  
    lcd.setCursor(0,0);
    
    lcd.print("InitComplete      ");
    lcd.setCursor(0,1);
    lcd.print("Waiting......");
    
    lcd.setCursor(0,1);  
     // Print a message to the LCD.  
    
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
*/

const int RXPin = 99, TXPin = 100;
const uint32_t GPSBaud = 9600; //Default baud of NEO-6M is 9600


TinyGPSPlus gps; // the TinyGPS++ object

void gpsFunction(){
  
  if (Serial1.available() > 0) {
    if (gps.encode(Serial1.read())) {
      if (gps.location.isValid()) {
        Serial.print(F("- latitude: "));
        Serial.println(gps.location.lat());

        Serial.print(F("- longitude: "));
        Serial.println(gps.location.lng());

        Serial.print(F("- altitude: "));
        if (gps.altitude.isValid())
          Serial.println(gps.altitude.meters());
        else
          Serial.println(F("Measuring..."));
      } else {
        Serial.println(F("- location: Measuring..."));
      }

      Serial.print(F("- speed: "));
      if (gps.speed.isValid()) {
        Serial.print(gps.speed.kmph());
        Serial.println(F(" km/h"));
      } else {
        Serial.println(F("Measuring..."));
      }

      Serial.print(F("- GPS date&time: "));
      if (gps.date.isValid() && gps.time.isValid()) {
        Serial.print(gps.date.year());
        Serial.print(F("-"));
        Serial.print(gps.date.month());
        Serial.print(F("-"));
        Serial.print(gps.date.day());
        Serial.print(F(" "));
        Serial.print(gps.time.hour());
        Serial.print(F(":"));
        Serial.print(gps.time.minute());
        Serial.print(F(":"));
        Serial.println(gps.time.second());
      } else {
        Serial.println(F("Measuring..."));
      }

      Serial.println();
    }
  }

  if (millis() > 5000 && gps.charsProcessed() < 10)
    Serial.println(F("No GPS data received: check wiring"));}


void setup() {
  Serial.begin(9600);
  Serial1.begin(9600);
  
  lcd.init();                  // 初始化LCD_1602A
  lcd.backlight();             //设置LCD背景等亮
  lcd.setCursor(0,0);
  lcd.print("Welcome!!!");
  lcd.setCursor(0,1);
  lcd.print("GPS is activating...");
  
  motorA->init();
  motorB->init();
  motorC->init();
  motorA->clockwise(32);
    motorA->anticlockwise(32);
  while(1){
  gpsFunction();
  lcd.setCursor(0,2);
  //lcd.print("Waiting...");
  if(gps.location.lat()){
  lcd.setCursor(0,0);  
  lcd.print("GPS module is ready");
  lcd.setCursor(0,1);
  lcd.print("- latitude: ");
  lcd.print(gps.location.lat());
  lcd.print("   ");
  lcd.setCursor(0,2);
  lcd.print("- longitude: ");
  lcd.print(gps.location.lng());
  break;
    
    }
  }

}



void loop() {
  /*
  gpsFunction();
  if(gps.location.lat()){
    lcd.setCursor(0,0);
  lcd.print("Finished Measuring!!!");
  lcd.setCursor(0,1);
  lcd.print(gps.location.lat());
    

    
    
    }
*/



while (Serial.available() > 0)
 {
     incomingByte += char(Serial.read());//读取单个字符值，转换为字符，并按顺序一个个赋值给incomingByte
     delay(10);//不能省略，因为读取缓冲区数据需要时间
 }

//The response of the specific component accordingly to the prescribed command  

 if ( incomingByte.length() > 0 ) 
 { 
   if( incomingByte == "c") {
    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("Self Test......");
    lcd.setCursor(0,1);
    lcd.print("Motor check");
    lcd.setCursor(0,2);
    lcd.print("Test GM-Sensor");
    lcd.setCursor(0,3);
    lcd.print("Test GPS ");
    


    motorA->clockwise(512);
    motorA->anticlockwise(512);
    motorB->clockwise(512);
    motorB->anticlockwise(512);
    motorC->clockwise(512);
    motorC->anticlockwise(512);
    lcd.setCursor(0,1);
    lcd.print("Motor check  OK");
    deflectionAngle = analogRead(SensorPin);   // Getting LM35 value and saving it in variable
    float deflectionValue = ( deflectionAngle/1024.0)*500;   // Getting the celsius value from 10 bit analog value
    lcd.setCursor(0,2);
    lcd.print("Test GM-Sensor ");
    lcd.print(deflectionValue);


    lcd.setCursor(0,3);
    lcd.print("Test GPS ");
    lcd.print(gps.location.lng());
  //int num=deflectionValue/0.140;
  //motorA->clockwise(num);


  //elevation = 
  /*
    lcd.setCursor(0,0);
    
    lcd.print("InitComplete      ");
    lcd.setCursor(0,1);
    lcd.print("Waiting......");
    
    lcd.setCursor(0,1);  
     // Print a message to the LCD.  
     */
    
    delay(500000);
    }
   else
   
  {
     incomingByte = "";//清空变量，准备下次输入
   }
  
  
  
  
  }

  delay(500);

    
}
