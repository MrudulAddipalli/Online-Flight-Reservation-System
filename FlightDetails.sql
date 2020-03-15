create database OFRS;

use OFRS;


select * from Registration;
create table Registration(UserID int Primary key identity(100000000,1),Password varchar(9),FirstName varchar(15),LastName varchar(15),DOB date,Age int,Gender varchar(7),Address varchar(30),PhoneNo varchar(10),UserType varchar(2) default 'U',LoginStatus tinyint default '0');
insert into Registration("Password","FirstName","LastName","DOB","Age","Gender","Address","PhoneNo","UserType","LoginStatus") values('Admin','Mrudul','Addipalli','1997-07-08',22,'Male','Vasai','857588548','A',0);
insert into Registration("Password","FirstName","LastName","DOB","Age","Gender","Address","PhoneNo","UserType","LoginStatus") values('User','Harsh','Patel','1997-07-08',22,'Male','Vasai','857588548','U',0);
insert into Registration("Password","FirstName","LastName","DOB","Age","Gender","Address","PhoneNo","UserType","LoginStatus") values('User','Imran','Khan','1997-07-08',22,'Male','Vasai','857588548','U',0);



select * from RouteTable;
select * from ScheduleTable;
select * from ReservationSeats;


select * from Flight; 
create table Flight(FlightID int Primary Key identity(1,1),FlightName varchar(30),Source varchar(30),Destination varchar(30),EstimatedTime varchar(10),SeatCapacity int,FlightType varchar(20));
insert into flight("FlightName","Source","Destination","EstimatedTime","SeatCapacity","FlightType") values('Air India','Mumbai','Delhi','03:00',60,'Domestic');
insert into flight("FlightName","Source","Destination","EstimatedTime","SeatCapacity","FlightType") values('Spice Jet','Mumbai','Hyderabad','04:00',60,'Domestic');
insert into flight("FlightName","Source","Destination","EstimatedTime","SeatCapacity","FlightType") values('Air India','Raipur','Hyderabad','05:00',80,'Domestic');
insert into flight("FlightName","Source","Destination","EstimatedTime","SeatCapacity","FlightType") values('Kingfisher','Mumbai','New York','19:00',90,'International');
insert into flight("FlightName","Source","Destination","EstimatedTime","SeatCapacity","FlightType") values('Fly Emirates','Dubai','Chennai','03:00',60,'International');


drop table ReservationSeats
select * from ReservationSeats;
create table ReservationSeats(FlightID int,ScheduleID int,FirstClassSeats int,BusinessSeats int,EconomySeats int,RemainingFirstClassSeats int,RemainingBusinessSeats int,RemainingEconomySeats int , FCPrice int , BCPrice int, ECPrice int);
insert into ReservationSeats (FlightID ,FirstClassSeats ,BusinessSeats,EconomySeats,RemainingFirstClassSeats ,RemainingBusinessSeats,RemainingEconomySeats, FCPrice , BCPrice, ECPrice) values(1,10,20,30,10,20,30,2540,1900,1200);
insert into ReservationSeats values(2,10,20,30,10,20,30,2500,1900,1280);
insert into ReservationSeats values(3,10,20,50,10,20,50,2500,1870,1200);
insert into ReservationSeats values(4,10,30,50,10,30,50,25600,16600,14000);
insert into ReservationSeats values(5,10,20,30,10,20,30,28900,18000,15600);



select * from ScheduleTable;
create table ScheduleTable(ScheduleID int Primary Key Identity(1,1),FlightID int,DepartureDateTime varchar(30),ArrivalDateTime varchar(30),RouteName varchar(50));
insert into ScheduleTable("FlightID","DepartureDateTime","ArrivalDateTime","RouteName") values ('1','2019-12-01 12:00 PM','2019-12-01 14:20 PM','Mumbai-Delhi');
insert into ScheduleTable("FlightID","DepartureDateTime","ArrivalDateTime","RouteName") values ('1','2019-12-04 13:00 PM','2019-12-04 15:35 PM','Mumbai-Delhi');
insert into ScheduleTable("FlightID","DepartureDateTime","ArrivalDateTime","RouteName") values ('2','2019-12-02 14:00 PM','2019-12-02 16:40 PM','Mumbai-Hyderabad');
insert into ScheduleTable("FlightID","DepartureDateTime","ArrivalDateTime","RouteName") values ('3','2019-12-03 14:50 PM','2019-12-03 17:00 PM','Raipur-Hyderabad');
insert into ScheduleTable("FlightID","DepartureDateTime","ArrivalDateTime","RouteName") values ('4','2019-12-03 16:25 PM','2019-12-05 18:00 PM','Mumbai-New York');
insert into ScheduleTable("FlightID","DepartureDateTime","ArrivalDateTime","RouteName") values ('5','2019-12-05 17:40 PM','2019-12-07 19:20 PM','Dubai-Chennai');




select * from RouteTable;
create table RouteTable(RouteID int Primary Key Identity(1,1),FlightID int,RouteName varchar(50)); 
insert into RouteTable("FlightID","RouteName") values('1','Mumbai-Delhi');
insert into RouteTable("FlightID","RouteName") values('2','Mumbai-Hyderabad');
insert into RouteTable("FlightID","RouteName") values('3','Raipur-Hyderabad');
insert into RouteTable("FlightID","RouteName") values('4','Mumbai-New York');
insert into RouteTable("FlightID","RouteName") values('5','Dubai-Chennai');



select * from UserBooking;
create table UserBooking(BID int primary key identity(1,1),UserID varchar(9),ScheduleID int,FCSeats int,BCSeats int,ECSeats int,TripPrice int,BookStatus varchar(1));


select * from CreditCard;
create table CreditCard (CCID int primary key identity(1,1),UserID varchar(9),CCNumber varchar(20),ValidFrom date,ValidTo date,Balance int);



select * from Passengers
create table Passengers (PID int Primary Key identity(1,1),BID varchar(10),Name varchar(30),Age Varchar (4),Gender varchar(15),SeatNo varchar(20),Class varchar(20),);

select * from Registration;


select * from Flight Inner Join ReservationSeats on Flight.FlightID=ReservationSeats.FlightID where ReservationSeats.ScheduleID!='' Order By Flight.FlightID DESC 


select * from Flight; 
select * from ReservationSeats;
select * from ScheduleTable;
select * from Passengers;
select * from UserBooking;
select * from RouteTable;
select * from CreditCard;


truncate table Flight; 
truncate table ReservationSeats;
truncate table ScheduleTable;
truncate table Passengers;
truncate table UserBooking;
truncate table RouteTable;
truncate table CreditCard;
