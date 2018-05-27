<?php
define('IN_XYS', 'misc');
require './source/class_application.php';
if (!isset($_GET['mod'])) $_GET['mod'] = 'none';
session_start();

if ($_GET['mod'] == 'seccode')
{
    $authkey = md5(APP_KEY.$_SERVER['HTTP_USER_AGENT'].get_client_ip());
    $rand = rand(100000, 999999);
    $authcode = rawurlencode(authcode($rand, 'ENCODE', $authkey, 180));
    $_SESSION['authcode'] = $authcode;
    $_SESSION['used'] = 0;
    show_seccode($authcode);
}
elseif ($_GET['mod'] == 'ajaxpost')
{
    $handlekey = "ajaxpost";

    if (!isset($_SESSION['authcode']) || !isset($_POST['seccode']) || !isset($_SESSION['used']) || $_SESSION['used'] != 0)
    {
        $message = '未知错误。请刷新页面。';
        $type = 'danger';
        include APP_ROOT.'./source/template_showmessage.php';
    }

    $authkey = md5(APP_KEY.$_SERVER['HTTP_USER_AGENT'].get_client_ip());
    $seccode = strtoupper($_POST['seccode']);
    if (!seccode::seccode_check(authcode(rawurldecode($_SESSION['authcode']), 'DECODE', $authkey), $seccode))
    {
        $message = '验证码错误。<script>$("#seccode").prop("src","misc.php?mod=seccode&'.time().'")</script>';
        $type = 'danger';
        include APP_ROOT.'./source/template_showmessage.php';
    }

    if (!isset($_SESSION['time'])) $_SESSION['time'] = 0;
    if (time() - $_SESSION['time'] < 30)
    {
        $message = '提交过于频繁，请在'.(time() - $_SESSION['time']).'秒后重试。<script>$("#seccode").prop("src","misc.php?mod=seccode&'.time().'")</script>';
        $type = 'warning';
        include APP_ROOT.'./source/template_showmessage.php';
    }

    $_SESSION['time'] = time();
    if (!isset($_POST['id'])) $_POST['id'] = '-1';
    if (!isset($_POST['compiler'])) $_POST['compiler'] = '-1';
    $cmp = intval($_POST['compiler']);
    $id = intval($_POST['id']);
    $stuid = intval($_POST['stuid']);

    if (!isset($problems[$id]) || !isset($compilers[$cmp]) || !isset($_POST['code']))
    {
        $message = '参数错误。';
        $type = 'danger';
        include APP_ROOT.'./source/template_showmessage.php';
    }

    $code = $_POST['code'];
	$len = strlen($code);

    if ($len > 8192 || $len < 20)
    {
        $message = '代码过于冗长或过短。';
        $type = 'warning';
        include APP_ROOT.'./source/template_showmessage.php';
    }

    $_SESSION['used'] = 1;
    $runid = C::t('submission')->insert([
        'time' => time(),
        'author' => $stuid,
        'proid' => $id,
        'status' => 8,
        'exetime' => 0,
        'exemem' => 0,
        'codelen' => $len,
        'lang' => $cmp,
        'ip' => get_client_ip(),
        'grade' => 0
    ], true);

    C::t('code')->insert([
        'runid' => $runid,
        'proid' => $id,
        'code' => $code,
        'lang' => $cmp,
        'ce' => ''
    ]);

    $message = '提交成功！正在跳转……<script>function goaway() { window.location.href = "view.php?id='.$runid.'"; }; setTimeout("goaway()", 2000);</script>';
    $type = 'success';
    include APP_ROOT.'./source/template_showmessage.php';
}
else
{
    die("Access Not Defined");
}