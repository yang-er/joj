<?php defined("IN_XYS") || die("Access Denied");

class table_submission extends table {

	public function __construct() {

		$this->_table = 'submission';
		$this->_pk    = 'runid';

		parent::__construct();
	}

    public function count($restrict = '') {
        return DB::result_first('SELECT COUNT(1) FROM '.DB::table($this->_table).($restrict == '' ? '' : ' WHERE '.$restrict));
    }

	public function all($start = 0, $range = 15) {
		return DB::fetch_all("SELECT * FROM ".DB::table($this->_table));
	}

	public function fetch_by_runid($name) {
		return $name ? DB::fetch_first('SELECT * FROM %t WHERE `runid`=%d', array($this->_table, $name)) : false;
	}

	public function reset($where = '') {
		DB::query('UPDATE `submission` SET `status`=8'.($where ? ' WHERE '.$where : ''));
	}

}
