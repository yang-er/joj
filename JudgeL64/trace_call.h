#pragma once
#include <sys/syscall.h>
#include <string.h>
#include <stdio.h>
#include <unistd.h>
#include <stdint.h>
#include <errno.h>
#include <signal.h>
#include <fcntl.h>

extern int *ok_call_langs[32];
const bool record_call = false;
const int call_array_size = 512;
extern unsigned int call_id;
extern unsigned int call_counter[call_array_size];

enum
{
	READ_END_OF_PIPE = 0,
	WRITE_END_OF_PIPE = 1,
};

void init_syscalls_limits(int lang[256]);
