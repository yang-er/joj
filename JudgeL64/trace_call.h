#pragma once
#include <sys/syscall.h>
#include <string.h>
#include <stdio.h>
#include <unistd.h>
#include <stdint.h>
#include <errno.h>
#include <signal.h>
#include <fcntl.h>

#define DLL extern "C"

extern int ok_call_cpp[256];
extern int ok_call_pascal[256];
extern int ok_call_java[256];
extern int ok_call_python[256];

const bool record_call = false;
const int call_array_size = 512;
extern unsigned int call_id;
extern unsigned int call_counter[call_array_size];

enum
{
	READ_END_OF_PIPE = 0,
	WRITE_END_OF_PIPE = 1,
};

extern "C" {

void init_syscalls_limits(int lang[256]);
void close_pipe_if_open(int fd);
int32_t create_pipe(int32_t pipeFds[2]);

}
