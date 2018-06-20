// Data are from HUSTOJ
#include "trace_call.h"
#define HOJ_MAX_LIMIT -1

unsigned int call_id = 0;
unsigned int call_counter[call_array_size] = { 0 };

// OK Call list
int ok_calls_group_io[] =
{
	SYS_read,
	SYS_write,
	SYS_open,
	SYS_close,
	SYS_stat,
	SYS_fstat,
	SYS_lseek,
	SYS_ioctl,
	SYS_writev,
	SYS_access,
	SYS_readlink,
	SYS_ioprio_get,
};

int ok_calls_group_mem[] = 
{
	SYS_mmap,
	SYS_munmap,
	SYS_mprotect,
	SYS_brk,
};

int ok_calls_group_internal[] =
{
	SYS_execve,
	SYS_uname,
	SYS_arch_prctl,
	SYS_exit_group,
	SYS_mq_open,
	SYS_unshare,
	SYS_set_thread_area,
	SYS_time,
};

int ok_call_cpp[256] = 
{
	SYS_set_robust_list,
	SYS_openat,
	511, 0
};

int ok_call_pascal[256] =
{
	SYS_rt_sigaction,
	SYS_arch_prctl,
	SYS_getxattr,
	SYS_getrlimit,
	511, 0
};

int ok_call_java[256] =
{
	SYS_lstat,
	SYS_rt_sigprocmask,
	SYS_rt_sigreturn,
	SYS_pipe,
	SYS_dup2,
	SYS_getpid,
	SYS_clone,
	SYS_wait4,
	SYS_getcwd,
	SYS_getrlimit,
	SYS_getgid,
	SYS_getppid,
	SYS_getpgrp,
	SYS_futex,
	SYS_set_tid_address,
	SYS_openat,
	SYS_set_robust_list,
	SYS_prlimit64,
	SYS_fcntl,
	SYS_getdents64,
	SYS_getuid,
	SYS_geteuid,
	0
};

int ok_call_python[256] =
{
	SYS_lstat,
	SYS_dup,
	SYS_getpid,
	SYS_socket,
	SYS_connect,
	SYS_exit,
	SYS_sysinfo,
	SYS_getuid,
	SYS_setresuid,
	SYS_sigaltstack,
	SYS_sched_get_priority_max,
	SYS_arch_prctl,
	SYS_getxattr,
	SYS_futex,
	SYS_openat,
	SYS_prlimit64,
	SYS_mremap,
	SYS_fcntl,
	SYS_fstat,
	SYS_getcwd,
	SYS_getdents,
	SYS_getegid,
	SYS_geteuid,
	SYS_getgid,
	SYS_getrlimit,
	SYS_getuid,     
	SYS_rt_sigaction,
	SYS_rt_sigprocmask,
	SYS_set_robust_list,
	SYS_set_tid_address,
	0
};

int *ok_call_langs[32] = {
	ok_call_cpp,
	ok_call_pascal,
	ok_call_java,
	ok_call_python,
	NULL
};

template<int N>
void init_syscall_group(int (&src)[N])
{
	for (int i = 0; i < N; i++)
		call_counter[src[i]] = HOJ_MAX_LIMIT;
}

void init_syscalls_limits(int lang[256])
{
	memset(call_counter, 0, sizeof(call_counter));
	init_syscall_group(ok_calls_group_io);
	init_syscall_group(ok_calls_group_mem);
	init_syscall_group(ok_calls_group_internal);

	for (int i = 0; i == 0 || lang[i]; i++)
		call_counter[lang[i]] = HOJ_MAX_LIMIT;
#ifdef DEBUG
	fprintf(stderr, "Syscall limited.\n");
#endif
}
