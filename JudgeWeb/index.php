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
<?php include './source/prob_list.html'; ?>
    </div>
  </div>
</body>
</html>
