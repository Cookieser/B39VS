/*Use some library*/
#include <Wire.h> 
#include <SoftwareSerial.h>

#include <TinyGPS++.h> // The library which can process GPS data
#include <Keypad.h> // The files about the operations of keypad
#include <LiquidCrystal_I2C.h> //The library of LiquidCrystal_I2C

/*Customize some classes*/
//Packaged the functions of the motor as a class
class Motor{
private:    //Private attribute includes four pins and lastNum used for turning back
  int Pin1;
  int Pin2;
  int Pin3;
  int Pin4;
  int lastNum;
  
public:
  Motor(int pinFirst)//Constructor of this class
  {
    Pin1 = pinFirst;
    Pin2 = pinFirst+1;
    Pin3 = pinFirst+2;
    Pin4 = pinFirst+3;
    };

  // Initialization of motor pins
  void init()
  {
    for (int i = Pin1; i <= Pin4; i++) 
        {
          pinMode(i, OUTPUT);
        }
  }
  
  // Achieve a certain angle of clockwise rotation  
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
  lastNum = num; //Recoring this value for turning back
}

// Achieve a certain angle of anticlockwise rotation 
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
  lastNum = -num;//Recoring this value for turning back
}

//The motor can reset through this after rotation 
void turnBack()
{
  if(lastNum<0)
  {
    clockwise(-lastNum);
  }else
  {
    anticlockwise(lastNum);
  }
}

};



/*Setting of equipments and pins*/
LiquidCrystal_I2C lcd(0x27,20,4); //Initialization of LCD by the device address to save pins

int SensorPin = A0;//Sensor pin

/*Setting of keypad*/
const byte rows = 4; //Four rows
const byte cols = 3; //Three columns
char keys[rows][cols] = {
  {'1','2','3'},
  {'4','5','6'},
  {'7','8','9'},
  {'*','0','#'}
};
byte rowPins[rows] = {25, 24, 23, 22}; //Row pins
byte colPins[cols] = {26, 27, 28}; //Columns pin
Keypad keypad = Keypad( makeKeymap(keys), rowPins, colPins, rows, cols );

/*Setting of motors*/
Motor *motorA = new Motor(10);
Motor *motorB = new Motor(6);
Motor *motorC = new Motor(2);

/*Setting of GPS module*/
const int RXPin = 99, TXPin = 100;
const uint32_t GPSBaud = 9600; //Default baud is 9600
TinyGPSPlus gps; // the TinyGPS++ object

/*Variable declaration*/
int deflectionAngle;
int elevation;
float deflectionValue;
String incomingByte = ""; //Declare variable with the type of string
int degree= 10;



/*Customize some functions*/

//Get the number from keypad
int scanValue(){
   String inputValue = "";
   while(1)
   {
      char customKey = keypad.getKey();//get an input char
      if (customKey)
      {
        if(customKey == '#') // Similar to the Confirm key
          {
            Serial.print("-HA: ");
            Serial.println(inputValue);
            break;
          }
        inputValue += customKey; // Splicing chars
    
        if(customKey == '*') // Similar to the Clear key
          {
            lcd.setCursor(7,1);
            lcd.print("                ");
            inputValue = "";
            lcd.setCursor(7,1);
          }
        lcd.setCursor(7,1);
        lcd.print(inputValue);//Show data in the lcd screen
     }
   }
   int res = atoi(inputValue.c_str());// Data type conversion from STRING to INT 
   return res;
  }




