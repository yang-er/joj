#include "stdafx.h"

pid_t setup_sandbox(
        rlim_t mem, rlim_t time, rlim_t proc,
        bool to_chroot, const char *to_chdir,
        bool to_ptrace,
        const char *argv,
        FILE **std_in, FILE **std_out, FILE **std_err
    )
{
    pid_t sub_proc = fork();
    if (sub_proc == 0)
    {
        // This means, currently running in child process.
        limit_memory(mem);
        limit_time(time);
        limit_proc(proc);
        if (to_chroot)
            set_chroot(to_chdir);
        ptrace(PTRACE_TRACEME, 0, NULL, NULL);
    }
    else
    {
        // This means, currently running in parent process.
    }
}

