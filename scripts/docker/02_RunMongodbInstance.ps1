docker run --name mongo-standalone `
    -h docker-local `
    -p 27017:27017 `
    -v D:\repos\ContainersData\mongo\standalone\db:/data/db `
    -v D:\repos\ContainersData\mongo\standalone\configdb:/data/configdb `
    --rm `
    mongo:6.0-focal `
    --noauth --replSet crs