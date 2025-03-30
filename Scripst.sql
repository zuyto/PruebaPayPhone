

CREATE DATABASE WalletDb;
GO

USE WalletDb;
GO

CREATE TABLE Wallet (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DocumentId NVARCHAR(20) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NOT NULL
);


CREATE TABLE [Transaction] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    WalletId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Type NVARCHAR(10) NOT NULL, -- 'Credit' o 'Debit'
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Wallet_Transaction FOREIGN KEY (WalletId) REFERENCES Wallet(Id)
);

USE WalletDb;
 GO

INSERT INTO Wallet (DocumentId, Name, Balance, CreatedAt, UpdatedAt)
VALUES 
('12345678', 'John Doe', 1000.00, GETDATE(), GETDATE()),
('87654321', 'Jane Smith', 500.00, GETDATE(), GETDATE());

INSERT INTO [Transaction] (WalletId, Amount, Type, CreatedAt)
VALUES 
(1, 200.00, 'Credit', GETDATE()),
(2, 100.00, 'Debit', GETDATE());