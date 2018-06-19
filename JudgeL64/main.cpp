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
"  -ptrace  Trace System Calls\n"
"  -chroot  Change Default Root\n"
"\n"
"NOTICE: All ENV will be passed!\n"
"        UID will be set to " USERNAME "'s\n"
"        NEED all param in order.\n"
;

sandbox_args::sandbox_args()
{
	ok_calls = NULL;
	mem = proc = time = 0;
	chroot = ptrace = false;
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
			ret->ok_calls = ok_call_langs[tmp];
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

	/*
	// Redirect stdio by pipe2
	int stdinFds[2] = { -1, -1 }, stdoutFds[2] = { -1, -1 }, stderrFds[2] = { -1, -1 };
	if ((create_pipe(stdinFds) != 0) ||
		(create_pipe(stdoutFds) != 0) ||
		(create_pipe(stderrFds) != 0))
	{
		fprintf(stderr, "pipe redirect failed.\n");
		close_pipe_if_open(stdinFds[READ_END_OF_PIPE]);
		close_pipe_if_open(stdinFds[WRITE_END_OF_PIPE]);
		close_pipe_if_open(stdoutFds[WRITE_END_OF_PIPE]);
		close_pipe_if_open(stdoutFds[READ_END_OF_PIPE]);
		close_pipe_if_open(stderrFds[WRITE_END_OF_PIPE]);
		close_pipe_if_open(stderrFds[READ_END_OF_PIPE]);
		return EPIPE;
	}*/

	pid_t child = fork();
	if (child == 0)
	{
		/*
		// Redirect stdio
		if ((dup2(stdinFds[READ_END_OF_PIPE], STDIN_FILENO) == -1) ||
			(dup2(stdoutFds[WRITE_END_OF_PIPE], STDOUT_FILENO) == -1) ||
			(dup2(stderrFds[WRITE_END_OF_PIPE], STDERR_FILENO) == -1))
		{
			_exit(errno);
		}*/

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
		/*
		dup2(stdinFds[READ_END_OF_PIPE], STDIN_FILENO);

		*std_in = stdinFds[WRITE_END_OF_PIPE];
		*std_out = stdoutFds[READ_END_OF_PIPE];
		*std_err = stderrFds[READ_END_OF_PIPE];

		// Parent doesn't need
		close_pipe_if_open(stdinFds[READ_END_OF_PIPE]);
		close_pipe_if_open(stdoutFds[WRITE_END_OF_PIPE]);
		close_pipe_if_open(stderrFds[WRITE_END_OF_PIPE]);*/

		if (my_args.ptrace)
		{
			int max_mem, max_time, exit_code;
			watch_sandbox(
				my_args.mem, my_args.time, child, my_args.ptrace,
				&max_mem, &max_time, &exit_code, false
			);
			fprintf(stderr, "Mem: %d B, Time: %d, ExitCode: %x\n", max_mem, max_time, exit_code);
			return WEXITSTATUS(exit_code);
		}
		else
		{
			int wait_status;
			waitpid(child, &wait_status, 0);
			return wait_status;
		}
	}
	else
	{
		return EINVAL;
	}
}
