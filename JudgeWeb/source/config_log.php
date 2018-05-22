<?php defined("IN_XYS") || die("Access Denied"); ?>
[22-May-2018 13:47:41 Asia/Shanghai] PHP Warning:  mysqli::real_connect(): (HY000/1045): Access denied for user 'judge'@'localhost' (using password: YES) in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php on line 191
[22-May-2018 13:47:41 Asia/Shanghai] PHP Warning:  mysqli_errno() expects exactly 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php on line 263
[22-May-2018 13:47:41 Asia/Shanghai] PHP Fatal error:  Uncaught DbException: notconnect in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php:317
Stack trace:
#0 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php(192): db_driver_mysqli->halt('notconnect', 0)
#1 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php(183): db_driver_mysqli->_dbconnect('localhost', 'judge', '2w2WXe02816e43', 'utf8', 'judge', '0')
#2 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php(332): db_driver_mysqli->connect()
#3 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_application.php(84): DB::init(NULL, Array)
#4 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_application.php(18): application::loadapp()
#5 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\index.php(3): require('C:\\Users\\tlylz\\...')
#6 {main}
  thrown in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php on line 317
[22-May-2018 13:47:53 Asia/Shanghai] PHP Warning:  mysqli::real_connect(): (HY000/1045): Access denied for user 'judge'@'localhost' (using password: YES) in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php on line 191
[22-May-2018 13:47:53 Asia/Shanghai] PHP Warning:  mysqli_errno() expects exactly 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php on line 263
[22-May-2018 13:47:53 Asia/Shanghai] PHP Fatal error:  Uncaught DbException: notconnect in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php:317
Stack trace:
#0 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php(192): db_driver_mysqli->halt('notconnect', 0)
#1 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php(183): db_driver_mysqli->_dbconnect('localhost', 'judge', '2w2WXe02816e43', 'utf8', 'judge', '0')
#2 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php(332): db_driver_mysqli->connect()
#3 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_application.php(84): DB::init(NULL, Array)
#4 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_application.php(18): application::loadapp()
#5 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php(3): require('C:\\Users\\tlylz\\...')
#6 {main}
  thrown in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php on line 317
[22-May-2018 13:48:05 Asia/Shanghai] PHP Warning:  mysqli::real_connect(): (HY000/1045): Access denied for user 'judge'@'localhost' (using password: YES) in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php on line 191
[22-May-2018 13:48:05 Asia/Shanghai] PHP Warning:  mysqli_errno() expects exactly 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php on line 263
[22-May-2018 13:48:05 Asia/Shanghai] PHP Fatal error:  Uncaught DbException: notconnect in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php:317
Stack trace:
#0 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php(192): db_driver_mysqli->halt('notconnect', 0)
#1 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php(183): db_driver_mysqli->_dbconnect('localhost', 'judge', '2w2WXe02816e43', 'utf8', 'judge', '0')
#2 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php(332): db_driver_mysqli->connect()
#3 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_application.php(84): DB::init(NULL, Array)
#4 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_application.php(18): application::loadapp()
#5 C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php(3): require('C:\\Users\\tlylz\\...')
#6 {main}
  thrown in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\source\class_db.php on line 317
