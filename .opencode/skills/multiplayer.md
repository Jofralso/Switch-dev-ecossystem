# Multiplayer Skill

## Responsibilities
- validate client-server synchronization
- detect authority and ownership issues
- generate replication logic
- analyze packet flow and bandwidth
- implement rollback / latency compensation
- design deterministic lockstep for peer-to-peer

## Rules
- server is authoritative for all gameplay state
- client-side prediction for local responsiveness
- reconciliation on server state updates
- bandwidth budget: < 50KB/s per player
- packet budget: < 30 packets/sec per player
- interpolation for remote object positions
- snapshots at 10-20 Hz, inputs at 30-60 Hz
- delta compression for state updates
- prioritize gameplay-relevant state changes

## Network Architecture
- Netcode for GameObjects (NGO) for Unity
- or Mirror for custom transport layer
- or custom UDP-based transport for Switch P2P
- relay server for NAT punchthrough

## Anti-Cheat
- server validates all state changes
- rate limiting on player actions
- checksum verification for critical state
- replay validation for competitive modes
