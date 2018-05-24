<?php
define('IN_XYS', 'index');
require './source/class_application.php';
$curip = get_client_ip();
if (!isset($_GET['id'])) $_GET['id'] = '0';
$id = intval($_GET['id']);
$cur = C::t('submission')->fetch_by_runid($id);
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
  <script src="static/highlight.min.js"></script>
  <title>提交详情 - JOJ Test Project</title>
</head>
<body>
  <div class="container pt-4">
    <ul class="nav nav-pills mb-4">
      <li class="nav-item">
        <a class="nav-link" href="./">代码提交</a>
      </li>
      <li class="nav-item">
        <a class="nav-link active" href="status.php">状态查看</a>
      </li>
    </ul>
<?php if ($cur && cur_ip($cur['ip'])) {
if (isset($_GET['reset']) && cur_ip('0.0.0.0')) C::t('submission')->reset('`runid`='.$id);
$codes = C::t('code')->fetch_by_runid($id);
$details = C::t('details')->fetch_by_runid($id);
?>
    <div class="row">
      <div class="col-md-8">
        <h1 class="mb-4">代码提交 #<?php echo $cur['runid']; ?></h1>
        <p><span class="mr-3">运行结果</span><?php echo $judge_states[$cur['status']]; ?></p>
        <p><span class="mr-3">编译语言</span><?php echo $compilers[$cur['lang']]; ?></p>
        <pre class="form-control" id="coded_area"><code class="cpp"><?php echo htmlspecialchars($codes['code']); ?></code></pre>
        <script>hljs.initHighlightingOnLoad();</script>
      </div>
      <div class="col-md-4">
        <p><span class="mr-3">提交时间</span><?php echo cst_date("Y-m-d H:i:s", $cur['time']); ?></p>
        <p><span class="mr-3">题目</span><?php echo '<a href="submit.php?id='.$cur['proid'].'">'.$cur['proid'].'#</a> '.$problems[$cur['proid']]; ?></p>
        <p><span class="mr-3">提交者</span><?php echo $cur['author']; ?></p>
<?php if ($codes['ce']) { ?>
        <p><span class="mr-3">编译提示信息</span><br><span id="coded_area"><?php echo nl2br(htmlspecialchars($codes['ce'])); ?></span></p>
<?php } ?>
        <table class="table">
          <thead>
            <tr>
              <th scope="col">状态</th>
              <th scope="col">内存</th>
              <th scope="col">时间</th>
            </tr>
          </thead>
          <tbody>
<?php if ($details) { foreach ($details as $detail) { ?>
            <tr>
              <td><?php if ($detail['status'] == 5 && isset($runtime_errors[$detail['exitcode']])) echo $runtime_errors[$detail['exitcode']]; else echo $judge_states[$detail['status']]; ?></td>
              <td><?php echo $detail['exemem']; ?>K</td>
              <td><?php echo $detail['exetime']; ?>ms</td>
            </tr>
<?php if ($detail['status'] == 6) break; } } else { ?>
            <tr>
              <td colspan="3">目前没有运行记录。</td>
            </tr>
<?php } ?>
          </tbody>
        </table>
      </div>
    </div>
<?php } else { ?>
    <h2 class="mb-3">没有权限查看</h2>
    <p>请检查您的登录账户与查看的文件编号。</p>
<?php } ?>
  </div>
</body>
</html>
