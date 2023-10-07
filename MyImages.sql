USE [master]
GO
DROP DATABASE if exists MyImages
GO
create  database MyImages
go
use MyImages
go
CREATE TABLE [User] (
	[Id] int identity(1,1) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[Password] [nvarchar](256) NULL,
	[FullName] [nvarchar] (256) NULL,
	[EmailConfirmed] [bit] default 0,
	[DateOfBirth] date null,
	[Address] nvarchar(256) null,
	[Phone] [nvarchar](max) NULL,
	[Role] varchar(50) constraint CK_Role check (  [Role] in ('admin','user','vip')) default('user'),
	[Status] varchar(10) constraint CK_Status check ([Status] in ('Pending','Enabled','Disabled')) default('Pending'),
	CreateDate datetime null

	constraint PK_User primary key (Id),
	constraint UNI_Email unique (Email),

)
go
create table PrintSize(
	Id int identity(1,1) not null,
	[Length] float null,
	[Width] float null,
	CreateDate datetime null,

	constraint PK_PrintSize Primary key (Id)
)
go
create table TemplateSize(
	Id int identity(1,1) not null,
	TemplateId int null,
	PrintSizeId int null,

	constraint PK_TemplateSize primary key (Id)
)
go
create table Template(
	Id int identity(1,1) not null,
	Name nvarchar(max) null,
	PricePlus decimal null,
	Status bit default(1) not null,
	QuantitySold int null,
	CreateDate datetime null

	constraint PK_Template primary key (Id)
)
go
create table Category(
	Id int identity(1,1) not null,
	Name nvarchar(max) null,
	ImageUrl varchar(500) null

	constraint PK_Category primary key (Id)
)
go

create table [dbo].[Collection](
	Id int identity(1,1) not null,
	Name nvarchar(max) null,
	ImageUrl varchar(500) null,
	CategoryId int null

	constraint PK_Collection primary key (Id)
)
go

create table CollectionTemplate(
	Id int identity(1,1) not null,
	CollectionId int null,
	TemplateId int null

	constraint PK_CollectionTemplate primary key (Id)
)
go
create table MaterialPage(
	Id int identity(1,1) not null,
	Name nvarchar(max) null,
	InchSold decimal null,
	PricePerInch float  null,
	Status bit default(1) not null,
	[Description] nvarchar(max)

	constraint PK_MaterialPage primary key (Id)

)
go
create table ProductDetail (
	Id int identity(1,1) not null,
	TemplateId int null,
	MaterialPageId int null,
	PurchaseOrderId int null,
	Price decimal null,
	Quantity int null,
	Status bit default(1),
	CreateDate datetime

	constraint PK_ProductDetail primary key (Id)
)
go
create table PurchaseOrder(
	Id int identity(1,1) not null,
	UserId int null,
	DeliveryInfoId int null,
	CreditCard varchar(max) null,
	PriceTotal decimal null,
	Status varchar(40) default('Pending') not null,
	CreateDate datetime null

	constraint PK_PurchaseOrder primary key (Id),
	constraint CK_StatusOrder check ([Status] in ('Order Placed','Order Paid','ToShip','Temporary'))
)
go
create table Review(
	Id int identity(1,1) not null,
	TemplateId int null,
	UserId int null,
	Content ntext null,
	Rating float null,
	ReviewDate datetime

	constraint PK_Review primary key (Id)
)
go
create table MonthlySpending(
	Id int identity(1,1) not null,
	UserId int null,
	Total decimal null,
	TimeDetail datetime null

	constraint PK_MonthlySpending primary key (Id)
)
go
create table [Image](
	Id int identity(1,1) not null,
	ProductDetailId int null,
	ImageUrl varchar(500) null,
	FolderName nvarchar(500) null,
	Status bit default(1) not null,
	CreateDate datetime null

	constraint PK_Image primary key (Id)
)
go
create table FeedBack(
	Id int identity(1,1) not null,
	Content ntext null,
	UserId int null,
	[Email] [nvarchar](256) NULL,
	FeedBackDate datetime null

	constraint PK_FeedBack primary key (Id)
)
go
create table DeliveryInfo(
	Id int identity(1,1) not null,
	UserId int null,
	[Email] [nvarchar](256) NULL,
	DeliveryAddress ntext null,
	[Phone] [nvarchar](max) NULL

	constraint PK_DeliveryInfo primary key (Id)
)
go
create table ContentEmail(
	Id int identity(1,1) not null,
	DeliveryInfoId int null,
	SubjectEmail varchar(256) null,
	BodyEmail ntext null,
	Type varchar(256) null

	constraint PK_ContentEmail primary key (Id)
)
go
create table RefeshToken (
	Id int identity(1,1) not null,
	UserId int null,
	JwtId varchar(max) null,
	Token varchar(max) null,
	IsUsed bit default(1) not null,
	IsRevoked bit default(1) not null,
	IssueAt datetime null,
	ExpireAt datetime null

	constraint PK_RefeshToken primary key (Id)
)
go
create table TemplateImage(
	Id int identity(1,1) not null,
	ImageUrl varchar(500) null,
	TemplateId int null,

	constraint PK_TemplateImage primary key (Id)
)
go
create table DescriptionTemplate(
	Id int identity(1,1) not null,
	Title varchar(max) null,
	[Description] ntext null,
	TemplateId int null

	constraint PK_DescriptionTemplate primary key (Id)
)
go
alter table TemplateSize
add constraint FK_TemplateSize_PrintSize
	foreign key (PrintSizeId) references PrintSize(Id)
