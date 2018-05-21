<?php

/**
 * Config Core
 *  - Thanks to Discuz!X
 */

defined("IN_XYS") || die("Access Denied");
define('APP_ROOT', substr(dirname(__FILE__), 0, -6));

ini_set('error_reporting', E_ALL);
ini_set('display_errors', 0);
ini_set('log_errors', 1);
ini_set('error_log', APP_ROOT.'./source/config_log.php');

$_SESSION = array();
spl_autoload_register('application::autoload');
application::loadapp();

class application {

    public static $_tables = array();
	private static $_imports;
    public static $_templates = null;
    public static $_caches = null;

    public static function t(string $table) {
        if(!isset(self::$_tables[$table])) {
            $class = "table_{$table}";
            self::$_tables[$table] = new $class;
        }
        return self::$_tables[$table];
    }

	public static function autoload(string $class) {
		$class = strtolower($class);
		if(strpos($class, '_') !== false) {
			$file = $class;
		} else {
			$file = 'class_'.$class;
		}

		try {
			self::import($file);
			return true;
		} catch (Exception $exc) {
			$trace = $exc->getTrace();
			foreach ($trace as $log) {
				if(empty($log['class']) && $log['function'] == 'class_exists') {
					return false;
				}
			}
			throw $exc;
		}
	}

	private static function import(string $name, string $folder = '', bool $force = true) {
		$key = $folder.$name;
		if(!isset(self::$_imports[$key])) {
			$path = APP_ROOT.'/source/'.$folder;
			if(strpos($name, '/') !== false) {
				$pre = basename(dirname($name));
				$filename = dirname($name).'/'.$pre.'_'.basename($name).'.php';
			} else {
				$filename = $name.'.php';
			}

			if(is_file($path.'/'.$filename)) {
				include $path.'/'.$filename;
				self::$_imports[$key] = true;

				return true;
			} elseif(!$force) {
				return false;
			} else {
				throw new Exception('Oops! System file lost: '.$filename);
			}
		}
		return true;
	}

    public static function loadapp() {
        include APP_ROOT.'./source/config_global.php';
        DB::init(null, $_CONFIG['db']);
        include APP_ROOT.'./source/function_core.php';
        include APP_ROOT.'./source/function_discuz.php';
        define('FORMHASH', self::formhash());
    }

	private static function formhash() {
		return substr(md5(substr(time(), 0, -4)), 16);
	}

    public static function debug_info(bool $force = false) {
        if (!$force && !$GLOBALS['debugmode']) return false;
        $db = DB::object();
		$GLOBALS['debuginfo'] = [
		    'time' => number_format((microtime(true) - $_SERVER['REQUEST_TIME_FLOAT']), 6),
		    'queries' => $db->querynum
		];
        return TRUE;
    }

}

class C extends application {}
