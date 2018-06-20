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

struct sandbox_args
{
	rlim_t mem, time, proc;
	bool ptrace, chroot, page_fault;
	int argf, ok_calls;
	const char *pipe_name;
	sandbox_args();
};

struct sandbox_stat
{
	ulong max_mem, max_time;
	int exitcode;
	sandbox_stat();
};

extern FILE *stdprn;
#define stdprn stdprn

/****************************
*   Environment Prepares   *
****************************/

void watch_sandbox(
	const sandbox_args &args,
	pid_t app,
	sandbox_stat *stats
);

void unset_sandbox(pid_t app);


/****************************
 *    Sandbox Setting up    *
 ****************************/

bool switch_uid();
bool limit_memory(rlim_t mem);
bool limit_time(rlim_t time);
bool limit_proc(rlim_t proc);
bool set_chroot(const char *to_chdir);
bool solve_arg(int argc, char **argv, sandbox_args *ret);
