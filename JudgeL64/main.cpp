#include "stdafx.h"
#include "trace_call.h"

const char *HELP = 
"Judge Sandbox version 1.0\n"
"usage: JudgeL64 [OPTIONS] FILE\n"
"       JudgeL64 -init\n"
"\n"
"Avaliable Options: \n"
"  -m128    128M Memory Limit\n"
"  -t1000   1000ms Time Limit\n"
"  -p1      1 Proc Count Limit\n"
"  -l0      The 0th lang syscalls\n"
"  -s/pipe  Send info in named pipe\n"
"  -ptrace  Trace System Calls\n"
"  -chroot  Change Default Root\n"
"\n"
"NOTICE: All ENV will be passed!\n"
"        UID will be set to " USERNAME "'s\n"
;

FILE *stdprn;

sandbox_args::sandbox_args()
{
	ok_calls = 0;
	pipe_name = NULL;
	mem = proc = time = 0;
	chroot = ptrace = page_fault = false;
}

sandbox_stat::sandbox_stat()
{
	exitcode = 0;
	max_mem = max_time = 0;
}

bool solve_arg(int argc, char **argv, sandbox_args *ret)
{
	ulong tmp;
	int &argf = ret->argf;

	for (argf = 1; argf <= argc; argf++)
	{
		if (strcmp(argv[argf], "-ptrace") == 0)
			ret->ptrace = true;
		else if (strcmp(argv[argf], "-chroot") == 0)
			ret->chroot = true;
		else if (sscanf(argv[argf], "-m%lu", &tmp) == 1)
			ret->mem = tmp;
		else if (sscanf(argv[argf], "-t%lu", &tmp) == 1)
			ret->time = tmp;
		else if (sscanf(argv[argf], "-p%lu", &tmp) == 1)
			ret->proc = tmp;
		else if (sscanf(argv[argf], "-l%lu", &tmp) == 1)
			ret->ok_calls = int(tmp);
		else if (strstr(argv[argf], "-s/") == argv[argf])
			ret->pipe_name = argv[argf] + 2;
		else
			break;
	}

	if (argf > argc) return false;
	return true;
}

int main(int argc, char **argv)
{
	// Security Checks
	if (getuid() != 0)
	{
		fprintf(stderr, "Need root permission.\n");
		return EACCES;
	}

	if (argc <= 1)
	{
		puts(HELP);
		return 0;
	}

	sandbox_args my_args;
	if (!solve_arg(argc, argv, &my_args))
		return EINVAL;
	if (my_args.chroot)
	{
		char buff[256];
		sprintf(buff, "%s%s", USERHOME, argv[my_args.argf]);
		if (access(buff, X_OK) != 0)
			return ENOENT;
	}
	else if (access(argv[my_args.argf], X_OK) != 0)
		return ENOENT;

	pid_t child = fork();
	if (child == 0)
	{
		// Setting limit
		if (my_args.mem)
			limit_memory(my_args.mem);
		if (my_args.time)
			limit_time(my_args.time / 1000 + 1);
		if (my_args.proc)
			limit_proc(my_args.proc);
		if (my_args.chroot)
			set_chroot("/");
		switch_uid();
		ptrace(PTRACE_TRACEME, 0, NULL, NULL);

		// bye
		execv(argv[my_args.argf], argv + my_args.argf);
		_exit(ECHILD);
	}
	else if (child > 0)
	{
		if (my_args.pipe_name)
			stdprn = fopen(my_args.pipe_name, "w");
		else if (true)
			stdprn = stderr;
		else
			stdprn = fopen("/dev/null", "w");
		sandbox_stat stats;
		watch_sandbox(my_args, child, &stats);
		fprintf(stdprn,
			"Mem: %d B, Time: %d, ExitCode: 0x%x\n",
			stats.max_mem, stats.max_time, stats.exitcode);
		return stats.exitcode;
	}
	else
	{
		return EINVAL;
	}
}
