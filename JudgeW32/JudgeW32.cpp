// JudgeW32.cpp: 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"

#include <jobapi2.h>
#include <Psapi.h>
#include <WerApi.h>
#include <string>
#pragma comment(lib, "wer.lib")

SIZE_T __stdcall PeakProcessMemoryInfo(HANDLE hProcess)
{
	PROCESS_MEMORY_COUNTERS pmc;
	memset(&pmc, 0, sizeof(pmc));
	pmc.cb = sizeof(pmc);
	if (K32GetProcessMemoryInfo(hProcess, &pmc, pmc.cb) == TRUE)
		return pmc.PeakWorkingSetSize;
	else
		return 0;
}

BOOL __stdcall CreateStdPipe(PHANDLE phParent, PHANDLE phChild, BOOL bInput)
{
	SECURITY_ATTRIBUTES sa;
	sa.bInheritHandle = TRUE;
	sa.nLength = sizeof(sa);
	sa.lpSecurityDescriptor = NULL;

	BOOL ret;
	HANDLE hTmp;
	if (bInput == TRUE)
	{
		ret = CreatePipe(phChild, &hTmp, &sa, 128 * 1024);
	}
	else
	{
		ret = CreatePipe(&hTmp, phChild, &sa, 128 * 1024);
	}
	if (ret == FALSE) return FALSE;

	if (!DuplicateHandle(GetCurrentProcess(), hTmp, GetCurrentProcess(), phParent,
		0, FALSE, DUPLICATE_SAME_ACCESS)) return FALSE;
	CloseHandle(hTmp);
	return TRUE;
}

DWORD __stdcall CreateJudgeProcess(PJUDGE_INFORMATION pInfo)
{
	STARTUPINFOW si;
	memset(&si, 0, sizeof(si));
	si.cb = sizeof(si);
	si.dwFlags = STARTF_USESTDHANDLES;
	
	CreateStdPipe(&pInfo->hStdIn, &si.hStdInput, TRUE);
	CreateStdPipe(&pInfo->hStdOut, &si.hStdOutput, FALSE);
	si.hStdError = GetStdHandle(-12);

	DWORD crFlag = CREATE_NO_WINDOW | CREATE_UNICODE_ENVIRONMENT | CREATE_SUSPENDED; // Sandbox

	PROCESS_INFORMATION pi;
	memset(&pi, 0, sizeof(pi));

	size_t len = wcslen(pInfo->pwzCmd) + wcslen(pInfo->pwzExe) + 2;
	PWSTR pwCmdLine = new WCHAR[len];
	pwCmdLine[0] = L'\0';
	wcscat_s(pwCmdLine, len, pInfo->pwzExe);
	wcscat_s(pwCmdLine, len, L" ");
	wcscat_s(pwCmdLine, len, pInfo->pwzCmd);

	// HANDLE hToken;
	// LogonUserW(L"Judge", NULL, L"123456", 0, 0, &hToken);

	WerAddExcludedApplication(pInfo->pwzExe, false);
	// CreateProcessAsUserW(hToken, NULL, pwCmdLine, NULL, NULL, TRUE,
	CreateProcessW(NULL, pwCmdLine, NULL, NULL, TRUE,
		crFlag, pInfo->pEnv, pInfo->pwzDir, &si, &pi);
	AssignProcessToJobObject(pInfo->hJob, pi.hProcess);
	ResumeThread(pi.hThread);
	CloseHandle(pi.hProcess);
	CloseHandle(pi.hThread);
	return pi.dwProcessId;
}

HANDLE __stdcall SetupSandbox(DWORD dwMemoryLimit, DWORD dwCPUTime, DWORD dwProcessCount)
{
	HANDLE hJob = CreateJobObjectW(NULL, NULL);
	BOOL result;

	JOBOBJECT_EXTENDED_LIMIT_INFORMATION Job_Limit;
	ZeroMemory(&Job_Limit, sizeof(Job_Limit));
	Job_Limit.BasicLimitInformation.LimitFlags =
		JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE |
		JOB_OBJECT_LIMIT_DIE_ON_UNHANDLED_EXCEPTION |
		// JOB_OBJECT_LIMIT_PROCESS_TIME |
		// JOB_OBJECT_LIMIT_WORKINGSET | // This leads to parament wrong
		JOB_OBJECT_LIMIT_ACTIVE_PROCESS;

	Job_Limit.BasicLimitInformation.PerProcessUserTimeLimit.QuadPart = 100000 * dwCPUTime;
	Job_Limit.BasicLimitInformation.MinimumWorkingSetSize = 1;
	Job_Limit.BasicLimitInformation.MaximumWorkingSetSize = 1024 * 1280 * dwMemoryLimit;
	Job_Limit.BasicLimitInformation.ActiveProcessLimit = dwProcessCount;
	result = SetInformationJobObject(hJob, JobObjectExtendedLimitInformation, &Job_Limit, sizeof(Job_Limit));
	// MessageBoxW(NULL, std::to_wstring(GetLastError()).c_str(), L"", 0);
	if (result == FALSE) fprintf(stderr, "JobObjectBasicLimitInformation failed to set up, 0x%x.\n", GetLastError());

	JOBOBJECT_BASIC_UI_RESTRICTIONS Job_UI_Limit;
	Job_UI_Limit.UIRestrictionsClass = JOB_OBJECT_UILIMIT_EXITWINDOWS |
		JOB_OBJECT_UILIMIT_READCLIPBOARD |
		JOB_OBJECT_UILIMIT_WRITECLIPBOARD |
		JOB_OBJECT_UILIMIT_SYSTEMPARAMETERS |
#ifndef NANO_SERVER
		JOB_OBJECT_UILIMIT_DISPLAYSETTINGS |
		JOB_OBJECT_UILIMIT_DESKTOP |
#endif
		JOB_OBJECT_UILIMIT_GLOBALATOMS |
		JOB_OBJECT_UILIMIT_HANDLES;
	result = SetInformationJobObject(hJob, JobObjectBasicUIRestrictions, &Job_UI_Limit, sizeof(Job_UI_Limit));
    // MessageBoxW(NULL, std::to_wstring(GetLastError()).c_str(), L"", 0);
	if (result == FALSE) fprintf(stderr, "JobObjectBasicUIRestrictions failed to set up, 0x%x.\n", GetLastError());

	return hJob;
}

void __stdcall UnsetSandbox(HANDLE hJob)
{
	TerminateJobObject(hJob, ~1);
	CloseHandle(hJob);
}