go
alter table TemplateSize
add constraint FK_TemplateSize_Template
	foreign key (TemplateId) references Template(Id)
go

alter table [dbo].[Collection]
add constraint FK_Collection
	foreign key (CategoryId) references Category(Id)

alter table CollectionTemplate
add constraint FK_CollectionTemplate_Collection
	foreign key (CollectionId) references [dbo].[Collection](Id)
go
alter table CollectionTemplate
add constraint FK_CollectionTemplate_Template
	foreign key (TemplateId) references Template(Id)
go
alter table ProductDetail
add constraint FK_ProductDetail_Template
	foreign key (TemplateId) references TemplateSize(Id)
go
alter table ProductDetail
add constraint FK_ProductDetail_MaterialPage
	foreign key (MaterialPageId) references MaterialPage(Id)
go
alter table ProductDetail
add constraint FK_ProductDetail_PurchaseOrder
	foreign key (PurchaseOrderId) references PurchaseOrder(Id)
go
alter table PurchaseOrder
add constraint FK_PurchaseOrder_User
	foreign key (UserId) references [User](Id)
go
alter table PurchaseOrder
add constraint FK_PurchaseOrder_Delivery
	foreign key (DeliveryInfoId) references DeliveryInfo(Id)
go
alter table Review
add constraint FK_Review_Detail
	foreign key (TemplateId) references Template(Id)
go
alter table Review
add constraint FK_Review_User
	foreign key (UserId) references [dbo].[User](Id)
go

alter table MonthlySpending
add constraint FK_MonthlySpending_User
	foreign key (UserId) references [User](Id)
go
alter table FeedBack
add constraint FK_FeedBack_User
	foreign key (UserId) references [User](Id)
go
alter table DeliveryInfo
add constraint FK_DeliveryInfo_User
	foreign key (UserId) references [User](Id)
go
alter table ContentEmail
add constraint FK_ContentEmail_DeliveryInfo
	foreign key (DeliveryInfoId) references DeliveryInfo(Id)
go
alter table RefeshToken
add constraint Fk_RefeshToken_User
	foreign key (UserId) references [User](Id)
go
alter table DescriptionTemplate
add constraint Fk_Description_Template
	foreign key (TemplateId) references [Template](Id)
go
alter table TemplateImage
add constraint Fk_Template_Image
	foreign key (TemplateId) references [Template](Id)
go
alter table Image
add constraint Fk_Product_Detail
	foreign key (ProductDetailId) references [ProductDetail](Id)
go
----------------------------------------------------------------------------------------
insert into Category
values('Book',null),('Calendar',null),('Gift',null),('Prints',null),('Card',null)
go

insert into [dbo].[Collection]
values('Book 1',null,1),('Book 2',null,1),('Calendar 1',null,2),('Calendar 2',null,2),('Gift 1',null,3)
,('Gift 2',null,3),('Prints 1',null,4),('Prints 2',null,4),('Card 1',null,5),('Card 2',null,5)
go

