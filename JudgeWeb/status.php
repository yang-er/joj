<?php
define('IN_XYS', 'index');
require './source/class_application.php';
if (!isset($_GET['page'])) $_GET['page'] = '1';
$page = intval($_GET['page']);
if ($page < 1) $page = 1;

$wheres = [];
if (isset($_GET['proid']) && $_GET['proid']) $wheres[] = '`proid`='.intval($_GET['proid']);
if (isset($_GET['author']) && $_GET['author']) $wheres[] = '`author`='.intval($_GET['author']);
isset($_GET['state']) || $_GET['state'] = '-1';
$find_state = intval($_GET['state']);
if ($find_state >= 0) $wheres[] = '`status`='.$find_state;
$where = implode(' AND ', $wheres);
$all_count = C::t('submission')->count($where);
$max_page = ceil($all_count / 10);
$start_limit = ($page - 1) * 10;
$page_url_base = $_SERVER['REQUEST_URI'];

function build_page($page) {
	$ret = '';
	if (isset($_GET['page'])) {
		$old = $_GET['page'];
		$_GET['page'] = $page;
		$ret = http_build_query($_GET);
	} else {
		$_GET['page'] = $page;
		$ret = http_build_query($_GET);
		unset($_GET['page']);
	}
	return $_SERVER['SCRIPT_NAME'].'?'.$ret;
}

if (!strchr($page_url_base, '?'))
	$page_url_base .= '?';
else
	$page_url_base .= '&';
$mrc = C::t('submission')->range($start_limit, 10, 'dsc', $where);
$curip = get_client_ip();
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
              <a class="page-link" href="<?php echo build_page($page-1); ?>" aria-label="前一页">
                <span aria-hidden="true">&laquo;</span>
                <span class="sr-only">前一页</span>
              </a>
            </li>
<?php if ($page > 5) {
    $min_page = $page - 3;
?>
            <li class="page-item"><a class="page-link" href="<?php echo build_page(1); ?>">1</a></li>
            <li class="page-item disabled"><a class="page-link" href="#">...</a></li>
<?php } else {
    $min_page = 1;
} if ($max_page - $page > 4) {
    $mmax_page = $max_page;
    $max_page = $page + 3;
} for ($i = $min_page; $i <= $max_page; $i++) { ?>
            <li class="page-item<?php if($page==$i) { ?> active<?php } ?>"><a class="page-link" href="<?php echo build_page($i); ?>"><?php echo $i; ?></a></li>
<?php } if (isset($mmax_page)) { ?>
            <li class="page-item disabled"><a class="page-link" href="#">...</a></li>
            <li class="page-item"><a class="page-link" href="<?php echo build_page($mmax_page); ?>"><?php echo $mmax_page; ?></a></li>
<?php } ?>
            <li class="page-item<?php if($page==$max_page) { ?> disabled<?php } ?>">
              <a class="page-link" href="<?php echo build_page($page+1); ?>" aria-label="后一页">
                <span aria-hidden="true">&raquo;</span>
                <span class="sr-only">后一页</span>
              </a>
            </li>
          </ul>
        </nav>
      </div>
      <div class="col-md-6 order-0">
        <div class="input-group justify-content-end">
          <input type="text" class="form-control" id="filter_proid" placeholder="问题编号" <?php if (isset($_GET['proid'])) echo 'value="'.$_GET['proid'].'"'; ?>>
          <select class="form-control custom-select" id="filter_stat">
            <option value="-1" <?php if ($find_state == -1) echo 'selected'; ?>>All Status</option>
<?php foreach ($judge_states_simple as $key => $value) { ?>
            <option value="<?php echo $key; ?>" <?php if ($find_state == $key) echo 'selected'; ?>><?php echo $value; ?></option>
<?php } ?>
          </select>
          <input type="text" class="form-control" id="filter_author" placeholder="提交者" <?php if (isset($_GET['author'])) echo 'value="'.$_GET['author'].'"'; ?>>
          <div class="input-group-append">
            <button class="btn btn-outline-secondary" id="filter_btn" type="button">查询</button>
          </div>
        </div>
      </div>
    </div>
  </div>
<script>
$('#filter_btn').on('click', function() {
window.location = '<?php echo $_SERVER['SCRIPT_NAME']; ?>?proid=' + $('#filter_proid').val() + '&state=' + $('#filter_stat').val() + '&author=' + $('#filter_author').val(); });
$('#filter_value').keydown(function(event) { if (event.which == 13) $('#filter_btn').click(); });
</script>
</body>
</html>
