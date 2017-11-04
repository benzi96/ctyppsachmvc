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