<?php
define('IN_XYS', 'index');
require './source/class_application.php';
if (!isset($_GET['page'])) $_GET['page'] = '1';
$page = intval($_GET['page']);
if ($page < 1) $page = 1;
$all_count = C::t('submission')->count();
$max_page = ceil($all_count / 10);
$start_limit = ($page - 1) * 10;
$mrc = C::t('submission')->range($start_limit, 10, 'dsc');
$curip = get_client_ip();
?>

<!doctype html>
<html lang="cn">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <link rel="stylesheet" href="static/bootstrap.css">
  <link rel="stylesheet" href="static/custom.css">
  <meta http-equiv="refresh" content="30">
  <title>提交状态 - JOJ Test Project</title>
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
    <table class="table table-responsive-lg">
      <thead>
        <tr>
          <th scope="col" style="width:7%;min-width:70px">#</th>
          <th scope="col" style="width:16%;min-width:160px">提交时间</th>
          <th scope="col" style="width:25%;min-width:250px">运行结果</th>
          <th scope="col" style="width:6%;min-width:60px">题号</th>
          <th scope="col" style="width:10%;min-width:100px">执行时间</th>
          <th scope="col" style="width:10%;min-width:100px">执行内存</th>
          <th scope="col" style="width:9%;min-width:90px">代码长度</th>
          <th scope="col" style="width:7%;min-width:70px">语言</th>
          <th scope="col" style="width:10%;min-width:100px">提交者</th>
        </tr>
      </thead>
      <tbody>
<?php if ($mrc) { foreach ($mrc as $one) { ?>
        <tr>
          <th scope="row"><?php echo $one['runid']; ?></th>
          <td><?php echo cst_date("Y-m-d H:i:s", $one['time']); ?></td>
          <td><?php echo $judge_states[$one['status']]; ?></td>
          <td><?php echo $one['proid']; ?></td>
          <td><?php echo $one['exetime']; ?>ms</td>
          <td><?php echo $one['exemem']; ?>K</td>
          <td><?php if (cur_ip($one['ip'])) { echo "<a href=\"view.php?id={$one['runid']}\">"; } echo $one['codelen'].'B'; if (cur_ip($one['ip'])) { echo "</a>"; } ?></td>
          <td><?php echo $build_langs[$one['lang']]; ?></td>
          <td><?php echo $one['author']; ?></td>
        </tr>
<?php }} else { ?>
        <tr>
          <th scope="row">-</th>
          <td colspan="8">暂时还没有人提交哦。</td>
        </tr>
<?php } ?>
      </tbody>
    </table>
    <div class="row">
      <div class="col-md-6 order-1">
        <nav class="float-md-right">
          <ul class="pagination">
            <li class="page-item<?php if($page==1) { ?> disabled<?php } ?>">
              <a class="page-link" href="status.php?page=<?php echo $page-1; ?>" aria-label="前一页">
                <span aria-hidden="true">&laquo;</span>
                <span class="sr-only">前一页</span>
              </a>
            </li>
<?php if ($page > 3) {
    $min_page = $page - 3;
?>
            <li class="page-item"><a class="page-link" href="status.php?page=1">1</a></li>
            <li class="page-item disabled"><a class="page-link" href="#">...</a></li>
<?php } else {
    $min_page = 1;
} if ($max_page - $page > 3) {
    $mmax_page = $max_page;
    $max_page = $page + 3;
} for ($i = $min_page; $i <= $max_page; $i++) { ?>
            <li class="page-item<?php if($page==$i) { ?> active<?php } ?>"><a class="page-link" href="status.php?page=<?php echo $i; ?>"><?php echo $i; ?></a></li>
<?php } if (isset($mmax_page)) { ?>
            <li class="page-item disabled"><a class="page-link" href="#">...</a></li>
            <li class="page-item"><a class="page-link" href="status.php?page=<?php echo $mmax_page; ?>"><?php echo $mmax_page; ?></a></li>
<?php } ?>
            <li class="page-item<?php if($page==$max_page) { ?> disabled<?php } ?>">
              <a class="page-link" href="status.php?page=<?php echo $page+1; ?>" aria-label="后一页">
                <span aria-hidden="true">&raquo;</span>
                <span class="sr-only">后一页</span>
              </a>
            </li>
          </ul>
        </nav>
      </div>
      <div class="col-md-6 order-0">
        <div class="input-group justify-content-end">
          <select class="custom-select" style="max-width:100px">
            <option selected>筛选</option>
            <option value="1">题目编号</option>
            <option value="2">通过情况</option>
            <option value="3">学号</option>
          </select>
          <input type="text" class="form-control" placeholder="筛选条件">
          <div class="input-group-append">
            <button class="btn btn-outline-secondary" type="button">查询</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</body>
</html>

<?php
print_r();