insert into Template
values('Prints&Enlargement',0,1,0,GETDATE()),
	  ('4x6 PrintBook',9.9,1,0,GETDATE()),
	  ('8.5x11 Everyday Adventures PrintBook',10.9,1,0,GETDATE()),
	  ('4x8 Desk Calendar',9.99,1,0,GETDATE()),
	  ('3x4 Glass',13.9,1,0,GETDATE()),
	  ('Thank You',2.99,1,0,GETDATE())
go

insert into TemplateImage
values('https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1',1),
	  ('https://images.pexels.com/photos/2226900/pexels-photo-2226900.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1',2),
	  ('https://images.pexels.com/photos/808466/pexels-photo-808466.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1',3),
	  ('https://images.pexels.com/photos/5342974/pexels-photo-5342974.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1',4),
	  ('https://images.pexels.com/photos/5342974/pexels-photo-5342974.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1',5),
	  ('https://images.pexels.com/photos/808466/pexels-photo-808466.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1',6)
go

insert into DescriptionTemplate
values('Print and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',1),
	('Product Details','<ul><li>New patented layflat pages for easier display</li><li>Eco-friendly, white matte paper</li><li>Features several photos per page, up to 50 pages (20 pages included in base price)</li></ul>',3),
	('Product Details','<ul><li>5x7 folded card on 85lb. cardstock</li><li>Some cards available in portrait or landscape formats</li><li>Card will print with white border</li></ul>',6),
	('Ship to You','<p>Get your photo products delivered directly to your home or business. Arrives in 3-10 business days</p>',6)
go

insert into PrintSize
values(6,4,GETDATE()),(11,8.5,GETDATE()),(8,4,GETDATE()),(4,3,GETDATE()),(7,5,GETDATE()),(4,4,GETDATE()),(8,8,GETDATE())
go

insert into TemplateSize
values(1,1),(1,3),(1,6),(1,7),(2,1),(3,2),(4,3),(5,4),(6,3)
go

insert into CollectionTemplate(TemplateId,CollectionId)
values(1,2),(2,1),(3,2),(4,3),(4,4),(5,5),(6,9)
go


insert into MaterialPage
values('High',0,0.5,1,'Best material page for print images'),
	   ('Medium',0,0.3,1,'Good material page for print images'),
	   ('Normal',0,0.1,1,'Material page for print images')
go

insert into [dbo].[User]
values('admin@gmail.com','123','Acc Van Min',1,'1995-10-15','356 Pham Van Dong TPHCM','09012345679','admin','Enabled',GETDATE()),
	  ('user1@gmail.com','123456','Huy Dep Trai',1,'1992-12-05','374 Ap Bac My Dinh','09023332223','user','Enabled',GETDATE()),
	  ('user2@gmail.com','1234567','Dong Dep Trai',1,'1992-12-05','374 Ap Bac My Dinh','09023332223','user','Enabled',GETDATE()),
	  ('user3@gmail.com','1234568','Phuoc Dep Trai',1,'1992-12-05','374 Ap Bac My Dinh','09023332223','user','Enabled',GETDATE()),
	  ('user4@gmail.com','1234562','Nam Dep Trai',1,'1992-12-05','374 Ap Bac My Dinh','09023332223','user','Enabled',GETDATE()),
	  ('user5@gmail.com','1234562','Minh Dep Trai',1,'1992-12-05','374 Ap Bac My Dinh','09023332223','user','Enabled',GETDATE())
go

insert into DeliveryInfo
values(2,'huy.tran9510@gmail.com','374 Binh Trieu Tp.HCM','09027837465')
go

insert into PurchaseOrder
values(2,1,'1303 3333 9999 8888',0,'Temporary',GETDATE())
go

insert into ProductDetail
values(1,1,1,10.3,5,1,GETDATE())
go

insert into Image
values(1,'https://images.pexels.com/photos/5342974/pexels-photo-5342974.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1','2-user1',1,GETDATE()),
	  (1,'https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1','2-user1',1,GETDATE())
go




insert into ContentEmail
values(1,'Send mail to Buy','Confirm Bill','Sale')
go

insert into Review
values(1,2,'Good page',4,GETDATE())
go