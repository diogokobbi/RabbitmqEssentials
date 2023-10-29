# RabbitmqEssentials

## Running RabbitMQ on Docker container

Use this command to run a rabbitmq container with management plugin enabled:

`docker run --rm -it -p 15672:15672 -p 5672:5672 rabbitmq:3-management`

## Solutions

### Basic
Using `queues` to publish and consume simple messages

### ContentEncoding
Using `content-encoding` properties to compress messages with zlib

### Automatically Expiring
Using `expiration` property and `x-message-ttl` queue argument to automatically expire messages.

### Message Tracking
Using `app-id` property to trace back the source of bad messages

### Dynamic Workflows
Use `reply-to` property to designate a private response queue for replies to a message

### Custom Message Routing
Using `headers` property for message routing

### Message Prioritization
Using `headers` property for message routing

### Performance Optimized
Using `delivery-mode` property to balance speed with safety and gauge performance with timestamp property.

Using no-ack mode for faster throughput.

### Distributed Brokers