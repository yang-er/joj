#include "stdafx.h"
#include "trace_call.h"

// Obsolete
pid_t setup_sandbox(
    rlim_t mem, rlim_t time, rlim_t proc,
    bool to_chroot, const char *to_chdir,
    bool to_ptrace,
    const char *fn, const char *args,
    int *std_in, int *std_out, int *std_err )
{
    pid_t sub_proc = fork();
    if (sub_proc == 0) // This means, currently running in child process.
    {
        // Redirect stdio
		if (std_in) create_pipe(std_in, STDIN_FILENO);
		if (std_out) create_pipe(std_out, STDOUT_FILENO);
		if (std_err) create_pipe(std_err, STDERR_FILENO);

		// Setting limit
        limit_memory(mem);
        limit_time(time);
        limit_proc(proc);
        if (to_chroot)
            set_chroot(to_chdir);
        ptrace(PTRACE_TRACEME, 0, NULL, NULL);

		// parsing args
		int argc = 0;
		char *argv[32], file_name[strlen(fn)+1];
		strcpy(file_name, fn);
		argv[argc++] = file_name;

		if (args)
		{
			int len = strlen(args);
			char cp_args[len + 2], *p = cp_args;
			strcpy(cp_args, args);
			argv[argc++] = p;
			while (p = strchr(p, ' '))
			{
				if (p[1] == ' ')
				{
					p[0] = 0;
					p++;
				}
				else
				{
					p[0] = 0;
					argv[argc++] = p + 1;
				}
			}
		}

		execv(fn, argv);
		exit(-1);
    }
    else // This means, currently running in parent process.
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

		return sub_proc;
    }
}

// Obsolete
int create_pipe(int *fd, int std)
{

}

// Thanks to hustoj
int get_proc_status(int pid, const char *mark)
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
int get_page_fault_mem(struct rusage & ruse, pid_t & pidApp)
{
	return ruse.ru_minflt * getpagesize();
}

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
		wait4(app, &status, __WALL, &ruse);
		
		if (first)
		{
			ptrace(PTRACE_SETOPTIONS, app, NULL,
				PTRACE_O_TRACESYSGOOD | PTRACE_O_TRACEEXIT);
			signal(SIGCHLD, SIG_IGN);
			first = false;
		}
		
		if (WIFEXITED(status)) break;
		*exitcode = WEXITSTATUS(status);

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

		ptrace(PTRACE_SYSCALL, app, NULL, NULL);
	}
}

// Thanks to hustoj
void unset_sandbox(pid_t app)
{
	ptrace(PTRACE_KILL, app, NULL, NULL);
}
