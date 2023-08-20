--USE master
--Drop database JavaFloristDb
--Create database JavaFloristDb
USE JavaFloristDb
GO


CREATE TABLE [Status]
(
    Id int identity,
    [Type] nvarchar(50) not null,
    EntityId int not null, 
    Note nvarchar(50),
    CONSTRAINT PK_Status PRIMARY KEY CLUSTERED (Id ASC) 
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ;
GO

CREATE TABLE Blog (
    Id int identity,
	[Image] nvarchar(max),
    Title nvarchar(255)  not null,
	BlogBrief nvarchar(255) not null,
    Content nvarchar(max)  not null,
    PublishDate datetime  not null,
    UserId int, --Author
	[StatusId] int,
	CONSTRAINT PK_Blog PRIMARY KEY CLUSTERED (Id ASC) 
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ;
go


CREATE TABLE [User]
(     
    Id int identity,
    PasswordHash varbinary(max) not null,
	PasswordSalt varbinary(max) not null,
    Email nvarchar(255) not null,
    Phone nvarchar(20) not null, 
    [Address] nvarchar(255), 
    Firstname nvarchar(50) not null,
    LastName nvarchar(50) not null, 
    FullName nvarchar(100) not null,
    Gender nvarchar(10) not null, 
    [DOB] date,
    [Role] nvarchar(30) not null,
	RefreshToken nvarchar(MAX),
	PasswordResetToken nvarchar(MAX),
	TokenCreated datetime2(7),
	TokenExpires datetime2(7),
    [StatusId] int,
    CONSTRAINT PK_User PRIMARY KEY CLUSTERED (Id ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ;
GO

CREATE TABLE Category(
    Id int identity,
    [Name] nvarchar(50) not null, 
    [Type] nvarchar(50) not null,
    CONSTRAINT PK_Category PRIMARY KEY CLUSTERED (Id ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ;
GO

-- create table Occasion
CREATE TABLE Occasion(
    Id int identity,
    [Message] nvarchar(255) not null, 
    [Type] nvarchar(50) not null,
    CONSTRAINT PK_Occasion PRIMARY KEY CLUSTERED (Id ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ;
GO

--Supplier
CREATE TABLE Florist(
    Id int identity,
    [Name] nvarchar(50) not null, 
    Logo nvarchar(255) not null, 
    Email nvarchar(255) not null, 
    Phone nvarchar(20) not null, 
    [Address] nvarchar(255) not null, 
    UserId int ,
    [StatusId] int,
    CONSTRAINT PK_Florist PRIMARY KEY CLUSTERED (Id ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ;
GO
--Product
CREATE TABLE Bouquet(
    Id int identity,
    [Name] nvarchar(50) not null,
    [UnitBrief] nvarchar(max) not null,
    [UnitPrice]float not null,
    [Image] nvarchar(255) not null,
    [BouquetDate] datetime not null,
    [Available] bit not null,
    [Description] nvarchar(max) not null,
    [CategoryId] int ,
    [FloristId] int ,
    [Quantity] int not null,
    [Discount]float,
    [Tag] nvarchar(50),
    [PriceAfter]float not null,
    CONSTRAINT PK_Bouquet PRIMARY KEY CLUSTERED (Id ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ;
GO

CREATE TABLE Receiver(
    Id int identity,
    [Name] nvarchar(50) not null,
    [Address] nvarchar(50) not null,
    Phone nvarchar(50) not null,
    ReceiverDate datetime not null,
    CONSTRAINT PK_Receiver PRIMARY KEY CLUSTERED (Id ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ;
GO

CREATE TABLE [Order](
    Id int identity,
    UserId int, 
    OrderDate datetime not null, 
    ReceiveDate datetime not null,
    ReceiverId int ,
    [Address] nvarchar(255) not null, 
    [OccasionId] int,
    Amount float not null,
    [StatusId] int,
    CONSTRAINT PK_Order PRIMARY KEY CLUSTERED (Id ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ;
GO

CREATE TABLE OrderDetail(
    Id int identity,
    OrderId int,
    BouquetId int,
    UnitPrice float not null, 
    Quantity int not null, 
    Discount float,
    CONSTRAINT PK_OrderDetail PRIMARY KEY CLUSTERED (Id ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ;
GO

CREATE TABLE Voucher(
    Id int identity,
    Code nvarchar(50) not null,
    UserId int ,
    DiscountPercent float not null,
    [StatusId] int,
    CONSTRAINT PK_Voucher PRIMARY KEY CLUSTERED (Id ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ;
GO

-- Table: [User]
ALTER TABLE [User]
WITH CHECK ADD CONSTRAINT FK_User_Status FOREIGN KEY ([StatusId]) 
REFERENCES [Status] (Id) 
ON UPDATE CASCADE 
ON DELETE CASCADE;
GO
-- Table: Florist
ALTER TABLE Florist
WITH CHECK ADD CONSTRAINT FK_Florist_User FOREIGN KEY (UserId) 
REFERENCES [User] (Id) 
ON UPDATE CASCADE 
ON DELETE CASCADE;
GO
ALTER TABLE Florist
WITH CHECK ADD CONSTRAINT FK_Florist_Status FOREIGN KEY (StatusId) 
REFERENCES [Status] (Id) 
GO
-- Table: Bouquet
ALTER TABLE Bouquet
WITH CHECK ADD CONSTRAINT FK_Bouquet_Category FOREIGN KEY (CategoryId) 
REFERENCES Category (Id) 
ON UPDATE CASCADE 
ON DELETE CASCADE;
GO
ALTER TABLE Bouquet
WITH CHECK ADD CONSTRAINT FK_Bouquet_Florist FOREIGN KEY (FloristId) 
REFERENCES Florist (Id) 
ON UPDATE CASCADE 
ON DELETE CASCADE;
GO
-- Table: [Order]
ALTER TABLE [Order]
WITH CHECK ADD CONSTRAINT FK_Order_Receiver FOREIGN KEY (ReceiverId) 
REFERENCES Receiver (Id) 
ON UPDATE CASCADE 
ON DELETE CASCADE;
GO

ALTER TABLE [Order]
WITH CHECK ADD CONSTRAINT FK_Order_Status FOREIGN KEY ([StatusId]) 
REFERENCES [Status] (Id) 
GO

ALTER TABLE [Order]
WITH CHECK ADD CONSTRAINT FK_Order_Occasion FOREIGN KEY ([OccasionId]) 
REFERENCES [Occasion] (Id) 
GO
-- Table: OrderDetail
ALTER TABLE OrderDetail
WITH CHECK ADD CONSTRAINT FK_OrderDetail_Order FOREIGN KEY (OrderId) 
REFERENCES [Order] (Id) 
ON UPDATE CASCADE 
ON DELETE CASCADE;
GO
ALTER TABLE OrderDetail
WITH CHECK ADD CONSTRAINT FK_OrderDetail_Bouquet FOREIGN KEY (BouquetId) 
REFERENCES Bouquet (Id)
ON UPDATE CASCADE 
ON DELETE CASCADE;
GO
-- Table: Voucher
ALTER TABLE Voucher
WITH CHECK ADD CONSTRAINT FK_Voucher_User FOREIGN KEY (UserId) 
REFERENCES [User] (Id) 
ON UPDATE CASCADE 
ON DELETE CASCADE;
GO

ALTER TABLE Voucher
WITH CHECK ADD CONSTRAINT FK_Voucher_Status FOREIGN KEY (StatusId) 
REFERENCES [Status] (Id) 
GO
-- Table: Blog
ALTER TABLE Blog
WITH CHECK ADD CONSTRAINT FK_Blog_User FOREIGN KEY (UserId) 
REFERENCES [User] (Id) 
ON UPDATE CASCADE 
ON DELETE CASCADE;
GO

ALTER TABLE Blog
WITH CHECK ADD CONSTRAINT FK_Blog_Status FOREIGN KEY ([StatusId]) 
REFERENCES [Status] (Id) 
GO

--Value
INSERT INTO [Status] ([Type], EntityId, Note)
VALUES ('Fail', 1, 'Progress is Faile '),
       ('Success', 2, 'Progress is Complete');   
GO

INSERT INTO [User] (PasswordHash, PasswordSalt, Email, Phone, [Address], Firstname, LastName, FullName, Gender, [DOB], [Role], RefreshToken, PasswordResetToken, TokenCreated, TokenExpires, [StatusId])
VALUES
    -- Sample 1
    (CONVERT(varbinary(max), 'SamplePasswordHash1'), CONVERT(varbinary(max), 'SamplePasswordSalt1'), 'myflowergift@gmail.com', '1234567890', '123 Sample St, City1', 'John', 'Doe', 'John Doe', 'Male', '1990-01-01', 'Florist', 'SampleRefreshToken1', 'SampleResetToken1', '2023-07-31 12:00:00', '2023-08-31 12:00:00', 2),
    
    -- Sample 2
    (CONVERT(varbinary(max), 'SamplePasswordHash2'), CONVERT(varbinary(max), 'SamplePasswordSalt2'), 'interfloria@gmail.com', '0987654321', '456 Sample Ave, City2', 'Jane', 'Smith', 'Jane Smith', 'Female', '1985-05-15', 'Florist', 'SampleRefreshToken2', 'SampleResetToken2', '2023-07-31 12:00:00', '2023-08-31 12:00:00', 2),

    -- Sample 3
    (CONVERT(varbinary(max), 'SamplePasswordHash3'), CONVERT(varbinary(max), 'SamplePasswordSalt3'), 'flowerchimp@gmail.com', '5555555555', '789 Sample Rd, City3', 'Michael', 'Johnson', 'Michael Johnson', 'Male', '1988-12-20', 'Admin', 'SampleRefreshToken3', 'SampleResetToken3', '2023-07-31 12:00:00', '2023-08-31 12:00:00', 2),

 

    -- Sample 10
    (CONVERT(varbinary(max), 'SamplePasswordHash10'), CONVERT(varbinary(max), 'SamplePasswordSalt10'), 'user10@example.com', '7777777777', '999 Sample Ln, City10', 'Sarah', 'Lee', 'Sarah Lee', 'Female', '1995-09-10', 'Florist', 'SampleRefreshToken10', 'SampleResetToken10', '2023-07-31 12:00:00', '2023-08-31 12:00:00', 1);

INSERT INTO Category ([Name], [Type])
VALUES ('Birthday Flowers', 'Birthday Flowers'),
       ('Congratulation Flowers', 'Congratulation Flowers'),
       ('Get Well', 'Get Well'),
       ('Graduation', 'Graduation'),
       ('Grand Opening Flowers', 'Grand Opening Flowers'),
       ('Love Flowers', 'Love Flowers'),
       ('Noel', 'Noel'),
       ('Sympathy - Funerals', 'Sympathy - Funerals'),
       ('Thanks Flowers', 'Thanks Flowers'),
       ('Wedding Decoration', 'Wedding Decoration');
GO

INSERT INTO Occasion([Message], [Type])
VALUES
('Count your life by smiles, not tears. Count your age by friends, not years. Happy birthday!', 'Birthday'),
('Wishing you a day filled with love, laughter, and joy. Happy birthday!', 'Birthday'),
('May this year bring you new adventures and endless happiness. Happy birthday!', 'Birthday'),
('Another year older, wiser, and more fabulous! Happy birthday to you!', 'Birthday'),
('Here''s to celebrating another amazing year of your life. Happy birthday!', 'Birthday'),
('On your special day, may all your dreams and wishes come true. Happy birthday!', 'Birthday'),
('Sending you lots of love and best wishes on your birthday. Have a fantastic day!', 'Birthday'),
('Birthdays are nature''s way of telling us to eat more cake. Enjoy your special day!', 'Birthday'),
('May your birthday be as wonderful and unique as you are. Cheers to you!', 'Birthday'),

('Hey, get well soon! You''ve got the strength to overcome anything.', 'Get Well'),
('Wishing you a speedy recovery and a return to good health. Get well soon!', 'Get Well'),
('Remember that you''re stronger than any illness. Sending healing thoughts your way.', 'Get Well'),
('Your positivity and strength will see you through this. Get well soon!', 'Get Well'),
('Hoping to see you up and about again soon. Take care and get well!', 'Get Well'),
('Sending you healing vibes and a virtual hug. Get well soon!', 'Get Well'),
('Rest, relax, and recover. You''ll be back to your amazing self in no time!', 'Get Well'),
('Your resilience is inspiring. Wishing you a quick and smooth recovery.', 'Get Well'),
('Keep fighting, and remember that brighter days are ahead. Get well soon!', 'Get Well'),

('Congratulations on your well-deserved graduation! The future is yours to conquer.', 'Graduation'),
('Your hard work and dedication have paid off. Congratulations on your graduation!', 'Graduation'),
('Hats off to your academic achievements. Congratulations on reaching this milestone!', 'Graduation'),
('As you step into a new chapter, may success and opportunities abound. Congrats!', 'Graduation'),
('You did it! Wishing you a future filled with success and fulfillment. Congratulations!', 'Graduation'),
('Your graduation is just the beginning of an exciting journey. Congratulations!', 'Graduation'),
('Celebrating your accomplishments and looking forward to the bright future ahead. Congrats!', 'Graduation'),
('You''ve earned this moment of pride. Congratulations on your graduation!', 'Graduation'),
('From student to graduate – you''ve made us proud. Congratulations and best wishes!', 'Graduation'),

('Congratulations on the grand opening of your store! Wishing you prosperity and success.', 'Grand Opening'),
('Your hard work and dedication have led to this exciting new venture. Congrats!', 'Grand Opening'),
('Here''s to a successful grand opening and a flourishing business ahead. Cheers!', 'Grand Opening'),
('May your new venture bring you joy and great success. Congratulations!', 'Grand Opening'),
('The doors are open, and the journey begins. Wishing you a smooth and thriving path ahead.', 'Grand Opening'),
('Congratulations on this exciting new chapter. Best wishes for a successful grand opening!', 'Grand Opening'),
('Your grand opening is a testament to your passion and determination. Congratulations!', 'Grand Opening'),
('Wishing you a triumphant grand opening and a future filled with accomplishments.', 'Grand Opening'),
('Cheers to your new endeavor! May it be filled with growth, prosperity, and happiness.', 'Grand Opening'),

('Thinking of you and sending all my love. You mean the world to me.', 'Love'),
('In every moment, my heart beats for you. I cherish and adore you endlessly.', 'Love'),
('Your love brightens my days and warms my heart. I''m grateful to have you in my life.', 'Love'),
('Distance may separate us, but our love knows no boundaries. I love you more each day.', 'Love'),
('You are the missing piece to my puzzle. With you, my heart is complete.', 'Love'),
('Through the highs and lows, our love remains unwavering. I''m blessed to have you.', 'Love'),
('Every day with you is a new adventure, and I''m excited for the journey ahead.', 'Love'),
('Your love is the anchor that keeps me grounded and the wings that let me soar.', 'Love'),
('Loving you is effortless, and I''m thankful for the love we share. You''re my everything.', 'Love'),

('May your Christmas be filled with warmth, joy, and the love of family and friends.', 'Noel'),
('Wishing you a magical Christmas season, filled with laughter and cherished moments.', 'Noel'),
('May the spirit of Christmas fill your heart with peace and happiness. Merry Christmas!', 'Noel'),
('Sending you holiday wishes wrapped in love and tied with a bow. Merry Christmas!', 'Noel'),
('May your days be merry and bright, and may all your Christmas dreams come true.', 'Noel'),
('During this season of giving, may you receive the love and joy you bring to others.', 'Noel'),
('From our family to yours, wishing you a Christmas filled with love and togetherness.', 'Noel'),
('May the wonder of Christmas surround you and the joy of the season uplift your spirit.', 'Noel'),
('Here''s to a festive season filled with love, laughter, and countless memories. Merry Christmas!', 'Noel'),

('During this difficult time, please know that you''re in our thoughts and prayers.', 'Sympathy - Funerals'),
('Words may fall short, but our hearts are with you as you navigate this loss. Deepest sympathies.', 'Sympathy - Funerals'),
('In the quiet moments of sorrow, may you find comfort in the memories you shared.', 'Sympathy - Funerals'),
('Wishing you strength and solace as you say goodbye to your loved one. We are here for you.', 'Sympathy - Funerals'),
('Holding you close in our hearts and offering our deepest condolences.', 'Sympathy - Funerals'),
('As you grieve, may you find moments of peace and the support of those who care about you.', 'Sympathy - Funerals'),
('Remembering a life well-lived and sending heartfelt sympathy to you and your family.', 'Sympathy - Funerals'),
('In this time of mourning, may you find healing and the love of those who surround you.', 'Sympathy - Funerals'),
('Cherishing the memories of your loved one and sending you strength during this difficult time.', 'Sympathy - Funerals'),

('Your kindness has made a lasting impact. Thank you from the bottom of my heart.', 'Thanks'),
('Expressing my sincere gratitude for your generosity and support. Thank you!', 'Thanks'),
('A big thank you for your thoughtfulness and the wonderful things you do.', 'Thanks'),
('Your help has made a world of difference. I''m so thankful to have you in my life.', 'Thanks'),
('Thank you for being there when I needed it the most. Your friendship means everything to me.', 'Thanks'),
('Gratitude fills my heart as I say thank you for your unwavering support and kindness.', 'Thanks'),
('Your actions speak louder than words, and I''m truly grateful for all you''ve done for me.', 'Thanks'),
('Saying thank you doesn''t seem like enough. Your selflessness is truly appreciated.', 'Thanks'),
('Your willingness to lend a hand has touched my heart. Thank you for being amazing.', 'Thanks'),

('Wishing you a lifetime filled with love, joy, and endless happiness. Congratulations on your wedding!', 'Wedding'),
('As you begin this beautiful journey together, may your love continue to grow and flourish.', 'Wedding'),
('Your love story is just beginning, and we can''t wait to see the chapters you''ll write together. Congrats!', 'Wedding'),
('May your marriage be as strong and radiant as your smiles on this special day. Congratulations!', 'Wedding'),
('Celebrating two wonderful people who have found their perfect match. Congratulations!', 'Wedding'),
('From this day forward, may your lives be filled with love, laughter, and shared dreams. Congrats!', 'Wedding'),
('Here''s to love, laughter, and a happily ever after. Congratulations on your wedding!', 'Wedding'),
('As you exchange vows and rings, may your hearts forever be intertwined. Best wishes!', 'Wedding'),
('Your wedding day marks the start of a beautiful adventure. Congratulations and all the best!', 'Wedding');
GO

INSERT INTO Florist ([Name], Logo, Email, Phone, [Address], UserId, [StatusId])
VALUES ('My Flower App', '/images/florist/logo1.jpg', 'myflowergift@gmail.com', '0123456789', '123 Rose Ave', 1, 1),
       ('Interfloria', '/images/florist/logo2.jpg', 'interfloria@gmail.com', '0908070605', '456 Lily Rd', 2, 2),
       ('Flower Chimp', '/images/florist/logo3.jpg', 'flowerchimp@gmail.com', '01213141516', '789 Tulip St', 3, 2);
     
GO

-- Vui lòng thay đổi dữ liệu theo yêu cầu thực tế
INSERT INTO Bouquet ([Name], [UnitBrief], [UnitPrice], [Image], [BouquetDate], [Available], [Description], [CategoryId], [FloristId], [Quantity], [Discount], [Tag], [PriceAfter])
VALUES 

--Birthday
('Romantic Rose Bouquet', 
'Product Contains:
10 Red Roses Bouquet
Wrapped in Red Paper
Tied with Red Ribbon
Seasonal Green Filler
The actual product received may differ from the image shown on the website (due to handmade characteristics and the natural properties of agricultural products)',
37, '/images/bouquet/romanticrosebouquet.jpg', '2023-07-29', 1,
'Love is the best emotion in this world and has been proved and argued over the decades. 
Be the one to set a new example by purchasing this Romantic Rose bouquet for the love of your life. 
This bouquet contains a pack of 12 red roses. They are completely fresh and you will keep remembering this gesture for the times to come. 
Buy this today and get it delivered whenever you want.',
1, 1, 120, 10, 'Sale', 33),

('Wishful Blooms', 'Constituents
Care Guide
Happy Birthday Round Acrylic Tag
Transparent Cage Box
Flowers
Specifications
Care Guide
Happy Birthday Round Acrylic Tag
Transparent Cage Box
Size: 13 X 10 Inches
Flowers
No. of Stems: 15
Type of Flowers: Avalanche rose ,Gypso ,Roses ,Spray Carnation
Colour of Flower: Assorted
Country of Origin: India
The actual product received may differ from the image shown on the website (due to handmade characteristics and the natural properties of agricultural products)',
33 , '/images/bouquet/wishfulblooms.jpg', '2023-07-29', 1, 'Nothing says Happy Birthday like love wrapped in a heavenly gift and the lavender
cherish box is a true embodiment of that - Peach roses, sweet avalanche roses, pink 
spray carnations and pink gypso.', 1, 2, 120, 10, 'Sale', 26),

('Blushing Ballad', 'Pink Roses (3 Stalks), Champagne Roses (3 Stalks), Pink Gerbera (3 Stalks), 
White Ping Pong (2 Stalks), White Baby Breath, Eucalyptus Leaves, Green Bell
The actual product received may differ from the image shown on the website (due to handmade characteristics and the natural properties of agricultural products)',
35 , '/images/bouquet/blushingballad.jpg', '2023-07-29', 1, 'A lovely arrangement of light pink and champagne roses, pink gerberas and white ping pongs that
are embellished with airy baby''ss breath. - Not only this is the pleasant gift for your beloved one but
it is also a sweet reminder of your heartfelt emotions. Expressing your true love and admiration 
while reminding them of how special they are in your eyes.', 1, 3, 120, 6, 'Sale', 33),

('Special Rose N Teddy Arrangement 
', 'Product Contains:

Beautiful Basket Arrangement

Flowers : 5 Red Roses, 5 White Roses

Chocolates : 10 Dairy Milk Chocolate (13.5 gm)

A Beautiful Cane Basket

A 6 Inch teddy in Centre

Tied with Red Ribbon
The actual product received may differ from the image shown on the website (due to handmade characteristics and the natural properties of agricultural products)',
68 , '/images/bouquet/specialrosenteddy.jpg', '2023-07-29', 1, 'A beautiful basket filled with 5 Red Roses, 5 white roses, 10 dairy milk chocolates,
and a 6 inches soft red and white teddy along with seasonal greens.', 1, 1, 120, 6, 'Sale', 66),

('Tasteful Indulgence Birthday Hamper', 'Constituents
Caramel Coated Crispies Treats 100 gms
Almond Brittle
4700BC Himalayan Salt Caramel Popcorn Tin 110g
Blue MDF Rectangle Tray
Care Guide
Epiphany California Pistachio Crunch
Small Wooden Box
Flowers
Specifications
Caramel Coated Crispies Treats 100 gms
Weight: 100gms
Type: Chocolates
Almond Brittle
Weight: 16gms
Type: Chocolates
4700BC Himalayan Salt Caramel Popcorn Tin 110g
Weight: 110gms
Type: Popcorns
Blue MDF Rectangle Tray
Size: 33 x 23 x 5 cm
Care Guide
Epiphany California Pistachio Crunch
Small Wooden Box
Flowers
No. of Stems: 15
Type of Flowers: Roses ,Spray Carnation ,Statice ,Table Palm
Colour of Flower: Assorted
Country of Origin: India
The actual product received may differ from the image shown on the website (due to handmade characteristics and the natural properties of agricultural products)', 
48, '/images/bouquet/tastefulindulgencebirthdayhamper.jpg', '2023-07-29', 1, 'A savory escape planned with peach roses & spray carnations with caramel popcorn, 
caramel coated rice krispies along with a birthday banner seal the deal.', 1, 2, 120, 6, 'Sale', 44),

('Trinity Box Deluxe Collection - Athena', 'Orange Rose (4 Stalks), Champagne Rose (4 Stalks), Two-Toned Orange Carnation Spray (3 Stalks), Red Berry (2 Stalks), Bupleurum
The actual product received may differ from the image shown on the website (due to handmade characteristics and the natural properties of agricultural products)',
80, '/images/bouquet/trinityboxdeluxecollectionathena.jpg', '2023-07-29', 1, 'Blooming with brilliant 2 tone yellow orange carnation sprays, amber and champagne roses, 
this blossoming arrangement is accompanied with red berries, bupleurum and tea leaves. 
Bring home the essence of Athena with our marvelous Athena Trinity Box.', 1, 3, 120, 6, 'Sale', 75),

('Hearty Sensation', 'Product contains:

A Flower Heart Shape Arrangement

Flowers : 20 Red Roses & 20 White Roses

Seasonal Green Filler
The actual product received may differ from the image shown on the website (due to handmade characteristics and the natural properties of agricultural products)',
133, '/images/bouquet/heartysensation.jpg', '2023-07-29', 1, 'This gorgeous arrangement of Heart shape with fresh handpicked
40 Red and White Roses is the way to pour your heart out to your
 loved one. You don''t need to say a word because this bouquet is 
going to express how much in love you are with your lover.', 1, 1, 120, 6, 'Sale', 124),

('A Vibrant Purity', 'Constituents
Care Guide
Medium Cylindrical White Box
Flowers
Specifications
Care Guide
Medium Cylindrical White Box
Size: 8 X 7.5
Flowers
No. of Stems: 87
Type of Flowers: Alstroemeria ,Carnations ,Eucalyptus ,Gypso ,Roses ,Statice
Colour of Flower: Assorted
Country of Origin: India
The actual product received may differ from the image shown on the website (due to handmade characteristics and the natural properties of agricultural products)',
156, '/images/bouquet/avibrantpurity.jpg', '2023-07-29', 1, 'A floral arrangement of yellow roses, sweet avalanche roses, green alstroemeria,
pink spray carnations, white statice, yellow gypso and eucalyptus arrayed in a tasteful white box personifies grace.
A perfect addition to elevate the expanse of your home.', 1, 2, 120, 6, 'Sale', 155),

('Notting Hill', 'Red Roses (10 Stalks), 
Pink Lilies (4 Stalks), 
Pink Carnations (5 Stalks)', 180, '/images/bouquet/nottinghill.jpg', '2023-07-21', 1, 'Like the splendid neighborhood of London, the series consists of red roses, pink lilies,
and pink carnations wrapped in beautiful wrapping. Perfect for birthdays, romantic events and anniversaries.', 1, 3, 75, 3, 'Summer', 167),

--Congratulation
('Pink and White Cuteness in Bouquet', 'Product Contains:
- 15 Pink and White Roses', 18, '/images/bouquet/pinkandwhitecuteness.jpg', '2023-07-21', 1, 'Send your heartiest wishes to someone by gifting our pink and 
white cuteness in Bouquet filled with 7 hand-picked pink roses and 
8 fresh white roses with green leaves tied up in a pink ribbon.', 2, 1, 75, 3, 'Summer', 15),

('A Walk in the Garden Basket', 'Constituents
Jade Plant
Peace lily plant
Care Guide
English Garden Basket
Flowers
Specifications
Jade Plant
Plant Type: Jade Plant
Plant Placement: Indoor Plant
Plant Height: Uptoo 5 Inches
Peace lily plant
Plant Type: Peace Lily Plant
Plant Placement: Indoor Plant
Care Guide
English Garden Basket
Size: 13.5 x 4.3 Inch
Flowers
No. of Stems: 11
Type of Flowers: Button Chrysanthemum ,Leather Fern ,Limonium ,Roses
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/awalkinthegardenbasket.jpg', '2023-07-21', 1, 'Glorious, luxurious yet humble - Lilac Roses, Green Spray Chrysanthemum, 
Mauve Limonium and Leather Fern have the old Eden Garden charm. 
Take a leap of faith in the hues of this royal hamper in English Garden Basket complemented with white stones.', 2, 2, 75, 3.75, 'Summer', 15),

('Perfection', 'Product includes:
Red chili pepper : 30
Orchid scorpion : 10
Red coin : 30', 18, '/images/bouquet/perfection.jpg', '2023-07-21', 1, 'Flower shelves with vibrant red tones, modern and youthful designs really stand out more than traditional flower shelves. 
The flower shelf has the meaning of a wish for good fortune. Suitable for gifting on occasions of congratulations, grand openings, anniversaries.', 2, 3, 75, 3, 'Summer', 15),

('Yellow Roses Basket', 'Product Contains:

A Floral Basket Arrangement

Flowers : 20 Yellow Roses

Arranged in a Cane Basket

Seasonal Green Filler', 18, '/images/bouquet/yellowrosesbasket.jpg', '2023-07-21', 1, 'This tremendous basket of 20 Yellow roses is an astonishing sight. 
The fresh yellow roses make the environment dazzling and emit a lot of influential energy to cheer up your loved ones.', 2, 1, 75, 3, 'Summer', 15),
('Unicorn Hues Bouquet', 'Constituents
Care Guide
Flowers
Specifications
Care Guide
Flowers
No. of Stems: 20
Type of Flowers: Gypso
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/unicornhuesbouquet.jpg', '2023-07-21', 1, 'A floral arrangement straight out of the fairytale, gypso stems: pink sprayed, 
lavender sprayed, yellow sprayed, and blue sprayed create this bouquet of unicorn 
hues to make all the dreams come true. Perfect for a lovely & colorful surprise.', 2, 2, 75, 3, 'Summer', 15),

('Sunbathing', 'Products include:
Single yellow carnation : 10
Old white rose: 11
Sunflower : 3
Dancers: 3
Yellow wolf muzzle: 10', 18, '/images/bouquet/sunbathing.jpg', '2023-07-21', 1, 'Inspired by the lovely, bright sunshine of Saigon, the flower pattern has a bright, bright yellow tone with a lovely, round figure. 
Basket of flowers as a special gift, send a little sunshine to your friends to wish them happy birthday, 
success or simply a good day.', 2, 3, 75, 3, 'Summer', 15),

('Red Roses Vase 30 Flowers', 'Product Contains:

30 Red Roses

Arranged in a Glass Vase

Seasonal Green Filler', 18, '/images/bouquet/redrosesvase30flowers.jpg', '2023-07-21', 1, 'Send your warm heartiest wishes with our beautiful red roses vase of 30 full-bloomed,
hand-picked red roses arranged in a glass vase with fresh, green seasonal leaves.', 2, 1, 75, 3, 'Summer', 15),

('Rosy Gardenia', 'Constituents
Care Guide
Luxury Box
Flowers
Specifications
Care Guide
Luxury Box
Size: 28 x 28 x 11 cm
Flowers
No. of Stems: 40
Type of Flowers: Gypso ,Saintiny White ,Spray roses ,Roses Sweet Avalanche
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/rosygardenia.jpg', '2023-07-21', 1, 'Uplifting, calming, oozing with lively spirit, this soothing pretty pink floral arrangement of sweet avalanche roses,
santini, pink roses, and pink gypso in a classic glass tapered vase with pearls 
reminds you of a summer scene from a romance novel.', 2, 2, 75, 3, 'Summer', 15),

('Perfection 01', 'Products include:
Pink sand wall border: 10
Pink apricot chrysanthemum: 10
New Lotus: 45
Little pink coin: 20', 18, '/images/bouquet/perfection01.jpg', '2023-07-21', 1, 'Flower shelves with gentle pink tones, modern and youthful designs really stand out more than traditional flower shelves. 
The flower shelf has the meaning of a wish of good luck and auspiciousness. 
Suitable for gifting on occasions of congratulations, grand openings, anniversaries.', 2, 3, 75, 3, 'Summer', 15),

-- Grand Opening
('Vase Arrangement of Orange Gerberas', 'Product Contains:

- 10 Orange Gerberas

- Seasonal Filler

- A Square Glass Vase', 18, '/images/bouquet/vasearrangementoforangegerberas.jpg', '2023-07-21', 1, 'This exclusive Vase arrangment of 10 fresh handpicked Orange Gerberas can bring smile to anyone''s lips, 
and is definitely a treat to the eyes. You can gift it to your loved ones, 
or even buy it for treating youself as the colour is so refreshing and vibrant!', 5, 1, 75, 3, 'Summer', 15),

('Hung Phat', 'Products include:
Choco single carnation : 15
Red chili pepper : 30
Red rose : 30
Orchid scorpion : 20
Red: 12', 18, '/images/bouquet/hungphat.jpg', '2023-07-21', 1, 'Red is a very popular base color when decorating according to feng shui. Considered the luckiest color, 
red is very suitable for celebrations and festivals. 
It also symbolizes the energy of Fire and strength... 
The congratulatory flower shelf is seriously invested by florists because we always understand that it is the reputation of the customer, the whole business. 
We always receive the love from customers, especially businesses 
who trust and order floral designs to celebrate openings, inaugurations, events and other important occasions.', 5, 2, 75, 3, 'Summer', 15),

('Tropical Joy', 'Sunflowers (6 Stalks), 
Light Pink Roses (8 Stalks), 
Red Roses (10 Stalks), 
Yellow Roses (7 Stalks), 
Yellow Peacock', 18, '/images/bouquet/tropicaljoy.jpg', '2023-07-21', 1, 'Oh happy day, a basket full of sunflowers and roses! Cheer up somebody with this basket full of fresh, sunshine happiness.', 5, 3, 75, 3, 'Summer', 15),

('Enjoy the Love with Lilies', 'Product Description:

- Bouquet of 10 Yellow Asiatic Lilies

- Wrapped in Orange Paper', 18, '/images/bouquet/enjoythelovewithlilies.jpg', '2023-07-21', 1, 'Lily symbolises innocence, happiness and devotion. 
In some countries it is presented on 2nd or 30th wedding anniversary. 
If you know someone whose anniversary is nearby, send them this bouquet of Love with Lilies.', 5, 1, 75, 3, 'Summer', 15),

('Floral Grandeur to Adore', 'Constituents
Caramel Almonds 50 gms (2 units)
Black Pepper Cashews 50 gms (2 units)
Vanilla Cupcakes (Pack of 6)
Almond Brittle (3 units)
Big Round Box
Care Guide
Rose Gold Heart Balloon (4 units)
Flowers
Specifications
Caramel Almonds 50 gms (2 units)
Weight: 50gms
Flavour: Caramel
Black Pepper Cashews 50 gms (2 units)
Weight: 50gms
Flavour: Black Pepper
Vanilla Cupcakes (Pack of 6)
Flavour: Vanilla
Quantity: 6
Type of Cup Cake: Cream
Almond Brittle (3 units)
Weight: 16gms
Type: Chocolates
Big Round Box
Size: 2 x 9 Inch
Care Guide
Rose Gold Heart Balloon (4 units)
Flowers
No. of Stems: 82
Type of Flowers: Gypso ,Leather Fern ,Roses
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/floralgrandeutoadore.jpg', '2023-07-21', 1, 'When words fall short to express your feelings for the recipient,
give a grand surprise with this premium arrangement; blush pink and lilac roses,
vanilla sprinkle cupcakes, potli of 4 Almond Brittle, Flavored Cashew,
Flavored Cashew Almonds & heart balloons that make a lavish floral scene', 5, 2, 75, 3, 'Summer', 15),

('Sunny Side Up', 'Yellow Gerbera (5 stalks), 
Sunflower (5 stalks), 
Eucalyptus Leaves', 18, '/images/bouquet/sunnysideup.jpg', '2023-07-21', 1, 'For any event of happiness, like a store or a new business opening, 
this flower bouquet will brighten your special day. Consisting of some beautiful sunflowers and some gerbera, 
this flower arrangement is available for reservation anytime.', 5, 3, 75, 3, 'Summer', 15),

('Perfectly Pleasing', 'Product Contains:

- 3 White Carnations, 
3 White Lilies, 
3 White Gerberas, 
3 White Rose in white paper packing', 18, '/images/bouquet/perfectlypleaseing.jpg', '2023-07-21', 1, 'Want a bouquet that looks simple and sober yet classy and elegant? 
Then this bouquet of all the exotic flowers is the perfect solution for you. 
A single glance on this can please someone''s heart and soul.', 5, 1, 75, 3, 'Summer', 15),

('Duchess of Blossom', 'Constituents
Care Guide
Pink and White Round Box
Pink Statement Bow
Flowers
Specifications
Care Guide
Pink and White Round Box
Pink Statement Bow
Flowers
No. of Stems: 84
Type of Flowers: Carnations ,Disbuds ,Gypso ,Roses
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/duchessofblossom.jpg', '2023-07-21', 1, 'Their words echo love. Their actions emanate care. So return their love with doubled abundance - Pink hydrangeas, 
sweet avalanche roses, deep purple roses, pink lilies, pink stock, 
pink gypso and white disbuds deftly packed with a pink statement bow.', 5, 2, 75, 3, 'Summer', 15),

('Happy Day', 'Red Gerbera (7 Stalks), 
Yellow Gerbera (6 Stalks), 
Orange Gerbera (5 Stalks), 
Orange Heliconia (3 Stalks), 
Yellow Peacock, 
Monstera', 18, '/images/bouquet/happyday.jpg', '2023-07-21', 1, 'Celebrate your special day with this special flower-flower - a blend of Gerbera and Bird of Paradise is awaiting to be served! 
This colorful series will be the perfect sweetener for the opening or graduation ceremonies, 
as well as some other important events and days.', 5, 3, 75, 3, 'Summer', 15),

-- Graduation
('Red Rocher Bouquet', 'Product Contains:

Flower N Chocolate Bouquet

Flowers : 8 Red Roses

Chocolates : 3 Ferrero Rocher

Wrapped in Yellow Paper

Tied with Red Ribbon

Seasonal Green Leaves', 18, '/images/bouquet/redrocherbouquet.jpg', '2023-07-21', 1, 'lowers, chocolate, and a gift is a best combo to impress any female. 
So don’t wait just buy this beautiful combo bouquet of Ferrero Rocher Chocolate 3 pcs, 
8 Red Roses, and 6inch White & Red soft Teddy Bear.', 4, 1, 75, 3, 'Summer', 15),

('Indigo Charm', 'Constituents
Care Guide
Flowers
Specifications
Care Guide
Flowers
No. of Stems: 12
Type of Flowers: Corkwood flowers ,Corn Leaf Rose ,Gypso ,Statice ,Table Palm Dried
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/indigocharm.jpg', '2023-07-21', 1, 'A floral lavender dream of dried bouquet with corn leaf roses, purple-shaded table palm, 
corkwood flower, purple statice and white gypso. 
It blends elegance with a dash of surrealism to create a fitting assemblage for any celebration.', 4, 2, 75, 3, 'Summer', 15),

('Champagne roses', 'Champagne Roses (12 Stalks),
Bupleurum', 18, 'champagneroses.jpg', '2023-07-21', 1, 'A glamorous arrangement of 12 champagne roses to remind your loved one
that they well-loved by you. 
A little effort goes a long way!', 4, 3, 75, 3, 'Summer', 15),

('Delightful Rocher Bouquet', 'Product Contains:

Rocher And Roses Bouquet

Flowers : 16 Red Roses

Ferrero Rocher : 16 pcs

Wrapped in Red Paper

Tied with Red Ribbon', 18, '/images/bouquet/delightfulrocherbouquet.jpg', '2023-07-21', 1, 'This bouqet of Roses and Rocher is undoubtedly the best way to show your loyalty to your lover in a royal way. 
Show them how much in love you are and how romantic 
is your way of expressing it with this gorgeous Bouquet of Ferrero Rocher Chocolates - 16 pcs. & Red Roses - 16.', 4, 1, 75, 3, 'Summer', 15),

('Pink Mystique', 'Constituents
Care Guide
Floral Printed Cylindrical Box
Flowers
Specifications
Care Guide
Floral Printed Cylindrical Box
Flowers
No. of Stems: 31
Type of Flowers: Alstroemeria ,Roses ,Veronica
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/pinkmystique.jpg', '2023-07-21', 1, 'Deep undertones and euphoric emotions coalesce to highlight the charm of this disposition. Dark pink roses, deep purple roses, lavender alstroemeria and white veronica, 
deftly arranged in a floral box that exudes the unparalleled fondness you share for your special few.', 4, 2, 75, 3, 'Summer', 15),

('Precious Purple', 'Purple Carnation (7 stalks), 
Pink Statice (2 stalks), 
Eucalyptus Leaves', 18, '/images/bouquet/preciouspurple.jpg', '2023-07-21', 1, 'A pop of purple for a rare gem. 
This royal colour represents opulence and success!', 4, 3, 75, 3, 'Summer', 15),

('Special Rose N Teddy Arrangement', 'Product Contains:

Beautiful Basket Arrangement

Flowers : 5 Red Roses, 5 White Roses

Chocolates : 10 Dairy Milk Chocolate (13.5 gm)

A Beautiful Cane Basket

A 6 Inch teddy in Centre

Tied with Red Ribbon', 18, '/images/bouquet/specialrosenteddyarrangement.jpg', '2023-07-21', 1, 'A beautiful basket filled with 5 Red Roses, 5 white roses, 10 dairy milk chocolates, 
and a 6 inches soft red and white teddy along with seasonal greens.', 4, 1, 75, 3, 'Summer', 15),

('Melody of Roses', 'Constituents
Macrame Basket
Bohemian Style Macrame Basket
Care Guide
Midnight Rose Tin Candle
Flowers
Specifications
Macrame Basket
Size: 10 x 4.5 x 7.4Inches
Bohemian Style Macrame Basket
Size: 10 x 4.5 x 7.4Inches
Care Guide
Midnight Rose Tin Candle
Flowers
No. of Stems: 37
Type of Flowers: Alstroemeria ,Roses ,Spray Carnation
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/melodyofroses.jpg', '2023-07-21', 1, 'Just as classic poetry illuminates hearts, this modish arrangement of peach roses, sweet avalanche roses, pink spray carnations, pink alstroemeria and midnight rose candle, 
coalesced in a macrame basket, brightens up your space. 
Truly a beacon for any occasion.', 4, 2, 75, 3, 'Summer', 15),

('Evelyn', 'Two-Toned Orange Tulips (10 Stalks), 
Blue Caspia, 
Eucalyptus Leaves', 18, '/images/bouquet/evelyn.jpg', '2023-07-21', 1, 'Surprise your loved ones with our beautifully arranged bouquet of imported Tulips from Holland. 
A bouquet perfect to convey your true love and loyalty to your dearest one.', 4, 3, 75, 3, 'Summer', 15),

-- Wedding
('Pink Carnation Basket', 'Product Contains:

- 12 Pink Carnation
- 1 Handle Basket

- Seasonal Green Filler', 18, '/images/bouquet/pinkcarnationbasket.jpg', '2023-07-21', 1, 'Express your affection for your special one by sending our 12 sweet pink carnation basket filled with lots of full-bloomed and hand-picked carnations
straight from the garden in a basket with green leaves tied in a pink ribbon.', 10, 1, 75, 3., 'Summer', 15),

('Love Me Tender', 'Constituents
Care Guide
Pink Heart Balloon
Rose Gold Heart Balloon
Pink and White Round Box
Flowers
Specifications
Care Guide
Pink Heart Balloon
Rose Gold Heart Balloon
Pink and White Round Box
Flowers
No. of Stems: 37
Type of Flowers: Roses
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/lovemetender.jpg', '2023-07-21', 1, 'They hold a piece of your heart, yet each time they''re around, the heart feels complete. 
Inspired by this dreamy power of love, the assortment echoes sweet romance - A pink houndstooth box,
a rose gold heart balloon, a pink heart balloon, sweet avalanche roses, aqua pink, lilac and a xoxo tag.', 10, 2, 75, 3, 'Summer', 15),

('Magical Wedding', 'Flowers (10 - 20 Stalks ), 
Foliage/Fillers (5 Stalks)', 18, '/images/bouquet/magicalwedding.jpg', '2023-07-21', 1, 'On the joyful day of celebration, is there any better way to spread joy than with a colourful board of flowers?

Size 1,2 m x 2 m, red background, with 4 small sized flower arrangements at the top, top right and left corner and at the bottom of the flower board.', 10, 3, 75, 3, 'Summer', 15),

('Red Hot Heart', 'Product Contains:

A Floral Heart Arrangement

Flowers : 25 White, Pink and Red Roses

Seasonal Green Filler', 18, '/images/bouquet/redhotheart.jpg', '2023-07-21', 1, 'Make this Valentine’s Day more special for your beloved by gifting her red hot heart-shaped rose arrangement containing 25 hand-picked 
and full-bloomed white, pink and red roses along with fresh green leaves.', 10, 1, 75, 3, 'Summer', 15),

('Eau De Love', 'Constituents
Care Guide
Round Striped Box
Interflora Fleur de Eden Perfume 100ml
Flowers
Specifications
Care Guide
Round Striped Box
Size: 23 x 13 cm
Interflora Fleur de Eden Perfume 100ml
Flowers
No. of Stems: 31
Type of Flowers: Roses ,Spray roses
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/eaudelove.jpg', '2023-07-21', 1, 'Introducing the aroma of true love and a delightful medley of florals that come together to create an awe-inspiring assortment for her, as pure as your love. 
Fleur de Eden - a luxury floral perfume with peony, lotus and magnolia & notes of mahogany and musk, set in a box of roses.', 10, 2, 75, 3, 'Summer', 15),

('Happily Ever After', 'Flowers (10 - 20 Stalks ), 
Foliage/Fillers (5 Stalks)', 18, '/images/bouquet/happilyeverafter.jpg', '2023-07-21', 1, 'On the joyful day of celebration, is there any better way to spread joy than with a colourful board of flowers?

Size 1,5 m x 2 m, Blue background, with large sized flower arrangements around flower board.', 10, 3, 75, 3, 'Summer', 15),

('Tender Touch', 'Product Contains:

- Combination of 25 Red roses, White roses and White carnations.', 18, 'tendertouch.jpg', '2023-07-21', 1, 'An exquisite of floral arrangement, Tender Touch is undoubtedly one of the best ways to please someone. 
Combination of 25 Red roses, White roses and White carnations gives it an incredible look.', 10, 1, 75, 3, 'Summer', 15),

('The Eternal Love', 'Constituents
Bohemian Style Macrame Basket
Care Guide
Flowers
Specifications
Bohemian Style Macrame Basket
Size: 10 x 4.5 x 7.4Inches
Care Guide
Flowers
No. of Stems: 50
Type of Flowers: Roses
Colour of Flower: Red
Country of Origin: India', 18, '/images/bouquet/theeternallove.jpg', '2023-07-21', 1, 'Love planted a rose and the world blossomed with passion, devotion and tenderness. 
Share the same feelings with the love of your life and uplift them with 
this grand assortment - Red roses packed deftly in a macrame basket.', 10, 2, 75, 3, 'Summer', 15),

('True Love', 'Flowers (10 - 20 Stalks), 
Foliage/Fillers (5 Stalks)', 18, '/images/bouquet/truelove.jpg', '2023-07-21', 1, 'On the joyful day of celebration, is there any better way to spread joy than with a colourful board of flowers?

Size 1,5 m x 2 m, red background, with 3 big sized flower arrangements at the top, side and bottom of the flower board.', 10, 3, 75, 3, 'Summer', 15),

--Sympathy - Funeral
('Mixed Roses Wreath', 'Product Contains:

A Floral Wreath Arrangement

Flowers : 50 Mixed Roses

Arranged in a O Shape

Seasonal Green Filler', 18, '/images/bouquet/mixedroseswreath.jpg', '2023-07-21', 1, 'Whatever the occasion may be, our unique mixed roses wreath full of full-bloomed hand-picked different shades of 50 pink,
 red and Yellow roses with seasonal green leaves decorated with a pink ribbon-made flower, is the best gift ever.', 8, 1, 75, 3, 'Summer', 15),

('Unforgetabble 50 White Roses Hand Tied', 'Constituents
Vertical Box
Care Guide
Flowers
Specifications
Vertical Box
Type Of Base: Box
Care Guide
Flowers
No. of Stems: 50
Type of Flowers: Roses
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/unforgetabble50whiteroseshandtied.jpg', '2023-07-21', 1, 'Single tone flower bouquets are such a sight of natural luxury, 
it can be perfect for personal decor, for gifting or to order in to celebrate an occasion. 
50 roses are ideally gifted to express undying love. Say it all with fresh roses, right from the farm arranged carefully in our luxury craft box.', 8, 2, 75, 3, 'Summer', 15),

('Premium Flower Stand', '/images/bouquet/Light Blue Hydrangea (1 Stalk), 
Yellow Gerbera (20 Stalks), 
Orange Gerbera (8 Stalks), 
White Poms (2 Stalks), 
White Peacock, 
Ivy Leaves', 18, '/images/bouquet/premiumflowestand.jpg', '2023-07-21', 1, 'This premium flower stand is a wonderful, colorful arrangement. 
Large in size it is suitable for new shop openings as well as funerals. 
The stand''s exquisite flowers coupled with the large size is guaranteed to make this one stand out from the crowd.', 8, 3, 75, 3, 'Summer', 15),

('White Carnation Basket', 'Product Contains:

12 White Carnations

1 Basket

Seasonal Green Filler', 18, '/images/bouquet/whitecarnationbasket.jpg', '2023-07-21', 1, 'The white carnations represent your love, care, 
and affection for your loving partner. 
Present these attractive flowers to your partner and express your long-lasting and visor dedication for them. 
Obtain this charming basket of 12 white carnations and make your partner happy.', 8, 1, 75, 3, 'Summer', 15),

('The Snow White', 'Constituents
Care Guide
Luxury Box
White Ribbed Ceramic Vase
Flowers
Specifications
Care Guide
\Luxury Box
Size: 28 x 28 x 11 cm
White Ribbed Ceramic Vase
Flowers
No. of Stems: 24
Type of Flowers: Disbuds ,Gypso ,Roses ,Spray Carnations
Colour of Flower: White
Country of Origin: India', 18, '/images/bouquet/thesnowwhite.jpg', '2023-07-21', 1, 'A rush of calm and blissful serenity, 
absorbed in a vivacious arrangement that''s a marker of new beginnings. 
White disbuds, white roses, white spray carnations, 
white gypso stems, deftly packed in a luxury vase to honour your loved ones.', 8, 2, 75, 3, 'Summer', 15),

('Tribute', 'White Lily (3 Stalks), 
White Orchid (10 Stalks), 
White Rose (10 Stalks), 
White Spider Mum (6 Stalks), 
White Tuberose (5 Stalks), 
Yellow Peacock, 
Iron Leaves, 
Rainforest Leaves', 18, '/images/bouquet/tribute.jpg', '2023-07-21', 1, 'A wonderful condolence stand for funeral services. 
Lush flowers, compiled in an extraordinary style make this arrangement one of our bestsellers.', 8, 3, 75, 3, 'Summer', 15),

('Yellow Lily Bunch', 'Product Contains:

- 4 Yellow Asiatic Lily

- Wrapped in White Paper

- Tied with Yellow Ribbon', 18, '/images/bouquet/yellowliliesbunch.jpg', '2023-07-21', 1, 'Send this stunning bouquet of 4 Asiatic yellow lilies which is a very unique way to make your dear ones feel very special on their valuable moments. 
Show your love, concern, warmth, and pleasure to your loved ones with these gorgeous lilies.', 8, 1, 75, 3, 'Summer', 15),

('Purest Wishes', 'Constituents
Care Guide
English Cane Handle Basket
Flowers
Specifications
Care Guide
English Cane Handle Basket
Flowers
No. of Stems: 43
Type of Flowers: Button Chrysanthemum ,Carnations ,Disbuds ,Eucalyptus ,Gerberas ,Gypso ,Roses ,Spray Carnation ,Statice
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/purestwishes.jpg', '2023-07-21', 1, 'Just like flowers, your special few are a welcome respite during the lows and partners in fun during the highs.
Express gratitude with a wholesome English willow basket comprising disbuds, roses, 
carnations, white spray carnations, green button chrysanthemums, eucalyptus and gerbera.', 8, 2, 75, 3, 'Summer', 15),

('Everlasting', 'Green Button Mum (12 stalks), 
White Gerbera (18 stalks), 
White Lily (3 stalks), 
Paku Leaf, 
Palm Leaf, 
White Statice', 18, '/images/bouquet/everlasting.jpg', '2023-07-21', 1, 'Express your heartfelt condolences with this funeral flower stand.', 8, 3, 75, 3, 'Summer', 15),

-- Get Well
('12 Flowers Bouquet with Cadbury Chocolates', 'Product Contains:

- 12 Mix Roses in Blue Paper packing

- 10 Dairy Milk Chocolate of 12.5gm', 18, '/images/bouquet/12flowersbouquetwithcadburychocolates.jpg', '2023-07-21', 1, 'Mix roses symbolize everlasting love and liking and thus, 
a wonderful gift for those special people in your life. 
12 Mix roses are taken together and attached with a ribbon bow. 
While the flawlessly arranged flowers reveal love and Dairy milk chocolate is confident to please one’s sweet tooth.', 3, 1, 75, 3, 'Summer', 15),

('Meadow in a Box', 'Constituents
Care Guide
Green Textured Cylindrical Box
Flowers
Specifications
Care Guide
Green Textured Cylindrical Box
Size: 5.5 X 6
Flowers
No. of Stems: 29
Type of Flowers: Wheat Grass ,Alstroemeria ,Button Chrysanthemum ,Eucalyptus ,Mustard dry ,Roses ,Spray Carnation ,Springery ,Statice
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/meadowinabox.jpg', '2023-07-21', 1, 'Confidantes, best friends and your forever keepers. 
Thank the special ones for their unmatched contribution to your life with this magnificent blooming arrangement in a box. 
Curated with white roses, green alstroemeria, white statice, eucalyptus and carnations with dried elements.', 3, 2, 75, 3, 'Summer', 15),

('Spring in Holland', 'Red Tulips (15 Stalks)', 18, '/images/bouquet/springinholland.jpg', '2023-07-21', 1, 'A beautiful bouquet of tulips is chosen directly from the Dutch plantation
and flown directly into your hands. 
Consisting 15 Red tulips stems with love, 
its is suitable to be presented to your loved ones as an anniversary gift, 
birthday, or just to show your deep love.', 3, 3, 75, 3, 'Summer', 15),

('Yellow Roses Vase', 'Product Contains:

A Floral Vase Arrangement

Flowers : 12 Yellow Roses

Arranged in a Glass Vase

Seasonal Green Filler', 18, '/images/bouquet/yellowrosesvase.jpg', '2023-07-21', 1, 'The yellow colour is practically associated to the sun and hence it can be gifted to cheer up people’s life. 
Grab this exclusive bouquet of 12 yellow roses elegantly arranged in a beautiful vase.', 3, 1, 75, 3, 'Summer', 15),

('Hope & Happiness Hamper', 'Constituents
Black Pepper Cashews 50 gms (2 units)
Lebanese Baklava Sweet 9 Pcs
Macrame Basket
Peace lily plant
Bohemian Style Macrame Basket
Care Guide
Gold Chevron Ceramic Planter
Flowers
Gold Floral Cannister
Specifications
Black Pepper Cashews 50 gms (2 units)
Weight: 50gms
Flavour: Black Pepper
Lebanese Baklava Sweet 9 Pcs
Type: Assorted Baklava
Weight: 220gms
Shelf Life: 90 Days
Macrame Basket
Size: 10 x 4.5 x 7.4Inches
Peace lily plant
Plant Type: Peace Lily Plant
Plant Placement: Indoor Plant
Bohemian Style Macrame Basket
Size: 10 x 4.5 x 7.4Inches
Care Guide
Gold Chevron Ceramic Planter
Size: 7 X 7 X 4.5 Inch
Material: Ceramic
Flowers
No. of Stems: 8
Type of Flowers: Roses ,Spray Carnation ,Statice
Colour of Flower: Assorted
Gold Floral Cannister
Country of Origin: India', 18, '/images/bouquet/hopehappinesshamper.jpg', '2023-07-21', 1, 'Elegant bouquet of white lilies', 3, 2, 75, 3, 'Summer', 15),

('Sweet Hues', 'Shocking Pink Carnation Spray (3 Stalks), 
Pink Eustoma (3 Stalks), 
Red Rose (8 Stalks), 
Eucalyptus Leaves', 18, '/images/bouquet/sweethues.jpg', '2023-07-21', 1, 'A beautiful basket of brightly colored Gerberas is an expert in delivering happiness. 
This basket series is perfect for birthdays, anniversaries, recovered greetings, and corporate events', 3, 3, 75, 3, 'Summer', 15),

('White Roses Heart Shape Arrangement', 'Product Contains:

- Heart Shape Arrangement

- 30 White Rose

- Seasonal Green Fillers', 18, '/images/bouquet/whiteroseheartshapearrangement.jpg', '2023-07-21', 1, 'White roses stand for purity; authenticity; innocence; unity as well as loyalty. 
These roses will make your ordinary day simply impressive. 
This heart-shaped 30 white roses arrangement is an ideal combination of attraction and sophistication.', 3, 1, 75, 3, 'Summer', 15),

('Aphrodite Blooms', 'Constituents
Flowers
Care Guide
Medium Cylindrical White Box
Specifications
Flowers
No. of Stems: 26
Type of Flowers: Alstroemeria ,Daisy Purple ,Roses
Colour of Flower: Assorted
Care Guide
Medium Cylindrical White Box
Size: 8 X 7.5
Country of Origin: India', 18, '/images/bouquet/aphroditeblooms.jpg', '2023-07-21', 1, 'Sense of deep love, lulled with affection, stricken in passion - deep purple roses, 
purple daisy, light pink alstroemeria, and kamini are wrapped in chic floral love to tell an unforgettable gatha.', 3, 2, 75, 3, 'Summer', 15),

('Basket Daisy', 'Pink Daisies', 18, '/images/bouquet/basketdaisy.jpg', '2023-07-21', 1, 'Oh happy day, a basket full of Pink Daisies! Cheer up somebody with this basket full of fresh, pink happiness.

The daisy can symbolize many things - a sign of love, sensuality, and fertility. 
The daisy also symbolizes the aspect of motherhood and childbirth in association with Freya, which carries on the theme of unblemished youth. 

Pick your preferred delivery date in the shopping cartand let us deliver your personal message!', 3, 3, 75, 3, 'Summer', 15),

-- Love
('White & Red Roses Box', 'Product Contains:

- 25 Red Roses and 15 White Roses in a Round Shape Black Colour Card Board Box Dimensions (H x W)- 9 x 6 Inches Approx.', 18, '/images/bouquet/whiteredrosesbox.jpg', '2023-07-21', 1, 'Want to welcome your sister in law into your family then this “White & Red Roses Box” is a perfect flower basket for you. 
The round shape black color cardboard box with dimensions of (H x W)- 9 x 6 Inches Approx. has 25 Red Roses 
and 15 White Roses and is tied with a red bow. Trust us this is a sweet surprise for her.', 6, 1, 75, 3, 'Summer', 15),

('The Royal Wish', 'Constituents
Flowers
Care Guide
Specifications
Flowers
No. of Stems: 34
Type of Flowers: Gypso ,Roses
Colour of Flower: Red
Care Guide
Country of Origin: India', 18, '/images/bouquet/theroyalwish.jpg', '2023-07-21', 1, 'A royal handtied of exotic red roses and red gypso in classic red and white striped ribbon to make your significant other smile ear to ear. 
Let the red take over and sweep them off their feet', 6, 2, 75, 3, 'Summer', 15),

('Trinity Box Deluxe Collection - Athena', 'Orange Rose (4 Stalks), 
Champagne Rose (4 Stalks), 
Two-Toned Orange Carnation Spray (3 Stalks), 
Red Berry (2 Stalks), 
Bupleurum', 18, '/images/bouquet/trinityboxdeluxecollectionathena.jpg', '2023-07-21', 1, 'Blooming with brilliant 2 tone yellow orange carnation sprays,
amber and champagne roses, this blossoming arrangement is accompanied with red berries, 
bupleurum and tea leaves. Bring home the essence of Athena with our marvelous Athena Trinity Box.', 6, 3, 75, 3, 'Summer', 15),

('Vase of Blue Orchids', 'Product Contains:

- 12 Blue Orchids

- A Glass Vase 5 Inches with Dry Sticks Dracaena Leaves', 18, '/images/bouquet/vaseofblueorchids.jpg', '2023-07-21', 1, 'This heavenly arrangement of blue orchids can impress anyone. 
Order this vase of beautiful and exotic flowers, 
send it to a special someone and get closer to their heart on a special day.', 6, 1, 75, 3, 'Summer', 15),

('100 Days of Love', 'Constituents
Care Guide
Flowers
Specifications
Care Guide
Flowers
No. of Stems: 100
Type of Flowers: Roses
Colour of Flower: Red
Country of Origin: India', 18, '/images/bouquet/100daysoflove.jpg', '2023-07-21', 1, 'A bouquet of 100 magical red roses that wraps your love, 
affection and care to express what you truly feel within. 
Pamper your love with this exquisite assortment and let it be the reason for you two to fall in love all over again.', 6, 2, 75, 3, 'Summer', 15),

('Teddy Love', 'Red Roses (3 Stalks), 
Pink Carnation (3 Stalks), 
Shocking Pink Carnation Spray (1 Stalk), 
Purple Caspia, 
Eucalyptus Leaves', 18, '/images/bouquet/teddylove.jpg', '2023-07-21', 1, 'A fun network of fresh red roses and a teddy bear doll, 
which will give a smile to the lucky recipient!', 6, 3, 75, 3, 'Summer', 15),

('Lovely Bunch Of Carnation', 'Product Contains:

15 Red Carnation Bouquet

Wrapped in White Paper

Tied with Red Ribbon

Seasonal Green Filler', 18, '/images/bouquet/lovelybunchofcarnation.jpg', '2023-07-21', 1, 'As the saying goes you must have heard “girls love flowers”. 
Not loving flowers is impossible. It is a pleasant delight to an eyes especially red coloured flowers and when bunch of 15 red carnation
is gifted to someone how could they not fall in love with those pretty little flowers? 
Buying those appealing bunch of red flowers can always help you win the heart.', 6, 1, 75, 3, 'Summer', 15),

('My Heartbeats', 'Constituents
Big Round Box
Care Guide
Flowers
Specifications
Big Round Box
Size: 2 x 9 Inch
Care Guide
Flowers
No. of Stems: 178
Type of Flowers: Gypso ,Roses
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/myheartbeats.jpg', '2023-07-21', 1, 'Love is nothing short of a celebration. And to mark this beautiful feeling, only a bed of roses will do. Express your neverending fondness for the love of your life with a selection 
that exudes infinite emotions- Sweet avalanche, deep purple, aqua pink, dark pink and red roses.', 6, 2, 75, 3, 'Summer', 15),

('99 Red roses Midnight Edition', 'White Lilies', 18, '/images/bouquet/99redrosesmidnightedition.jpg', '2023-07-21', 1, 'Convey your love and appreciation to that special someone with this stunning bouquet.
A bouquet of 99 majestic red roses represents a promise of forever love.
With this stunning gift, it will surely make the heart flutter.', 6, 3, 75, 3, 'Summer', 15),

-- Thank you
('Stunning Red Roses Bunch', 'Product Contains:

- 8 Red Rose

- Wrapped in Red Paper

- Seasonal Green Filler

- Tied with White Ribbon', 18, '/images/bouquet/stuningredrosesbunch.jpg', '2023-07-21', 1, 'Be bold in love. Be confident, too. How so? Get the Stunning Red Roses Bunch
ordered at your doorsteps or lover’s, on the time of delivery of your choice. 
These 8 roses in red paper packing will make you the best lover ever in the eyes of your partner. 
You can also swoon them off their feet with this lovely valentine’s surprise. 
Click on Buy Now option and fill your cart with love.', 9, 1, 75, 3, 'Summer', 15),

('Sweet Grandeur Love', 'Constituents
Flowers
Care Guide
Gold Metal Stand Base
Specifications
Flowers
No. of Stems: 40
Type of Flowers: Gypso ,Roses ,Spray Carnations ,Spray Chresynthamum
Colour of Flower: Assorted
Care Guide
Gold Metal Stand Base
Country of Origin: India', 18, '/images/bouquet/sweetgrandeurlove.jpg', '2023-07-21', 1, 'A magical larger than life hand-tied crafted with love. 
Tell them how much you adore their presence with this special arrangement of peach roses, 
sweet avalanche roses, mauve chrysanthemum, pink carnations, pink gypso in an astonishing gold metal stand.', 9, 2, 75, 3, 'Summer', 15),

('Chocolate Romeo', 'Red Roses (7 Stalks), 
errero Rocher (14 stalks), 
White Baby Breath', 18, '/images/bouquet/chocolateromeo.jpg', '2023-07-21', 1, 'Spoil somebody with the best of two worlds - Red Roses and Ferrero Rocher, 
united in a unique bouquet!', 9, 3, 75, 3, 'Summer', 15),

('Double Celebration Cake Combo', 'Product Description

- 4 Purple Orchids

- 4 Red Roses

- 4 Pink Carnations

- 6 inch Teddy

- 1 Basket

- Seasonal Green Fillers

- Half Kg Black Forest Cake', 18, '/images/bouquet/doublecelebrationcakecombo.jpg', '2023-07-21', 1, 'Basket of 4 Purple Orchids, 4 Red Roses and 4 Pink Carnations with 6 inch Teddy and Half Kg Black Forest Cake.', 9, 1, 75, 3, 'Summer', 15),

('Amethyst Dream', 'Constituents
Care Guide
Lilac Coupe Vase
Flowers
Specifications
Care Guide
Lilac Coupe Vase
Flowers
No. of Stems: 26
Type of Flowers: Daisy Purple ,Eucalyptus ,Gypso ,Lisianthus ,Roses ,Statice ,Veronica
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/amethystdream.jpg', '2023-07-21', 1, 'Convey your appreciation with an ethereal selection that''s just as vibrant and optimistic
as your loved ones - Purple lisianthus, 
deep purple roses, blue daisy, purple daisy, white veronica, 
eucalyptus and lavender gypso.', 9, 2, 75, 3, 'Summer', 15),

('Yellow Carnation Bouquet', 'Product Contains:

- 12 Yellow Carnations

- Yellow Packing Paper with Yellow Ribbon', 18, '/images/bouquet/yellowcarnationbouquet.jpg', '2023-07-21', 1, 'You can never go erroneous sending a glamorous Bouquet of 12 yellow carnations. 
Yellow carnations are neatly arranged in an attractive Bouquet. 
Merely fresh yellow carnations make a truly pleasant gift.', 9, 3, 75, 3, 'Summer', 15),

('6 White Orchids in a Vase', 'Product Contains:

- 6 White Orchid
- 1 Glass Vase', 18, '/images/bouquet/whiteorchidsinavase.jpg', '2023-07-21', 1, 'A cluster of striking flowers is certain to make any special day or celebration more thrilling,
colorful, and memorable. 
The bunch of 6 white orchids is ideal to produce a 
long-lasting impression on your dear ones.', 9, 1, 75, 3, 'Summer', 15),

('Rosy Gardenia', 'Constituents
Care Guide
Luxury Box
Flowers
Specifications
Care Guide
Luxury Box
Size: 28 x 28 x 11 cm
Flowers
No. of Stems: 40
Type of Flowers: Gypso ,Saintiny White ,Spray roses ,Roses Sweet Avalanche
Colour of Flower: Assorted
Country of Origin: India', 18, '/images/bouquet/rosygardenia.jpg', '2023-07-21', 1, 'Uplifting, calming, oozing with lively spirit,
this soothing pretty pink floral arrangement of sweet avalanche roses, santini, 
pink roses, and pink gypso in a classic glass tapered vase with pearls 
reminds you of a summer scene from a romance novel', 9, 2, 75, 3, 'Summer', 15),

('The Royalty', 'Constituents
Care Guide
Medium Cylindrical White Box
Godiva Prestige Dark Biscuit 12pc 100g
Flowers
Specifications
Care Guide
Medium Cylindrical White Box
Size: 8 X 7.5
Godiva Prestige Dark Biscuit 12pc 100g
Weight: 100gms
Brand: Godiva
Filter: Cookies
Flowers
No. of Stems: 18
Type of Flowers: Roses
Colour of Flower: White
Country of Origin: India', 18, '/images/bouquet/theroyalty.jpg', '2023-07-21', 1, 'Fill their space with captivating blooms and cravings with indulgence 
with this enthralling assortment that truly offers the best of both worlds - Godiva prestige dark chocolate biscuits,
luxury white roses in a white interflora gift box.', 9, 3, 75, 3, 'Summer', 15),

-- Noel
('Merry Christmas', 'Products include:
Single red carnation : 15
Red jade chain : 10
Chrysanthemum white: 10
Green wicky : 7', 18, '/images/bouquet/merrychristmas.jpg', '2023-07-21', 1, 'Although not a traditional holiday of Indian people, 
Christmas has long become a very familiar holiday. On this occasion, blue, red, and white colors seem to be on the throne. And the "loving Christmas" flower pattern is designed in the dominant red color, 
adding a little white as if to add the spirit of Christmas in it and hope to bring a lot of joy, 
happiness and peace. For the recipient.', 7, 1, 75, 3, 'Summer', 15),

('Hello December', '\- nRaw materials: Da Lat fresh cedar branches
- Decoration: Main decorative product with 1 main side, simple decoration on the back. Accessories are randomly decorated according to the color tone, but the shop is committed to ensuring the aesthetic value of the product.
- Plant height: 110-120cm (including pots)
- Fresh medlar branches can last from 2-4 weeks in sufficient water in foam and distilled in a cool place.', 18, 'hellodecember.jpg', '2023-07-21', 1, 'Hello December - the last month of the year - the month that evokes the most emotions... The feeling of being up-to-date and bustling of the whole year is about to 
pass mixed with the joy of welcoming the new year. 
Wishing everyone more love and dedication. The luxurious and beautiful decorated pine tree will add to the special Christmas atmosphere.', 7, 2, 75, 3, 'Summer', 15),

('White Wooden Fir 100cm', 'Wooden Fir', 18, '/images/bouquet/whitewoodenfir100cm.jpg', '2023-07-21', 1, 'Christmas is an indispensable image in every Christmas. 
Currently, in addition to the very popular fresh pine, 
there are also many types of pine made from different materials such as PE plastic, pressed wood....
Christmas pine products are made from recycled wood, friendly to the environment. environment and especially can be reused many times.', 7, 3, 75, 3, 'Summer', 15),

('Christmas tree', 'Christmas tree 1m2', 18, '/images/bouquet/christmastree.jpg', '2023-07-21', 1, 'Christmas tree or Christmas tree with many decorative accessories is the image attached, the soul of the Christmas holiday. According to the concept of many Westerners, blue is the color symbolizing eternity, prosperity and abundance. Plants that are green all year round often have a very special meaning to people. The Christmas tree chosen for Christmas is also because it can still be green in the cold winter. 
Many countries also believe that the color blue is an amulet to help ward off evil spirits and diseases.', 7, 3, 75, 3, 'Summer', 15),

('Back to December', '- Raw materials: Da Lat fresh medlar twigs
- Decoration: The main decorative product has 1 main side, the back is simply decorated. Accessories are randomly decorated according to the color tone, but the shop is committed to ensuring the aesthetic value of the product.
- Plant height: 80cm (including pot)
- Fresh medlar branches can be displayed for 2-4 weeks in sufficient water in foam and distilled in a cool place.', 18, '/images/bouquet/backtodecember.jpg', '2023-07-21', 1, 'Winter comes when the trees shed all their leaves, leaving only the thin branches like thin arms that look pitiful. But there are also trees that, despite the cold and harsh weather, still confidently put on a green, full of vitality. Therefore, those trees are chosen as symbols for winter, or as we often call them with familiar names like Christmas trees - Christmas trees. 
And today, every winter, people often choose to buy a pine tree and decorate it with small and beautiful accessories to add vitality to their own home.', 7, 1, 75, 3, 'Summer', 15),

('Christmas - New Year''s Eve', '- Materials: fresh pine, cut branches imported from Denmark
- Decoration: Main decorative product on 1 main side, simple decoration on the back. Accessories are randomly decorated according to the color tone, but the shop is committed to ensuring the aesthetic value of the product.
- Plant height: 60-70cm (including pots)
- Freshly cut pine pots can last from 2-4 weeks in sufficient water in foam and distilled in a cool place.', 18, '/images/bouquet/christmasnewyearseve.jpg', '2023-07-21', 1, '- Materials: fresh pine, cut branches imported from Denmark
- Decoration: Main decorative product on 1 main side, simple decoration on the back. Accessories are randomly decorated according to the color tone, but the shop is committed to ensuring the aesthetic value of the product.
- Plant height: 60-70cm (including pots)
- Freshly cut pine pots can last from 2-4 weeks in sufficient water in foam and distilled in a cool place.', 7, 2, 75, 3, 'Summer', 15),

('Best Christmas', 'Products include:
Single red carnation : 5
Red jade chain : 10
Green wicky : 4
Red Elegance : 4', 18, '/images/bouquet/bestchirstmas.jpg', '2023-07-21', 1, 'Christmas is also the time when we send meaningful gifts to friends and loved ones. But choosing the right gift is really not easy at all. For that reason, the florists of Hoayeuthuong.com have created meaningful 
and colorful flower patterns of a peaceful and happy Christmas. 
Surely these Christmas works will be great gifts for you to give to your loved ones.', 7, 2, 75, 3, 'Summer', 15);

GO

INSERT INTO Receiver ([Name], [Address], Phone, ReceiverDate)
VALUES ('Receiver 1', '123 Receiver St', '1112223333', '2023-07-20'),
       ('Receiver 2', '456 Receiver Rd', '4445556666', '2023-07-21'),
       ('Receiver 3', '789 Receiver Ave', '7778889999', '2023-07-22'),
       ('Receiver 4', '321 Receiver Blvd', '1234567890', '2023-07-23'),
       ('Receiver 5', '567 Receiver Lane', '5556667777', '2023-07-24'),
       ('Receiver 6', '987 Receiver St', '8889990000', '2023-07-25'),
       ('Receiver 7', '456 Receiver Rd', '1110001111', '2023-07-26'),
       ('Receiver 8', '890 Receiver Ave', '2223334444', '2023-07-27'),
       ('Receiver 9', '123 Receiver Blvd', '3334445555', '2023-07-28'),
       ('Receiver 10', '321 Receiver Ave', '6667778888', '2023-07-29');
GO

-- Vui lòng thay đổi dữ liệu theo yêu cầu thực tế
INSERT INTO [Order] (UserId, OrderDate, ReceiveDate, ReceiverId, [Address], [OccasionId], Amount, [StatusId])
VALUES (1, '2023-07-20', '2023-07-22', 1, '123 Order St', 1, 25.5, 2),
       (2, '2023-07-21', '2023-07-23', 2, '456 Order Rd', 2, 23.25, 2),
       -- Thêm dữ liệu cho 8 đơn hàng còn lại tương tự như trên
       (3, '2023-07-29', '2023-07-31', 10, '321 Order Ave', 3, 30.0, 2);
GO

-- Vui lòng thay đổi dữ liệu theo yêu cầu thực tế
INSERT INTO OrderDetail (OrderId, BouquetId, UnitPrice, Quantity, Discount)
VALUES (1, 1, 20.5, 5, 2.75),
       (2, 2, 18.75, 3, 1.125)
Go

INSERT INTO Voucher (Code, UserId, DiscountPercent, [StatusId])
VALUES ('FirstOrder', 1, 10, 1),
       ('FirstOrder', 2, 10 ,2),
       ('FirstOrder', 3, 10, 1),
       ('FirstOrder', 4, 10, 2)
GO

-- Thêm dữ liệu mẫu vào bảng Blog
INSERT INTO Blog (Title,[Image], BlogBrief, Content, PublishDate, UserId, [StatusId])
VALUES
    ('A Rose Story Part 1: How I Came to Roses','/images/blog/blog1.jpg','This summer I had the most amazing opportunity to reconnect with Anne Belovich, a well-known...', 'This summer I had the most amazing opportunity to reconnect with Anne Belovich, a well-known and beloved rosarian who left her mark on so many. Anne lived a long and beautiful life and sadly passed away shortly after turning 97 this past fall.
I am so thankful to have gotten a chance to know her and am so inspired by how fully and generously she lived her life. 
 This story is a long one, so we decided to break it up into four parts. The first part is about how I came to know Anne and the creation of our rose garden.
The second part is about how we’re helping to preserve her extraordinary collection of roses and the process we used to propagate some of her rare treasures.
In the third part, I share more about our rose collection here on the farm and my sources for rare, hard-to-find varieties.
And the fourth part is a wonderful interview with Anne which will leave you in tears—she was one of the most inspiring women I’ve ever met. I hope you enjoy this series.
 I have been collecting rare, heirloom varieties of roses for nearly 20 years now. Shortly after we bought our house in the country, I went to work trying to transform our acre of perfectly manicured lawn into a wild, magical secret garden inspired by all of the English gardening books that I checked out at the library. 
   A few years into my gardening journey, I became a full-fledged flower farmer and received my first grant to trial a wide range of roses that were known to have really great rose hips suitable for cutting. Some of the varieties were from here in the states, but many had to be imported from abroad, which was a very complicated, time-consuming, and expensive process. 
Our area USDA plant inspector connected me with a local rosarian who was very experienced when it came to the process of importing and she generously offered to walk me through all of the steps and show me her setup for quarantining plants once they arrived in the states. And that’s how I came to know Anne Belovich.
  Anne is somewhat of a legend in the rose world and has inspired so many with her passion and generous sharing. Her love of roses was contagious and while she came to gardening later in life (she grew her first rose at 60), she scoured the globe and amassed the largest collection of giant rambling roses in North America and ended up writing five books on the subject.
   I’ll never forget the first time I visited her garden, which was like stepping into another world. Anne and her husband Max were such warm hosts and gave me an afternoon of their time, walking me through all of the beautiful gardens and dozens of arbors smothered in arching canes of roses that were just about to bloom.
Every time I thought we were at the end of the tour, we would turn a corner into another section of the garden brimming with varieties I had only read about in books. 
After that first visit, Anne gave me permission to come back as often as I wanted and I returned a couple more times that season to wander around and soak in all of the magic. It was an absolute sight to behold. 
I got so caught up with the farm and raising the kids and trying to keep my head above water that I lost touch with Anne, but every June when all the old roses would bloom in my garden, I would think of her and long to go back. 
  Fast forward a dozen or so years and we finally had a larger piece of land to call our own. When we got the farm it was a blank slate of a field without any structure. It was hard to imagine what it could become, but one of the things I knew I wanted it to include was a rose garden—and to fill the farm with as many rare and heirloom varieties as I could possibly get my hands on.
So I went to work collecting plants from a wide range of nurseries and specialty growers across the country. In all, I gathered more than 250 individual varieties and nearly 1,000 plants. 
   Once I had all of the plants gathered, Becky Crowley (who came from England to help me design the farm) and I got to work mapping out where they would all go. In England, there are so many spectacular gardens to visit where you can draw inspiration from. But here on the West Coast, established gardens are few and far between, and finding any with old-fashioned roses is a rare treat. 
We spent a great deal of time at my favorite local nursery, Christianson’s, and the owners John and Toni were generous enough to let us tour their personal garden too. Both the nursery and their home garden were incredibly inspiring. It was during one of our conversations that Anne Belovich came up and I decided to reach out and see if we could pay her a visit. 
 Many years had passed since we last spoke, but Anne, now in her late 90s, was still just as wonderful as I had remembered.
While she didn’t have the energy to personally take us around the garden, her lovely family gave the ladies and me a tour and then turned us loose with our clippers and notebooks to cut as many roses as we possibly could. 
 The last time I visited, the extensive gardens and grounds were perfectly manicured. The beds were edged and mulched, the roses were trained to their beautiful arbors and arches, and the acres of lawn were freshly mowed.
But in the years since, Max had passed away and Anne was no longer able to keep up with the monumental task of maintaining the gardens alone. 
   While still a sight to behold, nature had crept in and the once-perfect garden had become wild. Roses were climbing high up into the trees, they had swallowed fences and small buildings and completely smothered their arbors.
At their feet, blackberries had taken hold and the two were competing for the same space.
   After our tour, we were all absolutely overwhelmed by the wild beauty and the magnitude of Anne’s collection and couldn’t figure out where to even begin.
So we decided to head back home and get some rest, collect our thoughts, gather supplies, and return the next morning to get to work.
 Anne’s daughter-in-law, Teddie, gave us a copy of Anne’s 20+ page rose list that included every variety on the property and its rough location. The tricky thing was that the location names were Anne’s shorthand and we weren’t sure which part of the gardens they referred to and Anne couldn’t quite remember what was where.
We were able to locate plastic labels on a number of varieties but many had faded and were illegible or were brittle and crumbled in our hands. 
   So we spent a couple of days trying to match the rose list to the varieties that did have labels and solve the mysteries through the process of elimination and a lot of Google searching.
We tried our best to ID and relabel as many of the roses as we possibly could, but there were still so many that remained a mystery.
    Our little team was buzzing with excitement and everyone took a different part of the project.
Chris took photographs and drone footage of the property so we could try and establish landmarks and key spots while Becky sketched out all of the beds, fences, and structures noting each variety that we were able to identify to ultimately pair up with the photographs so we could create an actual map. 
    Nina was on blackberry patrol and cut pathways through the brambles so we could get at the roses. Jill took labeling very seriously and got to the base of the plants by any means necessary.
 Angela carefully bundled each variety and got them into water and into the shade of the van. I ran around like a crazy person taking hundreds of cuttings and trying to help ID all of the mysteries.
 We had to keep stopping and reminding each other to breathe because we were so freaking excited about the roses and still in shock that we were even allowed into this magical secret world that Anne had created. 
    We returned three more times to gather as much cutting material as we could, but a record heatwave condensed the bloom window, which is usually about a month, into 10 short days. 
  I’m still in awe of how much progress we made in such a short amount of time. In all, we gathered more than 1,000 cuttings from Anne’s roses in hopes of propagating them to grow at the farm and eventually to share some of the most rare varieties with others. 
   In the next post, I’ll share our process for propagating old roses through cuttings.
', '2023-07-22', 1, 1),

    ('A Rose Story Part 2: Propagating Old Roses','/images/blog/blog2.jpg','One of my favorite things about old roses, in addition to their wild habit, their beautiful fragrance, and old-fashioned...','One of my favorite things about old roses, in addition to their wild habit, their beautiful fragrance, and old-fashioned appearance is how easily many of them can be propagated through cuttings and grown on their own roots.
Own root roses, while they are harder to find, are heartier, healthier, and have a longer life. When you order them from specialty nurseries they are shipped in pots versus bare root like these pictured below.
The nice thing about old rose varieties is that they are no longer protected by plant patents and can be propagated legally. 
 Many modern roses like hybrid teas, floribundas, and many of the newer David Austin varieties are typically propagated through a process of grafting or budding where licensed growers who have permission to propagate patented varieties take plant material from the variety that they want and graft it onto a rootstock.
This process allows rose growers to produce them quickly, efficiently, and on a large commercial scale. These are large, bare root grafted roses, and the first ones I planted in the cutting garden.
  In my experience, the downside to grafted roses is that they are just not as hardy overall. If they experience extreme cold temperatures, the top half of a grafted variety will die and the rootstock will live on which you won’t realize until the following season when it flowers with an ugly magenta/red single bloom! If a non-grafted (own root) rose is killed to the ground it will grow back true to type which is great for gardeners in colder climates. In my experience, I’ve found that own root plants overall are healthier and longer lived. 
While I prefer growing own root roses, it’s certainly not required, and I still have quite a few grafted varieties in my garden that are growing wonderfully. But if given the choice, I prefer to buy own root when they are available.
 It is important to note that it is illegal to propagate patented varieties. Rose patents last for around 20 years so if there’s a variety that you’re considering propagating, you’ll want to be sure that at least 20 years have passed.
Here’s a great article about how to know which varieties of roses are protected by plant patents and which ones you can propagate freely.  
 I’m not an expert when it comes to propagating roses through cuttings, but I’ve had decent success over the years.
There is so much conflicting advice out there and it can be super overwhelming when you start researching the different methods—there are so many ways you can do it.
 For me when it comes to propagating plants of any kind, especially roses, it’s a numbers game. I take as many cuttings as I possibly can knowing that many of them won’t make it.
So the more I do, the more chances I have to get it right. 
 After realizing that we would be able to take cuttings from Anne’s collection, which was an unexpected surprise, I wanted to do everything in my power to ensure that we had the highest success rate for this project possible so I reached out to Burling Leong at Burlington Roses (one of my favorite sources for rare and heirloom varieties), who generously shared her expertise. 
 Burling recommended that we set up a misting system with a fine fog nozzle so that the cuttings could get good airflow but also adequate moisture.
She recommended a blend of two-thirds perlite and one-third peat moss or coco coir and using 50- or 72-cell trays rather than pots because they don’t require as much soil or space and we had a lot of cuttings to do.
Per her advice, I switched from a liquid rooting hormone to a powdered version called Hormex which was really easy to use.
   To prepare the cell trays we mixed the perlite and peat together, wetted it down, and filled the trays with the mix, tamping them down on the table to remove any air pockets.
Then Jill went through with a pencil and pre-poked holes in the center of each cell until it hit the bottom of the tray. Then the trays were ready to receive the cuttings. 
  When we were at Anne’s we cut roughly 12-in pieces of rose canes (that were the thickness of a pencil) off of the plants, bundled them by variety, and transported them from her garden to our farm in jars of water.
  As soon as we got back to the farm we processed the cuttings by cutting each long cane down into smaller pieces that had at least three internodes (leaves) per section.
We then removed the lower leaves, typically leaving one set of leaves at the top of the stem if they weren’t wilted from the journey.
   Next, we dipped the stem end into the rooting powder and slipped them down through the pre-poked holes in the cell tray, and gently firmed the soil around the stem. 
   Once the trays were filled with cuttings, they were labeled with the variety name and date and set under the misters, which came on for a few minutes every hour during the day.
Then the hard part began … waiting to see if it worked. 
  Generally, cuttings take 4 to 6 weeks to root, sometimes longer depending on the variety.
Once we saw white roots start to form and poke out the bottom of the cell tray, we then very gently transplanted them into larger pots so they had room to spread out.
 These tender little cuttings will spend the winter inside the greenhouse (which is kept just above freezing) and then they will be transplanted again into larger pots in the spring once they start putting on new growth.
I’m guessing we’ll grow them in pots for at least a year before planting them into the garden just to be sure that they have the best chance of survival. 
 If you are a rose-cutting expert and have any advice or tricks to share, I’d love to hear them in the comments section below.
In the next post, I’ll be sharing my favorite specialty rose nurseries here in the States plus a little more about how we’ve planted roses here on the farm.
', '2023-07-23', 2, 2),

    ('A Rose Story Part 3: Floret’s Rose Collection','/images/blog/blog3.jpg','Ever since meeting Anne all those years ago, I’ve longed to create my own version of her magical garden and plant as many roses...', 'Ever since meeting Anne all those years ago, I’ve longed to create my own version of her magical garden and plant as many roses as I could get my hands on. But with every inch of our small farm devoted to annual cut flower production, I could only ever sneak roses in around the edges.
I had ramblers and climbers scrambling up and over my little flower studio and a few dozen heirloom treasures tucked up against the fence and on my back porch but that’s all that I could manage to squeeze in without impacting cut flower production.
 The day we signed papers for the new farm all I could think about was how long I had waited to be able to put down roots and be able to bring my garden dreams to life. One of the first things I did was start to collect roses for the future garden.
  I scoured the country looking for rare, heirloom, and old-fashioned roses to include on the property. So many of the specialty nurseries that used to be in business have since closed so it was quite the task to source everything on my wish list. 
Of all the nurseries I’ve ordered from for this project, below you’ll find my very favorite sources. 
  The Antique Rose Emporium
This long-standing Texas-based nursery has some of the best customer service around. They have a very unique offering that features more than 350 rare and hard-to-find antique varieties, including their Texas Pioneer rose series, which is a line of carefree, repeat-blooming varieties with an old-world quality.  
Angel Gardens
I have found some really special varieties from this Florida-based mail-order nursery and love visiting their website for the pictures alone. Angel Gardens offers more than 1,000 antique and modern roses and uses organic growing practices in their production. 
Burlington Rose Nursery
Owner Burling Leong maintains a large collection of rare and hard-to-find roses and has been one of my go-to sources when trying to track down rare, coveted varieties. To get a list of her most current availability list, email BurlingtonRoses@aol.com. 
Christianson’s Nursery
If you live in western Washington, this family-run nursery has a tremendous selection of potted roses (including many heirloom and hard-to-find treasures) that go on sale in January. While they don’t ship their plants, if you’re within driving distance it’s well worth the trip, and be sure to visit in June to see their English-style rose garden in full bloom—it’s a sight to behold!  
David Austin Roses
If you’ve grown roses for any length of time, chances are that you’ve fallen in love with the David Austin varieties, which are known for their old-fashioned-looking blooms that come in a wide range of soft colors and are generally repeat-blooming. I have been collecting David Austin roses for years and if you ever come across some of his older varieties, be sure to snatch them up because they are no longer being commercially propagated. 
The Friends of Vintage Roses
This Sebastopol, California-based nonprofit maintains one of the largest private collections of roses in the world and was originally started by Gregg Lowrey and Philip Robinson. Each year, they propagate thousands of cuttings that are sent to curators, collectors, and public gardens devoted to preserving these special plants. Twice yearly they offer the extra plants from their propagation efforts for sale to the public (local pickup only). You can find the availability list on their website. To learn more about the important work they are doing, you can read this interview with founder Gregg Lowery. 
Greenmantle Nursery
This California-based nursery has assembled a comprehensive collection of own root rare and old rose varieties that they have gathered from around the world. If you’re looking for something special, be sure to check their Rose Master List, which includes so many rare treasures. Roses from Greenmantle Nursery must be reserved with a deposit made in advance and their shipping season is January through May. It’s important to note that they only communicate through the mail or by telephone. 
Heirloom Roses
This rose nursery in Oregon has a tremendous selection of varieties grown on their own roots, including more than 60 David Austin varieties (many of the older ones), a huge range of historic roses, hybrid musks, and everything in between. 
High Country Roses
This Colorado-based nursery has an amazing selection of own root roses. Their collection features old garden roses, cold-tolerant varieties, and modern varieties, including some of the older David Austin treasures. 
Menagerie Farm & Flower
Our flower-farming friend Felicia Alvarez offers a beautiful collection of bare root roses that she has trialed over the years and are known to make excellent cut flowers. Her inventory typically sells fast, so be sure to sign up for her newsletter. Felicia also sells fresh-cut garden roses shipped to floral designers nationwide from late spring through early autumn. 
A Reverence for Roses
This Florida-based nursery specializes in own root roses and has a huge selection of heritage and old garden varieties, plus so many beautiful modern roses and a large selection of hybrid musks—my favorite group!
Rogue Valley Roses
This Oregon-based nursery offers more than 1,500 unique varieties and has one of the largest selections of rare, historic, and exceptional modern roses grown on their own roots. They ship live plants to U.S. and Canadian customers year-round, and can also ship bare root plants internationally in December and January. Be sure to use their “Join wait list” feature, since many of the plants sell out fast and it’s the only way to know when they come back in stock.   
Rose Petal Nursery
This Florida-based mail-order nursery offers a large selection of rare heritage roses which they are constantly expanding each year. If you find something special that isn’t available, be sure to use the “be notified” button and you’ll receive an email when it’s back in stock. I’ve found some real treasures here!
 After 3 years of collecting, I gathered more than 250 varieties and nearly 1,000 plants (these numbers do not include the roses from Anne), and while this seems like a lot, I still have a huge wish list of varieties that I’m searching for. I have a feeling that my rose collecting won’t slow down any time soon and they may very well end up being the subject of a book someday.  
I will definitely share more about the varieties that I chose and how they are performing once I have a little more time to get to know them. In the meantime, I’ve shared a few of my favorites below. 
    On the farm, our roses are nested into four main collections. The first are the rose varieties that we’ve planted specifically for cutting. We’ve devoted ten 70-foot rows of the cutting garden to these varieties that are organized loosely by color. 
These roses are all repeat-bloomers and more than half of them are David Austin varieties and most have a full cabbage rose look. 
 Some of my favorites from the cutting garden are ‘Abraham Darby’, ‘Golden Celebration’, ‘Grace’, ‘Teasing Georgia’, ‘Perlie Mae’, ‘Mother of Pearl’, and ‘French Lace’.
  The second group of roses is a mix of climbing and rambling varieties, both one-time and repeat-blooming. Down the center of the cutting garden, we have a series of archways that meet in the middle under a large metal dome that will eventually be engulfed with blooms.
   Additionally, down the main access road to the cutting garden, there are a series of alternating towers with climbing roses planted at their base which will soon climb up through and spill over the top. The cutting garden is going to be off the hook once all of these structures are covered! 
A few of my favorite climbing and rambling varieties are ‘Cecile Brunner’, ‘Glorie de Jon’, ‘Malvern Hills’, and ‘Alchymist’.
 The third group is all of the old roses (most are one-time blooming) that are tucked into the orchard, the hedgerows, or flank the main roadways throughout the farm.
The idea is that once established, these vigorous, hardy varieties will be able to withstand the harsher, more exposed conditions out on the main farm.  
A few of my favorite old rose varieties are ‘Dupontii’, ‘Kathleen’, ‘Moyesii’, and rosa glauca. 
   The fourth group is a mix of mainly shrub roses selected for their fragrance and delicate-looking blooms. We wanted to be able to observe these roses up close and really get to know them over time. 
For this formal rose garden we needed to get the plants in the ground quickly because they had outgrown their pots and we were going through a pretty extreme heatwave, so rather than tilling up the grass and waiting for it to break down, we instead rented a sod lifter and prepared the garden space in a long, back-breaking weekend. 
  We’d never used a sod lifter before and it’s definitely a great option if you need to remove grass from an area quickly, but if you have quack grass, it doesn’t get deep enough to remove the rhizomes so those still need to be grubbed out by hand.
Once the sod was cut, we rolled it up and took it back to the compost pile to decompose and then next spring, once it has broken down, we’ll return it to the garden in the form of compost. 
  Once the sod was out of the way, we amended the planting beds with a thick layer of compost and a heavy dose of Walt’s organic fertilizer, and then covered the beds with landscape fabric. Becky then marked the spots where each rose would go, we burned holes in the fabric and planted the roses into the holes.
The reason for this was to help suppress the weeds in this garden while the roses grow and establish because we just didn’t have the time to mulch and weed all of the planting beds by hand. Once the roses are larger, we will remove the landscape fabric and plant perennials and small shrubs amongst the roses. I can’t wait to see this garden come to life!
Some notable favorites planted in this garden are ‘Bishop Darlington’, ‘Buff Beauty’, ‘Penelope’, ‘Sally Holmes’, ‘Star of Republic’, and ‘Windrush’.
  I’ve saved the best for last. In the fourth and final post of this series, you’ll find an interview with Anne Belovich that she so generously shared with us all this summer, just a few short months before she passed away. 
', '2023-07-24', 1, 1),

    ('A Rose Story Part 4: An Interview with Anne Belovich','/images/blog/blog4.jpg', 'I’m thrilled to be able to share an interview with revered rosarian Anne Belovich (pictured below with her husband Max). This interview took place...', 'I’m thrilled to be able to share an interview with revered rosarian Anne Belovich (pictured below with her husband Max). This interview took place in August of 2021, just months before Anne passed away at the age of 97. 
I am so thankful to have gotten a chance to know her and am so inspired by how fully and generously she lived her life. 
 
You’ve had a very full career and an even fuller life—first as a botanist, then a teacher, then a sailor, a general contractor, and then a rosarian and a writer. Can you tell me a little bit more about your very diverse life path?
I have had a very full life and while much is owed to the length of my life, my longevity, likewise, probably benefited from having a full, diverse life. Life-long learning and growing keeps one engaged. 
It would probably surprise many of your readers to know that I used to consider myself a prisoner in my mother’s garden. We lived in Morro Bay on a high bluff. As a young child of three, maybe four, I used to run away—down the trail, that certainly wasn’t made for children, as fast as I could go down to the water. I’d get down there in the sand and there would be crabs and herons that would be fishing—so many wonderful things to see.
My mother would come screaming down after me, drag me back, and tell me to stay in the garden. The water is where I wanted to be and I managed to sneak out often until my mother put up THE fence.  My mother had a wonderful garden considering those times and her means. It was always about nature though. My appreciation of flowers came later.
My life could have played out very differently if not for losing my first husband in World War II. He was such an amazing man—very handsome, very good to me, and talented. I ran away from home to be with him and get married. He entered the military and became a fighter pilot in the 1st Air Commando group under Colonel Cochran, with the mission of flying behind Japanese lines to supply and evacuate troops and materials as well as provide fire.
My husband completed the ‘Thursday’ mission but died shortly after because of his airplane’s mechanical failure. This left me as a 19-year-old widow and new mother who had to find her own way in life. If not for this tragedy, I would have most likely had more children and settled down into a role very different from the many I’ve had since that time.
One thing that isn’t mentioned above, but is very dear to me, are the numerous volunteer and board positions I’ve held, especially co-creator of NOAH, the Northwest Organization for Animal Help, in Stanwood, Washington, which is dedicated to ending euthanasia of healthy, adoptable, and treatable homeless dogs and cats. Our humble beginnings consisted of volunteering once a week to answer phones and match the people who had lost their animals to those who had found animals on the Island.
As a result of the ever-growing need to save more animals and serve the community, we expanded adoption and a transfer/low-cost spaying and neutering program. In coordination with over 50 other shelters, NOAH transfers animals at risk of euthanasia to continue to work on giving them a second chance. I was on the board until my husband Max needed more care to stay in our home at the end of his life. However, I still donate to this important cause and others that lessen the suffering of animals, both domestic and wild, and work to conserve biodiversity on our planet. Much more work needs to be done in these areas.
 In the introduction to Ramblers & Other Rose Species Hybrids, you say that “fortunate circumstances” led you to start a small rose garden. That small rose garden evolved into 5 acres of nearly 1,000 unique varieties (which eventually became the largest private collection in North America). Can you tell Floret readers a little bit more about this fortunate circumstance? What was it about roses in particular that put you under their spell? 
I just love them so. I’m a very visual person and the beauty that roses bring into my world gives me great joy. The “fortunate circumstances” I referred to in my book on ramblers was that I read an advertisement about a nursery in Oregon that was going out of business and having a big sale.
The ramblers were a really good price, and I came back with a truckload of them which I put on the fences around the property. I also put some into trees by building trellises to help support their growth. There was something wonderfully mysterious about roses growing up the trellises and into the trees to make the tree look as though it was blooming.
 You’ve written five books on roses, but have a deep love of rambling roses. What do you wish others knew about this amazing group of plants and why they should consider growing them in their garden?
Ramblers are easy to grow. Once they are established, they need little care. They grow tall and are excellent for covering fences and arbors, and for growing into trees. They provide a quick and easy way to add color to the garden.   
 You’ve scoured the globe for rare rose varieties. What are some of your favorite specialty nurseries? And besides your own wonderful books, do you have any other books or resources that you’d recommend for beginning and experienced rose growers?
While I have traveled the world, I like to support local, small business owners as much as possible. Their work is hard, and we have lost many nurseries, especially those that focus on old roses. Vintage Roses used to be the best but went out of business. I enjoy going to Christianson’s Nursery & Greenhouse in Mount Vernon, Washington. Two others are Hortico in Canada and Rogue Valley Roses in Oregon.
As for books, Classic Roses by Peter Beales is a very important reference book if you are serious about roses. Another one that was very helpful to me in the beginning was David Austin’s English Roses by David Austin and Michael Marriott. 
 I always hate it when people ask me what my favorite flower is because there are too many treasures to choose from, but if you could only grow five roses in your garden, what would they be? 
I really don’t have a favorite. All of them are so unique and bring something special with them. If I must choose one, I think it would have to be Hybrid R. Moyesii ‘Geranium’. In my book, Ramblers and Other Rose Species Hybrids, I mention that mine had grown to 10 feet (3 meters). That was in 2016. It is now running way up into the trees and spills over in a striking cascade of scarlet red in early summer. It fills the view from my dressing room window.  
 
Pictured above: Anne visiting our rose garden this past summer
Your passion for roses started much later in life. Do you have any advice for someone who feels like it’s too late to pursue their dreams?
I remember when I turned 60. I thought 60 was so old. That was almost 37 years ago, well over one-third of my lifetime so far.
At almost 97 years old, it doesn’t seem that my passion for roses started all that late in life. It’s been over 30 years since I began this journey—longer than the careers of many. I pursued many dreams after turning 60, including starting my own contracting company and building over 25 (mostly Victorian style) houses, traveling the world, and my study of the older roses. It is never too late to act on your goals and dreams. 
My advice is to keep moving, stay out of bed, go out to lunch and visit with friends and family (don’t isolate yourself), read and stay up to date with current events, be open to new ideas, and commit yourself to life-long learning and skill development.  
In fact, I am about to launch my own website and blog. I’ve always wanted my own website and I don’t think it’s too late to do this. I bought my own domain last week.
 In your book, A Voyage of Determination, which chronicles your incredible adventure sailing your beloved boat from New Zealand to California, you share your formula for achieving any difficult goal, which I found incredibly inspiring as a woman who has big dreams. You write: 
“When I was alone I spent much of my time thinking about the fantastic trip I had been privileged to experience. It was of great value to me in a way that was quite separate from getting the boat back. I had learned that I was capable of accomplishing very difficult goals. I was able to face considerable hardships and even extreme danger when it was necessary to achieve those goals. Without being quite aware of it I had developed a formula for greatly improving the chances of achieving any difficult goal. It consisted of three main parts.
First, don’t let being a woman stop you from doing what is traditionally seen as a man’s job unless you really need a constant supply of testosterone to achieve your goal. Ask yourself if the activity requires big biceps and a beard. If not, go ahead with your dreams and fight the prejudice where you find it. Look carefully for the same prejudice in yourself. It could be lurking there without you realizing it and could cause you to not believe in yourself and to restrict you from following a difficult goal. If you are a man you are not apt to encounter prejudice in life’s goals because of your gender, but if you do don’t let it stop you. 
Next, you should try to know yourself, your talents, and limitations, but be careful to not underestimate what you are capable of doing. Becoming a rocket scientist will be a difficult goal if you struggle with math, but maybe some remedial instruction in math would help you overcome the problem. I learned to navigate the old-fashioned way with a sextant even though I didn’t learn my number combinations when I was a child because of constant moving and now I compute manually with difficulty.
On the other hand, you might want to pick something that comes to you more naturally. A passion for a particular hobby might be an indication of a special talent that could be pursued and turned into a rewarding career. Then, be willing to spend some time and energy preparing for what you want to do. I owe much of my success to this one.”
I put some things in the Voyage of Determination, and you have to do all of them. Determine what it is you want to do and then acquire any skills or knowledge that you are going to need, get the books, take the classes. I bought three books when I decided to build the house on Camano Island; how to frame a house, how to wire it, and how to do the plumbing. You need to study and become an apprentice.  
You can do anything a man can do except those things that require a lot of strength…I think I said ‘big biceps’ in there. However, you can even figure out how to use mechanical means to overcome that. For example, I bought and used a wall jack on a house I was building to lift the walls into place all by myself.  
You can meet and exceed your goals if you prepare yourself over time. You can’t become a rocket scientist without a great deal of study, and neither can anyone else. 
  You have so generously shared what you’ve learned with so many and I would love to know how myself and Floret readers can support your work into the future. How do we ensure that these rare and heirloom rose varieties live on? How can we pick up the torch and help carry your legacy forward?
Keep them watered, fertilized, and in a place with lots of sunshine. Roses, especially the old roses, are very easy to grow. However, inviting people into your gardens and sharing is perhaps most important. Don’t be stingy. To inspire and introduce people to the beauty of old roses and sending cuttings of roses into their home gardens, is an act of love … of friendship. This is the greatest legacy.  
Max and I used to have people over all the time to enjoy the property, and many friendships as lovely as the roses developed. I couldn’t do that for a while, but now that my son and his wife are restoring the gardens, we have been able to do this again on a very limited basis (and keeping public health recommendations in mind). It’s such a joy to see old friends again, such as you, Erin. I believe you were a young lady when I first met you. So many memories tie us all together.
I have given many roses away over the years—to individuals, nurseries, and to other special collections and demonstration gardens. Recently I was notified by Claude Graves, curator of the Chambersville rose garden in Texas and the Anne Belovich Rambler Garden there, that the American Rose Center Committee voted to begin the process of replicating my entire rambler collection in Chambersville into a new garden to be constructed at America’s Rose Garden at the ARS Headquarters in Shreveport.
It is a comfort and honor knowing that my rambler collection will be duplicated and conserved in a permanent internationally-acclaimed garden. I am grateful to Dean and Carol Oswald and Claude Graves for their dedication to and hard work on this large project. I am also grateful to my friends who have watered, fertilized, and put one of my rose cuttings in a place with lots of sunshine and continue to share cuttings with their friends, both old and new, and younger family members.
In addition, consider organizing volunteer efforts to help out in rose gardens that need extra hands. What we consider older roses now can be found in many home gardens that were started by people in their younger years. The work of weeding, fertilizing, and pruning can be satisfying but enriched by stories, expertise, cuttings, and new friendships. We will have our first pruning party in February with area-old garden rose enthusiasts and garden clubs. Consider these kinds of events in your local community.
 You can learn more about Anne on her website, where her family has begun publishing blog posts that she wrote before her passing. 
I had the opportunity to interview Anne’s daughter-in-law Teddie Mower who is now caring for her extensive collection of ramblers alongside her husband Rick, Anne’s son. In the interview, she gives an update on Anne’s roses, information for those interested in visiting the property, and how we can all help carry on Anne’s legacy. 
If Anne’s story has moved you, please consider adding one of her books to your library. Proceeds from the sales of her books will help support the preservation of her rambling rose collection.
Gallica Roses by Anne Belovich
Large-Flowered Climbing Roses by Anne Belovich
The Little Book of Alba Roses by Anne Belovich
Moss Roses by Anne Belovich and Harald Enders
Ramblers and Other Rose Species Hybrids by Anne Belovich
A Voyage of Determination by Anne Belovich
 I thought it would be fun to give away Anne’s complete library of books to three lucky readers. To enter to win, please share what part of her interview inspired you the most. This giveaway is open to both U.S. and international readers. Winners will be announced here on May 30.

Update: A huge congratulations to our winners: Michelle, Pam Blinten and Carrie K.
', '2023-07-25', 2, 2),

    ('Attracting Pollinators into the Garden','/images/blog/blog5.jpg','One of the big projects I wanted to tackle as part of the new farm design was finding a way to attract as many pollinators as possible...' ,'One of the big projects I wanted to tackle as part of the new farm design was finding a way to attract as many pollinators as possible to help with seed production and increase the overall life and biodiversity here on the farm. 
When it comes to pollinators, honeybees usually get all of the credit, but there are so many other hard-working creatures that aid in the important task of pollination, including bumblebees, honeybees, native bees, wasps, hornets, flies, moths, butterflies, and even birds. 
 As I started looking for information about what plants were most attractive to pollinators, I found myself getting a bit overwhelmed. There were numerous wildflower seed mixes available specifically blended by region, but when it came to perennials, I couldn’t find any planting plans, suggested plant combinations, or design samples that I could use for inspiration. 
 While many companies do a great job identifying which plants are attractive to pollinators (usually with a bee or butterfly icon), I found it hard to know which plants had similar growing requirements and would make good companions in the landscape.
So often when you see pictures of a pollinator-friendly garden it’s typically a jumble of color and feels chaotic and messy. While that effect is fitting in a wild, meadow-like setting, it’s not very suitable for a more curated garden. 
 I wanted to see if I could find a way to create a beautiful pollinator-friendly garden that was also low maintenance, drought tolerant, and would provide a food and nectar source for pollinators and songbirds throughout as much of the growing season as possible. 
The first step in this experiment was to source as many easy-to-grow pollinator-friendly perennials as I could find. Rather than investing in large plants, I instead opted to order plugs, which are smaller plants, usually sold in trays of 32 to 50.
Buying smaller plants in bulk was the most affordable option and necessary given the scale of the project. 
 If I were doing this on a smaller backyard scale, I would still choose to start with the smallest plants I could find because what I discovered was that nearly all of the perennials I grew as part of this project are fast-growing and fill in quickly, catching up to the size of a 1-gallon potted plant within a single growing season.  
When making my selections, I ordered everything that was noted as being attractive to pollinators and also easy to grow. I tried to select plants that had softer, more muted colors, rather than really bright and bold selections. 
 Becky and I considered a number of different approaches when it came to designing what eventually became the pollinator strips. We are both huge fans of Piet Oudolf’s style and how he composes plantings in large drifts that are repeated in a loose pattern throughout the garden. It creates the effect of wide brush strokes of color and texture.
If you don’t already have Piet’s books, they are all really wonderful, but my favorite is Planting the Natural Garden. 
 Becky organized the perennials based on color, size, and flowering time. For some of the pollinator strips, we opted for a monochromatic color palette of all whites or purples, while others included multiple colors in softer complimentary shades.
In the end, we settled on seven different color and plant combinations.
 We decided to keep the design as simple as possible and plant the pollinator strips in long rows along the edges of our flower fields, similar to how our field crops are grown. This allowed us to use drip irrigation in the beds and landscape fabric to mulch the pathways in between, making maintenance much easier in the long run.
 In the spring before planting, each bed was amended with a few inches of high-quality compost and natural fertilizer (I love Walt’s Rainy Pacific Northwest blend) and this mixture was incorporated into the soil with our walk-behind rototiller.
Each pollinator strip is roughly 3 ft wide and about 80 ft long with a 2 ft wide landscape fabric-covered path. Once the beds were prepared, Becky laid out all of the baby plants according to her designs and we followed behind tucking them into the ground.
We chose to space plants quite closely together (roughly 8 to 9 in) because we wanted them to establish quickly, essentially forming a living carpet so that they would be able to compete with the heavy weed pressure we have here on the farm. 
  We normally grow our annual field crops in pre-burned landscape fabric to help with weed suppression, but since perennials spread from the base as they mature, using fabric on the beds wasn’t an option for this project.
Instead, we covered the bare soil around the young plants with a layer of straw mulch to keep the weeds at bay while they established. Shortly after planting, we laid down four lines of drip irrigation and watered plants deeply twice a week whenever there was no rain. Plants went into the ground in late March, and to my surprise, by July most were in full bloom and nearly filled in.
 We went through and spot-weeded a few times in the summer, but overall they required very little maintenance and care. Plants established quickly and soon smothered out the weeds.
  We evaluated each combination of plants over two full growing seasons.
Some planting schemes fared better than others, and I have plans to recombine the strongest performers from each into some new plantings to see if I can perfect the plant combinations.
Of all the plants that were part of this project, my very favorites were the yarrow, asters, agastache, nepeta, salvia, milkweed, Joe Pye weed, echinacea, and goldenrod. All of these plants were vigorous, filled in quickly, and didn’t have any major pest pressure.
Plus, the pollinators adored them. 
 While I absolutely love echinacea and it’s hugely attractive to pollinators, we have such heavy vole pressure in the field, and plants didn’t survive the first growing season. If voles weren’t an issue, I would incorporate even more echinacea into future designs because they have so many wonderful characteristics. 
Below you’ll find a little more information about some of my favorite planting schemes. 
  One of the pollinator strips I was most excited about was the one composed of all-white flowers.
This strip included echinacea ‘White Swan’, Shasta daisy ‘Alaska’, common yarrow, milkweed ‘Ice Ballet’, and perennial asters.
 While it looked glorious in late June and early July, it didn’t hold its beauty all summer long like many of the others. By midsummer, the Shasta daisies had tipped over and everything else looked a little bit shabby and dingy.
While the floral display waned more quickly than I had hoped, this particular pollinator strip was a favorite with songbirds in the fall and winter so it still has a ton of merit in my book.
 One of my favorite plant combinations was the one composed of all yellow flowers, including a mixture of different types of goldenrod (‘Golden Glory’, ‘Crown of Rays’, ‘Fireworks’, ‘Sunny Glory’, and ‘Romantic Glory’), various black-eyed Susans (including ‘Little Henry’ and ‘Goldrush’), and tansy. 
 This pollinator strip had a kind of rugged feel to it and all of the plants were very textural and wild. I think the plant mix would look stunning in some type of meadow situation or planted on an even larger scale.
Of all the pollinator strips, this one was the most attractive to pollinators, especially wasps, native bees, and flies. 
  Another lovely plant combination that Becky put together was all rosy pink and purple flowers.
It included yarrow ‘Sassy Summer Taffy’, pink tickseed, echinacea ‘Magnus’, milkweed ‘Cinderella’, Japanese anemone ‘September Charm’, agastache ‘Blue Boa’, and bee balm ‘Grape Gumball’.
 It’s worth noting that we lost most of the echinacea due to vole pressure so there were a number of gaping holes in the design, but on the flip side the Japanese anemones filled in rapidly and put on a beautiful late-season floral show. 
 To my surprise, the design that I was least excited about, which featured all blues and purples, turned out to be the longest-flowering and most beautiful one of all. 
Because all of the plants included in it had a more compact habit, they stayed upright without any support. The varieties bloomed in a nearly perfect succession from early May through September and whenever I stood next to it, it was literally humming with life! 
If I were to recommend one planting scheme of the seven, this is my very favorite and we’ve put together a printable planting plan and plant list which you can download at the bottom of this post.
 As part of our low-maintenance approach to caring for the pollinator strips, we decided to leave all of the plant debris in place through the winter, rather than cleaning it up at the end of the growing season.
I had no idea just how many little creatures would feast on the remaining seed pods and make these wild spaces their home throughout the coldest months of the year. 
  After realizing what an important role they were playing for wildlife, we’ve adopted a similar approach to all of the gardens on the farm and are now leaving the dead plants in place until early spring.
The plant skeletons give the winter garden a hauntingly beautiful quality—especially when they are covered in a layer of frost or a dusting of snow. 
 Overall, the pollinator strip experiment has been a huge success and I’m excited to continue working on it this coming season.
Most of the perennials that were used in this project were sourced from Bluebird Nursery in Nebraska. This wholesale mail-order nursery offers more than 1,500 different varieties.
If you’re looking for perennial plugs in smaller quantities, be sure to check out Prairie Moon Nursery in Minnesota because they are now offering native flowers and grasses for home gardeners. 
I am so excited to keep exploring plantings for pollinators and will continue sharing updates here on the blog.
', '2023-07-26', 1, 1),
    ('Floret’s Favorite Books','/images/blog/blog6.jpg', 'I have been obsessed with books and the library ever since I was a little kid. All of the librarians...','I have been obsessed with books and the library ever since I was a little kid. All of the librarians knew me by name and would go to great lengths to help me find new books I hadn’t read or gather my long list of special requests and order them in for me. 
Before the internet was the amazing search tool that it is, books and first-hand experience were the only ways to learn about something new. I was so curious about so many things and the library fed that curiosity. I was always apologizing to the librarians for having such a long request list, but they never made me feel bad about being so curious and always helped me on my quest to find as much information as possible on my latest obsession. 
 I definitely passed my love of reading on to our kids. When they were small we visited the library at least once a week and had to have two library cards so that we could check out as many books as we wanted. I can’t remember a time when we didn’t max out the limit of 100 books per library card! I was always trying to make a deal with the librarian to sneak in a couple more. 
Even now, I am still just as obsessed with books, and any time I find a bookstore (which sadly is becoming so rare), I can’t leave empty-handed even though my home library is overflowing to the point of having books stacked on the floor and in every corner of the house. 
 I have always had such gratitude for people who take the time to share their wisdom, experience, and gifts with the world through books. But it wasn’t until I became an author myself that I realized what an undertaking it is to make a book. Now every time someone I follow or admire writes a book I pre-order multiple copies and try to spread the word in every way that I can. 
My personal library is a bit out of control and I know not everyone has the appetite or space to collect books like I do so I thought I would try and narrow down my very favorite books by category if you’re looking for some new titles to add to your own collection. 
 Top books for gifting
If you’re looking for a great book to give as a gift, these are my go-to’s. I bet I’ve gifted two dozen copies of each this year alone. Each one of these titles has such rich storytelling, stunning photography, and something to offer for everyone (activities, recipes, and inspiration) no matter their experience level. 
An American in Provence: Art, Life and Photography by Jamie Beck
I first started following Jamie Beck on Instagram after discovering her through a mutual flower friend and have been completely obsessed with her work ever since. For those of you not familiar, Jamie Beck is a photographer and visual artist who lives and works in Provence. In 2016, Jamie left her busy life in New York City for a one-year sabbatical in the south of France and never turned back. In her first book, An American in Provence, she documents this cultural experience and her journey of self-discovery—all through heartbreakingly beautiful photos, essays, and even French recipes. What I love most about Jamie’s work is that it has the ability to transport you into another world. It’s like falling down the most beautiful rabbit hole, getting lost in an entirely different universe. This book will change you—there’s no way around it and I can’t recommend it highly enough!
Life in the Studio: Inspiration and Lessons on Creativity by Frances Palmer
If you’re looking for some serious inspiration, this book is a must have! Frances Palmer is a renown East Coast potter who has lived an extraordinarily rich life and has grown a very intentional business centered around creativity. Frances opens up her life to readers and shares the most beautiful collection of essays, photographs, recipes, tutorials, and life lessons. Every single person I have given this book to has read it cover to cover in one sitting. It’s one of my top five favorite books and will leave you changed. In addition, Frances has just released two new jigsaw puzzles featuring her beautiful flower photographs which would make lovely gifts. Be sure to visit her website to see all of her incredible creations including her coveted handmade pottery, signed books, photo prints, and more.
Five Marys Ranch Raised Cookbook: Homegrown Recipes from Our Family to Yours by Mary Heffernan
I’ve been a fan of Mary Heffernan of Five Marys Farms for many years after discovering her on Instagram, and we’ve since become real-life friends. Mary, her husband, and their four daughters, all named Mary, have a ranch in northern California where they raise Black Angus cattle, Navajo Churro lambs, and Berkshire heritage pigs. They sell their pasture-raised meats and other farm-produced products direct-to-consumer. We’ve been Farm Club members since discovering Mary and her family and are always thrilled with the quality of their meat. Mary is an incredible businesswoman and a wealth of information. Mary released her first cookbook in 2020 and it has fast become one of the most used cookbooks on my shelf. All of the recipes we’ve made have been delicious and the stories and photography give this book so much depth and interest. If you want to learn more about Mary and her incredible business, you can read my past interview with her here. 
A Year Full of Flowers: Gardening for All Seasons by Sarah Raven
I credit Sarah Raven with inspiring me to start growing cut flowers nearly 20 years ago. I first discovered one of her books at my local library in the very early days of my flower farming journey. At the time, I was looking for any information I could find on selecting the best varieties for cutting, growing super long-stemmed blooms, and germinating a handful of difficult-to-grow flowers. From the first moment I opened her book, I was mesmerized. Sarah’s books, award-winning website, products, and videos continue to inspire me today. Sarah’s latest title might be my very favorite of all the books that she’s written. A Year Full of Flowers is filled with so much helpful information including Sarah’s favorite varieties across so many different plant categories and the photographs by Jonathan Buckley are out of this world. If you’re looking for gardening inspiration, this is the book to get. 
In Bloom: Growing, Harvesting, and Arranging Homegrown Flowers All Year Round by Clare Nolan
I had the pleasure of meeting Clare when we visited England a few years ago shortly after her book came out. I have been a fan of her work for so long and finally getting to connect in person was a dream come true. This book is one of my very favorites and I have gifted it to every single gardener in my life. In addition to covering everything you need to know about growing and enjoying homegrown flowers all year long, the abundance of beautiful photos will keep you glued to the pages from beginning to end. This book is a must add to your flower library!
The Complete Gardener: A Practical, Imaginative Guide to Every Aspect of Gardening by Monty Don 
Monty Don has such a gift for teaching and storytelling and I’ve looked to him as a mentor since I first started growing. He has authored 18 books on gardening and is probably the most well known and respected gardener in the world for good reason. The Complete Gardener is hands down, my all-time favorite gardening book ever! This book has been my go-to source of information and inspiration for nearly 20 years. One of the most comprehensive gardening books available, it’s packed with everything you need to know to garden organically, including composting, soil health, hedges, perennials, growing your own food … this book covers it all. In 2021, Monty released an extensively revised new edition that is even better than the original. 
 Floral design books
Arranging Flowers by Martha Stewart (Best of Martha Stewart Living Series)
An oldie, but a goodie! This book has stood the test of time. Organized seasonally, each page is filled with gorgeous photographs of flowers harvested from Martha’s garden at Turkey Hill. 
The Art of Wearable Flowers: Floral Rings, Bracelets, Earrings, Necklaces, and More by Susan McLeary
Sue is one of the most talented, innovative, and generous floral designers I’ve ever met. Her beautiful book is filled with so many helpful tutorials for creating wearable floral art. 
The Flower Hunter: Seasonal flowers inspired by nature and gathered from the garden by Lucy Hunter
This book just arrived in the mail and I dropped everything to sit down and pore over its pages. It is overflowing with gorgeous photography, wonderful step-by-step tutorials, and beautiful essays. 
Floret Farm’s A Year in Flowers: Designing Gorgeous Arrangements for Every Season by Erin Benzakein with Jill Jorgensen and Julie Chai
My second book teaches you how to create beautiful arrangements using flowers from your garden or those grown close to home any time of the year. To our shock and delight, A Year in Flowers became an instant New York Times best-selling book, which helped shine a spotlight on the local, seasonal flower movement and the incredibly hardworking people behind the blooms. 
Flowers for the Table: Arrangements and Bouquets for All Seasons by Ariella Chezar
Even though 15 years have passed since its publication date, this beautiful little book has inspired more florists than perhaps any other book on floral design, myself included. Ariella’s focus on fresh, seasonal blooms has helped make important changes in the flower industry to include more local and seasonal flowers and foliage.
The Flower Workshop: Lessons in Arranging Blooms, Branches, Fruits, and Foraged Materials by Ariella Chezar
Ariella’s second book is filled with heartbreakingly beautiful designs in a wide range of complex color palettes. It is Ariella’s exquisite approach to color that distinguishes her work from any other designer, and has influenced my own designs tremendously. 
Seasonal Flower Arranging: Fill Your Home with Blooms, Branches, and Foraged Material All Year Round by Ariella Chezar
Like everything she touches, this book by Ariella Chezar is pure magic. Her latest book offers even more inspiration for connecting more closely with nature through seasonal floral design.  
On Flowers: Lessons from an Accidental Florist by Amy Merrick
Amy approaches floral design with an artist’s lens, taking ordinary blooms and helping us see them in a completely unique way. Her book feels like an intimate look into the pages of her scrapbook, tracing her love of flowers through her career as a florist in New York City and beyond, as she travels the world. 
 Flower growing books
A Year Full of Flowers: Gardening for All Seasons by Sarah Raven
Sarah’s latest title might be my very favorite of all the books that she’s written. A Year Full of Flowers is filled with so much helpful information including Sarah’s favorite varieties across so many different plant categories and the photographs by Jonathan Buckley are out of this world. If you’re looking for gardening inspiration, this is the book to get. 
The Cutting Garden: Growing & Arranging Garden Flowers by Sarah Raven
This was the first book I ever discovered on growing cut flowers and has been a source of inspiration for nearly 20 years. I credit Sarah and this book for inspiring me to get my start in flowers. 
Grow Your Own Cut Flowers by Sarah Raven
If I had to pick, this is probably my favorite book on growing flowers and I’ve referenced it so many times over the years that the spine is broken and almost every page is smudged with dirt. I love the way that this book is organized because Sarah crammed so much helpful information into the pages, but in a really easy to understand and reference way. While out of print now, if you can find a used copy, it’s worth whatever you have to pay.  
Dahlia Breeding for the Farmer-Florist and the Home Gardener: A Step by Step Guide to Hybridizing New Dahlia Varieties from Seed by Kristine Albrecht
I’ve been a fan of Kristine’s incredible dahlias for many years and have sought her advice on breeding my own new varieties many times. If you want to try your hand at dahlia breeding and learn from the best, this wonderful little book has everything you need. 
Floret Farm’s Cut Flower Garden: Grow, Harvest & Arrange Stunning Seasonal Blooms by Erin Benzakein with Julie Chai 
My first book covers everything you need to know about growing flowers on a small scale and is the perfect jumping-off point for beginning gardeners. It includes detailed growing instructions for more than 175 different flower varieties and is overflowing with so many beautiful photos. Cut Flower Garden won the American Horticultural Society book award, which was such an incredible honor.
Floret Farm’s Discovering Dahlias: A Guide to Growing and Arranging Magnificent Blooms by Erin Benzakein with Jill Jorgensen and Julie Chai 
This book does a deep dive into one of the most beloved cut flowers—dahlias. In addition to sharing all of my secrets to successfully growing dahlias, this book also features 360 of my very favorite varieties organized by color and also includes a chapter on breeding your own new dahlias, plus how to save your seed.
The Flower Farmer: An Organic Grower’s Guide to Raising and Selling Cut Flowers, 2nd edition, by Lynn Byczynski
There’s good reason this book is considered the bible for beginning flower farmers: it includes everything you need to know to become a flower farmer. If you are considering getting into the business, you’ll want to read this book–twice!
Grow and Gather: A Gardener’s Guide to a Year of Cut Flowers by Grace Alexander
This delightful new book is filled with reflective essays, journal entries, and growing advice. The beautiful photographs provide a window into Grace’s lovely world and offer inspiration for gardens of any size. 
In Bloom: Growing, Harvesting, and Arranging Homegrown Flowers All Year Round by Clare Nolan
This book is one of my very favorites and I have gifted it to every single gardener in my life. In addition to covering everything you need to know about growing and enjoying homegrown flowers all year long, the abundance of beautiful photos will keep you glued to the pages from beginning to end. This book is a must-add to your flower library!
 General gardening books
The Bold and Brilliant Garden by Sarah Raven 
I remember the first time I opened this book and was blown away by Sarah’s fearless use of color, combining bright, rich, saturated hues in ways I had never seen before. It inspired so many gardeners, myself included, to push the boundaries and really think outside the box when it comes to plant and color combinations.
The Complete Gardener: A Practical, Imaginative Guide to Every Aspect of Gardening by Monty Don 
Hands down, my all-time favorite gardening book ever! This book has been my go-to source of information and inspiration for nearly 20 years. One of the most beautiful and comprehensive gardening books available, it’s packed with everything you need to know to garden organically, including composting, soil health, hedges, perennials, growing your own food … this book covers it all. In 2021, Monty released an extensively revised new edition that is even better than the original. 
Martha Stewart’s Gardening: Month by Month by Martha Stewart 
While this book is nearly 30 years old, it has stood the test of time. This massive photo-filled resource takes you through an entire year in Martha’s garden and kitchen at Turkey Hill. Whenever I see copies at used bookstores I always grab them because they make the best gifts. If you’re looking for a book to get lost in over the winter as you plan out your garden, be sure to add this to your stack. 
Organic farming books 
On Good Land: The Autobiography of an Urban Farm by Michael Ableman and Alice Waters
I read this book shortly after Elora was born and I was trying to figure out what I wanted to do with my life. When I finished the last page I sat up in my chair and declared to Chris that we needed to leave the city as soon as possible and start a farm. This beautiful memoir tells the moving story of a small farm that is nearly swallowed up by development but becomes the heart of a community. 
The New Organic Grower: A Master’s Manual of Tools and Techniques for the Home and Market Gardener by Eliot Coleman
It is hard to pick just one of Eliot Coleman’s books because they are all fantastic, but I find myself referencing this incredible resource the most often. Eliot is the godfather of organic vegetable farming and his production techniques and generous sharing have revolutionized the way many small farms operate.  
The Market Gardener: A Successful Grower’s Handbook for Small-Scale Organic Farming by Jean-Martin Fortier
This brilliant book is a must-have for anyone interested in organic farming. Like Eliot Coleman’s books, this volume is focused on vegetable farming, but many of the principles are transferable to flowers. Both authors beautifully detail how it is possible to farm on a small scale without big tractors or other fancy equipment.
 Business & personal development books
The Fire Starter Sessions: A Soulful + Practical Guide to Creating Success on Your Own Terms by Danielle LaPorte 
Hands down, one of the best books I’ve ever read in this genre. Part sermon, part therapy, and part ass-kicking … this book will blow you out of the water and into action towards the life and business of your dreams. 
Turning Pro: Tap Your Inner Power and Create Your Life’s Work by Steven Pressfield 
More than any other book, this one changed my life the most. I remember when I finished the last page of the book and realized how much of my life I had been making excuses for not going after my dreams. The very next day I sat down and wrote the proposal for Cut Flower Garden. If you feel like you have something inside of you that you want to share with the world, this book will be the spark you need to take the next step. 
Start with Why: How Great Leaders Inspire Everyone to Take Action by Simon Sinek
This book really helped me shift my perspective around business and pursuing creative, meaningful work. Rather than focusing on what you do and how you do it, Simon challenges readers to get to the heart of why they do what they do and from there, everything else follows. 
StrengthsFinder 2.0 by Tom Rath 
I remember the first time I read this book I had such a breakthrough around focusing on my strengths rather than trying to change my weaknesses. The theory behind this book is that when we embrace the way that we are naturally wired and pour our energy into areas of strength we find more satisfaction in life and work. We’ve used this book to grow our team and it has been an absolute game-changer. 
 
In the comments below, I’d love to hear what some of your favorite books are and why you love them.
', '2023-07-25', 2, 2),

    ('Discovering Dried Flowers', '/images/blog/blog7.jpg','Only in recent years have I become a fan of dried flowers. I always used to turn up my nose at them because they reminded me of tacky, dated flower...', 'Only in recent years have I become a fan of dried flowers. I always used to turn up my nose at them because they reminded me of tacky, dated flower books from the late ‘80s.
Now I can’t believe it took me so long to discover their benefits. Drying flowers means you can preserve the abundance from your garden to be enjoyed later, when nothing is blooming. Back when we were finishing the winter chapter of A Year in Flowers, I realized how useful and versatile dried flowers really are.  
 If you use the right method, you can dry just about anything, and there are dozens of books on the subject lining the shelves of used bookstores and thrift stores. A couple of years ago I discovered a dated but incredibly helpful book, Flowers: Growing-Drying-Preserving, by Alan Cormack and David Carter, that goes into great detail on all the different varieties that you can dry, plus step-by-step instructions for how to do it, whether you’re air drying or using silica gel.
There are so many ways to use dried flowers, seed pods, and grasses: in late autumn arrangements, adorning fresh holiday wreaths, or even mixed with fresh blooms. I thought it would be helpful to share some of the varieties that are the easiest to grow and most popular for drying.
  Strawflowers (pictured above) are a traditional standby, but the gorgeous new colors and varieties make them seem entirely different from those ’80s flowers, and they actually look incredible when mixed with fresh blooms.

Start seed indoors in trays 6 weeks before your last frost. Seed requires light to germinate so do not cover. Bottom-water until seedlings emerge, and transplant out after all danger of frost has passed. For drying, you can cut them at the desired stage of openness, and they’ll hold in that stage.

  Statice, another standby, is one of the best flowers for drying and also wonderful when used fresh. Easy to grow and great for beginners, this versatile plant’s papery flowers bloom all summer long.

Start seeds indoors 6 weeks before last frost; transplant out when all danger of frost has passed. Harvest when all flowers on a stem have appeared. If picked too soon, stems will wilt. Fresh flowers have a 7 to 10 day vase life.
 Larkspur (second from right, above) is one of the easiest cut flowers to grow—cold-tolerant and early to bloom, it adds tall, colorful spikes to spring gardens.

Direct seed in late fall or early spring or start seed indoors in trays 6 to 8 weeks before your last frost, and plant out while weather is still cool. Plants do best when sown directly in the garden. Larkspur can be planted in fall in even the coldest corners of the world. Speed up germination by chilling seed in a refrigerator or freezer for a week before sowing.

To dry, let all but the top 3 to 4 blooms open, then pick and hang upside down in a warm, dry place out of bright light for 2 weeks.
 Grown for their unique textural blooms, celosias are vigorous and free-flowering. These easy-to-grow flowers come in a wide variety of shapes, colors, and forms, ranging from a crested cockscomb to spikey, plumed forms that are great accents for bouquets.
Flower heads get bigger over time, so pick when they are the size that you want, but before they go to seed. Celosias often last 2 weeks as fresh flowers. To dry them, hang freshly cut stems upside down in a warm dark place for 2 to 3 weeks or until they are firm to the touch.
 Globe Amaranth have adorable, button-like blooms that look great in bouquets. This late summer darling thrives in the heat and is hard-working in both the garden and in the vase.
Start seed indoors in trays 4 to 6 weeks before last frost; transplant out after all danger of frost has passed. Freshly harvested flowers can last up to 2 weeks in the vase, and dried flowers look nearly the same as fresh ones.
 Eucalyptus is a staple, much in demand by florists and for weddings. Its blue-green and silvery hues set off both cool and warm floral palettes, and everyone loves its distinctive methol fragrance. Our favorite is ‘Round-Leaved Mallee’, pictured above.
Eucalyptus can be grown as an annual from seed if started early. Sow seed on the surface of the soil and do not cover. Seeds are very slow to germinate and take 45 days to sprout, so be patient. Harvest once foliage is mature and tips are no longer droopy.
Cut fresh, eucalyptus is a long-lasting foliage—often 2 weeks in the vase. As a dried foliage, it’s a favorite in autumn wreaths.
 I discovered cress, a fantastic filler, almost a decade ago and have been a fan ever since. Just a few stems of these seedy treasures transform every bouquet. The tall, sturdy plants are smothered in beautiful silvery seed pods that aren’t prone to wilting or shattering.
Cress is extremely quick to germinate and produces a bumper crop in just 2 months. We direct sow it every 2 to 3 weeks from our last spring frost through early summer for a steady supply.
Harvest when the seed pods are fully formed and the top blooms have faded for a 7 to 10 day vase life. If you succession sow. you’ll have plenty to dry for autumn bouquets and wreaths. Stems dry easily; just hang them upside down in a warm, dry place.
 ‘Bunny Tails’ is an irresistible ornamental grass that’s as soft as a well-worn baby’s blanket. Compact plants produce graceful gray-green blades with elongated heads that turn a delicate cream color and soften as they age.
Harvest at any point once seed heads emerge. If you cut it fresh, expect a vase life of 7 days; no preservative needed.
To dry, wait until the pollen sheds, pick and hang upside down in a warm, dark place. Everyone who visits the farm loves this grass! It mixes well with everything and looks fantastic dried.
 Poppy pods have long been a favorite in mixed bouquets. They are easy to grow and make a wonderful addition to any garden.
Breadseed poppies produce large decorative seed pods that can be dried and used indefinitely. ‘Rattle Poppy’ pods are as large as limes! Breadseed poppies do best when direct sown, but slugs love them, so keep an eye out.
 Shirley Poppies (pictured above) yield a bumper crop of miniature silver pods with darker tops that are excellent for handwork, bridal bouquets, and dried crafts.
Direct sow into the garden after all danger of frost has passed. Shirley Poppies can be started indoors; just take care when planting out not to disturb the roots too much.
 Many other materials can be dried. You may want to experiment with lunaria, love-in-a mist (pictured above), hydrangea, and ornamental grasses.
  When we were starting out with drying, we kept things easy and simply hung the harvested bunches upside down in the back of the garage, where it gets really hot and dry in the summer. Standard advice is to dry flowers in a warm, dark place, but thankfully ours dried so quickly that their color didn’t fade in the sunlight.
After the bunches were completely dry, we wrapped them in pieces of kraft paper and stored them in plastic Rubbermaid bins until we were ready to use them.
I gave two huge boxes of dried goodies to my friend Nina who makes the sweetest little dried wreaths that she sells at craft fairs. I divided the rest among the team, and the ladies had fun crafting with them.
 What got me going on the dried flower bandwagon in the first place was my flower friend Siri Thorson.
Siri lives on one of the most remote San Juan islands and travels between her family’s farm and destinations worldwide arranging flowers. She makes the most stunning works of art from dried flowers that she grows on her family’s farm and ships nationwide around the holidays.
Pictured are some of Siri’s everlasting wreaths. Aren’t they amazing?
 
 Here are a few important things to keep in mind if you’re planning to dry flowers:
•	Flowers for drying should be picked more open than you would for fresh cuts, but make sure they’re not too ripe. I would suggest picking blooms when they are about three-quarters of the way open. If overly ripe, they will fall apart during the drying process.
•	After flowers are harvested, you’ll want to remove all of the foliage and leaves on the stem because they will turn brown and crispy when they dry.

•	Be sure to hang your bunches upside down while they are drying because the flower heads will be fixed in whatever position they were in when they dried. Hanging them upside down will ensure straight, usable stems.
•	Handle dried flowers with care because they are quite fragile and can break easily. If you aren’t going to use them right away, you can wrap them in tissue or kraft paper and store them away until needed.
Even if you only save aside a few bunches of flowers for drying, I’d highly recommend that you give it a try. You’ll be glad to have them on hand during the lean winter months.
 I’d love to hear what you think about dried flowers. Are you drying any flowers from your garden this season, or do you think of them as tacky and dated? If you’re a dried flower fan, I’d love to know your favorite varieties for drying and any resources that you’d recommend for beginners.
', '2023-07-27', 1, 1),
    ('Workwear for Women','/images/blog/blog8.jpg','Whenever I’m curious about something and want to learn more I end up conducting some type of trial. Being able to compare all...' ,'Whenever I’m curious about something and want to learn more I end up conducting some type of trial. Being able to compare all of the options that are available side by side is one of my favorite ways to learn.
Over the years, I have trialed so many different things, from thousands of different varieties of plants to tools, vases, rain gear, shoes, pens, notebooks, and bizarrely, even salad dressing!
 This past season, my trialing obsession grew to include women’s workwear. Finding workwear for women that’s durable, comfortable, flattering, made to actually work in, and that fits different body types has proven to be quite the challenge. I have pretty much tested every single brand on the market now and I have a lot of thoughts to share.  
While writing an entire article about clothing feels a little silly, workwear is expensive and it can be time-consuming to figure out what style or brand will fit your own unique set of needs, especially if you don’t fall into the standard size options that are most commonly available. 
 There are a lot of really cute styles out there these days, but when put to the test, I have found that many fall short. Below you will find my review of the best brands I’ve discovered when it comes to quality workwear for women, including the things that I like about each company and also a few criticisms.
Jill and Nina shared their favorites as well, so there should be something here for everyone.
 Dovetail Workwear
Of all the brands that I’ve tried, Dovetail is hands-down my favorite. I love that this female-founded and run company has put so much effort into creating legitimate workwear for women across many different industries, including farming, landscaping, construction, welding … and on the “badassery” scale their clothes are 10/10. 
 Their clothing is available at most places that offer workwear, but if you need special sizing, you’ll have to order it through their online store (their return/exchange process is very user-friendly). They offer a wide range of inseam lengths for both overalls and work pants depending on your height. They also have some really nice flannels and lightweight work jackets. 
Overall, Dovetail clothing is a little more fitted in nature. Their overalls are tighter through the waist and their pants have a slimmer silhouette. They fit my body type really well, but both Jill and Nina find their pants and overalls too tight through the middle. If you don’t like form-fitting clothing or need a bit more room, then this brand might not be a good fit. 
 Freshley Overalls in Grey Thermal Denim (pictured above) are my go-to overalls whenever the weather is cool. I tend to run cold and during cooler weather I typically wear long johns under my pants, but these insulated overalls eliminate the need for an extra layer and they are so comfortable and stretchy. It feels like you’re wearing your pj’s out in the field!
 Freshley Overalls in Saddle Brown Canvas (pictured above) are the lightest weight Dovetail overalls that I’ve tried and are my go-to when working in the greenhouses and field when the weather is warm.
They are moderately stretchy so are great for crawling around on the ground harvesting seed crops, weeding, and scouting for pests. 
 I also have the Freshley Overalls in Natural Canvas (pictured above). This fabric is much thicker and doesn’t have any stretch, so I would say that if you’re going to be moving around a lot in them I would size up. They hold their shape really well and are a beautiful earthy green-brown. 
In addition to overalls, Dovetail has some great work pants. What I love about them, in addition to them being comfortable, durable, and flattering, is that they don’t slide down when you bend over, which is a huge plus.
 I love the Britt Utility in Grey Thermal Denim (pictured above, left) in cooler weather. These fleece-lined work pants are super stretchy and cozy. 
After Carhartt changed their double-front jean design, I’ve been on the hunt for a replacement. The Britt Utility Reinforced Indigo Denim work pants (pictured above, right) are a great alternative and so far I’ve been really happy with them. They don’t have any stretch, which is the one downside in my book because it seems like I’m always crawling around on the ground and can use a little extra give. 
 The Givens Work Shirt in Indigo Stripe (pictured above, left) is a medium-weight work shirt that is perfect for wearing every day in the garden. The first time I put it on it felt like it was already broken in and it actually has enough room through the shoulders without looking boxy. 
I recently bought the Givens Work Shirt Stretch Flannel in Vintage Blue (pictured above, right) and plan to get all of the other colors. It’s super comfortable, stretchy, fits through the shoulders (which is always a challenge for me), and I really like the color combination. 
 I got the Eli Chore Coat in Paprika (pictured above) a few years ago and pretty much have lived in it ever since. This medium-weight waterproof jacket is so comfortable. The shoulders are wide enough to move in and the fabric has a good amount of stretch to it.
I normally wear a down vest underneath it to add a bit of extra warmth, but it is my everyday garden jacket. 
 The Kent Chore Coat (pictured above) is a mid-weight flannel-lined work jacket. It has nice deep pockets, comes with a detachable hood, and is really comfortable if you’re doing work that requires a lot of movement. I haven’t had mine for very long but so far I’m a huge fan. 
If you order from Dovetail, you can get $10 off your order by using the code FLORETFLOWER at checkout. 
 Carhartt
I have tried out pretty much every piece of Carhartt work clothing that they make and while I appreciate how much energy they’ve devoted to expanding their range of women’s wear, unfortunately, I’ve found that many of their new designs fall short when put to the test on the farm.
I also don’t love how logo-heavy they’ve gotten when it comes to their sweatshirts. 
 Of all the things they offer, I keep going back to their overalls, both for comfort and durability. While you can’t usually find special sizing available in stores, many of their styles come in both short and tall lengths, and they offer a great range of plus sizes online.
Jill loves their Women’s Utility Leggings because they’re stretchy, but her favorite part is that they don’t slide down when she’s crouching or bending over taking notes in the field. 
 Carhartt is probably best known for its duck canvas fabric—it’s what made them so famous. This material is long lasting and durable, but the downside is that it takes forever to break in. I have two pairs of their heavy-duty duck overalls (that seem to be discontinued) that are finally at that soft, cozy, broken-in stage, and I wear them all the time.
Overall, their fit is very loose and kind of boxy, which is great if you like to wear long johns or flannel underneath or need more room through the middle.
 Last summer, I got a pair of the Women’s Rugged Flex® Loose Fit Canvas Bib Overalls in Natural (pictured above) and they are awesome. The fabric is lightweight, breathable, and stretchy, and while it seems crazy to wear white in the garden, they are actually perfect in hot weather because they reflect the sun. 
 Jill has a pair of Women’s Relaxed Fit Denim Bib Overalls in Railroad Stripe (pictured above) that she wears all the time. She loves that they are soft and roomy, and says she always gets compliments on them when she goes to the grocery store after work. 
 When Carhartt first released their Straight Fit Double-Front Jeans (pictured above), I ordered a bunch of pairs because I was so in love with how well they fit. Thankfully I stocked up because they changed the original design shortly after release and the newer versions don’t have the same great fit or fabric.
The reason I list them here is that I get so many questions and compliments online whenever I wear the original pairs. If you like a more fitted work jean, a good alternative is the Britt Utility Reinforced Indigo Denim work pants that I listed above.
 I have a number of different flannels from Carhartt and have been really happy with how well they fit and hold up with heavy use and frequent washing. If you have broad shoulders, they fit really well. Unfortunately, I don’t see any of them currently available. 
 Patagonia
When Becky Crowley came from England to help design the farm a few years ago, she got a few pieces of Patagonia’s new (at the time) workwear and couldn’t stop raving about it, so I decided to give it a try.
The company itself is such an inspiration and the way it conducts its business with such high ethical and environmental standards is impressive. They also have a great buyback program and will repair any of the pieces that you get from them indefinitely. 
 At this point, I’ve tried pretty much all of their workwear and have been really impressed with the quality, comfort, and durability. I’ll report back on their work coats once I give them a little more time in the garden. 
 I love their All Seasons Hemp Canvas Bib Overalls (pictured above) and wear them every day in the summer. The canvas part of the name is misleading because they actually feel like more of a heavy linen and are very loose and breathable.
They are my go-to overalls for hot weather, even when I’m roguing flowers in a 100°F-plus greenhouse. 
 If you’re lucky, you can find long sizes through their website, but they don’t always stock them, so it’s worth signing up to be notified when they are restocked if you are tall. 
Because I love their overalls so much, I thought their work pants would also be great, and while they are comfortable unless you get the ones where you can cinch the back, they slide down when you bend over. 
 Duluth Trading Company
I put a poll up on Instagram asking about people’s favorite workwear brands for the garden and Duluth, by far, had the most praise. The styles they offer fit women with curves and those who need a shorter inseam. I have ordered multiple pairs hoping to find the right style, but unfortunately, none of them have a long enough cut to fit my frame. 
Jill and Nina are both huge fans of Duluth’s overalls and they each have multiple pairs. 
 Jill loves the Heirloom Gardening Bib Overalls and has them in Coal and Spice (pictured above, but not currently listed online). She loves that they are available in short sizes (she’s 5’ 3”) and that the material is breathable and dries quickly if it gets wet. The fabric also has a nice stretch to it that is great for crawling around on the ground, which she frequently does! She wore them every day last summer and so far they’ve shown no signs of wear.
Her only complaint is that the material (while it has its many benefits) is very loud and swooshy when she walks—I can hear her coming from a mile away!
 Nina loves the Rootstock Gardening Overalls in Railroad Stripe (pictured above) because the material is nice and stretchy, which makes them extra comfy, and the double layer of fabric over the knees gives them extra reinforcement. She’s also a big fan of all the pockets.
 KSX ART by Kellie Swanson
A few years ago I discovered Kellie Swanson, a Montana-based artist who specializes in applying cyanotype photography to clothing. In addition to offering beautifully printed upcycled overalls, jackets, and workwear, Kellie also takes a limited number of custom orders each season. While the wait is a little long, you can send her your favorite jacket, overalls, or shirt and she can personalize it for you. 
 I sent her a pair of my Carhartt Women’s Rugged Flex® Loose Fit Canvas Bib Overalls in Natural (pictured above) and Dovetail Freshley Overalls in Heather Black Denim that she tricked out for me.
The only downside is that whenever I wear them out in public so many people come up to me asking where I got them. While I love sharing about Kellie’s amazing work, it’s a little terrifying for an introvert like me. Haha!
To be the first to know when her new collections drop or when she has an opening for custom orders, be sure to sign up for her newsletter on her site. 
Grundéns
Here in Washington we get a lot of rain and are often working in wet, muddy weather, so having high-quality rain gear on hand is a must. I have tried all the brands and all the styles and keep coming back to Grundéns. 
This brand is known in the fishing industry as the gold standard, but many farmers are fans as well, including our crew. 
 I wear the Men’s Neptune 509 Bibs and Men’s Neptune 319 Commercial Fishing Jacket (both pictured above) in a size small and love that they are lightweight, durable, and really easy to work in. I have to order men’s sizing for the extra length.
Unfortunately, as far as I know, their women’s line doesn’t have a good tall option (yet).
 
 If you order from Grundéns, you can get 20% off your order by using the code FloretFlowers20 at checkout. 
 Land’s End
I know it seems strange to list this brand under workwear, but I have yet to find any other company that makes such comfortable, well-fitting, durable jackets that are also affordable. They always have some kind of great sale running on their website and you can find really good deals. 
One of my favorite things is that they also offer tall sizing, which I need for arm length, and a longer cut that is both flattering and offers extra coverage.
 Over the years I have seriously put these jackets to the test! I’ve had my black down coat from them since Jasper was a baby and I’m still wearing it in the garden. Mistakenly, I wore it to prune roses too many times, so my mom has added a whole collection of darling patches which makes me love it even more. 
 None of the styles that I have are still available but they have lots of other great options to choose from.
Jill used to tease me about wearing my Land’s End coats in the garden and now she’s singing their praises, too.

 Footwear
I wasn’t planning to include shoes and boots in this review but I get so many questions about my favorites. Again, I have tried just about everything on the market and keep coming back to just a handful of brands. 
 Here in Washington, having insulated boots is a must if you don’t want cold feet. I get a fresh pair of Bogs Neo-classic Mid boots (pictured above) every fall and they last me almost 12 months to the day before cracking along the seam. I put these boots to the test, walking miles every day and no other brand has been able to keep up with that level of wear. Given their amount of use, I am really happy with how long a pair holds up.
My favorite part about these boots is that you can slip them on and off without having to reach down and use your hands because my arms are always full going in and out of the house. They have lots of other cuter styles, but none of those easily slip on and off. 
 It pains me to write this because these shoes are so incredibly unattractive, but here on the farm once the weather warms up, there’s nothing more comfortable than a good ol’ pair of Keens, Kacie III Slip-Ons (pictured above) to be exact. But what they lack in style they make up for in durability and comfort.
They slip on and off really easily, especially when your hands are full, and if you have flat feet (Jill) or long toes (me), they’ll fit you great! 
 Farmer-Florist Tool Belt
My farmer-florist tool belt is hands down my all-time favorite tool for gardening and flower farming. I’ve been wearing the original prototype of this belt every day, in every kind of weather, for more than 8 years now and it’s still going strong. This handcrafted tool belt, custom made for us by leatherworker Wheeler Munroe, has been a total game changer. 
After years of tearing holes in the back pockets of every pair of pants and misplacing phones, pens, and flower snips throughout the day, this tool belt changed everything for me.
 We offer five different colors (brown, black, rosewood, wheat, and gray) in the Floret Shop. We also have left-handed styles in both black and brown. 
 Scarves
And last but not least, one of my all-time favorite companies, Block Shop Textiles. Sisters Lily and Hopie Stockman started their business back in 2013 with a small collection of scarves made in Jaipur, India with the goal of supporting and celebrating the Indian hand block printing tradition. 
Their business has grown tremendously and now works with a number of small, family-owned workshops and studios based in India, Italy, and the U.S. 
While technically not workwear, their beautiful cotton/silk scarves are lightweight but surprisingly warm. I’ve accumulated a pretty sizable collection over the years and love every single one. They make the perfect birthday present or gift to yourself. 
 It has been really encouraging to see so many companies devoting time and energy to developing quality workwear for women in recent years. I’m hopeful that this trend will continue into the future and that even more options will be available for our shapes, sizes, and body types. 
The good folks at Dovetail, Carhartt, and Grundéns have generously offered to sponsor an awesome giveaway for five Floret readers. Each winner will receive three gift certificates—one to Dovetail ($150), one to Carhartt ($100), and one to Grundéns ($150). 
To enter to win one of five of these gift certificate bundles, please tell us about a favorite piece of workwear that you’ve owned or a company you think we should know about. This giveaway is open to U.S. residents only and winners will be announced on April 25.
Update: Congratulations to our winners, Heather, D. Hamilton, Lisa Mabry, Margaret LaPlant, and Sandy!
', '2023-07-28', 2, 2),

    ('The Story of the Floret Workshops Part 1', '/images/blog/blog9.jpg','I started growing flowers more than 15 years ago in my backyard when my kids were still really little. At that time social media...', 'I started growing flowers more than 15 years ago in my backyard when my kids were still really little. At that time social media wasn’t even a thing yet and blogging was just starting to gain popularity.
Flower growing was such an obscure topic and there was very little information available for me to reference. Lynn Byczynski’s book The Flower Farmer and Sarah Raven’s book The Cutting Garden were the only resources that I had to go off of.
  Not long into my flower growing journey, I joined the Association of Specialty Cut Flower Growers (ASCFG) in hopes of learning more about flower farming. I was welcomed into a generous community of like-minded growers, which was the first time I felt like I had belonged anywhere in my adult life.
The ASCFG had a members chat room and I spent countless hours reading every past conversation thread, asking a million questions, and sharing any information or experience I had (which was very little at that time) on a given topic. I was what you would call a “power user,” and I’m sure I annoyed the heck out of the more experienced growers with all of my newbie questions, but for some reason they took pity on me and answered every one of them over time.
 My passion and curiosity quickly eclipsed the resources that were available, so I started looking around for other ways to learn more about growing unique flower varieties on a small scale without chemicals.
I started applying for growers grants in order to get funding to conduct variety trials. One of the grant requirements was that I would share my findings with others in my industry and that’s where I got my start with writing.
 Those trial reports eventually turned into articles for The Cut Flower Quarterly, then I got a job writing a monthly flower column in Growing for Market magazine, and then in an effort to practice writing, I also started this blog.
   I came to flowers and to writing with no formal training, just an insatiable curiosity and a love of sharing. I made a lot of mistakes in those early years but I also learned a ton, and over time built up a loyal following of readers both in print and online. Their encouragement spurred me forward to keep learning new things and sharing my findings.
Through the ASCFG, I met another young passionate farmer-florist who had a small urban flower farm in the heart of Philadelphia named Jennie Love. Jennie and I became fast friends and in an effort to keep our creativity alive while working incredibly long hours to build our flower businesses, we came up with a fun idea.
 Jennie and I dared each other to make a bouquet every week for the entire growing season using only locally-sourced flowers that we grew ourselves or sourced from local growers within a 60-mile radius. Living on opposite sides of the country in very different climates with very different tastes, we wanted to see what was happening in each other’s garden and keep creatively connected during our busiest season. Every week we posted a picture of our arrangements on our shared blog called The Seasonal Bouquet Project and listed the ingredients we used in our designs.
In addition to providing us with a creative outlet, we were trying to demonstrate that it was possible to use only local, seasonal product and inspire other growers and designers to think outside the box and work with what was available anytime of the year. At that time, this idea was completely unheard of. I don’t think either one of us knew how much of an impact that project would make, but by the end of the year we had built up quite the following and as a way of celebrating the project’s success, we decided to host a little class at Jennie’s farm to end the season on a high.
   Tickets for our first workshop sold out within a couple of hours and to meet the demand, we added two more dates.
It was such an inspiring and eye-opening experience to see just how much people were craving practical knowledge about growing flowers on a small scale and arranging seasonal blooms in a natural way.
After that experience, I came home and decided that I wanted to help as many growers as I possibly could by teaching them about what we had figured out on our small farm. And that’s how the idea for the Floret Workshop was born.
  The first few workshops we taught were pretty rough around the edges. We spruced up our dingy little garage and rented two dozen folding chairs and filled the days with as many hands-on demonstrations as we possibly could.
We had tiny spiral-bound notebooks printed which included my favorite plant lists, social media tips, and some simple step-by-step instructions for bending hoops, proper plant spacing, and succession planting.
 After each workshop, we would spend the next week debriefing how it went, how we could do better next time, where students still had questions, what we needed to expand upon…
The list of improvements went on forever and we revamped and upgraded after every single workshop until we eventually built a full curriculum and wrote a complete course book.
 Over the next five years, we welcomed more than 500 students to our tiny little farm for three-day intensive classes. In all, we hosted 24 on-farm workshops on small-scale flower growing and seasonally-based floral design.
The experience was life-changing for everyone involved. We watched students from all over the world go through our workshops and have huge breakthroughs in their lives and then go on to start hundreds of successful flower-based businesses.
For each workshop, we assembled the most amazing support team of experienced farmer-florists, many of whom still help us today. Having a chance to share their wisdom with so many newcomers was both rewarding and really meaningful.
   For me personally, the workshops pushed me so far out of my comfort zone (I’m an extreme introvert) and it felt like taking a crash course in leadership and public speaking.
In order to show students exactly how we built Floret from nothing into a thriving business, we opened up our farm and home so that they could see the full picture.
  Simultaneously we were working on developing a comprehensive curriculum, demonstrating all of the techniques that we used on our farm in a way that would accommodate different learning styles (visual, physical, logical, etc.) all while answering every question that was asked and adhering to a really tight schedule.
 Our small team managed to pull off the massive feat of planning, coordinating, and hosting these jam-packed events all while creating such a warm and safe environment where students could really let down their guard and dig into their dreams.
They managed to do this seamlessly each and every time no matter how crazy things were behind the scenes.
   We did all of this while continuing to run our wholesale flower business and arrange flowers for dozens of weddings each season.
When I look back at that time, I still don’t know how we managed to do it all. It was one of the hardest, most rewarding, and fulfilling times of my life.
   Each year when we would open up registration, tickets would sell out faster and faster until finally we filled seven workshops in under 2 minutes and crashed our website.
At that moment we realized that hosting workshops on the farm just wasn’t sustainable anymore.
People who had been on our waitlist for three or more years weren’t even able to secure a seat and we knew there had to be a better way.
 We spent the better part of the winter holed up in my dining room transitioning our in-person curriculum into an online program that we then filmed over the course of the next growing season.
The online format opened up a whole new world of possibility and allowed us to take students through an entire season here on the farm, getting to demonstrate all of the important techniques in detail, and giving students the ability to rewatch any of the videos they wanted whenever they needed a refresher.
It also removed a lot of barriers. We no longer had a limited class size, students didn’t have to travel, they could go through the program at their own pace, and we could expand and upgrade the material based on the needs of our students at any time.
 When we transitioned the workshop online we were so worried about losing the community and connection piece that was the heart of the on-farm experience. But what we found is that nothing could be farther from the truth.
The amazing farmer-florists that once assisted at our in-person workshops now moderate our online community forum and lend their advice and wisdom to students all over the world. And we’ve seen so many students form real-life friendships, set up dahlia tuber exchanges, host meetups, go in on plant orders together, and visit each other’s farms to learn and get inspired by one another.
   Now, 10 years later, our little farming course has evolved into a full curriculum that spans six foundational modules, includes more than 150 video tutorials, and has a 290-page printed course book, which we lovingly refer to as book number four.
 During the online workshop, Jill and I host weekly Q&A sessions and students have the opportunity to participate in the Floret Learning Community, which is an online forum moderated by some of the best flower farmers in the country.
We’ve poured so much of our hearts into this program, and of all the things we’ve ever made, it’s by far the one I’m most proud of. It has been so rewarding to have the opportunity to share what we’ve learned along the way with so many people around the world.
 
Thanks for joining me on this little walk down memory lane! In the second part of this blog series, I share more about the process of creating the Floret Online Workshop and why we designed it the way that we did. You can read part 2 here.
If you’d like to learn more about our next Floret Online Workshop, you can join the waitlist below.
', '2023-07-29', 1, 1),

    ('The Story of the Floret Workshops Part 2','/images/blog/blog10.jpg','As we’re preparing for our upcoming scholarship program and to welcome our next class into the fold, I wanted to share a little...', 'As we’re preparing for our upcoming scholarship program and to welcome our next class into the fold, I wanted to share a little bit more about the process of creating the course and why we designed it the way that we did.
In the first part of this blog series, I shared the story of how our workshops came to be. You can read it here. 
   I am a pen and paper kind of person and carry a notebook everywhere that I go. I can fill a 5-subject notebook in under a month just taking notes about what we’re working on day to day here on the farm.
There is something about holding paper in your hands that makes learning and understanding (at least for me) so much easier. I need to be able to see and touch things, and write down my thoughts as I go along.
  So when it came time to create the written portion of the course, I knew in my heart that it needed to become a book—one that we could give to students so they too could hold it in their hands.
The first year we taught our on-farm workshops the course book was a tiny black and white spiral-bound notebook that was maybe two dozen pages in total. At the time it seemed like such a big deal, but looking back I can’t help but laugh because it’s come so far since those early days. 
  This year’s course book came in at 290 beautiful colored pages filled with so many helpful how-to’s, step-by-step instructions, and resources that we’ve compiled over the years. It’s so big now that it needed an actual index, which technically makes it book number four.
In addition to the course book, we also created a sample cutting garden plan to demonstrate crop planning and succession planting on a small scale. The best part of this sample plan is that you can lay it out on the table and see such important concepts, which are normally very hard to understand, come to life right in front of your eyes.
   One of the most challenging parts when we were teaching on-farm workshops was trying to squeeze all of the information into three short days. So many of the farm tasks and chores that I wanted to share couldn’t be demonstrated in person because of their seasonal nature. 
What I love most about moving our workshop to an online format is that it has allowed us to share an entire year on the farm—from planning in the winter months to sowing seeds and planting in the spring to caring for all the flowers and harvesting in the summer, and then finally putting the garden to bed in the autumn.
 The course includes more than 150 videos broken down into short, bite-sized pieces so that students can go at their own pace and watch and re-watch the video tutorials as many times as they need to in order to master the material.
Our intention behind doing it this way was that we wanted to structure it like an apprenticeship with a very hands-on learning approach. I am such a hands-on learner and need to see things demonstrated before I feel comfortable enough to try them myself. 
The best part about this format is that we’re able to teach a wide variety of topics to students with varying skill levels and learning styles in a way that is easy to understand and implement in their lives and gardens, no matter how big or small.  
 Because the course includes so much information we intentionally broke it down into six core modules, each one building on the last. Modules are released weekly during the 6-week course, and then students have access to all of the material to reference any time they like from then on. 
The reason we don’t release all of the material at once is that we want students to have enough time and support to work through that week’s module before moving onto the next. It’s so easy to want to jump to the fun stuff like seed-starting or bouquet making and skip over the more challenging (but incredibly important) things like goal setting, planning, and marketing. 
 In Module 1, we spend a lot of time digging into the heart of why you want to grow flowers and then go through a series of exercises to refine your goals, highlight your strengths, and creatively work with what you have. 
In Module 2, we dive into planning and all that goes into setting yourself up for a successful season. This module is often the most challenging but students always say that it is the most helpful and important one in the course. 
Module 3 is all about getting off to a good start and we dig into seed starting, important supplies you’ll need, and then explore a wide range of different cut flower varieties and their specific needs. 
In Module 4, we wade into the nitty-gritty side of growing flowers, including soil preparation, how to manage weeds, plant spacing, irrigation, and growing in tunnels and greenhouses.  
Module 5 covers marketing, pricing, and the many different options you have when it comes to selling your flowers. Learning how to grow is only half of what it takes if you want a successful flower business—being able to sell them is just as important. I love this module because so many people who have struggled with this topic in the past (myself included) have huge breakthroughs going through this material. 
In Module 6, I share all of my tips and tricks for harvesting flowers efficiently and getting them to last as long as possible. It also covers efficient bouquet-making techniques, how to package and deliver flowers, and the most important things that you need to know when selling to florists, wholesalers, and grocery stores. 
 When we transitioned the workshop online we were so worried about losing the community and connection piece that was the heart of the on-farm experience. But what we found is that nothing could be farther from the truth.
The amazing farmer-florists that once assisted at our in-person workshops now help moderate the Floret Learning Community, an online forum for students in the course.
Our workshop “den moms” lend their wisdom and climate-specific advice to students all over the world. 
  Through the Learning Community, we’ve seen so many students form real-life friendships, set up dahlia tuber exchanges, host meetups, go in on plant orders together, and visit each other’s farms and gardens to learn and be inspired by one another.
 During the 6-week course, we also host a weekly Q&A session where Jill and I answer questions from students about that week’s module. These video sessions are always so much fun to film and we love being able to connect with students and cheer them on. 
At the end of the course each year we send out a survey and gather student feedback about the workshop that we then take into our annual audit so that the program continues to improve over time. 
   Students have lifetime access to the course and any new material that’s created in the future. Because of the online format, there’s no way to fall behind and students have the freedom to work their way through the material at their own pace. 
In the 10 years that we’ve been teaching the Floret workshop, both on-farm and online, we’ve welcomed students ranging in age from 16 to 76 from more than 50 different countries around the world.
 Our students have included stay-at-home parents, corporate professionals, farmers, brand new growers, retirees looking for a joy job, people working a standard 9-5, and everything in between.
We’ve also taught all types of organizations, including community gardens, schools, universities, prisons, veteran rehabilitation programs, and programs serving people with a wide range of abilities. 
 If I would have known way back when we taught our very first workshop in my backyard that it would have blossomed into this I never would have believed it. Being able to share all of the lessons I learned along the way with others has been the honor of a lifetime. I am so incredibly grateful to have had this opportunity. 
 
If you’d like to learn more about our next Floret Online Workshop, you can join the waitlist below.
', '2023-07-30', 2, 2),

('It’s Seed-starting Time!','/images/blog/blog11.jpg','Every year around mid February, I am ready for winter to be over and start longing to get my hands dirty and to dig in the soil again...' ,'Every year around mid February, I am ready for winter to be over and start longing to get my hands dirty and to dig in the soil again. While most of the field work is still several weeks away, there is plenty to do in preparation for the season ahead. At the top of the late winter to-do list: sow seeds.
Starting your own seeds is a great way to get a jump on the season. It also gives you access to hundreds of specialty flowers that you won’t find at your local nursery or big box store. Plus, it is the most affordable way to fill a garden fast.
 I start roughly 90 percent of my seeds inside the greenhouse. If you don’t have a greenhouse, don’t worry. A simple wire rack rigged with lights will work just fine. The first few years I grew flowers, I didn’t have a greenhouse and I started all of my seeds in the basement, on shelves, under lights. It was easy, inexpensive and a great way to grow lots of plants in a small space.
Starting seeds indoors allows me to transplant much larger, more established plants into the field once the weather has warmed. Larger plants have a better chance of contending with weed and insect pressure.
 I’ve learned a lot about seed starting over the years, mostly through killing a lot of baby plants. There’s nothing I hate more than seeing trays of beautiful little baby flowers go downhill before my eyes because I overwatered, underwatered, or got too excited about transplanting and didn’t properly harden them off.
Learning the hard way isn’t the most fun way to start seeds, so hopefully this post will help you avoid discouraging mistakes. The following is a quick list of tips meant to compliment other seed starting resources we’ve created.
 After filling your trays or containers with soil, be sure to lightly tap them on a hard surface so that the soil settles, eliminating any air pockets. Add any additional soil needed to fill all of the cells or containers to the top. 
For many years I used regular potting mix to cover the seeds in their trays. But I found that if the soil dried out it formed a crust on the top of the tray which inhibited growth and made it harder for the little seedlings to push up through the surface once they germinated. I switched to dusting newly sown trays with fine vermiculite a few years ago and haven’t had this issue since.
There are a handful of varieties such as ageratum, columbine, flowering tobacco and statice that require light to germinate. So when sowing, do not cover these seeds as it will inhibit sprouting.
 Hardy annuals such as bachelor’s buttons, bells of Ireland, bupleurum, larkspur, love-in-a-mist, orlaya and queen Anne’s lace can be tricky to germinate in the greenhouse, so pop seeds into the freezer for 10-14 days before sowing; then they will sprout readily. If planting outdoors, this step is not necessary, but it can help speed up germination.
Some varieties benefit from sowing multiple seeds per cell. They don’t need as much room to spread out on their own. These include bachelor’s buttons, bupleurum, cress, dill, flax, grains, grasses, gypsophila, larkspur, love-in-a-mist, breadseed poppies, saponaria, stock, tickseed and queen Anne’s lace.
 Be sure to moisten the seed starting mix before filling your trays or containers. If you plant your seeds into dry potting mix and then try to overhead water, you’ll end up washing away your little seeds. When I’m sowing a lot of seeds I open up a 3-4 bags of potting soil, get it nice and wet with the hose and let the soil fully wick up the water before I start filling trays.
Super tiny seeds such as Iceland poppies, snapdragons and foxglove are like dust and require special care to get started. After sowing seeds, I barely cover them with a fine dusting of vermiculite. Bottom water the seeds by setting your tray in a flat of standing water and let it wick up the moisture from below versus overhead watering. If you water from overhead you risk washing away the tiny seeds with the powerful spray. 
 In the rush to get growing, it is easy to want to get started sowing too early. It’s important to know just how early you can start for your area.Before you go crazy sowing seeds in late winter and early spring, it’s important to know just how early you can start—if in doubt, ask your local Master Gardener group or staff at a trusted nursery for the expected last frost date.
Fast-growing annuals that bloom in summer (such as cosmos, sunflowers, and zinnias) shouldn’t be started more than 4 to 6 weeks before the last spring frost, otherwise they’ll get too big for their growing container and have soft, weak foliage and overgrown roots by the time you can plant them out into the garden.
On the other hand, slow-growing plants like perennials can take a couple of weeks to germinate, so sow them indoors 10 to 12 weeks before the last spring frost date. Once you know your last frost date, check the back of each seed packet, or catalog description for days-to-harvest to figure out how soon you can start them indoors.
 It is amazing how much faster and how much better seeds germinate with a little added heat. Propagation mats work great for this. If you are a home gardener or small scale flower farmer you can get by with just one or two mats.
Leave your seed starting trays on the heat mat only until they germinate. Once sprouted, move the tray off the heat and make room for the next seed starting tray(s).
 Don’t seed more than one type of flower in the tray, especiallyif you plan to use a plastic dome lid. Germination rates vary by variety so it is best to have all the cells filled with the same flowers, that way you won’t be forced to remove the dome too soon for a row of early germinators or too late for those slow to germinate.
Plus, having variable plant heights in the same tray makes adjusting the height of the lights over the trays difficult. Shorter plants within the tray can get leggy when light is adjusted for the taller plants.
 It’s extremely important to label your seed trays immediately after sowing. Avoid the curse of the “mystery plants” by making sure to always write the name of the flower you are sowing and the date it was sown on the back of a waterproof plant tag.
I always stick the label in the same corner of every seed tray, so they line up uniformly.
 If you use plastic dome lids, be sure to remove them as soon as your seeds germinate. Domes are only needed to encourage germination but once seedling emerge they need fresh air and maximum light.
I like to have a fan running in the greenhouse to help with air circulation and the gentle breeze stimulates young plants, preventing spindly, weak growth.
 If you use grow lights, be sure to adjust them so that they are no more than three inches above the tops of your plants. When I was a newbie, this was not intuitive and as a result, I grew lots of gangly, leggy plants because they weren’t getting enough light. The bulbs were simply too far away from the foliage canopy to provide adequate light.
Once I realized my mistake, I adjusted the lights to about two inches above the top of the leaves. While this seems close, it is much better for the plant. Once I had the lights adjusted, I found that the plants grew so much better, with nice strong stems.
If you can, invest in automatic timers. If you are using lights to start your seeds indoors, you’ll want to invest in an inexpensive timer that will automatically turn on the light for a preset amount of time each day. This will help you avoid forgetting turning your lights on and off.  Plants need 14 to 16 hours  of light each day to grow.
 Be sure to “harden off” your plants before you transplant them. I am embarrassed to admit just how many plants I fried because I didn’t do this key step. In my excitement to transplant my baby plants into the field, I didn’t give them any chance to acclimate to their new outside environment. “Hardening off” is simply a process of allowing your plants time to gradually adjust to their new environment.
Think about it: your little plants have been in a warm and cozy, temperature-controlled environment for weeks, or months. If you suddenly take them from that space and expose them to bright sun, wind and temperature swings in the open garden, it is stressful to the plant.
 You will likely have leftover seed after sowing which can be saved for future use. Be sure to store your seeds in a cool, dark and dry place where no insects or rodents can get to them. Though germination rates will decrease over time, most seeds will maintain their viability for up to two years. 
 On your seed starting journey you will inevitably make mistakes and kill some plants. But cut yourself some slack. Just know that mistakes are inevitable. That is part of the joy in gardening is learning what systems work well for your situation, growing system and your climate.
 Starting your own seeds can be intimidating for new gardeners, but once you get the hang of it there’s nothing to fear and it can be great fun.
In addition to some of the tips I’m sharing today, I want to make sure you know about the following Floret resources:
– My recent book, Floret Farm’s Cut Flower Garden: Grow, Harvest & Arrange Stunning Seasonal Blooms has detailed seed starting tips and tricks.
-In the Floret Resources section, I have created a little Starting Seeds 101 tutorial and photo essay (be sure to click the arrows to advance the images) with some of the basics.
-Here on the blog, you’ll find a past post covering Seed Starting Basics.
-In the Floret Shop, I’ve included sowing and growing instructions for dozens of my favorite flowers.
One of my goals here on the site is to provide you with the best information, to help you grow great flowers and hopefully dispel the notion that success is only possible for professionals. You can do it!
I’d love to hear any tips or tricks that you swear by when it comes to seed starting. I always love to learn new things.

', '2023-07-31', 2, 2);
GO
