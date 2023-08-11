# start kong on docker

> curl -Ls https://get.konghq.com/quickstart | bash

# build Websocket server

> docker build . -f src/Websocket/Dockerfile -t websocket --no-cache

# run Websocket server

> docker run --rm -it -p 8080:80 --network=kong-quickstart-net --name websocket websocket 