<?php
define('IN_XYS', 'index');
require './source/class_application.php';

if (!isset($_GET['id']) || !isset($problems[intval($_GET['id'])])) {
    header("Location: ./");
    exit();
}

$id = intval($_GET['id']);

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
                <a class="nav-link" href="./">首页</a>
            </li>
            <li class="nav-item">
                <a class="nav-link active" href="#">代码提交</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="status.php">状态查看</a>
            </li>
        </ul>
        <h1 class="mb-4"><small class="mr-3"><?php echo $id; ?>#</small><?php echo $problems[$id]; ?></h1>
        <form action="submit.php?mod=ajax&id=<?php echo $id; ?>" method="post">
            <div class="form-group">
                <label for="stuid">提交学号</label>
                <input class="form-control" type="number" placeholder="八位教学号，不好意思就写0吧" name="stuid" id="stuid">
            </div>
            <div class="form-group">
                <label for="compiler_selc">选择编程语言</label>
                <select class="form-control" name="compiler" id="compiler_selc">
                    <?php if (isset($compilers)) {
                    foreach ($compilers as $key => $value) {?>
                    <option value="<?php echo $key; ?>"><?php echo $value; ?></option>
                    <?php }} ?>
                </select>
            </div>
            <div class="form-group">
                <label for="code_area">输入你的代码</label>
                <textarea class="form-control" id="code_area" name="code" placeholder="#include <bits/stdc++.h>"></textarea>
            </div>
            <div class="form-group">
                <input class="btn btn-success" type="submit" value=" 提交 ">
            </div>
        </form>
    </div>
</body>
</html>
