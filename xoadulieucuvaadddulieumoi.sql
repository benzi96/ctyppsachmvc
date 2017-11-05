delete from ctdmsdb
delete from ctpn
delete from ctpx
delete from congnotheothoigian
delete from sotienphaitrachonxb
delete from tonkho
delete from danhmucsachdaban
delete from phieunhap
delete from phieuxuat
delete from phieutratien
delete from hangtoncuadaily
delete from daily
delete from sach
delete from nxb
delete from linhvuc

--add lai du lieu
DBCC CHECKIDENT ('[linhvuc]', RESEED, 0);
DBCC CHECKIDENT ('[daily]', RESEED, 0);
DBCC CHECKIDENT ('[nxb]', RESEED, 0);
DBCC CHECKIDENT ('[sach]', RESEED, 0);

insert into linhvuc values(N'văn học')
insert into linhvuc values(N'toán học')
insert into linhvuc values(N'tiểu thuyết')
insert into linhvuc values(N'Sách giáo khoa')
--delete from linhvuc

insert into daily values(N'Nhà sách Phú Thọ', N'12/3 Hồ Chí Minh','01010123232', 0)
insert into daily values(N'Đại lý sách A', N'98/6 Hà Nội', '09071143133', 0)
insert into daily values(N'Đại lý sách B', N'28/4 Đà Nẵng', '090713434633', 0)
insert into daily values(N'Đại lý sách C', N'96/8 Hải Phòng', '09074546533', 0)

insert into nxb values(N'Nhà xuất bản A', N'56/32 Vũ Tàu', '0906123455','213124344253456', 0)
insert into nxb values(N'Nhà xuất bản B', N'78/42 Bình Phước', '0934523455','21312437878743456', 0)
insert into nxb values(N'Nhà xuất bản C', N'96/52 Hà Nội', '09064564455','2131243457583456', 0)
insert into nxb values(N'Nhà xuất bản D', N'26/12 Hồ Chí Minh', '090612522345','2131243445463456', 0)

insert into sach values(N'sách A', N'Tác giả A', 1, 1, 40000, 30000, 0)
insert into sach values(N'sách B', N'Tác giả B', 2, 2, 50000, 40000, 0)
insert into sach values(N'sách C', N'Tác giả C', 3, 3, 60000, 50000, 0)
insert into sach values(N'sách D', N'Tác giả D', 1, 1, 80000, 60000, 0)
insert into sach values(N'sách E', N'Tác giả E', 4, 4, 90000, 70000, 0)