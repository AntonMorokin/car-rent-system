create table clients_ms.clients (
	id integer not null generated always as identity primary key,
	first_name text not null,
	last_name text not null,
	birth_date date not null
);