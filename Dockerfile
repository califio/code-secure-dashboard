FROM node:22 AS build
EXPOSE 80
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --prod

FROM nginx:alpine
COPY --from=build /app/dist/code-secure-ui/browser /usr/share/nginx/html
