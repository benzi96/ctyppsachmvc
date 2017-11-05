create database ctyppsach
go

create table nxb
(
	idnxb int primary key identity(1,1),
	tennxb nvarchar(50),
	diachi nvarchar(50),
	sodt nvarchar(15),
	sotk nvarchar(50)
)
go

create table daily
(
	iddl int primary key identity(1,1),
	tendl nvarchar(50),
	diachi nvarchar(50),
	sodt nvarchar(15),
)
go

create table linhvuc
(
	idlv int primary key identity(1,1),
	tenlv nvarchar(50),
)
go

create table sach
(
	idsach int primary key identity(1,1),
	tensach nvarchar(50),
	tacgia nvarchar(50),
	idnxb int,
	idlv int,
	giaxuat decimal(10,0),
	gianhap decimal(10,0),
	
	foreign key(idnxb) references nxb(idnxb),
	foreign key(idlv) references linhvuc(idlv),
)
go

create table phieunhap
(
	idpn int primary key identity(1,1),
	idnxb int,
	nguoigiaosach nvarchar(50),
	ngaynhap datetime,
	nguoivietphieu nvarchar(50),
	
	foreign key(idnxb) references nxb(idnxb),
)
go

create table ctpn
(
	idpn int,
	idctpn int,
	idsach int,
	soluong int,
	
	foreign key(idpn) references phieunhap(idpn),
	foreign key(idsach) references sach(idsach),
	primary key(idpn, idctpn)
)
go

create table phieuxuat
(
	idpx int primary key identity(1,1),
	iddl int,
	nguoinhan nvarchar(50),
	ngayxuat datetime,
	
	foreign key(iddl) references daily(iddl),
)
go

create table ctpx
(
	idpx int,
	idctpx int,
	idsach int,
	soluong int,
	
	foreign key(idpx) references phieuxuat(idpx),
	foreign key(idsach) references sach(idsach),
	primary key(idpx, idctpx)
)
go

create table tonkho
(
	idtk int primary key identity(1,1),
	idsach int not null,
	thoidiem datetime not null CONSTRAINT DF_tonkho_thoidiem_GETDATE DEFAULT GETDATE(),
	soluongton int,
	idphieu int,

	foreign key(idsach) references sach(idsach),
)
go

create table hangtoncuadaily
(
	idht int primary key identity(1,1),
	iddl int,
	idsach int,
	soluongchuaban int,

	foreign key(iddl) references daily(iddl),
	foreign key(idsach) references sach(idsach)
)
go

create table congnotheothoigian
(
	id int primary key identity(1,1),
	iddl int,
	congno decimal(10,1),
	thoidiem datetime not null CONSTRAINT DF_congnothethoigian_thoidiem_GETDATE DEFAULT GETDATE(),

	foreign key(iddl) references daily(iddl)
)
go

create table danhmucsachdaban
(
	iddmsdb int primary key identity(1,1),
	iddl int,
	thoigian datetime,
	sotiendathanhtoan decimal(10,1),
	sotienconno decimal(10,1),

	foreign key(iddl) references daily(iddl)
)
go

create table ctdmsdb
(
	idctdmsdb int,
	iddmsdb int,
	idsach int,
	soluong int,

	foreign key(iddmsdb) references danhmucsachdaban(iddmsdb),
	foreign key(idsach) references sach(idsach),
	primary key(iddmsdb, idctdmsdb)
)
go

create table sotienphaitrachonxb
(
	id int primary key identity(1,1),
	idnxb int,
	sotienphaitra decimal(10,1),

	foreign key(idnxb) references nxb(idnxb)
)
go

create table phieutratien
(
	idptt int primary key identity(1,1),
	idnxb int,
	sotientra decimal(10,1),
	tinhtrang nvarchar(50),

	foreign key(idnxb) references nxb(idnxb)
)

alter table sotienphaitrachonxb
add thoidiem datetime;

alter table sach
add soluongton int;

--version 2.0
alter table nxb
add sotienphaitra decimal(10,0);

alter table daily
add congno decimal(10,0);

alter table phieutratien
add ngaytaophieu datetime;

--version 2.3
alter table danhmucsachdaban
add tinhtrang varchar(50);