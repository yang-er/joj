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

$GLOBALS['compilers'] = [0 => 'Visual C++ Compiler (v14.1)', 1 => 'G++ (MinGW Toolchain 6.3.0)'];
$GLOBALS['problems'] = [1001 => 'A + B Problem'];
