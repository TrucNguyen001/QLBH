USE [QuanLyBanHang]
GO
/****** Object:  Table [dbo].[CauHinh]    Script Date: 3/30/2024 7:52:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CauHinh](
	[CauHinhId] [nchar](36) NOT NULL,
	[ManHinh] [nvarchar](50) NULL,
	[DoPhanGiai] [nvarchar](50) NULL,
	[CongNgheManHinh] [nvarchar](50) NULL,
	[RAM] [nvarchar](50) NULL,
	[BoNho] [nvarchar](50) NULL,
	[ChatLieu] [nvarchar](50) NULL,
	[KichThuoc] [nvarchar](50) NULL,
	[TrongLuong] [nvarchar](50) NULL,
	[Camera] [nvarchar](200) NULL,
	[Pin] [nvarchar](200) NULL,
	[CPU] [nvarchar](200) NULL,
	[HinhAnh_CPU] [nchar](20) NULL,
	[HinhAnh_RAM] [nchar](20) NULL,
	[HinhAnh_Camera] [nchar](20) NULL,
	[HinhAnh_Pin] [nchar](20) NULL,
	[NoiDung_CPU] [nvarchar](1000) NULL,
	[NoiDung_RAM] [nvarchar](1000) NULL,
	[NoiDung_Camera] [nvarchar](1000) NULL,
	[NoiDung_Pin] [nvarchar](1000) NULL,
	[Chip] [nvarchar](50) NULL,
	[SanPhamId] [nchar](36) NOT NULL,
 CONSTRAINT [PK_CauHinh] PRIMARY KEY CLUSTERED 
(
	[CauHinhId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 3/30/2024 7:52:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[CommentId] [nchar](36) NOT NULL,
	[NoiDung] [nvarchar](200) NOT NULL,
	[NgayTao] [date] NOT NULL,
	[SanPhamId] [nchar](36) NOT NULL,
	[AccountId] [nchar](36) NOT NULL,
	[TrangThai] [int] NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonHang]    Script Date: 3/30/2024 7:52:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHang](
	[DonHangId] [nchar](36) NOT NULL,
	[MaDonHang] [char](50) NULL,
	[TenKhachHang] [nvarchar](50) NOT NULL,
	[DiaChi] [nvarchar](200) NOT NULL,
	[Email] [nchar](20) NULL,
	[SoDienThoai] [nchar](20) NOT NULL,
	[MaGiamGiaId] [nchar](36) NOT NULL,
	[AccountId] [nchar](36) NOT NULL,
	[TongTien] [decimal](18, 0) NULL,
	[NgayTao] [date] NULL,
	[TrangThai] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GiamGia]    Script Date: 3/30/2024 7:52:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GiamGia](
	[MaGiamGiaId] [nchar](36) NOT NULL,
	[Code] [nchar](50) NULL,
	[SoTienGiam] [decimal](18, 0) NULL,
	[SoLanNhap] [int] NULL,
	[SoLanDaNhap] [int] NULL,
	[NgayHetHan] [date] NULL,
	[DonHangToiThieu] [float] NULL,
	[TrangThai] [int] NULL,
	[NgayTao] [date] NULL,
	[NguoiTao] [nvarchar](50) NULL,
	[NgaySua] [date] NULL,
	[NguoiSua] [nvarchar](50) NULL,
 CONSTRAINT [PK_GiamGia] PRIMARY KEY CLUSTERED 
(
	[MaGiamGiaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GioHang]    Script Date: 3/30/2024 7:52:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GioHang](
	[GioHangId] [nchar](36) NOT NULL,
	[SanPhamId] [nchar](36) NULL,
	[AccountId] [nchar](36) NULL,
	[DonHangId] [nchar](36) NULL,
	[DonGiaBan] [decimal](18, 0) NULL,
	[SoLuongBan] [int] NULL,
	[TrangThai] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoaiSanPham]    Script Date: 3/30/2024 7:52:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiSanPham](
	[LoaiSanPhamId] [nchar](36) NOT NULL,
	[MaLoaiSanPham] [nchar](50) NOT NULL,
	[TenLoai] [varchar](50) NOT NULL,
	[NgayTao] [date] NULL,
	[NguoiTao] [varchar](100) NULL,
	[NgaySua] [date] NULL,
	[NguoiSua] [varchar](100) NULL,
 CONSTRAINT [PK_LoaiSanPham] PRIMARY KEY CLUSTERED 
(
	[LoaiSanPhamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhaCungCap]    Script Date: 3/30/2024 7:52:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhaCungCap](
	[NhaCungCapId] [char](36) NOT NULL,
	[MaNhaCungCap] [nchar](50) NOT NULL,
	[TenNhaCungCap] [nvarchar](50) NOT NULL,
	[DiaChi] [nvarchar](200) NOT NULL,
	[SoDienThoai] [nchar](50) NOT NULL,
	[NgayTao] [date] NULL,
	[NguoiTao] [nvarchar](50) NULL,
	[NgaySua] [date] NULL,
	[NguoiSua] [nvarchar](50) NULL,
 CONSTRAINT [PK_NhaCungCap] PRIMARY KEY CLUSTERED 
(
	[NhaCungCapId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhanHoi]    Script Date: 3/30/2024 7:52:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhanHoi](
	[PhanHoiId] [nchar](36) NOT NULL,
	[NoiDung] [nvarchar](200) NOT NULL,
	[CommentId] [nchar](36) NOT NULL,
	[AccountId] [nchar](36) NOT NULL,
	[NgayTao] [date] NULL,
	[NguoiTao] [nvarchar](50) NULL,
	[NgaySua] [date] NULL,
	[NguoiSua] [nvarchar](50) NULL,
 CONSTRAINT [PK_PhanHoi] PRIMARY KEY CLUSTERED 
(
	[PhanHoiId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanPham]    Script Date: 3/30/2024 7:52:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPham](
	[SanPhamId] [nchar](36) NOT NULL,
	[MaSanPham] [nchar](50) NOT NULL,
	[LoaiSanPham] [int] NOT NULL,
	[TenSanPham] [nvarchar](200) NOT NULL,
	[Avatar] [nchar](20) NOT NULL,
	[HinhAnh] [nchar](100) NOT NULL,
	[MoTa] [nvarchar](2000) NOT NULL,
	[SoLuong] [int] NOT NULL,
	[SoLuongMua] [int] NOT NULL,
	[Gia] [float] NOT NULL,
	[GiaBan] [float] NOT NULL,
	[GiaGiam] [float] NOT NULL,
	[NhaCungCap] [int] NOT NULL,
	[TrangThai] [int] NOT NULL,
	[DoHot] [int] NOT NULL,
	[NgayTao] [date] NULL,
	[NguoiTao] [varchar](100) NULL,
	[NgaySua] [date] NULL,
	[NguoiSua] [varchar](100) NULL,
 CONSTRAINT [PK_SanPham] PRIMARY KEY CLUSTERED 
(
	[SanPhamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 3/30/2024 7:52:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[AccountId] [nchar](36) NOT NULL,
	[TaiKhoan] [nchar](50) NOT NULL,
	[MatKhau] [nchar](50) NOT NULL,
	[HoTen] [nvarchar](100) NULL,
	[SoDienThoai] [nchar](50) NULL,
	[Email] [nchar](50) NULL,
	[DiaChi] [nvarchar](200) NULL,
	[GioiTinh] [int] NULL,
	[role] [int] NOT NULL,
	[SoLanMuaHang] [int] NULL,
	[SoLanHuyHang] [int] NULL,
	[TrangThai] [int] NULL,
	[NgaySua] [date] NULL,
	[NgayTao] [date] NULL,
 CONSTRAINT [PK_TaiKhoan] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TinTuc]    Script Date: 3/30/2024 7:52:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TinTuc](
	[TinTucId] [nchar](36) NOT NULL,
	[MaTinTuc] [nchar](50) NOT NULL,
	[TieuDe] [nvarchar](500) NOT NULL,
	[NoiDung] [text] NOT NULL,
	[HinhAnh] [nchar](20) NOT NULL,
	[GioiThieu] [nvarchar](1000) NULL,
	[TrangThai] [int] NULL,
	[NgayTao] [date] NULL,
	[NguoiTao] [nvarchar](50) NULL,
	[NgaySua] [date] NULL,
	[NguoiSua] [nvarchar](50) NULL,
 CONSTRAINT [PK_TinTuc] PRIMARY KEY CLUSTERED 
(
	[TinTucId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
