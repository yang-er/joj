#include "stdafx.h"

bool switch_uid()
{
	uid_t r, e, s;
	setresgid(USERID, USERID, USERID);
	getresgid(&r, &e, &s);
	if (!r || !e || !s) exit(-1);
	setreuid(USERID, USERID);
	getresuid(&r, &e, &s);
	if (!r || !e || !s) exit(-1);
	return true;
}

bool limit_memory(rlim_t mem)
{
	rlimit limits;
	limits.rlim_cur = limits.rlim_max = rlim_t((mem << 20) * 1.1);
	int ret = setrlimit(RLIMIT_DATA, &limits);
	if (ret != 0) exit(ret);
	ret = setrlimit(RLIMIT_STACK, &limits);
	if (ret != 0) exit(ret);
	//limits.rlim_cur = limits.rlim_max = 0;
	ret = setrlimit(RLIMIT_AS, &limits);
	if (ret == 0) return true;
	else exit(ret);
}

bool limit_time(rlim_t time)
{
	rlimit limits;
	limits.rlim_cur = limits.rlim_max = time;
	int ret = setrlimit(RLIMIT_CPU, &limits);
	alarm(0);
	alarm(time);
	if (ret == 0) return true;
	else exit(ret);
}

bool limit_proc(rlim_t proc)
{
	rlimit limits;
	limits.rlim_cur = limits.rlim_max = proc;
	int ret = setrlimit(RLIMIT_NPROC, &limits);
	if (ret != 0) exit(ret);
	limits.rlim_cur = limits.rlim_max = 0;
	ret = setrlimit(RLIMIT_CORE, &limits);
	if (ret == 0) return true;
	else exit(ret);
}

bool set_chroot(const char *to_chdir)
{
	chroot("/home/judge");
	chdir(to_chdir ? to_chdir : "/");
	return true;
}
