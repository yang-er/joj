-- phpMyAdmin SQL Dump
-- version 4.7.0-beta1
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: May 20, 2018 at 10:46 PM
-- Server version: 5.5.27
-- PHP Version: 7.0.20

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `judge`
--

CREATE TABLE `code` (
  `runid` int(11) NOT NULL,
  `proid` int(11) NOT NULL,
  `code` varchar(16384) NOT NULL,
  `lang` int(11) NOT NULL,
  `ce` varchar(4096) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

CREATE TABLE `details` (
  `testid` int(11) NOT NULL,
  `runid` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  `exemem` int(11) NOT NULL,
  `exetime` int(11) NOT NULL,
  `exitcode` int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

CREATE TABLE `submission` (
  `runid` int(11) NOT NULL,
  `time` int(10) NOT NULL,
  `author` int(11) NOT NULL,
  `proid` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  `exetime` int(11) NOT NULL,
  `exemem` int(11) NOT NULL,
  `codelen` int(11) NOT NULL,
  `lang` int(11) NOT NULL,
  `ip` varchar(20) NOT NULL,
  `grade` int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

ALTER TABLE `code`
  ADD PRIMARY KEY (`runid`);

ALTER TABLE `details`
  ADD PRIMARY KEY (`testid`),
  ADD KEY `runid` (`runid`);

ALTER TABLE `submission`
  ADD PRIMARY KEY (`runid`);

ALTER TABLE `details`
  MODIFY `testid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `submission`
  MODIFY `runid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;


/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
