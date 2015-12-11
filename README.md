# Introduction

This repository has Windows 10 Bluetooth client library and two sample applications that show how to connect to BLE device using the library. The library supports TI CC2650STK aka SensorTag BLE device. 


# Project structure

## BleClient
BLE client library that lists available SensorTags, connects to the selected device and reads data from it. Before running the application you should pair SensorTag with Windows. Current pairing status can be checked from the Manage bluetooth devices tool. 

## SimpleApp
Demo application that reads data from SensorTag's sensors and shows it as a graph. 
![SimpleApp](https://github.com/juhanak/BleClient/blob/master/images/SimpleApp.png?raw=true "SimpleApp")

## BleGame
Do you want to control a spaceship with SensorTag? The game shows how to read data from Bluetooth and use it to control 
a player in a simple game.

![BleGame](https://github.com/juhanak/BleClient/blob/master/images/LukeVsKirk.png?raw=true "BleGame")

## ServiceBusClient
Simple code that publishes sensor data to Azure event hub.
