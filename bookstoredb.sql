/*
 Navicat Premium Data Transfer

 Source Server         : My Local 8.0
 Source Server Type    : MySQL
 Source Server Version : 80036
 Source Host           : localhost:3307
 Source Schema         : bookstoredb

 Target Server Type    : MySQL
 Target Server Version : 80036
 File Encoding         : 65001

 Date: 10/07/2025 15:17:03
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for admins
-- ----------------------------
DROP TABLE IF EXISTS `admins`;
CREATE TABLE `admins`  (
  `AdminId` int(0) NOT NULL AUTO_INCREMENT,
  `UserId` int(0) NOT NULL,
  `Department` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `AccessLevel` int(0) NULL DEFAULT 1,
  PRIMARY KEY (`AdminId`) USING BTREE,
  INDEX `UserId`(`UserId`) USING BTREE,
  CONSTRAINT `admins_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of admins
-- ----------------------------
INSERT INTO `admins` VALUES (1, 1, 'IT', 5);

-- ----------------------------
-- Table structure for books
-- ----------------------------
DROP TABLE IF EXISTS `books`;
CREATE TABLE `books`  (
  `BookId` int(0) NOT NULL AUTO_INCREMENT,
  `Title` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Author` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ISBN` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Price` decimal(10, 2) NOT NULL,
  `StockQuantity` int(0) NOT NULL DEFAULT 0,
  `CategoryId` int(0) NOT NULL,
  `Publisher` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `PublishedDate` date NULL DEFAULT NULL,
  `Pages` int(0) NULL DEFAULT NULL,
  `Language` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT 'English',
  `ImageUrl` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `IsActive` tinyint(1) NULL DEFAULT 1,
  `CreatedDate` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedDate` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`BookId`) USING BTREE,
  UNIQUE INDEX `ISBN`(`ISBN`) USING BTREE,
  INDEX `idx_books_title`(`Title`) USING BTREE,
  INDEX `idx_books_category`(`CategoryId`) USING BTREE,
  CONSTRAINT `books_ibfk_1` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`CategoryId`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of books
-- ----------------------------
INSERT INTO `books` VALUES (1, 'The Great Gatsby', 'F. Scott Fitzgerald', '9780743273565', 'A classic American novel', 12.99, 100, 1, 'Scribner', '1925-04-10', 180, 'English', 'https://cdn.pixabay.com/photo/2014/06/03/19/38/board-361516_1280.jpg', 1, '2025-07-08 22:18:28', '2025-07-09 17:59:39');
INSERT INTO `books` VALUES (2, 'Clean Code', 'Robert C. Martin', '9780132350884', 'A handbook of agile software craftsmanship', 45.99, 49, 3, 'Prentice Hall', '2008-08-01', 464, 'English', 'https://cdn.pixabay.com/photo/2014/06/03/19/38/board-361516_1280.jpg', 1, '2025-07-08 22:18:28', '2025-07-09 17:59:39');
INSERT INTO `books` VALUES (3, 'Think and Grow Rich', 'Napoleon Hill', '9781585424337', 'Personal development and success', 15.99, 74, 4, 'Sound Wisdom', '1937-01-01', 238, 'English', 'https://cdn.pixabay.com/photo/2014/06/03/19/38/board-361516_1280.jpg', 1, '2025-07-08 22:18:28', '2025-07-09 17:59:40');
INSERT INTO `books` VALUES (4, 'Sapiens: A Brief History of Humankind', 'Yuval Noah Harari', '978-0-06-231609-7', 'This book offers a broad overview of human history, from the Stone Age to the present day, examining the key turning points in our species\' development.', 10.00, 50, 7, 'Dvir Publishing House Ltd. (Israel)', '2011-01-01', 443, 'English', '', 1, '2025-07-09 17:59:13', '2025-07-09 19:34:01');

-- ----------------------------
-- Table structure for cartitems
-- ----------------------------
DROP TABLE IF EXISTS `cartitems`;
CREATE TABLE `cartitems`  (
  `CartItemId` int(0) NOT NULL AUTO_INCREMENT,
  `CartId` int(0) NOT NULL,
  `BookId` int(0) NOT NULL,
  `Quantity` int(0) NOT NULL DEFAULT 1,
  `Price` decimal(10, 2) NOT NULL,
  `CreatedDate` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`CartItemId`) USING BTREE,
  INDEX `CartId`(`CartId`) USING BTREE,
  INDEX `BookId`(`BookId`) USING BTREE,
  CONSTRAINT `cartitems_ibfk_1` FOREIGN KEY (`CartId`) REFERENCES `carts` (`CartId`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `cartitems_ibfk_2` FOREIGN KEY (`BookId`) REFERENCES `books` (`BookId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for carts
-- ----------------------------
DROP TABLE IF EXISTS `carts`;
CREATE TABLE `carts`  (
  `CartId` int(0) NOT NULL AUTO_INCREMENT,
  `CustomerId` int(0) NOT NULL,
  `CreatedDate` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedDate` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`CartId`) USING BTREE,
  INDEX `CustomerId`(`CustomerId`) USING BTREE,
  CONSTRAINT `carts_ibfk_1` FOREIGN KEY (`CustomerId`) REFERENCES `customers` (`CustomerId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of carts
-- ----------------------------
INSERT INTO `carts` VALUES (1, 1, '2025-07-09 18:04:23', '2025-07-09 23:22:08');

-- ----------------------------
-- Table structure for categories
-- ----------------------------
DROP TABLE IF EXISTS `categories`;
CREATE TABLE `categories`  (
  `CategoryId` int(0) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `IsActive` tinyint(1) NULL DEFAULT 1,
  `CreatedDate` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedDate` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`CategoryId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of categories
-- ----------------------------
INSERT INTO `categories` VALUES (1, 'Fiction', 'Fictional books and novels', 1, '2025-07-08 22:18:28', '2025-07-08 22:18:28');
INSERT INTO `categories` VALUES (2, 'Non-Fiction', 'Non-fictional books and educational content', 1, '2025-07-08 22:18:28', '2025-07-08 22:18:28');
INSERT INTO `categories` VALUES (3, 'Technology', 'Technology and programming books', 1, '2025-07-08 22:18:28', '2025-07-08 22:18:28');
INSERT INTO `categories` VALUES (4, 'Business', 'Business and management books', 1, '2025-07-08 22:18:28', '2025-07-08 22:18:28');
INSERT INTO `categories` VALUES (5, 'Health', 'Health and wellness books', 1, '2025-07-08 22:18:28', '2025-07-08 22:18:28');
INSERT INTO `categories` VALUES (6, 'History', 'Historical books and documentation', 1, '2025-07-08 22:18:28', '2025-07-08 22:18:28');
INSERT INTO `categories` VALUES (7, 'Science', 'Scientific books and research', 1, '2025-07-08 22:18:28', '2025-07-08 22:18:28');
INSERT INTO `categories` VALUES (8, 'Biography', 'Biographies and memoirs', 1, '2025-07-08 22:18:28', '2025-07-08 22:18:28');

-- ----------------------------
-- Table structure for customers
-- ----------------------------
DROP TABLE IF EXISTS `customers`;
CREATE TABLE `customers`  (
  `CustomerId` int(0) NOT NULL AUTO_INCREMENT,
  `UserId` int(0) NOT NULL,
  `Address` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `DateOfBirth` date NULL DEFAULT NULL,
  `Gender` enum('Male','Female','Other') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  PRIMARY KEY (`CustomerId`) USING BTREE,
  INDEX `UserId`(`UserId`) USING BTREE,
  CONSTRAINT `customers_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of customers
-- ----------------------------
INSERT INTO `customers` VALUES (1, 2, 'Epic Techno Village, Kaduwela Road', '1991-11-19', 'Male');

-- ----------------------------
-- Table structure for feedbacks
-- ----------------------------
DROP TABLE IF EXISTS `feedbacks`;
CREATE TABLE `feedbacks`  (
  `FeedbackId` int(0) NOT NULL AUTO_INCREMENT,
  `CustomerId` int(0) NOT NULL,
  `BookId` int(0) NOT NULL,
  `Rating` int(0) NULL DEFAULT NULL,
  `Comment` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `IsApproved` tinyint(1) NULL DEFAULT 0,
  `CreatedDate` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`FeedbackId`) USING BTREE,
  INDEX `idx_feedbacks_book`(`BookId`) USING BTREE,
  INDEX `idx_feedbacks_customer`(`CustomerId`) USING BTREE,
  CONSTRAINT `feedbacks_ibfk_1` FOREIGN KEY (`CustomerId`) REFERENCES `customers` (`CustomerId`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `feedbacks_ibfk_2` FOREIGN KEY (`BookId`) REFERENCES `books` (`BookId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of feedbacks
-- ----------------------------
INSERT INTO `feedbacks` VALUES (1, 1, 4, 1, 'This is best book', 0, '2025-07-09 19:35:35');
INSERT INTO `feedbacks` VALUES (2, 1, 4, 1, 'xfgf', 0, '2025-07-10 02:06:23');

-- ----------------------------
-- Table structure for orderdetails
-- ----------------------------
DROP TABLE IF EXISTS `orderdetails`;
CREATE TABLE `orderdetails`  (
  `OrderDetailId` int(0) NOT NULL AUTO_INCREMENT,
  `OrderId` int(0) NOT NULL,
  `BookId` int(0) NOT NULL,
  `Quantity` int(0) NOT NULL,
  `UnitPrice` decimal(10, 2) NOT NULL,
  `TotalPrice` decimal(10, 2) NOT NULL,
  PRIMARY KEY (`OrderDetailId`) USING BTREE,
  INDEX `OrderId`(`OrderId`) USING BTREE,
  INDEX `BookId`(`BookId`) USING BTREE,
  CONSTRAINT `orderdetails_ibfk_1` FOREIGN KEY (`OrderId`) REFERENCES `orders` (`OrderId`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `orderdetails_ibfk_2` FOREIGN KEY (`BookId`) REFERENCES `books` (`BookId`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of orderdetails
-- ----------------------------
INSERT INTO `orderdetails` VALUES (1, 1, 2, 1, 45.99, 45.99);
INSERT INTO `orderdetails` VALUES (2, 2, 3, 1, 15.99, 15.99);

-- ----------------------------
-- Table structure for orders
-- ----------------------------
DROP TABLE IF EXISTS `orders`;
CREATE TABLE `orders`  (
  `OrderId` int(0) NOT NULL AUTO_INCREMENT,
  `CustomerId` int(0) NOT NULL,
  `OrderNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TotalAmount` decimal(10, 2) NOT NULL,
  `OrderStatus` enum('Pending','Processing','Shipped','Delivered','Cancelled') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT 'Pending',
  `ShippingAddress` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PaymentMethod` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `PaymentStatus` enum('Pending','Paid','Failed','Refunded') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT 'Pending',
  `OrderDate` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP,
  `ShippedDate` datetime(0) NULL DEFAULT NULL,
  `DeliveredDate` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`OrderId`) USING BTREE,
  UNIQUE INDEX `OrderNumber`(`OrderNumber`) USING BTREE,
  INDEX `idx_orders_customer`(`CustomerId`) USING BTREE,
  INDEX `idx_orders_status`(`OrderStatus`) USING BTREE,
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`CustomerId`) REFERENCES `customers` (`CustomerId`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of orders
-- ----------------------------
INSERT INTO `orders` VALUES (1, 1, 'ORD000001', 45.99, 'Shipped', 'Matugama', 'Cash on Delivery', 'Pending', '2025-07-09 18:04:55', '2025-07-09 22:10:17', NULL);
INSERT INTO `orders` VALUES (2, 1, 'ORD000002', 15.99, 'Pending', 'Kaluthara', 'Credit Card', 'Pending', '2025-07-09 23:27:24', NULL, NULL);

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `UserId` int(0) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Phone` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `UserType` enum('Admin','Customer') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsActive` tinyint(1) NULL DEFAULT 1,
  `CreatedDate` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedDate` datetime(0) NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(0),
  PRIMARY KEY (`UserId`) USING BTREE,
  UNIQUE INDEX `Email`(`Email`) USING BTREE,
  INDEX `idx_users_email`(`Email`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES (1, 'Admin', 'User', 'admin@bookstore.com', '$2a$12$KhpOZ80K/SX.WH1CkR81PO9XYxinIGbhEO5xoAnau3PYRGLx35Vje', NULL, 'Admin', 1, '2025-07-08 22:18:28', '2025-07-09 17:17:16');
INSERT INTO `users` VALUES (2, 'Chinthaka', 'Wahalathanthri', 'chinthaka@gmail.com', '$2a$11$WZeSdkfSq3sUctXkYG/fTe3K0hLC0niG7Np.g6dH5LaaUy/0yIexW', '0717730773', 'Customer', 1, '2025-07-09 18:03:24', '2025-07-09 21:59:07');

SET FOREIGN_KEY_CHECKS = 1;
