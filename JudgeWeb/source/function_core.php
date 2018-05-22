<?php

/**
 * Core Functions
 */

defined("IN_XYS") || die("Access Denied");

function checkformhash() {
    if ($_POST['formhash'] != FORMHASH) {
        header('HTTP/1.1 400 Bad Request');
        exit();
    }
}

function cst_date(string $format, int $timestamp = -1) {
    if ($timestamp == -1) $timestamp = time();
    date_default_timezone_set('Asia/Shanghai');
    return date($format, $timestamp);
}

function create_token(string $salt) {
    $time = time();
    $file = __FILE__;
    return substr(md5("{$time}{$file}{$salt}"), -10);
}

function get_client_ip() {
    if(getenv('HTTP_CLIENT_IP')) {
        return getenv('HTTP_CLIENT_IP');
    } elseif(getenv('HTTP_X_FORWARDED_FOR')) {
        return getenv('HTTP_X_FORWARDED_FOR');
    } elseif(getenv('REMOTE_ADDR')) {
        return getenv('REMOTE_ADDR');
    } else {
        return "{$_SERVER['REMOTE_ADDR']}";
    }
}

function cur_ip($new) {
    global $curip;
    return $curip == '::1' || $curip == '127.0.0.1' || $curip == $new;
}

function check_update(array $origin, array $to_check) {
    $ret = array();

    foreach ($to_check as $name) {
        if (!isset($_POST[$name])) 
            continue;
        $val = $_POST[$name];
        if (is_string($_POST[$name]))
            $val = trim($val);
        if ($origin[$name] != $val)
            $ret[$name] = $val;
    }

    return $ret;
}
