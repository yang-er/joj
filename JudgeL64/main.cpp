#include "stdafx.h"

const char *HELP = 
"Judge Sandbox version 1.0\n"
"usage: JudgeL64 [OPTIONS] FILE\n"
"       JudgeL64 -init\n"
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
;

bool solve_arg(char *arg)
{
	ulong tmp;
	//printf("Solving %s..\n", arg);

	// Detect whether flags
	if (strcmp(arg, "-ptrace") == 0)
		return set_ptrace();
	else if (strcmp(arg, "-chroot") == 0)
		return set_chroot(NULL);
	else if (sscanf(arg, "-m%lu", &tmp) == 1)
		return limit_memory(tmp);
	else if (sscanf(arg, "-t%lu", &tmp) == 1)
		return limit_time(tmp);
	else if (sscanf(arg, "-p%lu", &tmp) == 1)
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
		puts(HELP);
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
	return execv(new_argv[0], new_argv);
}
