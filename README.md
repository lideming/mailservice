# Mailservice

A simple self-hosted email sending HTTP API service using SMTP.

## Why?

- Somehow the SMTP connections are blocked by ISP or the SMTP server, but a third server works.
- Centerized email sending configuration.

## Configuration

Edit `appsettings.json`, add STMP account configurations:

```json
{
  "Urls": "http://127.0.0.1:8080",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "Mail": {
    "Accounts": [
      {
        "Token": "should_be_random_generated",
        "From": "sender@example.com",
        "FromName": "The Sender",
        "Host": "smtp.exmail.qq.com",
        "Port": 465,
        "Tls": true,
        "Username": "sender@example.com",
        "Password": ""
      }
    ]
  }
}
```

## Run from source code

```shell
git clone https://github.com/lideming/mailservice.git
cd mailservice
dotnet run -c Release
```

## Run in Docker

```shell
docker run -d -v /PATH/TO/SETTINGS.json:/app/appsettings.json yuuza/mailservice
```

## API Usage

POST `/api/mail` with JSON body:

```ts
interface Mail {
  fromName: string;
  to: string;
  toName: string;
  subject: string;
  body: string;
  bodyType:
    | "text" // "text/plain"
    | "html" // "text/html"
    | string // any MIME type
    ;
}
```
