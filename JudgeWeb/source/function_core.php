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

function multipage($page, $max_page) {
	$ret = '<ul class="pagination">';
	
	# First Page Button
	$ret .= '<li class="page-item'.($page==1 ? ' disabled' : '').'">';
	$ret .= '<a class="page-link" href="'.build_page($page-1).'" aria-label="前一页">';
	$ret .= '<span aria-hidden="true">&laquo;</span><span class="sr-only">前一页</span>';
	$ret .= '</a></li>';
	
	# Previous Page Button
    if ($page > 5) {
		$min_page = $page - 3;
		$ret .= '<li class="page-item"><a class="page-link" href="'.build_page(1).'">1</a></li>';
		$ret .= '<li class="page-item disabled"><a class="page-link" href="#">...</a></li>';
	} else {
		$min_page = 1;
	}
	
	if ($max_page - $page > 4) {
		$mmax_page = $max_page;
		$max_page = $page + 3;
	}
	
	// Middle Page Button
	for ($i = $min_page; $i <= $max_page; $i++) {
		$ret .= '<li class="page-item'.($page==$i ? ' active' : '').'">';
		$ret .= '<a class="page-link" href="'.build_page($i).'">'.$i.'</a></li>';
	}
	
	if (isset($mmax_page)) {
		$ret .= '<li class="page-item disabled"><a class="page-link" href="#">...</a></li>';
		$ret .= '<li class="page-item"><a class="page-link" href="'.build_page($mmax_page).'">'.$mmax_page.'</a></li>';
    }

	// Forward Page Button
	$ret .= '<li class="page-item'.($page==$max_page ? ' disabled' : '').'">';
	$ret .= '<a class="page-link" href="'.build_page($page+1).'" aria-label="后一页">';
	$ret .= '<span aria-hidden="true">&raquo;</span><span class="sr-only">后一页</span>';
	$ret .= '</a></li>';
	
	$ret .= '</ul>';
	return $ret;
}
