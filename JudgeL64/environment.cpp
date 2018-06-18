#include "stdafx.h"
#include "trace_call.h"

pid_t setup_sandbox(
    rlim_t mem, rlim_t time, rlim_t proc,
    bool to_chroot, const char *to_chdir, bool to_ptrace,
    const char *fn, char* const argv[], char* const envp[],
	bool redIn, bool redOut, bool redErr,
    int *std_in, int *std_out, int *std_err
  )
{
	if (std_in == NULL || std_out == NULL || std_err == NULL)
	{
		errno = EINVAL;
	}

	if (access(fn, X_OK) != 0) return -1;

	// Redirect stdio by pipe2
	int stdinFds[2] = { -1, -1 }, stdoutFds[2] = { -1, -1 }, stderrFds[2] = { -1, -1 };
	if ((redIn  && create_pipe(stdinFds) != 0) ||
		(redOut && create_pipe(stdoutFds) != 0) ||
		(redErr && create_pipe(stderrFds) != 0))
	{
		fprintf(stderr, "pipe redirect failed.\n");
		close_pipe_if_open(stdinFds[READ_END_OF_PIPE]);
		close_pipe_if_open(stdinFds[WRITE_END_OF_PIPE]);
		close_pipe_if_open(stdoutFds[WRITE_END_OF_PIPE]);
		close_pipe_if_open(stdoutFds[READ_END_OF_PIPE]);
		close_pipe_if_open(stderrFds[WRITE_END_OF_PIPE]);
		close_pipe_if_open(stderrFds[READ_END_OF_PIPE]);
		return -1;
	}

    pid_t sub_proc = fork();

    if (sub_proc == 0) // This means, currently running in child process.
    {
        // Redirect stdio
		if ((redIn && dup2(stdinFds[READ_END_OF_PIPE], STDIN_FILENO) == -1) ||
			(redOut && dup2(stdoutFds[WRITE_END_OF_PIPE], STDOUT_FILENO) == -1) ||
			(redErr && dup2(stderrFds[WRITE_END_OF_PIPE], STDERR_FILENO) == -1))
		{
			_exit(errno);
		}

		// Setting limit
        limit_memory(mem);
        limit_time(time / 1000 + 1);
        limit_proc(proc);
		if (to_chroot) chroot(USERHOME);
		chdir(to_chdir ? to_chdir : "/");
        if (to_ptrace)
			ptrace(PTRACE_TRACEME, 0, NULL, NULL);

		// bye
		execve(fn, argv, envp);
		exit(-1);
    }
    else if (sub_proc > 0) // This means, currently running in parent process.
    {
		if (to_ptrace)
		{
			use_ptrace = true;
			ptrace(PTRACE_SETOPTIONS, sub_proc, NULL, 
				PTRACE_O_TRACESYSGOOD | PTRACE_O_TRACEEXIT);
		}
		else
		{
			use_ptrace = false;
		}

		*std_in = stdinFds[WRITE_END_OF_PIPE];
		*std_out = stdoutFds[READ_END_OF_PIPE];
		*std_err = stderrFds[READ_END_OF_PIPE];

		// Parent doesn't need
		close_pipe_if_open(stdinFds[READ_END_OF_PIPE]);
		close_pipe_if_open(stdoutFds[WRITE_END_OF_PIPE]);
		close_pipe_if_open(stderrFds[WRITE_END_OF_PIPE]);

		return sub_proc;
    }
	else
	{
		*std_in = -1;
		*std_out = -1;
		*std_err = -1;
		return -1;
	}
}

// Thanks to hustoj
int get_proc_status(pid_t pid, const char *mark)
{
	FILE * pf;
	char fn[96], buf[150];
	int ret = 0;
	sprintf(fn, "/proc/%d/status", pid);
	pf = fopen(fn, "re");
	int m = strlen(mark);
	while (pf && fgets(buf, 149, pf))
	{
		buf[strlen(buf) - 1] = 0;
		if (strncmp(buf, mark, m) == 0)
			sscanf(buf + m + 1, "%d", &ret);
	}
	if (pf) fclose(pf);
	return ret;
}

// Thanks to hustoj

int get_miliseconds(timeval &r)
{
	return r.tv_sec * 1000 + r.tv_usec / 1000;
}

// Thanks to hustoj
void watch_sandbox(
	rlim_t _mem, rlim_t _time, pid_t app,
	int *max_mem, int *max_time, int *exitcode, bool pf
  )
{
	init_syscalls_limits(ok_call_cpp);
	int status, sig, tempmemory;
	user_regs_struct reg;
	rusage ruse;
	uint call_id;
	bool first = true;
	time_t p = time(NULL);

	while (true)
	{
		fprintf(stderr, "before wait4\n");
		wait4(app, &status, __WALL, &ruse);
		
		printf("after wait4");
		if (first)
		{
			ptrace(PTRACE_SETOPTIONS, app, NULL,
				PTRACE_O_TRACESYSGOOD | PTRACE_O_TRACEEXIT);
			first = false;
		}
		
		if (WIFEXITED(status)) break;
		*exitcode = WEXITSTATUS(status);

		printf("WEXITSTATUS\n");
		tempmemory = pf ? ruse.ru_minflt * getpagesize() 
			: get_proc_status(app, "VmPeak:") << 10;

		if (tempmemory > *max_mem)
			*max_mem = tempmemory;

		if (*max_mem > _mem << 20)
		{
			unset_sandbox(app);
			break;
		}

		if (time(NULL) > p + _time / 100)
		{
			unset_sandbox(app);
			break;
		}

		if (*exitcode != 5 && *exitcode != 0 && *exitcode != 133)
		{
			fprintf(stderr, "ExitSignal: %s\n", strsignal(*exitcode));
			unset_sandbox(app);
			break;
		}

		printf("PTRACE_GETREGS\n");
		ptrace(PTRACE_GETREGS, app, NULL, &reg);
		call_id = (unsigned int)reg.REG_SYSCALL % call_array_size;

		if (call_counter[call_id])
		{
			//call_counter[reg.REG_SYSCALL]--;
		}
		else if (record_call)
		{
			call_counter[call_id] = 1;
		}
		else
		{
			fprintf(stderr, "Not allowed syscall: %d.\n", call_id);
			unset_sandbox(app);
			break;
		}

		printf("PTRACE_SYSCALL\n");
		ptrace(PTRACE_SYSCALL, app, NULL, NULL);
		printf("next round\n");
	}
}

// Thanks to hustoj
void unset_sandbox(pid_t app)
{
	ptrace(PTRACE_KILL, app, NULL, NULL);
}
