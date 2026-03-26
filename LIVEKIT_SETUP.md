# LiveKit Self-Hosted Setup

This guide shows how to run LiveKit locally or on a server and connect it to your .NET API.

## 1) Run LiveKit (Docker)

Create a `livekit.yaml` file (same folder you run docker from):

```yaml
port: 7880
rtc:
  tcp_port: 7881
  udp_port: 7882
keys:
  LK_API_KEY: LK_API_SECRET
turn:
  enabled: false
```

Start LiveKit:

```bash
docker run --rm -p 7880:7880 -p 7881:7881 -p 7882:7882/udp \
  -v ${PWD}/livekit.yaml:/livekit.yaml \
  livekit/livekit-server:latest \
  --config /livekit.yaml
```

Notes:
- `LK_API_KEY` and `LK_API_SECRET` must match your .NET API config.
- For production, enable TURN (see LiveKit docs).

## 2) Configure .NET API

In `API/appsettings.json`:

```json
"LiveKit": {
  "ApiKey": "LK_API_KEY",
  "ApiSecret": "LK_API_SECRET",
  "TokenTtlMinutes": "10"
}
```

## 3) Frontend (Angular)

Use LiveKit Web SDK in Angular to connect with the token from:

```
POST /api/LiveSession/{sessionId}/join
```

Response includes:
- `roomId`
- `token`
- `role`
- `canSpeak`

## 4) Quick Test

1. Start LiveKit docker.
2. Start your API.
3. Create session -> join -> use token in frontend to connect.

## 5) Production Tips

- Put LiveKit behind HTTPS + valid cert.
- Enable TURN for real-world NAT traversal.
- Use short token TTLs (5-10 min).
