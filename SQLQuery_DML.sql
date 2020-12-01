
-- insert products
insert into product(productid,name,category,price) values('p101','diet coke','drink',1.0);
insert into product(productid,name,category,price) values('p102','regular coke','drink',2.0);
insert into product(productid,name,category,price) values('p103','pizza','frozen food',3.0);
insert into product(productid,name,category,price) values('p104','milk','diary',4.0);

--insert login credential
insert into credential(email,password) values ('JSmith@gmail.com', '52640JSmith');
insert into credential(email,password) values ('ASavage@gmail.com', '52640ASavage');
insert into credential(email,password) values ('KKong@gmail.com', '52640KKong');
insert into credential(email,password) values ('TCook@gmail.com', '52640TCook');

-- insert customer
insert into customer(customerid,firstname,lastname,email,phonenumber) values('cus1','John','Smith','JSmith@gmail.com','6021231234');
insert into customer(customerid,firstname,lastname,email,phonenumber) values('cus2','Adam','Savage','ASavage@gmail.com','4801231234');
insert into customer(customerid,firstname,lastname,email,phonenumber) values('cus3','King','Kong','KKong@gmail.com','9291231234');
insert into customer(customerid,firstname,lastname,email,phonenumber) values('cus4','Tim','Cook','TCook@gmail.com','7771231234');





-- insert store
insert into store(storeloc,storephone) values('Central Ave 1', '1111111111');
insert into store(storeloc,storephone) values('South Ave 2', '2222222222');
insert into store(storeloc,storephone) values('Mountain View 3', '3333333333');
insert into store(storeloc,storephone) values('River View 4', '4444444444');


-- insert order
-- recalculate total for total cost
-- total
insert into orderr(orderid,storeloc,customerid,totalcost) values('o001','Central Ave 1','cus1',1);


insert into orderr(orderid,storeloc,customerid,totalcost) values('o002','South Ave 2','cus2',4);
insert into orderr(orderid,storeloc,customerid,totalcost) values('o003','Mountain View 3','cus3',9);
insert into orderr(orderid,storeloc,customerid,totalcost) values('o004','River View 4','cus4',16);

-- insert bridges store - product
insert into Inventory(storeloc,productid,quantity) values('Central Ave 1', 'p101',100);
insert into Inventory(storeloc,productid,quantity) values('Central Ave 1', 'p102',100);
insert into Inventory(storeloc,productid,quantity) values('Central Ave 1', 'p103',100);
insert into Inventory(storeloc,productid,quantity) values('Central Ave 1', 'p104',100);

-- insert brideges product - order
-- processid is auto generated
-- quantity
insert into orderproduct(orderid,productid,quantity) values('o001','p101',1);
insert into orderproduct(orderid,productid,quantity) values('o002','p102',2);
insert into orderproduct(orderid,productid,quantity) values('o003','p103',3);
insert into orderproduct(orderid,productid,quantity) values('o004','p104',4);

update Inventory set quantity = 99 where productid = 'p101';
update Inventory set quantity = 98 where productid = 'p102';
update Inventory set quantity = 97 where productid = 'p103';
update Inventory set quantity = 96 where productid = 'p104';



-- insert bridges store - customer
insert into storecustomer(storeloc,customerid) values('Central Ave 1','cus1');
insert into storecustomer(storeloc,customerid) values('South Ave 2','cus2');
insert into storecustomer(storeloc,customerid) values('Mountain View 3','cus3');
insert into storecustomer(storeloc,customerid) values('River View 4','cus4');

--
select* from store;
select* from customer;
select* from credential;
select* from storecustomer;
select* from orderr;
select* from orderproduct;
SELECT* from inventory;
select* from product;




select* from inventory;

