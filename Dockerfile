FROM node:22 AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --prod

FROM ghcr.io/califio/code-secure-api:latest AS api
EXPOSE 8080
WORKDIR /app
COPY --from=build /app/dist/code-secure-dashboard/browser wwwroot
