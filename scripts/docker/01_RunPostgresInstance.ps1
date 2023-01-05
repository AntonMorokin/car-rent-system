docker run --name pgsql-standalone `
    -e POSTGRES_USER=postgres `
    -e POSTGRES_PASSWORD=1234 `
    -p 5432:5432 `
    -v D:\repos\ContainersData\postgres:/var/lib/postgresql/data `
    --rm `
    postgres:14.4-alpine