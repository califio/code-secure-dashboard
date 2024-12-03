FROM node:22 AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --prod

FROM registry.gitlab.com/code-secure/code-secure-api:1.1.6 AS api
EXPOSE 8080
WORKDIR /app
COPY --from=build /app/dist/code-secure-dashboard/browser wwwroot