//GPS data processing
void gpsFunction(){
  if (Serial1.available() > 0) //Receive the GPS information
  {
    if (gps.encode(Serial1.read()))//Read the GPS information
    {
      if (gps.location.isValid()) //Judge  whether the location data is valid
      {
        Serial.print(F("- latitude: "));//Show some info to upper computer
        Serial.println(gps.location.lat());

        Serial.print(F("- longitude: "));
        Serial.println(gps.location.lng());

        
      } else {
        Serial.println(F("- location: Measuring..."));
      }

      Serial.print(F("- speed: "));
      if (gps.speed.isValid()) //Judge whether the speed data is valid
      {
        Serial.print(gps.speed.kmph());
        Serial.println(F(" km/h"));
      } else {
        Serial.println(F("Measuring..."));
      }

      Serial.print(F("- GPS date&time: "));
      if (gps.date.isValid() && gps.time.isValid()) //Judge whether the time data is valid
      {
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

  if (millis() > 5000 && gps.charsProcessed() < 10) // Error prompt
    Serial.println(F("No GPS data received: check wiring"));}


void setup() {
  
  //Serial port setting
  Serial.begin(9600);
  Serial1.begin(9600);//Use for GPS module
  
  //LCD sets up and shows some info
  lcd.init();                  
  lcd.backlight();            
  lcd.setCursor(0,0);
  lcd.print("Welcome!!!");
  lcd.setCursor(0,1);
  lcd.print("GPS is activating...");
  lcd.setCursor(0,2);
  lcd.print("Waiting...");

  //Motors starts to activate
  motorA->init();
  motorB->init();
  motorC->init();

  //GPS activates and send messages to LCD when it has finished
  /*
  while(1){
  gpsFunction();
  if(gps.location.lat())
  {
    //Send messages
    lcd.setCursor(0,0);  
    lcd.print("GPS module is ready");
    Serial.println("The GPS module is activated successfully");
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
     incomingByte += char(Serial.read());//Read the single character value, convert to char and assign to incomingByteRead one by one in order
     delay(10);//This cannot be ignored because reading buffer data needs time

//The response of the specific component accordingly to the prescribed command  

 if ( incomingByte.length() > 0 ) 
 { 
  //Self Test & Init
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
    Serial.print("- GM-Sensor: ");
    Serial.println(deflectionValue);
    Serial.print("- latitude: ");
    Serial.println(gps.location.lat());
    Serial.println("The process of self-Test and initialization has finished! ");
    Serial.println("\n");
  

    incomingByte = "";
    }
   else if(incomingByte == "m"){

    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("Self-adaption...");
    lcd.setCursor(0,1);
    lcd.print("Elevation: ");
    lcd.println(gps.location.lat());
    
    Serial.print("- latitude: ");
    Serial.println(gps.location.lat());
    
    lcd.setCursor(0,2);
    lcd.print("deflection: ");
    int deflectionNew = ( analogRead(SensorPin)/1024.0)*500;   // Getting the celsius value from 10 bit analog value
    lcd.print(deflectionNew);
    Serial.print("- Deflection: ");
    Serial.println(deflectionNew);
    if(deflectionNew >= deflectionValue){
      int moveDegree =  deflectionNew - deflectionValue;
      motorA->clockwise(moveDegree/0.140);
      deflectionValue =  deflectionNew;
      }else if(deflectionNew < deflectionValue){
        int moveDegree =   deflectionValue - deflectionNew;
      motorA->anticlockwise(moveDegree/0.140);
      deflectionValue =  deflectionNew;
        
        }
     incomingByte = "";
     delay(1000);
    
     
    }
    
    else if(incomingByte == "r"){
    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("Tracking Star...");
    Serial.println("Tracking Star...");
    lcd.setCursor(0,1);
    lcd.print("input: ");
    motorC->clockwise(scanValue()/0.140);
    lcd.setCursor(0,1);
    lcd.print("Finished!!!");
    Serial.println("Finished!!!");
    //To do: Add a motor which can move with a fixed speed   
    
    }
   
    else
   
  {
    lcd.clear();
    lcd.setCursor(0,0);
    lcd.print("-c :Self Test&Init");
    lcd.setCursor(0,1);
    lcd.print("-m :Self-adaption");
    lcd.setCursor(0,2);
    lcd.print("-r :Tracking Star");
    incomingByte = "";//Clear the variables for the next input
   }
  
  
  
  
  }

  delay(500);

    
}
}
