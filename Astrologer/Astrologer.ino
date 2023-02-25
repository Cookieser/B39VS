#include <TinyGPS++.h>
#include <SoftwareSerial.h>
#include <Keypad.h>
#include <Wire.h> //调用wire库
#include <LiquidCrystal_I2C.h> //调用LiquidCrystal_I2C库
LiquidCrystal_I2C lcd(0x27,20,4); //设置LCD设备地址 

String incomingByte = ""; //Declare variable with the type ofstring
int degree= 10;
int SensorPin = A0;
int deflectionAngle;
int elevation;
float deflectionValue;


const byte rows = 4; //four rows
const byte cols = 3; //three columns
char keys[rows][cols] = {
  {'1','2','3'},
  {'4','5','6'},
  {'7','8','9'},
  {'*','0','#'}
};
byte rowPins[rows] = {25, 24, 23, 22}; //连接行引脚
byte colPins[cols] = {26, 27, 28}; //连接的列引脚
Keypad keypad = Keypad( makeKeymap(keys), rowPins, colPins, rows, cols );
int scanValue(){
  String inputValue="";
   while(1){
   char customKey = keypad.getKey();
   
  if (customKey){
    
    //Serial.println(customKey);
   
    if(customKey=='#') 
    {
      Serial.println(inputValue);
      break;
    
    }
    inputValue+=customKey;
    
     if(customKey=='*') {
      lcd.setCursor(7,1);
      lcd.print("                ");
      inputValue="";
      lcd.setCursor(7,1);
      lcd.print(inputValue);
      }
      lcd.setCursor(7,1);
      lcd.print(inputValue);
  }
   }
   int res = atoi(inputValue.c_str());
   return res;
  }

class Motor{
private:    //私有成员，用来保存色彩的RGB分量
  int Pin1;
  int Pin2;
  int Pin3;
  int Pin4;
  int lastNum=0;
  

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
  lastNum = num;
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
  lastNum = -num;
}

void init(){
    for (int i = Pin1; i <= Pin4; i++) {
      pinMode(i, OUTPUT);
    }
  }
void turnBack(){
  if(lastNum<0){
    clockwise(-lastNum);
    
    }else{
      anticlockwise(lastNum);
      }
  }

};



Motor *motorA = new Motor(10);
Motor *motorB = new Motor(6);
Motor *motorC = new Motor(2);


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
  lcd.setCursor(0,2);
  lcd.print("Waiting...");
  motorA->init();
  motorB->init();
  motorC->init();
  /*
  while(1){
  gpsFunction();
  
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
  lcd.setCursor(0,2);
  lcd.print("Waiting for commands");
  break;
    
    }
  }
  */

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
  //初始化与系统自测
   if( incomingByte == "c") {
    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("Self Test & Init");
    lcd.setCursor(0,3);
    lcd.print("Motor check");
    lcd.setCursor(0,2);
    lcd.print("Test GM-Sensor");
    deflectionAngle = analogRead(SensorPin);   // Getting LM35 value and saving it in variable
    deflectionValue = ( deflectionAngle/1024.0)*500;   // Getting the celsius value from 10 bit analog value
    lcd.print(deflectionValue);

    lcd.setCursor(0,1);
    lcd.print("Test GPS ");
    lcd.print(gps.location.lat());
    
    motorA->clockwise(deflectionValue/0.140);
    motorB->clockwise(gps.location.lat()/0.140);

    motorC->clockwise(1);
    motorC->anticlockwise(1);
    
    lcd.setCursor(0,3);
    lcd.print("Motor check  OK");
    
    lcd.setCursor(0,2);
    lcd.print("Test GM-Sensor ");
    lcd.print(deflectionValue);


    lcd.setCursor(0,1);
    lcd.print("Test GPS ");
    lcd.print(gps.location.lat());
  

    incomingByte = "";
    }
   else if(incomingByte == "m"){
    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("Self-adaption...");
    lcd.setCursor(0,1);
    lcd.print("elevation: ");
    lcd.print(gps.location.lat());
    lcd.setCursor(0,2);
    lcd.print("deflection: ");
    int deflectionNew = ( analogRead(SensorPin)/1024.0)*500;   // Getting the celsius value from 10 bit analog value
    lcd.print(deflectionNew);
    if(deflectionNew >= deflectionValue){
      int moveDegree =  deflectionNew - deflectionValue;
      motorA->clockwise(moveDegree/0.140);
      deflectionValue =  deflectionNew;
      }else if(deflectionNew < deflectionValue){
        int moveDegree =   deflectionValue - deflectionNew;
      motorA->anticlockwise(moveDegree/0.140);
      deflectionValue =  deflectionNew;
        
        }else{
          lcd.setCursor(0,3);
          lcd.print("......");
          }
    }else if(incomingByte == "mr"){
    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("Tracking Star...");
    lcd.setCursor(0,1);
    lcd.print("input: ");
    motorC->clockwise(scanValue()/0.140);
    lcd.setCursor(0,1);
    lcd.print("Finished!!!");
    /*
    To do: Add a motor which can move with a fixed speed   
    */
    }else
   
  {
    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("-c :Self Test&Init");
    lcd.setCursor(0,1);
    lcd.print("-m :Self-adaption");
    lcd.setCursor(0,2);
    lcd.print("-r :Tracking Star");
    incomingByte = "";//清空变量，准备下次输入
   }
  
  
  
  
  }

  delay(500);

    
}
