<?php

/**
 * Cofigure For Database
 */

defined("IN_XYS") || die("Access Denied");

$_CONFIG['db']['host'] = 'localhost';
$_CONFIG['db']['user'] = 'judge';
$_CONFIG['db']['password'] = '2w2WXe02816e43';
$_CONFIG['db']['charset'] = 'utf8';
$_CONFIG['db']['pconnect'] = '0';
$_CONFIG['db']['name'] = 'judge';
$_CONFIG['db']['tablepre'] = '';

$GLOBALS['compilers'] = [
    0 => 'Visual C++ Compiler (v14.1)',
    1 => 'G++ (MinGW Toolchain 6.3.0)',
];

$GLOBALS['build_langs'] = [
    0 => 'VC++',
    1 => 'G++',
];

$GLOBALS['problems'] = [
    1001 => 'A + B Problem',
];

$GLOBALS['judge_states'] = [
    0 => '<span class="state state-ac">Accepted</span>',
    1 => '<span class="state state-wa">Wrong Answer</span>',
    2 => '<span class="state state-le">Time Limit Exceeded</span>',
    3 => '<span class="state state-le">Memory Limit Exceeded</span>',
    4 => '<span class="state state-le">Output Limit Exceeded</span>',
    5 => '<span class="state state-re">Runtime Error</span>',
    6 => '<span class="state state-ce">Compile Error</span>',
    7 => '<span class="state state-pe">Presentation Error</span>',
    8 => '<span class="state">Pending</span>',
    9 => '<span class="state">Running</span>',
   10 => '<span class="state state-re">Undefined Error</span>',
];

$GLOBALS['runtime_errors'] = [
    -1073741676 => 'INTEGER_DIVIDE_BY_ZERO',
    -1073740940 => 'SEGMENTATION_FAULT',
    -1073741819 => 'ACCESS_VIOLATION',
    -1073741571 => 'STACK_OVERFLOW',
    -1 => 'UNDEFINED',
];