<?php

/**
 * Table For Anhui Like
 */

defined("IN_XYS") || die("Access Denied");

class table_code extends table {

	public function __construct() {

		$this->_table = 'code';
		$this->_pk    = 'runid';

		parent::__construct();
	}

	public function fetch_by_runid($runid) {
		return $runid ? DB::fetch_first('SELECT * FROM %t where `runid`=%d', array($this->_table, $runid)) : false;
	}

}
