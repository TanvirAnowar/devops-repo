-- --------------------------------------------------------
-- Host:                         54.155.205.148
-- Server version:               8.0.43-0ubuntu0.24.04.2 - (Ubuntu)
-- Server OS:                    Linux
-- HeidiSQL Version:             12.11.0.7065
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for myappdb
DROP DATABASE IF EXISTS `myappdb`;
CREATE DATABASE IF NOT EXISTS `myappdb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `myappdb`;

-- Dumping structure for table myappdb.ActiveOrders
DROP TABLE IF EXISTS `ActiveOrders`;
CREATE TABLE IF NOT EXISTS `ActiveOrders` (
  `Id` varchar(10) NOT NULL DEFAULT '0',
  `IdStopLoss` varchar(10) DEFAULT '0',
  `IdTakeProfit` varchar(10) DEFAULT '0',
  `Unites` varchar(10) DEFAULT NULL,
  `EntryPrice` varchar(10) DEFAULT NULL,
  `StopLossPrice` varchar(10) DEFAULT NULL,
  `TakeProfitPrice` varchar(10) DEFAULT NULL,
  `OrderJsonObject` text,
  `OrderDateTime` datetime DEFAULT NULL,
  `IsOrderActive` bit(1) DEFAULT NULL,
  `ShouldTrailStopLoss` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table myappdb.ActiveOrders: ~1 rows (approximately)
DELETE FROM `ActiveOrders`;
INSERT INTO `ActiveOrders` (`Id`, `IdStopLoss`, `IdTakeProfit`, `Unites`, `EntryPrice`, `StopLossPrice`, `TakeProfitPrice`, `OrderJsonObject`, `OrderDateTime`, `IsOrderActive`, `ShouldTrailStopLoss`) VALUES
	('117', '120', '119', '612000', '1.17415', '1.17247', '1.18423', '{"orderCreateTransaction":{"Id":"117","Time":"2025-09-29T13:19:02.171654226Z","UserID":24777125,"AccountID":"101-004-24777125-001","BatchID":"117","Type":"MARKET_ORDER","Instrument":"EUR_USD","Units":"612000","TimeInForce":"FOK","PositionFill":"DEFAULT","TakeProfitOnFill":{"Price":"1.18423"},"StopLossOnFill":{"Price":"1.17247"}},"orderFillTransaction":{"Id":"118","Time":"2025-09-29T13:19:02.171654226Z","UserID":24777125,"AccountID":"101-004-24777125-001","BatchID":"117","Type":"ORDER_FILL","OrderID":"117","TradeOpened":{"TradeID":"118","Units":"612000","Price":"1.17421"},"Pl":"0.0000","Financing":"0.0000","Commission":"0.0000"},"RelatedTransactionIDs":["117","118","119","120"],"LastTransactionID":"120"}', '2025-09-29 13:19:16', b'0', NULL),
	('125', '128', '127', '631000', '1.17412', '1.17249', '1.18390', '{"orderCreateTransaction":{"Id":"125","Time":"2025-09-29T13:22:53.012516994Z","UserID":24777125,"AccountID":"101-004-24777125-001","BatchID":"125","Type":"MARKET_ORDER","Instrument":"EUR_USD","Units":"631000","TimeInForce":"FOK","PositionFill":"DEFAULT","TakeProfitOnFill":{"Price":"1.18390"},"StopLossOnFill":{"Price":"1.17249"}},"orderFillTransaction":{"Id":"126","Time":"2025-09-29T13:22:53.012516994Z","UserID":24777125,"AccountID":"101-004-24777125-001","BatchID":"125","Type":"ORDER_FILL","OrderID":"125","TradeOpened":{"TradeID":"126","Units":"631000","Price":"1.17378"},"Pl":"0.0000","Financing":"0.0000","Commission":"0.0000"},"RelatedTransactionIDs":["125","126","127","128"],"LastTransactionID":"128"}', '2025-09-29 13:23:15', b'1', NULL);

-- Dumping structure for table myappdb.TradeStatuses
DROP TABLE IF EXISTS `TradeStatuses`;
CREATE TABLE IF NOT EXISTS `TradeStatuses` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StatusKey` varchar(50) DEFAULT NULL,
  `StatusValue` varchar(50) DEFAULT NULL,
  `UpdatedTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table myappdb.TradeStatuses: ~2 rows (approximately)
DELETE FROM `TradeStatuses`;
INSERT INTO `TradeStatuses` (`Id`, `StatusKey`, `StatusValue`, `UpdatedTime`) VALUES
	(1, 'EURUSD_ACTIVE_TRADE', '0', '2025-09-24 14:23:05'),
	(2, 'CLEAR_TO_PLACE_TRADE', '1', '2025-09-24 14:23:45');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
