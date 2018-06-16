#pragma once
#include <sys/syscall.h>
#include <string.h>
#include <stdio.h>

extern int ok_call_cpp[256];
extern int ok_call_pascal[256];
extern int ok_call_java[256];
extern int ok_call_python[256];

const bool record_call = false;
const int call_array_size = 512;
extern unsigned int call_id;
extern unsigned int call_counter[call_array_size];

extern "C" void init_syscalls_limits(int lang[256]);
