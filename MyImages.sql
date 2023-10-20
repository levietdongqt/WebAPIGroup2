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
	[Avatar] [nvarchar](max) NULL,
	[Gender] [bit] NULL,
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
	PricePlusPerOne float null,
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
	MaterialPageId int null,
	TemplateSizeId int null,
	MyImageId int null,
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
	constraint CK_StatusOrder check ([Status] in ('Order Placed','Order Paid','ToShip','Temporary','Received','Canceled'))
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
	MyImagesId int null,
	ImageUrl varchar(500) null,
	FolderName nvarchar(500) null,
	Status bit default(1) not null,
	[Index] int null,
	CreateDate datetime null

	constraint PK_Image primary key (Id),
	constraint CK_Index_Image check ([Index] > 0)

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
create table MyImages(
	Id int identity(1,1) not null,
	TemplateId int null,
	PurchaseOrderId int null,
	Status bit default(1)

	constraint PK_MyImages primary key (Id)
)
go
create table TemplateImage(
	Id int identity(1,1) not null,
	ImageUrl varchar(500) null,
	TemplateId int null,
	[Index] int null

	constraint CK_Index check ([Index] > 0),
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
add constraint FK_ProductDetail_MaterialPage
	foreign key (MaterialPageId) references MaterialPage(Id)
go
alter table ProductDetail
add constraint FK_ProductDetail_MyImages
	foreign key (MyImageId) references MyImages(Id)
go
alter table ProductDetail
add constraint FK_ProductDetail_TemplateSize
	foreign key (TemplateSizeId) references TemplateSize(Id)
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
add constraint Fk_Image_MyImages
	foreign key (MyImagesId) references MyImages(Id)
go
alter table MyImages
add constraint FK_MyImages_Template
	foreign key (TemplateId) references Template(Id)
go
alter table MyImages
add constraint FK_MyImages_PerchaseOrder
	foreign key (PurchaseOrderId) references PurchaseOrder(Id)
----------------------------------------------------------------------------------------
insert into Category
values('Book','/CategoryImage/Book.jpg'),('Calendar','/CategoryImage/Calender.jpg'),('Gift','/CategoryImage/Gift.jpg'),('Prints','/CategoryImage/Poster.jpg'),('Card','/CategoryImage/Card.jpg')
go

insert into [dbo].[Collection]
values('Book 1','/CollectionImage/Bookmarks.jpg',1),('Book 2','/CollectionImage/Bookmarks.jpg',1),('Calendar 1','/CollectionImage/Calender1.jpg',2),('Calendar 2','/CollectionImage/Calender1.jpg',2),('Gift 1','/CollectionImage/houseGift.jpg',3)
,('Gift 2','/CollectionImage/GiftTags.jpg',3),('Prints 1',null,4),('Prints 2',null,4),('Card 1','/CollectionImage/Holidaycards.jpg',5),('Card 2','/CollectionImage/ChristmasCard.jpg',5)
go

insert into Template
values('Prints&Enlargement',0,1,0,GETDATE()),
	  ('4x6 PrintBook',9.9,1,0,GETDATE()),
	  ('8.5x11 Everyday Adventures PrintBook',10.9,1,0,GETDATE()),
	  ('4x8 Desk Calendar',9.99,1,0,GETDATE()),
	  ('3x4 Glass',13.9,1,0,GETDATE()),
	  ('Thank You',2.99,1,0,GETDATE())
go

insert into Template
values('Graphic Year',3.9,1,0,GETDATE()),
	  ('Bright Seasonal',7.9,1,0,GETDATE()),
	  ('Sketchy Expressions',10.9,1,0,GETDATE()),
	  ('Black and Bashful',9.99,1,0,GETDATE()),
	  ('The Life You Love',13.9,1,0,GETDATE())
go

insert into Template
values('A Baby Story',3.9,1,0,GETDATE()),
	  ('Always and Forever',7.9,1,0,GETDATE()),
	  ('JouneyBook',10.9,1,0,GETDATE()),
	  ('Life at the Beach',9.99,1,0,GETDATE()),
	  ('Smitten',13.9,1,0,GETDATE())
go

insert into Template
values('Collage Season Greetings',1.3,1,0,GETDATE()),
	  ('Christmas',1.9,1,0,GETDATE())
go

insert into TemplateImage
  values('/Image/prints1.jpg',1,1);

  insert into TemplateImage
  values('/Image/prints2.jpg',1,2);

   insert into TemplateImage
  values('/Image/prints3.jpg',1,3);

insert into TemplateImage
  values('/Image/book11.jpg',2,1);

   insert into TemplateImage
  values('/Image/book12.jpg',2,2);

   insert into TemplateImage
  values('/Image/book13.jpg',2,3);

insert into TemplateImage
  values('/Image/book21.jpg',3,1);

  insert into TemplateImage
  values('/Image/book22.jpg',3,2);

   insert into TemplateImage
  values('/Image/book23.jpg',3,3);

   insert into TemplateImage
  values('/Image/calendar1.jpg',4,1); 

    insert into TemplateImage
  values('/Image/calendar2.jpg',4,2);

   insert into TemplateImage
  values('/Image/calendar3.jpg',4,3);

insert into TemplateImage
  values('/Image/Glass1.jpg',5,1);

  insert into TemplateImage
  values('/Image/Glass2.jpg',5,2);

   insert into TemplateImage
  values('/Image/Glass3.png',5,3);

 insert into TemplateImage
  values('/Image/card1.jpg',6,1);

  insert into TemplateImage
  values('/Image/card2.jpg',6,2);

  insert into TemplateImage
  values('/Image/gy1.jpg',7,1);

   insert into TemplateImage
  values('/Image/gy2.jpg',7,2);

   insert into TemplateImage
  values('/Image/gy3.jpg',7,3);

   insert into TemplateImage
  values('/Image/bs1.jpg',8,1);

   insert into TemplateImage
  values('/Image/bs2.jpg',8,2);

   insert into TemplateImage
  values('/Image/bs3.jpg',8,3);

   insert into TemplateImage
  values('/Image/se1.jpg',9,1);

  insert into TemplateImage
  values('/Image/se2.jpg',9,2);

  insert into TemplateImage
  values('/Image/se3.jpg',9,3);

  insert into TemplateImage
  values('/Image/baf1.jpg',10,1);

   insert into TemplateImage
  values('/Image/baf2.jpg',10,2);

   insert into TemplateImage
  values('/Image/baf3.jpg',10,3);

  insert into TemplateImage
  values('/Image/tl1.jpg',11,1);

  insert into TemplateImage
  values('/Image/tl2.jpg',11,2);

  insert into TemplateImage
  values('/Image/tl3.jpg',11,3);

  insert into TemplateImage
  values('/Image/abs1.jpg',12,1);

   insert into TemplateImage
  values('/Image/abs2.jpg',12,2);

   insert into TemplateImage
  values('/Image/abs3.jpg',12,3);

   insert into TemplateImage
  values('/Image/aaf1.jpg',13,1);

   insert into TemplateImage
  values('/Image/aaf2.jpg',13,2);

   insert into TemplateImage
  values('/Image/aaf3.jpg',13,3);

   insert into TemplateImage
  values('/Image/jb1.jpg',14,1);

  insert into TemplateImage
  values('/Image/jb2.jpg',14,2);

  insert into TemplateImage
  values('/Image/jb3.jpg',14,3);

  insert into TemplateImage
  values('/Image/latb1.jpg',15,1);

   insert into TemplateImage
  values('/Image/latb2.jpg',15,2);

   insert into TemplateImage
  values('/Image/latb3.jpg',15,3);

  insert into TemplateImage
  values('/Image/s1.jpg',16,1);

  insert into TemplateImage
  values('/Image/s2.jpg',16,2);

  insert into TemplateImage
  values('/Image/s3.jpg',16,3);

   insert into TemplateImage
  values('/Image/csg1.jpg',17,1);

   insert into TemplateImage
  values('/Image/csg2.jpg',17,2);


   insert into TemplateImage
  values('/Image/ct1.jpg',18,1);

   insert into TemplateImage
  values('/Image/ct2.jpg',18,2);

insert into DescriptionTemplate
values('Print and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',1),
	('Product Details','<ul><li>New patented layflat pages for easier display</li><li>Eco-friendly, white matte paper</li><li>Features several photos per page, up to 50 pages (20 pages included in base price)</li></ul>',3),
	('Product Details','<ul><li>5x7 folded card on 85lb. cardstock</li><li>Some cards available in portrait or landscape formats</li><li>Card will print with white border</li></ul>',6),
	('Ship to You','<p>Get your photo products delivered directly to your home or business. Arrives in 3-10 business days</p>',6)
go

insert into DescriptionTemplate
VALUES('Book and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',2)
go

insert into DescriptionTemplate
VALUES('Calendar and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',4)
go

insert into DescriptionTemplate
VALUES('Glass and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',5)
go

 insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',7)
go

  insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',8)
go

  insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',9)
go

  insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',10)
go

  insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',11)
go

insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',12)
go

  insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',13)
go

  insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',14)
go

  insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',15)
go

  insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',16)
go

  insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',17)
go

  insert into DescriptionTemplate
VALUES('Calender and share your favorite memories','<ul><li>Printed on glossy photo paper</li><li>Available sizes: 4x6, 5x7, 8x10, 4x5.3, 4x4 and 8x8</li></ul>',18)
go

insert into PrintSize
values(6,4,GETDATE()),(11,8.5,GETDATE()),(8,4,GETDATE()),(4,3,GETDATE()),(7,5,GETDATE()),(4,4,GETDATE()),(8,8,GETDATE())
go

insert into TemplateSize
values(1,1),(1,3),(1,6),(1,7),(2,1),(3,2),(4,3),(5,4),(6,3)
go

insert into TemplateSize
values(7,1),(7,3),(7,6),(7,7),(8,1),(8,3),(8,6),(8,7),(9,1),(9,3),(9,6),(9,4),(10,1),(10,3),(10,6),(11,7),(11,1),(11,2),(11,3),(11,7)
go

insert into TemplateSize
values(12,1),(12,3),(12,6),(12,7),(13,1),(13,3),(13,6),(13,7),(14,1),(14,3),(14,6),(14,4),(14,1),(15,3),(15,6),(15,7),(16,1),(16,2),(16,3),(16,7)
go

insert into TemplateSize
values(17,1),(17,3),(17,6),(17,7),(18,1),(18,3),(18,6),(18,7)
go


insert into CollectionTemplate(TemplateId,CollectionId)
values(1,2),(2,1),(3,2),(4,3),(4,4),(5,5),(6,9)
go

insert into CollectionTemplate(TemplateId,CollectionId)
values(7,3),(8,3),(9,4),(10,4),(11,3)
go

insert into CollectionTemplate(TemplateId,CollectionId)
values(12,2),(13,1),(14,2),(15,1),(16,2)
go

insert into CollectionTemplate(TemplateId,CollectionId)
values(17,9),(18,10)
go

insert into MaterialPage
values('High',0,0.5,1,'Best material page for print images'),
	   ('Medium',0,0.3,1,'Good material page for print images'),
	   ('Normal',0,0.1,1,'Material page for print images')
go

insert into [dbo].[User]
values('admin@gmail.com','123','Acc Van Min',1,'1995-10-15','356 Pham Van Dong TPHCM','09012345679','',1,'admin','Enabled',GETDATE()),
	  ('user1@gmail.com','123456','Huy Dep Trai',1,'1992-12-05','374 Ap Bac My Dinh','09023332223','',1,'user','Enabled',GETDATE()),
	  ('user2@gmail.com','1234567','Dong Dep Trai',1,'1992-12-05','374 Ap Bac My Dinh','09023332223','',1,'user','Enabled',GETDATE()),
	  ('user3@gmail.com','1234568','Phuoc Dep Trai',1,'1992-12-05','374 Ap Bac My Dinh','09023332223','',1,'user','Enabled',GETDATE()),
	  ('user4@gmail.com','1234562','Nam Dep Trai',1,'1992-12-05','374 Ap Bac My Dinh','09023332223','',1,'user','Enabled',GETDATE()),
	  ('user5@gmail.com','1234562','Minh Dep Trai',1,'1992-12-05','374 Ap Bac My Dinh','09023332223','',1,'user','Enabled',GETDATE())
go

insert into DeliveryInfo
values(2,'huy.tran9510@gmail.com','374 Binh Trieu Tp.HCM','09027837465')
go
insert into ContentEmail
values(1,'Send mail to Buy','Confirm Bill','Sale')
go

insert into Review
values(1,2,'Good page',4,GETDATE())
go

insert into Review
values(1,2,'Bad',2.5,GETDATE())
go

insert into Review
values(1,2,'Web is good',4.7,GETDATE())
go

insert into Review
values(2,2,'Excelent',5,GETDATE())
go

insert into Review
values(2,2,'Web is good',4.2,GETDATE())
go

insert into Review
values(3,3,'Excelent',5,GETDATE())
go

insert into Review
values(3,2,'Web is good',3.5,GETDATE())
go

insert into Review
values(1,2,'Bad',2.5,GETDATE())
go

insert into Review
values(1,2,'Web is good',4.7,GETDATE())
go

insert into Review
values(2,2,'Excelent',5,GETDATE())
go

insert into Review
values(2,2,'Web is good',4.2,GETDATE())
go


insert into Review
values(3,3,'Excelent',5,GETDATE())
go

insert into Review
values(3,2,'Web is good',3.5,GETDATE())
go
