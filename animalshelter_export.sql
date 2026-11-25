-- Állatmenhely - teljes adatbázis export (animalshelter)
CREATE DATABASE IF NOT EXISTS `animalshelter` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `animalshelter`;

DROP TABLE IF EXISTS `animals`;
DROP TABLE IF EXISTS `shelters`;

CREATE TABLE `shelters` (
  `Id` char(36) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `City` varchar(40) NOT NULL,
  `Capacity` int NOT NULL,
  `RegTime` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `shelters` (`Id`, `Name`, `City`, `Capacity`, `RegTime`) VALUES
('dd87a9dd-de9b-55c7-8fa6-fdafdfcc4a64', 'Négylábú Otthon', 'Miskolc', 30, '2026-04-10 08:46:25'),
('8d32af8e-bca2-5907-997f-05ff3b325fe7', 'Gazdira Vár', 'Debrecen', 25, '2026-08-05 08:04:01'),
('95f1187f-f2c2-54b7-afc1-3dba453580ab', 'Cirmos Menedék', 'Eger', 15, '2026-07-18 11:51:48'),
('a2691474-e139-594f-a782-443415ee7d41', 'Boldog Mancs', 'Szeged', 40, '2026-01-08 15:34:23'),
('c41845d3-62ac-5c31-9b16-80193b82aec0', 'Kis Menedék', 'Salgótarján', 3, '2026-05-25 09:52:06');

CREATE TABLE `animals` (
  `Id` char(36) NOT NULL,
  `Name` varchar(40) NOT NULL,
  `Species` varchar(30) NOT NULL,
  `Age` int NOT NULL,
  `Weight` double NOT NULL,
  `RegTime` datetime DEFAULT CURRENT_TIMESTAMP,
  `ShelterId` char(36) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `ShelterId` (`ShelterId`),
  CONSTRAINT `animals_ibfk_1` FOREIGN KEY (`ShelterId`) REFERENCES `shelters` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `animals` (`Id`, `Name`, `Species`, `Age`, `Weight`, `RegTime`, `ShelterId`) VALUES
('11111111-1111-4111-8111-111111111111', 'Bodri',  'kutya',   4, 18.5, '2026-04-12 09:00:00', 'dd87a9dd-de9b-55c7-8fa6-fdafdfcc4a64'),
('22222222-2222-4222-8222-222222222222', 'Szurka', 'macska',  2,  3.8, '2026-04-15 10:30:00', 'dd87a9dd-de9b-55c7-8fa6-fdafdfcc4a64'),
('33333333-3333-4333-8333-333333333333', 'Rex',    'kutya',   7, 27.2, '2026-08-06 08:15:00', '8d32af8e-bca2-5907-997f-05ff3b325fe7'),
('44444444-4444-4444-8444-444444444444', 'Cirmi',  'macska',  1,  2.9, '2026-07-19 12:00:00', '95f1187f-f2c2-54b7-afc1-3dba453580ab'),
('55555555-5555-4555-8555-555555555555', 'Nyuszi', 'nyúl',    3,  1.7, '2026-01-10 14:20:00', 'a2691474-e139-594f-a782-443415ee7d41'),
('66666666-6666-4666-8666-666666666666', 'Pötty',  'kutya',   5, 12.4, '2026-05-26 09:00:00', 'c41845d3-62ac-5c31-9b16-80193b82aec0'),
('77777777-7777-4777-8777-777777777777', 'Mici',   'macska',  6,  4.1, '2026-05-26 09:10:00', 'c41845d3-62ac-5c31-9b16-80193b82aec0'),
('88888888-8888-4888-8888-888888888888', 'Hópihe', 'nyúl',    2,  1.9, '2026-05-26 09:20:00', 'c41845d3-62ac-5c31-9b16-80193b82aec0');
