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

#define DLL extern "C"

/****************************
 *    Common Definitions    *
 ****************************/
// Env adapt
#define USERNAME "judge"
#define USERID 1001
#define USERHOME "/home/" USERNAME
extern char *workdir;

// Register
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

// Status
#define JOJ_AC  0
#define JOJ_WA  1
#define JOJ_TLE 2
#define JOJ_MLE 3
#define JOJ_OLE 4
#define JOJ_RE  5
#define JOJ_CE  6
#define JOJ_PE  7
#define JOJ_PND 8
#define JOJ_RUN 9
#define JOJ_UE  10

/****************************
*   Environment Prepares   *
****************************/

// Watch for PTRACE
DLL void watch_sandbox(
	rlim_t mem, rlim_t time, pid_t app,
	int *max_mem, int *max_time, int *exitcode, bool pf
);

// Unset sandbox
DLL void unset_sandbox(pid_t app);

// Setup a sandbox
DLL pid_t setup_sandbox(
	rlim_t mem, rlim_t time, rlim_t proc,
	bool to_chroot, const char *to_chdir, bool to_ptrace,
	const char *fn, char* const argv[], char* const envp[],
	bool redIn, bool redOut, bool redErr,
	int *std_in, int *std_out, int *std_err
);


/****************************
 *    Sandbox Setting up    *
 ****************************/

DLL bool use_ptrace;

// Switch the uid to `USERID`
DLL bool switch_uid();

// Set Memory Limit
DLL bool limit_memory(rlim_t mem);

// Set CPU Time Limit
DLL bool limit_time(rlim_t time);

// Set Process Limit
DLL bool limit_proc(rlim_t proc);

// Setup PTRACE
DLL bool set_ptrace();

// Setup chroot
DLL bool set_chroot(const char *to_chdir);

// Solve arguments
DLL bool solve_arg(char *arg);
