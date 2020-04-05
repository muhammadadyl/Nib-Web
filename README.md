# Nib-Web
Evaluation test

## Backend Architecture
Backend Architecture is designed to support Microservice Architecture, It is a loose and practical form of Clean Architecture. GRPC is being used at backend, along with ASP.NET Core Web API for exposing service to frontend. This Implementation also contains Simple Memory Caching for backend data.

GRPC Server implemented through Protobuf and same files are being used for creating GRPC client that is being used by Web API for REST service.

## Frontend Architecture
Frontend Architecture is designed on React and Redux in Typescript using CRA [Create React App](https://github.com/facebookincubator/create-react-app). 80% css is custom written, while 20% was a part of CRA framework.

## DevOps
For easy setup all projects can be run through `docker-compose`. API also exposed to use out side of docker too.

## Run
Project can be Run in 2 ways.
- By running instances of `Nib.Career.GrpcServer`, `Nib.Career.RestApi`, and `Nib.Career.Web` projects through VS (for Web you can also use `npm start` command through CLI)
- By running `docker-compose build && docker-compose up`
