<?php
define('IN_XYS', 'index');
require './source/class_application.php';
?>
<!doctype html>
<html lang="cn">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <link rel="stylesheet" href="static/bootstrap.css">
  <link rel="stylesheet" href="static/custom.css">
  <script src="static/jquery.min.js"></script>
  <script src="static/popper.min.js"></script>
  <script src="static/bootstrap.bundle.min.js"></script>
  <title>提交代码 - JOJ Test Project</title>
</head>
<body>
  <div class="container pt-4">
    <ul class="nav nav-pills mb-4">
      <li class="nav-item">
        <a class="nav-link active" href="./">代码提交</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" href="status.php">状态查看</a>
      </li>
    </ul>
    <div class="card-columns">
      <div class="card">
        <div class="card-body">
          <h3 class="card-title">欢迎来到 JOJ 测试项目</h3>
          <p class="card-text">欢迎在此提交代码，进行在线测评。</p>
          <p class="card-text">在这个平台上提交代码，不需要使用 <code>freopen</code>、<code>fstream</code> 控制文件输入输出，请直接使用标准输入输出。另外，可以在 <code>stderr</code> 中打印调试信息，但是调试信息不会被反馈。</p>
          <p class="card-text">请不要在网页上直接编码，如果你有按缩进的习惯，会GG。下次我弄好点啦。</p>
          <p class="card-text">目前已经支持的编程语言：</p>
		  <ol>
			<li>Visual C++ 2017</li>
			<li>GCC / G++ (MinGW)</li>
		  </ol>
		  <p class="card-text">即将支持的语言：</p>
		  <ol>
			<li>Java 1.8</li>
			<li>Python 3.7</li>
			<li>C# 7.0</li>
		  </ol>
		  <p class="card-text">请不要提交恶意代码。不过……你大概提交了也会被我的程序解决掉？只会给你留下一个 Running 状态和后期的查水表。</p>
        </div>
      </div>
<?php include './source/prob_list.html'; ?>
    </div>
  </div>
</body>
</html>
