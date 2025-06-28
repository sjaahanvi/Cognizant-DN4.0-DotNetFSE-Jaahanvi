CREATE DATABASE DeepSkillings;
GO
USE DeepSkillings;
CREATE TABLE Productss(
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(50),
    Category VARCHAR(50),
    Price INT
);
INSERT INTO Productss VALUES
(1, 'iPhone 13', 'Electronics', 80000),
(2, 'Samsung Galaxy', 'Electronics', 60000),
(3, 'MacBook Air', 'Electronics', 120000),
(4, 'Dell XPS', 'Electronics', 100000),
(5, 'Shirt', 'Clothing', 1500),
(6, 'Jacket', 'Clothing', 3000),
(7, 'Jeans', 'Clothing', 2000),
(8, 'Hoodie', 'Clothing', 3000);
SELECT 
    ProductName,
    Category,
    Price,
    ROW_NUMBER() OVER(PARTITION BY Category ORDER BY Price DESC) AS RowNum
FROM Productss;
SELECT 
    ProductName,
    Category,
    Price,
    RANK() OVER(PARTITION BY Category ORDER BY Price DESC) AS PriceRank
FROM Productss;
SELECT 
    ProductName,
    Category,
    Price,
    DENSE_RANK() OVER(PARTITION BY Category ORDER BY Price DESC) AS DensePriceRank
FROM Productss;
WITH RankedProducts AS (
    SELECT 
        *,
        ROW_NUMBER() OVER(PARTITION BY Category ORDER BY Price DESC) AS RowNum
    FROM Productss
)
SELECT ProductName, Category, Price
FROM RankedProducts
WHERE RowNum <= 3;
