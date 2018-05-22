<?php

/**
 * Table For Anhui Like
 */

defined("IN_XYS") || die("Access Denied");

class table_submission extends table {

	public function __construct() {

		$this->_table = 'submission';
		$this->_pk    = 'runid';

		parent::__construct();
	}

	public function all($start = 0, $range = 15) {
		return DB::fetch_all("SELECT * FROM ".DB::table($this->_table));
	}

	public function fetch_by_name($name) {
		return $name ? DB::fetch_all('SELECT * FROM %t WHERE id=%d', array($this->_table, $name)) : false;
	}

}
