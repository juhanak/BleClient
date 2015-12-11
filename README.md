# Introduction

This repository has Windows 10 Bluetooth client library and two sample applications that use it. 
The library connects to TI CC2650STK aka SensorTag and reads sensor values from it. 

# Project structure

## BleClient
BLE client library that lists available SensorTags, connects to them and reads data from characteristic. 

## SimpleApp
Demo application that reads data from SensorTag's sensors and shows it as a graph. 

## BleGame
Do you want to control a spaceship with SensorTag? The game shows how to read data from Bluetooth and use it to control 
a player in a simple game. 

## ServiceBusClient
Simple code that publishes sensor data to Azure event hub.
