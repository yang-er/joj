#!/bin/bash
mkdir -p obj
cd obj
g++ --shared -fPIC ../main.cpp ../sandbox.cpp ../trace_call.cpp ../environment.cpp -o JudgeL64.so
g++ ../main.cpp ../sandbox.cpp ../trace_call.cpp ../environment.cpp -o JudgeL64.out
sudo cp * /lib/
cd ..
