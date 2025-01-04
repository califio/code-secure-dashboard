# Docker Compose

### Environment

| ENV               | Description                                                                                                                                                                 |
|-------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| SYSTEM_PASSWORD   | The password for the system user. Use this user for the first login. If the password is blank, Code Secure will automatically generate a random password.                   |
| ACCESS_TOKEN_KEY  | The access token key used to verify JWT access tokens. Example: 3afd551d-6882-4a59-8027-09d2f0f723ac                                                                        |
| REFRESH_TOKEN_KEY | The refresh token key used to verify JWT refresh tokens. The refresh token key should be different from the access token key. Example: 5cf90573-d3ad-4ce8-8801-59f9bc93c703 |

docker-compose.yml
```yaml
services:
  codesecure:
    image: ghcr.io/califio/code-secure-dashboard:latest
    depends_on:
      - db
    environment:
      DB_SERVER: db
      DB_USERNAME: codesecure
      DB_PASSWORD: codesecure
      DB_NAME: codesecure
      SYSTEM_PASSWORD: "" # change system's password. Example: S3cur3Pa$$w0rd
      ACCESS_TOKEN_KEY: "" # change me
      REFRESH_TOKEN_KEY: "" # change refresh tokenkey. example: 5cf90573-d3ad-4ce8-8801-59f9bc93c703
    ports:
      - "8080:8080"
  db:
    image: postgres
    environment:
      POSTGRES_USER: ${POSTGRES_USER:-codesecure}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD:-codesecure}
      PGDATA: /data/postgres
    volumes:
      - codesecure_db:/data/postgres
    ports:
      - "54321:5432"
    restart: unless-stopped

volumes:
  codesecure_db:

```
