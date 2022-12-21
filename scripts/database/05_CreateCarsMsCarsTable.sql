create table cars_ms.cars (
    id int not null generated always as identity primary key,
    number text not null unique,
    brand text not null,
    model text not null,
    mileage real not null
);