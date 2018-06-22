#include "stdafx.h"
#include "trace_call.h"

const char *HELP = 
"Judge Sandbox version 1.0\n"
"usage: JudgeL64 std [OPTIONS] FILE\n"
"       JudgeL64 init %LANG\n"
"       JudgeL64 kill %PPID (%SIG=9)\n"
"\n"
"init Tips:\n"
"  Copy language %LANG's needed files\n"
"  to /home/" USERNAME " to prepare run-env.\n"
"\n"
"kill Tips:\n"
"  Directly send %SIG to %PPID and\n"
"  its children processes. No waitpid.\n"
"\n"
"std Avaliable Options: \n"
"  -m128    128M Memory Limit\n"
"  -t1000   1000ms Time Limit\n"
"  -p1      1 Proc Count Limit\n"
"  -l0      The 0th lang syscalls\n"
"  -s/pip   Send info in named pipe\n"
"  -pt      Trace System Calls\n"
"  -ch      Change Default Root\n"
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

	for (argf = 2; argf <= argc; argf++)
	{
		if (strcmp(argv[argf], "-pt") == 0)
			ret->ptrace = true;
		else if (strcmp(argv[argf], "-ch") == 0)
			ret->chroot = true;
		else if (sscanf(argv[argf], "-m%lu", &tmp) == 1)
			ret->mem = tmp;
		else if (sscanf(argv[argf], "-t%lu", &tmp) == 1)
			ret->time = tmp;
		else if (sscanf(argv[argf], "-p%lu", &tmp) == 1)
			ret->proc = tmp;
		else if (sscanf(argv[argf], "-l%lu", &tmp) == 1)
			ret->ok_calls = int(tmp);
		else if (strncmp(argv[argf], "-s/", 3) == 0)
			ret->pipe_name = argv[argf] + 2;
		else
			break;
	}

	if (argf > argc) return false;
	return true;
}

int modstd(int argc, char **argv)
{
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
			limit_time(my_args.time / 1000);
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
			stdprn = fopen(my_args.pipe_name, "wx");
		else if (true)
			stdprn = stderr;
		else
			stdprn = fopen("/dev/null", "w");
		sandbox_stat stats;
		watch_sandbox(my_args, child, &stats);
		fprintf(stdprn,
			"%lu %lu %d\n",
			stats.max_mem, stats.max_time, stats.exitcode);
		fclose(stdprn);
		return stats.exitcode;
	}
	else
	{
		return EINVAL;
	}
}

int modinit(int argc, char **argv)
{
	puts("Not Implemented.");
}

int modkill(int argc, char **argv)
{
	stdprn = stderr;
	int ppid, sig;
	if (sscanf(argv[2], "%d", &ppid) != 1)
		return -1;
	if (argv[3] && sscanf(argv[3], "%d", &sig) == 1);
	else sig = SIGKILL;
	bool skip_parent = argv[4] && strcmp(argv[4], "kip") == 0;
	return kill_proc_tree(ppid, sig, skip_parent);
}

int modhelp(int argc, char **argv)
{
	puts(HELP);
	return 0;
}

int main(int argc, char **argv)
{
	// Security Checks
	if (getuid() != 0)
	{
		fprintf(stderr, "Need root permission.\n");
		return EACCES;
	}

	if (argc <= 2)
		return modhelp(argc, argv);
	else if (strcmp(argv[1], "std") == 0)
		return modstd(argc, argv);
	else if (strcmp(argv[1], "init") == 0)
		return modinit(argc, argv);
	else if (strcmp(argv[1], "kill") == 0)
		return modkill(argc, argv);
	else
		return modhelp(argc, argv);
}