[22-May-2018 14:11:34 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 14:14:43 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 14:18:56 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 14:35:32 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 170
[22-May-2018 14:37:12 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 170
[22-May-2018 14:39:00 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 170
[22-May-2018 14:44:03 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 176
[22-May-2018 14:44:27 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 176
[22-May-2018 14:44:37 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 176
[22-May-2018 14:45:21 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 176
[22-May-2018 14:45:38 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 176
[22-May-2018 14:45:42 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 176
[22-May-2018 14:47:39 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 180
[22-May-2018 14:49:05 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:49:07 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:49:10 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:49:12 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:49:40 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:49:53 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:49:57 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:50:00 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:50:05 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:50:08 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:50:10 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:50:12 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:50:14 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:50:16 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:50:19 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:50:21 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 184
[22-May-2018 14:50:40 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:50:46 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:52:10 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:54:21 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:54:24 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:00 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:04 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:07 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:10 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:18 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:27 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:30 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:32 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:40 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:42 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:44 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:46 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:56 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:55:57 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:56:00 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:56:03 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:56:07 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:56:11 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:56:14 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:57:38 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 14:57:55 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 183
[22-May-2018 16:25:05 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 185
[22-May-2018 16:29:05 Asia/Shanghai] PHP Notice:  Undefined index: stuid in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 60
[22-May-2018 16:29:05 Asia/Shanghai] PHP Notice:  Undefined index: stuid in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 60
[22-May-2018 16:29:05 Asia/Shanghai] PHP Notice:  Undefined index: stuid in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 60
[22-May-2018 16:29:05 Asia/Shanghai] PHP Notice:  Undefined index: stuid in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 60
[22-May-2018 16:29:06 Asia/Shanghai] PHP Notice:  Undefined index: stuid in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 60
[22-May-2018 16:29:06 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 202
[22-May-2018 16:31:57 Asia/Shanghai] PHP Notice:  Undefined index: stuid in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 61
[22-May-2018 16:31:57 Asia/Shanghai] PHP Notice:  Undefined index: stuid in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 61
[22-May-2018 16:31:57 Asia/Shanghai] PHP Notice:  Undefined index: stuid in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 61
[22-May-2018 16:31:57 Asia/Shanghai] PHP Notice:  Undefined index: stuid in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 61
[22-May-2018 16:31:57 Asia/Shanghai] PHP Notice:  Undefined index: stuid in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 61
[22-May-2018 16:31:57 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 203
[22-May-2018 16:32:43 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 203
[22-May-2018 17:33:12 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 17:39:50 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 17:40:30 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:02:57 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:03:04 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:03:26 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:03:27 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:03:32 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:05:02 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:05:03 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:05:04 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:05:20 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:06:00 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:06:10 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:06:12 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:06:25 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:08:21 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:08:22 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:08:26 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:08:30 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:09:39 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:09:52 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:09:53 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:09:57 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:10:00 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:11:59 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:12:04 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:12:55 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:12:56 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:14:35 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:14:44 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:14:51 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:14:52 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:22:33 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:22:34 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:22:37 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:22:46 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:38:28 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:38:29 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:38:33 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:38:42 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:39:16 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:39:17 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:44:33 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:44:40 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:44:41 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:50:39 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:50:40 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:50:43 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:52:22 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:52:23 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:53:07 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:53:08 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:53:45 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:53:54 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:53:56 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:54:03 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:56:43 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:56:44 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:56:48 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:56:51 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:57:04 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:57:34 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:57:35 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:58:05 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:58:06 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:58:18 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:58:36 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 18:58:37 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:58:53 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:59:14 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 18:59:20 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:07:29 Asia/Shanghai] PHP Notice:  Undefined index: compiler in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\submit.php on line 16
[22-May-2018 19:07:30 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:07:34 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:07:53 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:08:20 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:08:24 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:10:25 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:10:33 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:10:37 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:11:08 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:11:15 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:11:17 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:13:41 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:13:44 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:13:55 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 126
[22-May-2018 19:14:50 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:15:21 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:15:49 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:16:20 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:16:51 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:17:22 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:17:54 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:18:25 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:18:56 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:19:27 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:19:58 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:22:30 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:22:46 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:23:01 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:23:13 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:23:44 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:24:02 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:24:15 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:24:46 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:25:17 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:25:49 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:26:20 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:26:25 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:26:29 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:27:00 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:27:31 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:28:02 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:28:33 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:29:04 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:29:35 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:30:06 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:30:37 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:30:41 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:37:40 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:38:11 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:38:42 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:39:13 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:39:45 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:40:16 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:40:47 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:41:18 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:41:49 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:42:20 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:42:51 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:43:22 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:43:53 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:44:25 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:44:56 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:44:58 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:45:27 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:45:58 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:46:29 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:47:00 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:47:31 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:48:01 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:48:32 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:49:03 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:49:34 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:50:05 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:50:36 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:51:07 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:51:39 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:52:10 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:52:41 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:53:12 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:53:43 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:54:14 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:54:45 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:55:16 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:55:47 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:56:18 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:56:49 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:57:21 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:57:38 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:57:43 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:57:52 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:58:23 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:58:54 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:59:25 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 19:59:56 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:00:27 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:00:58 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:01:29 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:02:00 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:02:31 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:03:02 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:03:33 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:04:05 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:04:36 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:05:07 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:05:38 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:06:09 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:06:40 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:07:11 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:07:42 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:08:13 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:08:44 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:09:15 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:09:47 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:10:18 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:10:49 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:11:20 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:11:51 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:12:22 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:12:24 Asia/Shanghai] PHP Notice:  iconv(): Detected an illegal character in input string in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\view.php on line 41
[22-May-2018 20:12:53 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:12:59 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:13:24 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:13:55 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:14:27 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:14:58 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:15:29 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:16:00 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:16:31 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:17:02 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:18:43 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:19:15 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:20:12 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:27:19 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:34:50 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:40:23 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:40:33 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:40:36 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:40:57 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:41:00 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:41:02 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:41:05 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:41:17 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:41:26 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:41:45 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:41:48 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:41:52 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:42:23 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:42:54 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:43:25 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:43:56 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:44:27 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:44:33 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 127
[22-May-2018 20:44:50 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:45:13 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:45:44 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:46:15 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:49:59 Asia/Shanghai] PHP Notice:  Undefined variable: id in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\index.php on line 31
[22-May-2018 20:50:44 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:51:54 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:51:59 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:52:02 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:52:04 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:52:35 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:53:02 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:53:11 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:53:42 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:54:13 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:54:44 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:55:15 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:55:46 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:56:17 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:56:49 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:57:20 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:57:51 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:58:22 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:58:53 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:59:24 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 20:59:55 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 21:00:26 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 21:00:57 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 21:04:13 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 21:05:03 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 21:05:13 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
[22-May-2018 21:05:16 Asia/Shanghai] PHP Warning:  print_r() expects at least 1 parameter, 0 given in C:\Users\tlylz\Source\Repos\joj\JudgeWeb\status.php on line 124
