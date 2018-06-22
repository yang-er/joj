#include "stdafx.h"
#include "trace_call.h"
#include <vector>
#include <algorithm>
using std::vector;
using std::binary_search;

// Thanks to hustoj
int get_proc_status(pid_t pid, const char *mark)
{
	FILE * pf;
	char fn[96], buf[150];
	int ret = 0;
	sprintf(fn, "/proc/%d/status", pid);
	pf = fopen(fn, "re");
	size_t m = strlen(mark);
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
ulong get_miliseconds(timeval &r)
{
	return r.tv_sec * 1000 + r.tv_usec / 1000;
}

// Thanks to hustoj
void watch_sandbox(
	const sandbox_args &args,
	pid_t app,
	sandbox_stat *stats )
{
	init_syscalls_limits(ok_call_langs[args.ok_calls]);
	int status;
	uint64_t write_len = 0;
	ulong tempmemory;
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
			first = false;
		}
		
		stats->exitcode = WEXITSTATUS(status);
		if (WIFEXITED(status)) break;

		tempmemory = args.page_fault ? ruse.ru_minflt * getpagesize() 
			: get_proc_status(app, "VmSize:") << 10;

		if (tempmemory > stats->max_mem)
			stats->max_mem = tempmemory;

		if (args.mem && stats->max_mem > (args.mem << 20))
		{
			unset_sandbox(app);
			stats->exitcode = SIGUSR2;
			break;
		}

		if (args.time && ulong(time(NULL) - p) > args.time / 300)
		{
			fprintf(stdprn, "run out of 2nd limit.\n");
			unset_sandbox(app);
			stats->exitcode = SIGXCPU;
			break;
		}

		stats->max_time = get_miliseconds(ruse.ru_utime);
		if (stats->max_time > args.time)
		{
			fprintf(stdprn, "run out of 1st limit.\n");
			unset_sandbox(app);
			stats->exitcode = SIGXCPU;
			break;
		}

		if (stats->exitcode != 5 && stats->exitcode != 0 && stats->exitcode != 133 && (args.ptrace && stats->exitcode != 17))
		{
			switch (stats->exitcode)
			{
				case SIGCHLD:
				case SIGALRM:
					alarm(0);
				default:
					break;
			}
			fprintf(stdprn, "ExitSignal: %s\n", strsignal(stats->exitcode));
			unset_sandbox(app);
			break;
		}

		if (args.ptrace)
		{
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
				fprintf(stdprn, "Not allowed syscall: %d.\n", call_id);
				stats->exitcode = SIGSYS;
				unset_sandbox(app);
				break;
			}

			if (call_id == SYS_write)
			{
				write_len += reg.REG_RET;
				if (write_len > LIM_FILE_SIZE)
				{
					stats->exitcode = SIGXFSZ;
					unset_sandbox(app);
					break;
				}
			}
		}

		ptrace(PTRACE_SYSCALL, app, NULL, NULL);
	}
}

// Thanks to hustoj
void unset_sandbox(pid_t app)
{
	kill_proc_tree(app, SIGKILL, true);
	ptrace(PTRACE_KILL, app, NULL, NULL);
}

int read_proc_tree(pid_t begins, vector<pid_t> &to_kill)
{
    DIR *dp = opendir("/proc");
    if (dp == NULL) return -1;
    dirent *dr;
    pid_t temp;
    while (dr = readdir(dp))
    {
        if (dr->d_type != 4)
            continue;
        else if (sscanf(dr->d_name, "%d", &temp) != 1)
            continue;
        else if (temp < begins)
            continue;
        else if (temp == begins)
            to_kill.push_back(begins);
        else if (binary_search(to_kill.begin(), to_kill.end(), get_proc_status(temp, "PPid:")))
            to_kill.push_back(temp);
    }
}

int kill_proc_tree(pid_t pid, int sig, bool kip)
{
    vector<pid_t> to_kill;
    read_proc_tree(pid, to_kill);

    for (pid_t cur : to_kill)
    {
		if (kip && cur == pid)
			continue;
        fprintf(stdprn, "%d ", cur);
        kill(cur, sig);
    }

    fprintf(stdprn, "SIG%d sent.\n", sig);
    return 0;
}
