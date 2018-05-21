<?php

/**
 * Table For Anhui Like
 */

defined("IN_XYS") || die("Access Denied");

class table_details extends table {

	public function __construct() {

		$this->_table = 'details';
		$this->_pk    = 'runid';

		parent::__construct();
	}

	public function fetch_by_runid($runid) {
		return $runid ? DB::fetch_all('SELECT * FROM %t WHERE `runid`=%d', array($this->_table, $runid)) : false;
	}

}
