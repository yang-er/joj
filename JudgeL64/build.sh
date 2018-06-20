#!/bin/bash
mkdir -p obj
cd obj
g++ ../main.cpp ../sandbox.cpp ../trace_call.cpp ../environment.cpp -o JudgeL64.out
sudo cp JudgeL64.out /lib/
cd ..
