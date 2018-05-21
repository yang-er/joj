<?php

/**
 * Core Functions
 */

defined("IN_XYS") || die("Access Denied");

function showmessage(string $message, string $type = 'info') {
    if(isset($_REQUEST['inajax'])) {
        $handlekey = $_REQUEST['handlekey'];
        include template("showmessage");
        exit();
    } else {
        exit("TODO");
    }
}

function template(string $file) {
    return C::$_templates->gettpl($file);
}

function cache(string $file) {
    return C::$_caches->read($file);
}

function module(string $file) {
    return libfile($file, 'module');
}

function libfile(string $libname, string $folder = '') {
	$libpath = '/source/'.$folder;
	if(strstr($libname, '/')) {
		list($pre, $name) = explode('/', $libname);
		$path = "{$libpath}/{$pre}/{$pre}_{$name}";
	} else {
		$path = "{$libpath}/{$libname}";
	}
	return preg_match('/^[\w\d\/_]+$/i', $path) ? realpath(APP_ROOT.$path.'.php') : false;
}

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

function parse_flag(int $val, int $size = 8) {
    $ret = array();

    # Get bits
    for ($i = 0; $i < $size; $i++) {
        $ret[] = $val % 2;
        $val /= 2;
    }

    # Check if it is over
    if ($val != 0) {
        array_merge($ret, parse_flag($val));
    }

    return $ret;
}

function create_flag(array $vals) {

    $val = [0,0,0,0,0,0,0,0];
    foreach ($vals as $one) {
        $val[7 - intval($one)] = 1;
    }

    $ret = 0;
    for ($i = 0; $i < 8; $i++) {
        $ret = $ret * 2 + $val[$i];
    }

    return $ret;
}

function create_token(string $salt) {
    $time = time();
    $file = __FILE__;
    return substr(md5("{$time}{$file}{$salt}"), -10);
}

function list_city(int $pid, int $default = 0) {
    $province = C::t('hat_city')->province;
    echo '<option '.(0==$default?'selected ':'').'disabled>请选择</option>';
    if (!isset($province[$pid])) return;
    foreach (C::t('hat_city')->fetch_by_province($pid) as $city) {
        echo '<option value="'.$city['city'].'"'.($city['city']==$default?' selected':'').'>'.$city['name'].'</option>';
    }
}

function list_province(int $default = 0) {
    $province = C::t('hat_city')->province;
    echo '<option '.(0==$default?'selected ':'').'disabled>请选择</option>';
    foreach ($province as $pid => $name) {
        echo '<option value="'.$pid.'"'.($pid==$default?' selected':'').'>'.$name.'</option>';
    }
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
