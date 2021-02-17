USE [OnlineDevDb]
GO
/****** Object:  Table [dbo].[AdminLog]    Script Date: 18.02.2021 00:02:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminLog](
	[AdminLogID] [int] IDENTITY(1,1) NOT NULL,
	[ActionName] [nvarchar](50) NULL,
	[ControllerName] [nvarchar](50) NULL,
	[IPAddress] [nvarchar](20) NULL,
	[Url] [nvarchar](250) NULL,
	[AdminID] [int] NULL,
	[ExecutionMs] [nvarchar](10) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_AdminLog] PRIMARY KEY CLUSTERED 
(
	[AdminLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AdminTBL]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminTBL](
	[AdminID] [int] IDENTITY(1,1) NOT NULL,
	[KullaniciAdi] [nvarchar](500) NULL,
	[Sifre] [nvarchar](500) NULL,
 CONSTRAINT [PK_AdminTBL] PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AtananDersler]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AtananDersler](
	[AtananDersID] [int] IDENTITY(1,1) NOT NULL,
	[OgretmenID] [int] NULL,
	[DersID] [int] NULL,
	[Sinif] [int] NULL,
	[Sube] [nvarchar](50) NULL,
	[DonemID] [int] NULL,
	[AktifMi] [bit] NULL,
 CONSTRAINT [PK_AtananDersler] PRIMARY KEY CLUSTERED 
(
	[AtananDersID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DersGruplari]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DersGruplari](
	[DersGrupID] [int] IDENTITY(1,1) NOT NULL,
	[DersGrupAdi] [nvarchar](50) NULL,
 CONSTRAINT [PK_DersGruplari] PRIMARY KEY CLUSTERED 
(
	[DersGrupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dersler]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dersler](
	[DersID] [int] IDENTITY(1,1) NOT NULL,
	[DersAdi] [nvarchar](250) NULL,
	[OkulID] [int] NULL,
	[AktifMi] [bit] NULL,
	[DonemID] [int] NULL,
	[SinifDuzey] [int] NULL,
 CONSTRAINT [PK_Dersler] PRIMARY KEY CLUSTERED 
(
	[DersID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Donemler]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Donemler](
	[DonemID] [int] IDENTITY(1,1) NOT NULL,
	[DonemAdi] [nvarchar](150) NULL,
	[AktifMi] [bit] NULL,
	[Baslama] [datetime] NULL,
	[Bitis] [datetime] NULL,
 CONSTRAINT [PK_Donemler] PRIMARY KEY CLUSTERED 
(
	[DonemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ilceler]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ilceler](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ilceadi] [nvarchar](255) NOT NULL,
	[sehirid] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[iller]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[iller](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sehiradi] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ogrenciler]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ogrenciler](
	[OgrenciID] [int] IDENTITY(1,1) NOT NULL,
	[OgrenciNo] [nvarchar](10) NULL,
	[Adi] [nvarchar](50) NULL,
	[Soyadi] [nvarchar](50) NULL,
	[Sinif] [int] NULL,
	[Sube] [nvarchar](50) NULL,
	[AktifMi] [bit] NULL,
	[OkulID] [int] NULL,
	[KayitTarihi] [datetime] NULL,
	[GuncellemeTarihi] [datetime] NULL,
	[DonemID] [int] NULL,
	[SilmeTarihi] [datetime] NULL,
 CONSTRAINT [PK_Ogrenciler] PRIMARY KEY CLUSTERED 
(
	[OgrenciID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ogretmenler]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ogretmenler](
	[OgretmenID] [int] IDENTITY(1,1) NOT NULL,
	[AdSoyad] [nvarchar](150) NULL,
	[KullaniciAdi] [nvarchar](50) NULL,
	[Sifre] [nvarchar](50) NULL,
	[AktifMi] [bit] NULL,
	[KayitTarihi] [datetime] NULL,
	[OkulID] [int] NULL,
	[DonemID] [int] NULL,
 CONSTRAINT [PK_Ogretmenler] PRIMARY KEY CLUSTERED 
(
	[OgretmenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OgretmenLog]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OgretmenLog](
	[OgretmenLogID] [int] IDENTITY(1,1) NOT NULL,
	[ActionName] [nvarchar](50) NULL,
	[ControllerName] [nvarchar](50) NULL,
	[IPAddress] [nvarchar](20) NULL,
	[Url] [nvarchar](250) NULL,
	[OgretmenID] [int] NULL,
	[ExecutionMs] [nvarchar](10) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_OgretmenLog] PRIMARY KEY CLUSTERED 
(
	[OgretmenLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Okullar]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Okullar](
	[OkulID] [int] IDENTITY(1,1) NOT NULL,
	[Adi] [nvarchar](500) NULL,
	[YetkiliAdSoyad] [nvarchar](150) NULL,
	[YetkiliTel] [nvarchar](15) NULL,
	[YetkiliEmail] [nvarchar](150) NULL,
	[ilID] [int] NULL,
	[ilceID] [int] NULL,
	[Adres] [text] NULL,
	[AktifMi] [bit] NULL,
	[KayitTarihi] [datetime] NULL,
	[BaslangicTarihi] [datetime] NULL,
	[BitisTarihi] [datetime] NULL,
	[KullaniciAdi] [nvarchar](50) NULL,
	[Sifre] [nvarchar](50) NULL,
	[OgrenciListeYuklediMi] [bit] NULL,
 CONSTRAINT [PK_Okullar] PRIMARY KEY CLUSTERED 
(
	[OkulID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OkulLog]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OkulLog](
	[OkulLogID] [int] IDENTITY(1,1) NOT NULL,
	[ActionName] [nvarchar](50) NULL,
	[ControllerName] [nvarchar](50) NULL,
	[IPAddress] [nvarchar](20) NULL,
	[Url] [nvarchar](250) NULL,
	[OkulID] [int] NULL,
	[ExecutionMs] [nvarchar](10) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_OkulLog] PRIMARY KEY CLUSTERED 
(
	[OkulLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Talepler]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Talepler](
	[TalepID] [int] IDENTITY(1,1) NOT NULL,
	[Tarih] [datetime] NULL,
	[AktifMi] [bit] NULL,
	[OkulID] [int] NULL,
	[Aciklama] [text] NULL,
 CONSTRAINT [PK_Talepler] PRIMARY KEY CLUSTERED 
(
	[TalepID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Yoklamalar]    Script Date: 18.02.2021 00:02:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Yoklamalar](
	[YoklamaID] [int] IDENTITY(1,1) NOT NULL,
	[OgretmenID] [int] NULL,
	[OgrenciIDListe] [nvarchar](max) NULL,
	[DersID] [int] NULL,
	[DersGrupID] [int] NULL,
	[Tarih] [datetime] NULL,
	[DonemID] [int] NULL,
	[AktifMi] [bit] NULL,
	[OkulID] [int] NULL,
	[Sinif] [int] NULL,
	[Sube] [nvarchar](50) NULL,
 CONSTRAINT [PK_Yoklamalar] PRIMARY KEY CLUSTERED 
(
	[YoklamaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[AdminLog]  WITH CHECK ADD  CONSTRAINT [FK_AdminLog_AdminTBL] FOREIGN KEY([AdminID])
REFERENCES [dbo].[AdminTBL] ([AdminID])
GO
ALTER TABLE [dbo].[AdminLog] CHECK CONSTRAINT [FK_AdminLog_AdminTBL]
GO
ALTER TABLE [dbo].[AtananDersler]  WITH CHECK ADD  CONSTRAINT [FK_AtananDersler_Dersler] FOREIGN KEY([DersID])
REFERENCES [dbo].[Dersler] ([DersID])
GO
ALTER TABLE [dbo].[AtananDersler] CHECK CONSTRAINT [FK_AtananDersler_Dersler]
GO
ALTER TABLE [dbo].[AtananDersler]  WITH CHECK ADD  CONSTRAINT [FK_AtananDersler_Donemler] FOREIGN KEY([DonemID])
REFERENCES [dbo].[Donemler] ([DonemID])
GO
ALTER TABLE [dbo].[AtananDersler] CHECK CONSTRAINT [FK_AtananDersler_Donemler]
GO
ALTER TABLE [dbo].[AtananDersler]  WITH CHECK ADD  CONSTRAINT [FK_AtananDersler_Ogretmenler] FOREIGN KEY([OgretmenID])
REFERENCES [dbo].[Ogretmenler] ([OgretmenID])
GO
ALTER TABLE [dbo].[AtananDersler] CHECK CONSTRAINT [FK_AtananDersler_Ogretmenler]
GO
ALTER TABLE [dbo].[Dersler]  WITH CHECK ADD  CONSTRAINT [FK_Dersler_Donemler] FOREIGN KEY([DonemID])
REFERENCES [dbo].[Donemler] ([DonemID])
GO
ALTER TABLE [dbo].[Dersler] CHECK CONSTRAINT [FK_Dersler_Donemler]
GO
ALTER TABLE [dbo].[Dersler]  WITH CHECK ADD  CONSTRAINT [FK_Dersler_Okullar] FOREIGN KEY([OkulID])
REFERENCES [dbo].[Okullar] ([OkulID])
GO
ALTER TABLE [dbo].[Dersler] CHECK CONSTRAINT [FK_Dersler_Okullar]
GO
ALTER TABLE [dbo].[ilceler]  WITH CHECK ADD  CONSTRAINT [FK_ilceler_iller] FOREIGN KEY([sehirid])
REFERENCES [dbo].[iller] ([id])
GO
ALTER TABLE [dbo].[ilceler] CHECK CONSTRAINT [FK_ilceler_iller]
GO
ALTER TABLE [dbo].[Ogrenciler]  WITH CHECK ADD  CONSTRAINT [FK_Ogrenciler_Donemler] FOREIGN KEY([DonemID])
REFERENCES [dbo].[Donemler] ([DonemID])
GO
ALTER TABLE [dbo].[Ogrenciler] CHECK CONSTRAINT [FK_Ogrenciler_Donemler]
GO
ALTER TABLE [dbo].[Ogrenciler]  WITH CHECK ADD  CONSTRAINT [FK_Ogrenciler_Okullar] FOREIGN KEY([OkulID])
REFERENCES [dbo].[Okullar] ([OkulID])
GO
ALTER TABLE [dbo].[Ogrenciler] CHECK CONSTRAINT [FK_Ogrenciler_Okullar]
GO
ALTER TABLE [dbo].[Ogretmenler]  WITH CHECK ADD  CONSTRAINT [FK_Ogretmenler_Donemler] FOREIGN KEY([DonemID])
REFERENCES [dbo].[Donemler] ([DonemID])
GO
ALTER TABLE [dbo].[Ogretmenler] CHECK CONSTRAINT [FK_Ogretmenler_Donemler]
GO
ALTER TABLE [dbo].[Ogretmenler]  WITH CHECK ADD  CONSTRAINT [FK_Ogretmenler_Okullar] FOREIGN KEY([OkulID])
REFERENCES [dbo].[Okullar] ([OkulID])
GO
ALTER TABLE [dbo].[Ogretmenler] CHECK CONSTRAINT [FK_Ogretmenler_Okullar]
GO
ALTER TABLE [dbo].[OgretmenLog]  WITH CHECK ADD  CONSTRAINT [FK_OgretmenLog_Ogretmenler] FOREIGN KEY([OgretmenID])
REFERENCES [dbo].[Ogretmenler] ([OgretmenID])
GO
ALTER TABLE [dbo].[OgretmenLog] CHECK CONSTRAINT [FK_OgretmenLog_Ogretmenler]
GO
ALTER TABLE [dbo].[Okullar]  WITH CHECK ADD  CONSTRAINT [FK_Okullar_ilceler] FOREIGN KEY([ilceID])
REFERENCES [dbo].[ilceler] ([id])
GO
ALTER TABLE [dbo].[Okullar] CHECK CONSTRAINT [FK_Okullar_ilceler]
GO
ALTER TABLE [dbo].[Okullar]  WITH CHECK ADD  CONSTRAINT [FK_Okullar_iller] FOREIGN KEY([ilID])
REFERENCES [dbo].[iller] ([id])
GO
ALTER TABLE [dbo].[Okullar] CHECK CONSTRAINT [FK_Okullar_iller]
GO
ALTER TABLE [dbo].[OkulLog]  WITH CHECK ADD  CONSTRAINT [FK_OkulLog_Okullar] FOREIGN KEY([OkulID])
REFERENCES [dbo].[Okullar] ([OkulID])
GO
ALTER TABLE [dbo].[OkulLog] CHECK CONSTRAINT [FK_OkulLog_Okullar]
GO
ALTER TABLE [dbo].[Talepler]  WITH CHECK ADD  CONSTRAINT [FK_Talepler_Okullar] FOREIGN KEY([OkulID])
REFERENCES [dbo].[Okullar] ([OkulID])
GO
ALTER TABLE [dbo].[Talepler] CHECK CONSTRAINT [FK_Talepler_Okullar]
GO
ALTER TABLE [dbo].[Yoklamalar]  WITH CHECK ADD  CONSTRAINT [FK_Yoklamalar_DersGruplari] FOREIGN KEY([DersGrupID])
REFERENCES [dbo].[DersGruplari] ([DersGrupID])
GO
ALTER TABLE [dbo].[Yoklamalar] CHECK CONSTRAINT [FK_Yoklamalar_DersGruplari]
GO
ALTER TABLE [dbo].[Yoklamalar]  WITH CHECK ADD  CONSTRAINT [FK_Yoklamalar_Dersler] FOREIGN KEY([DersID])
REFERENCES [dbo].[Dersler] ([DersID])
GO
ALTER TABLE [dbo].[Yoklamalar] CHECK CONSTRAINT [FK_Yoklamalar_Dersler]
GO
ALTER TABLE [dbo].[Yoklamalar]  WITH CHECK ADD  CONSTRAINT [FK_Yoklamalar_Donemler] FOREIGN KEY([DonemID])
REFERENCES [dbo].[Donemler] ([DonemID])
GO
ALTER TABLE [dbo].[Yoklamalar] CHECK CONSTRAINT [FK_Yoklamalar_Donemler]
GO
ALTER TABLE [dbo].[Yoklamalar]  WITH CHECK ADD  CONSTRAINT [FK_Yoklamalar_Ogretmenler] FOREIGN KEY([OgretmenID])
REFERENCES [dbo].[Ogretmenler] ([OgretmenID])
GO
ALTER TABLE [dbo].[Yoklamalar] CHECK CONSTRAINT [FK_Yoklamalar_Ogretmenler]
GO
ALTER TABLE [dbo].[Yoklamalar]  WITH CHECK ADD  CONSTRAINT [FK_Yoklamalar_Okullar] FOREIGN KEY([OkulID])
REFERENCES [dbo].[Okullar] ([OkulID])
GO
ALTER TABLE [dbo].[Yoklamalar] CHECK CONSTRAINT [FK_Yoklamalar_Okullar]
GO

