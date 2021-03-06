// stdafx.h : 标准系统包含文件的包含文件，
// 或是经常使用但不常更改的
// 特定于项目的包含文件
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // 从 Windows 头中排除极少使用的资料
// Windows 头文件: 
#include <windows.h>



// TODO: 在此处引用程序需要的其他头文件

typedef struct
{
	HANDLE hJob;
	HANDLE hStdIn;
	HANDLE hStdOut;
	HANDLE hStdErr;
	LPVOID pEnv;
	LPCWSTR pwzExe;
	LPCWSTR pwzCmd;
	LPCWSTR pwzDir;
} JUDGE_INFORMATION, *PJUDGE_INFORMATION;

SIZE_T __stdcall PeakProcessMemoryInfo(HANDLE hProcess);
HANDLE __stdcall SetupSandbox(DWORD dwMemoryLimit, DWORD dwCPUTime);
void __stdcall UnsetSandbox(HANDLE hJob);
DWORD __stdcall CreateJudgeProcess(PJUDGE_INFORMATION pInfo);
