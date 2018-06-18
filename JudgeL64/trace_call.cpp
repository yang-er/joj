// Data are from HUSTOJ
#include "trace_call.h"
#define HOJ_MAX_LIMIT -1

unsigned int call_id = 0;
unsigned int call_counter[call_array_size] = { 0 };

void init_syscalls_limits(int lang[256])
{
	memset(call_counter, 0, sizeof(call_counter));
	for (int i = 0; i == 0 || lang[i]; i++)
		call_counter[lang[i]] = HOJ_MAX_LIMIT;
#ifdef DEBUG
	fprintf(stderr, "Syscall limited.\n");
#endif
}

void close_pipe_if_open(int fd)
{
	if (fd >= 0) close(fd);
}

int32_t create_pipe(int32_t pipeFds[2])
{
	int32_t result;
	while ((result = pipe2(pipeFds, 02000000)) < 0 && errno == EINTR);
	return result;
}

// OK Call list

int ok_call_cpp[256] = {
	0, 1, 2, 3, 4, 5, 8, 9, 11, 12, 20, 21, 59, 63, 89, 158, 231, 240, 272, 511,
	SYS_time, SYS_read, SYS_uname, SYS_write, SYS_open, SYS_close, SYS_execve,
	SYS_access, SYS_brk, SYS_munmap, SYS_mprotect, SYS_mmap, SYS_fstat,
	SYS_set_thread_area, 252, SYS_arch_prctl, 0
};

int ok_call_pascal[256] = {
	0, 1, 2, 3, 4, 9, 11, 13, 16, 59, 89, 97, 158, 191, 201, 231, 252, 511,
	SYS_open, SYS_set_thread_area, SYS_brk, SYS_read, SYS_uname,
	SYS_write, SYS_execve, SYS_ioctl, SYS_readlink, SYS_mmap,
	SYS_rt_sigaction, SYS_getrlimit, SYS_close,
	SYS_exit_group, SYS_munmap, SYS_time, 0
};

int ok_call_java[256] = {
	0, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 16, 21, 22, 33, 39, 56, 59,
	61, 79, 89, 97, 104, 110, 111, 158, 202, 218, 231, 257, 273, 302,
	SYS_fcntl, SYS_getdents64, SYS_getrlimit, SYS_rt_sigprocmask, SYS_futex,
	SYS_read, SYS_mmap, SYS_stat, SYS_open, SYS_close, SYS_execve, SYS_access,
	SYS_brk, SYS_readlink, SYS_munmap, SYS_close, SYS_uname, SYS_clone,
	SYS_uname, SYS_mprotect, SYS_rt_sigaction, SYS_getrlimit, SYS_fstat,
	SYS_getuid, SYS_getgid, SYS_geteuid, SYS_getegid, SYS_set_thread_area,
	SYS_set_tid_address, SYS_set_robust_list, SYS_exit_group, 0
};

int ok_call_python[256] = {
	0, 1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 16, 21, 32, 39, 41, 42, 59, 60, 72, 78, 79,
	89, 97, 99, 102, 104, 107, 108, 117, 131, 146, 158, 191, 202, 218, 231, 257, 273, 302,
	SYS_mremap, SYS_access, SYS_arch_prctl, SYS_brk, SYS_close, SYS_execve, SYS_exit_group,
	SYS_fcntl, SYS_fstat, SYS_futex, SYS_getcwd, SYS_getdents, SYS_getegid, SYS_geteuid,
	SYS_getgid, SYS_getrlimit, SYS_getuid, SYS_ioctl, SYS_lseek, SYS_lstat, SYS_mmap, SYS_mprotect,
	SYS_munmap, SYS_open, SYS_read, SYS_readlink, SYS_rt_sigaction, SYS_rt_sigprocmask,
	SYS_set_robust_list, SYS_set_tid_address, SYS_stat, SYS_write, 0
};
