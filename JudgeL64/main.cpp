// JudgeL64
// Copied from HUSTOJ
// GNU GPLv2

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

#define IGNORE_ESOL   //ignore the ending space char of lines while comparing
#define STD_MB 1048576LL
#define STD_T_LIM 2
#define STD_F_LIM (STD_MB<<5)  //default file size limit 32m ,2^5=32
#define STD_M_LIM (STD_MB<<7)  //default memory limit 128m ,2^7=128
#define BUFFER_SIZE 5120       //default size of char buffer 5120 bytes

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

/**/

#define USERNAME "judge"
#define USERID 1001

const char *HELP = 
"Judge Sandbox version 1.0\n"
"usage: JudgeL64 [OPTIONS] FILE\n"
"\n"
"Avaliable Options: \n"
"  -m128    128M Memory Limit\n"
"  -t1      1s Time Limit\n"
"  -p1      1 Proc Count Limit\n"
"  -ptrace  Trace System Calls\n"
"  -chroot  Change Default Root\n"
"\n"
"NOTICE: All ENV will be passed!\n"
"        UID will be set to " USERNAME "'s\n"
"        NEED all param in order.\n"
"\n";

// Switch the uid to `USERID`
inline bool switch_uid()
{
	setreuid(USERID, USERID);
	uid_t r, e, s;
	getresuid(&r, &e, &s);
	if (r && e && s) return true;
	else exit(-1);
}

// Set Memory Limit
inline bool limit_memory(rlim_t mem)
{
	rlimit limits;
	limits.rlim_cur = limits.rlim_max = mem << 20;
	int ret = setrlimit(RLIMIT_DATA, &limits);
	if (ret != 0) exit(ret);
	ret = setrlimit(RLIMIT_STACK, &limits);
	if (ret != 0) exit(ret);
	limits.rlim_cur = limits.rlim_max = 0;
	ret = setrlimit(RLIMIT_AS, &limits);
	if (ret == 0) return true;
	else exit(ret);
}

// Set CPU Time Limit
inline bool limit_time(rlim_t time)
{
	rlimit limits;
	limits.rlim_cur = limits.rlim_max = time;
	int ret = setrlimit(RLIMIT_CPU, &limits);
	if (ret == 0) return true;
	else exit(ret);
}

// Set Process Limit
inline bool limit_proc(rlim_t proc)
{
	rlimit limits;
	limits.rlim_cur = limits.rlim_max = proc;
	int ret = setrlimit(RLIMIT_NPROC, &limits);
	if (ret != 0) exit(ret);
	limits.rlim_cur = limits.rlim_max = 0;
	ret = setrlimit(RLIMIT_CORE, &limits);
	if (ret == 0) return true;
	else exit(ret);
}

// Setup PTRACE
bool set_ptrace()
{
	return false;
}

// Setup chroot
bool set_chroot()
{
	chroot(".");
	return true;
}

// Solve arguments
bool solve_arg(char *arg)
{
	ulong tmp;
	printf("Solving %s..\n", arg);

	// Detect whether flags
	if (strcmp(arg, "-ptrace") == 0)
		return set_ptrace();
	else if (strcmp(arg, "-chroot") == 0)
		return set_chroot();
	else if (sscanf(arg, "-m%u", &tmp) == 1)
		return limit_memory(tmp);
	else if (sscanf(arg, "-t%u", &tmp) == 1)
		return limit_time(tmp);
	else if (sscanf(arg, "-p%u", &tmp) == 1)
		return limit_proc(tmp);

	// Indicates left things are args
	return false;
}

int main(int argc, char **argv)
{
	// Security Checks
	if (getuid() != 0)
	{
		printf("Need root permission.\n");
		return -1;
	}

	if (argc <= 1)
	{
		printf(HELP);
		return 0;
	}

	int argf = 1;
	for (; argf <= argc && solve_arg(argv[argf]); argf++);
	switch_uid();
	
	int new_argc = argc - argf + 2;
	char *new_argv[new_argc];
	for (int i = 0; i < new_argc - 1; i++)
		new_argv[i] = argv[argf + i];
	new_argv[new_argc - 1] = NULL;
	execv(new_argv[0], new_argv);
    return 0;
}