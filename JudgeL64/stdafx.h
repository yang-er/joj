#pragma once
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <dirent.h>
#include <unistd.h>
#include <time.h>
#include <stdarg.h>
#include <ctype.h>
#include <sys/wait.h>
#include <sys/ptrace.h>
#include <sys/types.h>
#include <sys/user.h>
#include <sys/syscall.h>
#include <sys/time.h>
#include <sys/resource.h>
#include <sys/signal.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <assert.h>
#include <errno.h>

#ifdef __i386
#define REG_SYSCALL orig_eax
#define REG_RET eax
#define REG_ARG0 ebx
#define REG_ARG1 ecx
#else
#define REG_SYSCALL orig_rax
#define REG_RET rax
#define REG_ARG0 rdi
#define REG_ARG1 rsi
#endif

// Common Definitions
#define USERNAME "judge"
#define USERID 1001
#define USERHOME "/home/" USERNAME
extern char *workdir;


/****************************
*   Environment Prepares   *
****************************/

bool kill_ptrace();

pid_t setup_sandbox(
    rlim_t mem, rlim_t time, rlim_t proc,
    bool to_chroot, const char *chdir,
    bool to_ptrace,
    const char *argv,
    FILE **std_in, FILE **std_out, FILE **std_err
);


/****************************
 *    Sandbox Setting up    *
 ****************************/

extern bool use_ptrace;

// Switch the uid to `USERID`
bool switch_uid();

// Set Memory Limit
bool limit_memory(rlim_t mem);

// Set CPU Time Limit
bool limit_time(rlim_t time);

// Set Process Limit
bool limit_proc(rlim_t proc);

// Setup PTRACE
bool set_ptrace();

// Watch for PTRACE
void watch_ptrace();

// Setup chroot
bool set_chroot(const char *to_chdir);

// Solve arguments
bool solve_arg(char *arg);
