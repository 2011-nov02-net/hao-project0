-- DDL for project0

create table product
(
    productid NVARCHAR(20) primary key not null,
    name NVARCHAR(20) not null,
    category NVARCHAR(20) not null,
    price float not null,
    check (price > 0)
);

create table store
(
    storeloc NVARCHAR(100) primary key not null,
    storephone NVARCHAR(10) unique not null,
    check(len(storephone) = 10),
    check(storephone like '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
)

create table customer
(
    customerid NVARCHAR(20) primary key not null,
    firstname NVARCHAR(20) not null,
    lastname NVARCHAR(20) not null,
    phonenumber NVARCHAR(10) not null,
    check(len(phonenumber) = 10),
    check(phonenumber like '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
)

create table orderr
(
    orderid NVARCHAR(20) primary key not null,
    storeloc NVARCHAR(100) not null,
    customerid NVARCHAR(20) not null,
    orderedtime DATETIME not null default getdate(),
    totalcost float not null,
    check(totalcost > 0),
    foreign key(storeloc) REFERENCES store(storeloc)
        on delete cascade on update cascade,
    foreign key(customerid) REFERENCES customer(customerid)
        on delete cascade on update cascade

);

-- all p.k of junction tables are int, have IDENTITY
-- junction table btw product and order n:n relationship
create table orderproduct
(
    processid int primary key not null IDENTITY,
    orderid NVARCHAR(20) not null,
    productid NVARCHAR(20) not null,
    quantity int not null,
    check (quantity >0),
    foreign key(orderid) REFERENCES orderr(orderid)
        on delete cascade on update cascade,
    foreign key(productid) REFERENCES product(productid)
        on delete cascade on update cascade
)

-- junction table btw store and product n:n relationship
-- more like updateinventory 
create table inventory
(
    supplyid int primary key not null IDENTITY,
    storeloc NVARCHAR(100) not null,
    productid NVARCHAR(20) not null,
    quantity int not null,
    check(quantity >0),
    foreign key(storeloc) REFERENCES store(storeloc)
        on delete cascade on update cascade,
    foreign key(productid) REFERENCES product(productid)
        on delete cascade on update cascade

);

-- junction table btw store and customer
create table storecustomer
(
    relationid int primary key not null IDENTITY,
    storeloc NVARCHAR(100) not null,
    customerid NVARCHAR(20) not null,
    foreign key(storeloc) REFERENCES store(storeloc)
        on delete cascade on update cascade,
    foreign key(customerid) REFERENCES customer(customerid)
        on delete cascade on update cascade

);


drop table storecustomer; drop table inventory; drop table orderproduct; drop table orderr; drop table customer;
drop table customer; drop table store; drop table product

