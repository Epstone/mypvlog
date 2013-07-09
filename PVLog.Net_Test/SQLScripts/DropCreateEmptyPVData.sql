# --------------------------------------------------------
# Host:                         127.0.0.1
# Server version:               5.1.41
# Server OS:                    Win32
# HeidiSQL version:             6.0.0.3603
# Date/time:                    2011-02-12 10:49:38
# --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

# Dumping database structure for dev_pv_data
DROP DATABASE IF EXISTS `dev_pv_data`;
CREATE DATABASE IF NOT EXISTS `dev_pv_data` /*!40100 DEFAULT CHARACTER SET latin1 COLLATE latin1_general_ci */;
USE `dev_pv_data`;


# Dumping structure for table dev_pv_data.average_measures_tmp
CREATE TABLE IF NOT EXISTS `average_measures_tmp` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Inverter Temperatur',
  `SystemID` smallint(5) unsigned NOT NULL COMMENT 'ID der PV Anlage',
  `InverterID` tinyint(3) unsigned DEFAULT NULL COMMENT 'Inverter ID',
  `SystemStatus` tinyint(3) unsigned DEFAULT NULL COMMENT 'Betriebsart',
  `DBAddDate` datetime NOT NULL COMMENT 'Datum',
  `GeneratorVoltage` double unsigned DEFAULT NULL COMMENT 'Generatorspannung in V',
  `GeneratorAmperage` double unsigned DEFAULT NULL COMMENT 'Generatorstrom in A (DC)',
  `GeneratorWattage` double unsigned DEFAULT NULL COMMENT 'Generatorleistung in W',
  `GridVoltage` double unsigned DEFAULT NULL COMMENT 'Netzspannung in V(AC)',
  `GridAmperage` double unsigned DEFAULT NULL COMMENT 'Netzstrom, Einspeisestrom in A(AC)',
  `OutputWattage` double unsigned DEFAULT NULL COMMENT 'Eingespeiste Leistung in W',
  `SystemTemperature` smallint(6) DEFAULT NULL COMMENT 'Inverter Temperatur',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

# Dumping data for table dev_pv_data.average_measures_tmp: 0 rows
DELETE FROM `average_measures_tmp`;
/*!40000 ALTER TABLE `average_measures_tmp` DISABLE KEYS */;
/*!40000 ALTER TABLE `average_measures_tmp` ENABLE KEYS */;


# Dumping structure for table dev_pv_data.kwh_by_day
CREATE TABLE IF NOT EXISTS `kwh_by_day` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'kwh ID',
  `Date` date NOT NULL COMMENT 'Date of this Day',
  `SystemID` smallint(5) unsigned NOT NULL COMMENT 'PV Anlagen ID',
  `InverterID` tinyint(3) unsigned NOT NULL COMMENT 'Inverter ID',
  `kwh` double unsigned NOT NULL COMMENT 'Kwh for this day',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

# Dumping data for table dev_pv_data.kwh_by_day: 0 rows
DELETE FROM `kwh_by_day`;
/*!40000 ALTER TABLE `kwh_by_day` DISABLE KEYS */;
/*!40000 ALTER TABLE `kwh_by_day` ENABLE KEYS */;


