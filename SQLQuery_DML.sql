
-- insert products
insert into product(productid,name,category,price) values('p101','diet coke','drink',1.0);
insert into product(productid,name,category,price) values('p102','regular coke','drink',2.0);
insert into product(productid,name,category,price) values('p103','pizza','frozen food',3.0);
insert into product(productid,name,category,price) values('p104','milk','diary',4.0);

-- insert customer
insert into customer(customerid,firstname,lastname,phonenumber) values('cus1','John','Smith','6021231234');
insert into customer(customerid,firstname,lastname,phonenumber) values('cus2','Adam','Savage','4801231234');
insert into customer(customerid,firstname,lastname,phonenumber) values('cus3','King','Kong','9291231234');
insert into customer(customerid,firstname,lastname,phonenumber) values('cus4','Tim','Cook','7771231234');

-- insert store
insert into store(storeloc,storephone) values('Central Ave 1', '1111111111');
insert into store(storeloc,storephone) values('South Ave 2', '2222222222');
insert into store(storeloc,storephone) values('Mountain View 3', '3333333333');
insert into store(storeloc,storephone) values('River View 4', '4444444444');


-- insert order
-- recalculate total for total cost
-- total
insert into orderr(orderid,storeloc,customerid,totalcost) values('o001','Central Ave 1','cus1',1);

/*
insert into orderr(orderid,storeloc,customerid,totalcost)
select sum(op.quantity * p.price) 
 from order o
 join orderproduct op on o.orderid = op.orderid
 join product p on op.productid = p.productid
  );
*/
insert into orderr(orderid,storeloc,customerid,totalcost) values('o002','South Ave 2','cus2',4);
insert into orderr(orderid,storeloc,customerid,totalcost) values('o003','Mountain View 3','cus3',9);
insert into orderr(orderid,storeloc,customerid,totalcost) values('o004','River View 4','cus4',16);


-- insert brideges product - order
-- processid is auto generated
-- quantity
insert into orderproduct(orderid,productid,quantity) values('o001','p101',1);
-- debugging row
insert into orderproduct(orderid,productid,quantity) values('o001','p101',10);
insert into orderproduct(orderid,productid,quantity) values('o002','p102',2);
insert into orderproduct(orderid,productid,quantity) values('o003','p103',3);
insert into orderproduct(orderid,productid,quantity) values('o004','p104',4);
-- debugging row
insert into orderproduct(orderid,productid,quantity) values('o001','p101',9);





-- insert bridges store - product
insert into Inventory(storeloc,productid,quantity) values('Central Ave 1', 'p101',10);
insert into Inventory(storeloc,productid,quantity) values('South Ave 2', 'p102',10);
insert into Inventory(storeloc,productid,quantity) values('Mountain View 3', 'p103',10);
insert into Inventory(storeloc,productid,quantity) values('River View 4', 'p104',10);

-- insert bridges store - customer
insert into storecustomer(storeloc,customerid) values('Central Ave 1','cus1');
insert into storecustomer(storeloc,customerid) values('South Ave 2','cus2');
insert into storecustomer(storeloc,customerid) values('Mountain View 3','cus3');
insert into storecustomer(storeloc,customerid) values('River View 4','cus4');

select* from product;
select* from orderr;
select* from customer;
select* from store;
select* from orderproduct;
select* from inventory;
select* from storecustomer;


select o.orderid,p.productid,op.quantity,p.price,o.totalcost
from orderr o 
join orderproduct op on o.orderid = op.orderid
join product p on op.productid = p.productid






