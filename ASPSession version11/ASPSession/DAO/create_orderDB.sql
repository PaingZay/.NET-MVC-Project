create DataBase ShoppingCartDB
go
use ShoppingCartDB
go

create Table [dbo].[Orders]
([OrderID] [int] NOT NULL,
[CustomerID][int]NOT NULL,
[ProductID][int] NOT NULL,
[OrderDate][datetime] NOT NULL,
[ProductQty][int] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([OrderId] ASC)
)
GO

create Table[dbo].[ActivationCodes]
([OrderID] [int] NOT NULL,
[ActivationID][uniqueidentifier]NOT NULL,
CONSTRAINT [PK_ActivationCodes] PRIMARY KEY CLUSTERED ([ActivationID] ASC)
)
GO

INSERT INTO dbo.[Orders](OrderID,CustomerID,ProductID,OrderDate,ProductQty) VALUES
(N'10020',N'11',N'1',N'2022-10-06',N'2'),
(N'10021',N'12',N'2',N'2022-10-03',N'1'),
(N'10022',N'13',N'5',N'2022-09-30',N'3'),
(N'10024',N'11',N'3',N'2022-09-29',N'1'),
(N'10026',N'11',N'4',N'2022-09-01',N'1')