# Dumping structure for table dev_pv_data.latest_measures
CREATE TABLE IF NOT EXISTS `latest_measures` (
  `SystemID` smallint(5) unsigned NOT NULL COMMENT 'ID der PV Anlage',
  `InverterID` tinyint(3) unsigned DEFAULT NULL COMMENT 'Inverter ID',
  `SystemStatus` tinyint(3) unsigned DEFAULT NULL COMMENT 'Betriebsart',
  `DBAddDate` datetime NOT NULL COMMENT 'Datum',
  `GeneratorVoltage` double DEFAULT NULL COMMENT 'Generatorspannung in V',
  `GeneratorAmperage` double DEFAULT NULL COMMENT 'Generatorstrom in A (DC)',
  `GeneratorWattage` double DEFAULT NULL COMMENT 'Generatorleistung in W',
  `GridVoltage` double DEFAULT NULL COMMENT 'Netzspannung in V(AC)',
  `GridAmperage` double DEFAULT NULL COMMENT 'Netzstrom, Einspeisestrom in A(AC)',
  `OutputWattage` double DEFAULT NULL COMMENT 'Eingespeiste Leistung in W',
  `SystemTemperature` smallint(6) DEFAULT NULL COMMENT 'Inverter Temperatur'
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

# Dumping data for table dev_pv_data.latest_measures: 0 rows
DELETE FROM `latest_measures`;
/*!40000 ALTER TABLE `latest_measures` DISABLE KEYS */;
/*!40000 ALTER TABLE `latest_measures` ENABLE KEYS */;


# Dumping structure for table dev_pv_data.logs
CREATE TABLE IF NOT EXISTS `logs` (
  `LogLevel` varchar(100) COLLATE latin1_general_ci DEFAULT NULL,
  `ExceptionMessage` varchar(200) COLLATE latin1_general_ci DEFAULT NULL,
  `ExceptionStacktrace` varchar(1000) COLLATE latin1_general_ci DEFAULT NULL,
  `CustomMessage` varchar(200) COLLATE latin1_general_ci DEFAULT NULL,
  `Date` datetime DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

# Dumping data for table dev_pv_data.logs: 0 rows
DELETE FROM `logs`;
/*!40000 ALTER TABLE `logs` DISABLE KEYS */;
/*!40000 ALTER TABLE `logs` ENABLE KEYS */;


# Dumping structure for table dev_pv_data.measures_1
CREATE TABLE IF NOT EXISTS `measures_1` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Messwert ID',
  `InverterID` tinyint(3) unsigned DEFAULT NULL COMMENT 'Inverter ID',
  `SystemStatus` tinyint(3) unsigned DEFAULT NULL COMMENT 'Betriebsart',
  `DBAddDate` datetime NOT NULL COMMENT 'Datum',
  `GeneratorVoltage` double unsigned DEFAULT NULL COMMENT 'Generatorspannung',
  `GeneratorAmperage` double unsigned DEFAULT NULL COMMENT 'Generatorstrom in A (DC)',
  `GeneratorWattage` double unsigned DEFAULT NULL COMMENT 'Generatorleistung in W',
  `GridVoltage` double unsigned DEFAULT NULL COMMENT 'Netzspannung in V(AC)',
  `GridAmperage` double unsigned DEFAULT NULL COMMENT 'Netzstrom, Einspeisestrom in A(AC)',
  `OutputWattage` double unsigned DEFAULT NULL COMMENT 'Eingespeiste Leistung in W',
  `SystemTemperature` smallint(6) DEFAULT NULL COMMENT 'Inverter Temperatur',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

# Dumping data for table dev_pv_data.measures_1: 0 rows
DELETE FROM `measures_1`;
/*!40000 ALTER TABLE `measures_1` DISABLE KEYS */;
/*!40000 ALTER TABLE `measures_1` ENABLE KEYS */;


# Dumping structure for table dev_pv_data.measures_2
CREATE TABLE IF NOT EXISTS `measures_2` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Messwert ID',
  `InverterID` tinyint(3) unsigned DEFAULT NULL COMMENT 'Inverter ID',
  `SystemStatus` tinyint(3) unsigned DEFAULT NULL COMMENT 'Betriebsart',
  `DBAddDate` datetime NOT NULL COMMENT 'Datum',
  `GeneratorVoltage` double unsigned DEFAULT NULL COMMENT 'Generatorspannung',
  `GeneratorAmperage` double unsigned DEFAULT NULL COMMENT 'Generatorstrom in A (DC)',
  `GeneratorWattage` double unsigned DEFAULT NULL COMMENT 'Generatorleistung in W',
  `GridVoltage` double unsigned DEFAULT NULL COMMENT 'Netzspannung in V(AC)',
  `GridAmperage` double unsigned DEFAULT NULL COMMENT 'Netzstrom, Einspeisestrom in A(AC)',
  `OutputWattage` double unsigned DEFAULT NULL COMMENT 'Eingespeiste Leistung in W',
  `SystemTemperature` smallint(6) DEFAULT NULL COMMENT 'Inverter Temperatur',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

# Dumping data for table dev_pv_data.measures_2: 0 rows
DELETE FROM `measures_2`;
/*!40000 ALTER TABLE `measures_2` DISABLE KEYS */;
/*!40000 ALTER TABLE `measures_2` ENABLE KEYS */;


# Dumping structure for table dev_pv_data.pv_systems
CREATE TABLE IF NOT EXISTS `pv_systems` (
  `SystemID` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `Password` varchar(50) CHARACTER SET latin1 NOT NULL,
  `Name` varchar(20) CHARACTER SET latin1 NOT NULL,
  `Settings` varchar(10000) COLLATE latin1_general_ci DEFAULT NULL,
  PRIMARY KEY (`SystemID`),
  UNIQUE KEY `SystemID` (`SystemID`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

# Dumping data for table dev_pv_data.pv_systems: 2 rows
DELETE FROM `pv_systems`;
/*!40000 ALTER TABLE `pv_systems` DISABLE KEYS */;
INSERT INTO `pv_systems` (`SystemID`, `Password`, `Name`, `Settings`) VALUES
	(1, '19057f323b951c696c7935363a63debca977b1c7', 'Test Name', NULL),
	(2, '19057f323b951c696c7935363a63debca977b1c7', 'Testanlage2', NULL);
/*!40000 ALTER TABLE `pv_systems` ENABLE KEYS */;


# Dumping structure for table dev_pv_data.user_owns_system
CREATE TABLE IF NOT EXISTS `user_owns_system` (
  `SystemID` int(10) unsigned NOT NULL COMMENT 'SystemID',
  `UserID` int(11) unsigned NOT NULL COMMENT 'UserID',
  `PlantRole` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`SystemID`,`UserID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;

# Dumping data for table dev_pv_data.user_owns_system: 0 rows
DELETE FROM `user_owns_system`;
/*!40000 ALTER TABLE `user_owns_system` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_owns_system` ENABLE KEYS */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
