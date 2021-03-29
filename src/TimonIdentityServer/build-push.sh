#!/usr/bin/env bash

docker build . -t jjchiw/timon-identity-server:$(git rev-parse --short HEAD)
docker push jjchiw/timon-identity-server:$(git rev-parse --short HEAD)
git rev-parse --short HEAD
