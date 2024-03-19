# Astrologer

## 1.Introduction

The instruments called equatorial instruments are widely used to track objects in the observational process. We want to use  simple mechanical structures to build a lightweight automated equatorial instruments that can  automatically track celestial bodies.

![下载](https://pic-1306483575.cos.ap-nanjing.myqcloud.com/images/%E4%B8%8B%E8%BD%BD.png)

## 2.Solution

### 2.1 Small Automated System

Based on the equatorial coordinate system, we can use the two parameters of right ascension and declination to determine the specific orientation of a celestial body in the sky at a certain time. The equator instrument is created on the above principle. The principal body of this automatic instrument is constituted by two rotating axes, the right ascension axis and the declination axis, and the supporting tripod. 

The instrument needs to be initialized at first, and we could use sensors to transmit the needed parameter which will assist the machine to find Polaris from the view of this certain time and direction. After the initialization, the system divides the star searching mode into brief and precise mode and complete the automatic tracking by built-in data and information from the sensor.

### 2.2 Sensor

As we said above, we need some types of sensor to accomplish the system, which will be described in detail below.

#### 2.2.1 Geomagnetic sensor

We need this sensor to get the angle gap between our machine and due north, so we could drive the motor according to this value. What's more, the machine will be self-correcting to point in the due north automatically no matter how the position is moved.

Through research and investigation, the sensor requires a series of corrective measures due to its property which is easy to be impressed by the surrounding environment and actually this sensor is not provided in Proteus. Hence, we try to use the temperature sensor to simulate its function simply, which means the value represented on the sensor will be seen as the angle gap instead of the temperature.

#### 2.2.2 GPS

For astronomical products, the basic position coordinates are naturally an essential component, so we use the GPS to get some information such as the longitude, latitude and so on.

The value of the latitude will be the angle of elevation for this machine at the initialization stage. Thus, the latitude, longitude and time will the basic information for the process of automatic tracking.

#### 2.2.3 Infrared Sensor(QR scanner)

In system, if we want to get fine observation, we could use QR scanner to transmit the planet data we want to observe  (Hour Angle, a value related to the time, longitude and latitude, which can be obtained directly by many applications nowadays). Since there is no QR scanner in Proteus, we could use the infrared module to imitate the information transfer. 

### 2.3 Physically interact

After the initialization, we roughly divide the automatic tracking into two ways. Firstly, we could use some basic parameters such as time, latitude and  built-in data to observe a star briefly. Secondly, if we want to get fine observation, we could use QR scanner to transmit the planet data we want to observe. Then, the Arduino gets this parameter and tells the FPGA to drive the time disk and declination disk to rotate accordingly. After that, the other motor starts to drive the right ascension axis to rotate, which will complete the automatic tracking.

## 3.Highlights

### 3.1 Novelty

As a system design aiming at learning and master these technology, it is a safe choice to make a classical system such as the humidity controller and temperature controller. However, I think we could try to combine with other fields and do something interesting indeed just as this Lightweight Automatic Equator Instrument.

### 3.2 Complexity

The underlying principle of the system is astronomy, so some of the principles in the system are complicated and uneasy for us to understand. What's more, because of the special field, it is difficult to find and use some components

### 3.3 Multimode

This project has strong practicability and applicability. The system divides the star searching mode into brief and precise mode, and users could choose freely according to different scenarios.

### 3.4 Market

Through the previous marketing research, we find that  related products on the market are extremely focus on accuracy, which makes them very expensive and cumbersome. This is unfriendly for astrophiles who don't care about accuracy but the price. Obviously, our project fills the gap in this respect with certain commercial value

## 4.Issue

* In the simulation of Proteus, the rotation angle of the stepper motor has a slight deviation from the desired angle because of the coding design and motor attribute, so the system can't achieve particularly precise movement at present.
* The information processing process of the GPS module is somewhat complex, which means that more codes beyond the project design scope will be added. We will keep trying and retain the option to use other simply sensors as a simulator for this function at the same time
* Because Google charges for its services, the GPS module cannot load the real location normally. We can only choose to set the latitude and longitude ahead in the setting of GPS module and use this to verify the feasibility of the total system

## 5.Finished

ComAssist：

- [x] Broadcast when receiving and sending data
- [x] Polish annotation
- [x] Debug
- [x]  Optimized language switching

Proteus：

- [x] Polish annotation
- [x] Add a motor
- [x] Collate communication commands
- [x] LCD and Upper computer information Management
- [x] Show PWM

FPGA：

- [x] Verilog
- [x] Test Files

![img](https://pic-1306483575.cos.ap-nanjing.myqcloud.com/images/QQ%E5%9B%BE%E7%89%8720230226220649.png)

![img](https://pic-1306483575.cos.ap-nanjing.myqcloud.com/images/QQ%E5%9B%BE%E7%89%8720230226220645.png)

![img](https://pic-1306483575.cos.ap-nanjing.myqcloud.com/images/2M1R%5BIQTHL%5D7DW%60ZPO0BFEH.png)

![img](https://pic-1306483575.cos.ap-nanjing.myqcloud.com/images/QQ%E5%9B%BE%E7%89%8720230225162349.png)

![img](https://pic-1306483575.cos.ap-nanjing.myqcloud.com/images/image-20230319180025041.png)
