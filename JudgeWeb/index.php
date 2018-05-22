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
    <div class="row">
<?php foreach ($problems as $pid => $problem) { ?>
      <div class="col-md-6 col-xl-4 mb-4">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title"><?php echo $problem;?></h5>
            <p class="card-text">With supporting text below as a natural lead-in to additional content.</p>
            <a href="submit.php?id=<?php echo $pid;?>" class="btn btn-primary">尝试提交</a>
          </div>
        </div>
      </div>
<?php } ?>
    </div>
  </div>
</body>
</html>
