# Distribution Inventory Tracker

## Overview

This project consists of an Angular application and a .NET Core server, both utilizing RabbitMQ for messaging. The Angular application is built with version 16.1.2.

## Prerequisites

Before running the project, ensure you have the following prerequisites installed on your machine:

- Node.js and npm (for Angular)
- .NET Core SDK
- RabbitMQ Server

## Project Structure

- `distinv` folder: Angular application
- `Server` folder: .NET Core server

## Running the Angular Application

1. Navigate to the `distinv` folder:

    ```bash
    cd distinv
    ```

2. Install the required dependencies:

    ```bash
    npm install
    ```

3. Build and run the Angular application:

    ```bash
    ng serve
    ```

   The application will be accessible at `http://localhost:4200/`.

## Running the .NET Core Server

1. Navigate to the `Server` folder:

    ```bash
    cd Server
    ```

2. Build and run the .NET Core server:

    ```bash
    dotnet run
    ```

   The server will be accessible at `http://localhost:5263/`.

## Setting Up RabbitMQ Management Console

To enable the RabbitMQ Management Console for monitoring and managing your RabbitMQ server, follow these steps:

### For RabbitMQ Installed via Package Manager (apt, yum):

1. Install the RabbitMQ Management Plugin:

    ```bash
    sudo rabbitmq-plugins enable rabbitmq_management
    ```

2. Restart the RabbitMQ service:

    ```bash
    sudo service rabbitmq-server restart
    ```

### For RabbitMQ Installed via Homebrew (Mac):

1. Enable the RabbitMQ Management Plugin:

    ```bash
    rabbitmq-plugins enable rabbitmq_management
    ```

2. Start the RabbitMQ service:

    ```bash
    brew services start rabbitmq
    ```

### For RabbitMQ Installed as a Docker Container:

If you're running RabbitMQ as a Docker container, enable the Management Console during container creation:

    ```bash
    docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management
    ```



### Setting Up RabbitMQ Fanout Exchange

To create a RabbitMQ fanout exchange and bind it to a queue, run the provided PowerShell script. Ensure you have the RabbitMQ Management Plugin enabled.

# RabbitMQ server details
$rmqUrl = "http://localhost:15672" # Change the URL if your RabbitMQ server is running on a different host or port
$rmqUser = "guest"
$rmqPassword = "guest"

# Exchange and Queue details
$exchangeName = "exchange"
$queueName = "myqueue"
$bindingKey = "" # Fanout exchanges ignore routing keys, so it can be an empty string

# Base64 encoding for authentication
$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("${rmqUser}:${rmqPassword}")))

# Function to create a RabbitMQ fanout exchange
function CreateFanoutExchange {
    param (
        [string]$exchangeName
    )

    $exchangeConfig = @{
        type = "fanout"
        auto_delete = $false
        durable = $true
    }

    $exchangeUri = "$rmqUrl/api/exchanges/%2F/$exchangeName"
    Invoke-RestMethod -Uri $exchangeUri -Method PUT -Headers @{ Authorization = "Basic $base64AuthInfo" } -Body ($exchangeConfig | ConvertTo-Json) -ContentType 'application/json'
}

# Function to bind a queue to a fanout exchange
function BindQueueToExchange {
    param (
        [string]$exchangeName,
        [string]$queueName,
        [string]$bindingKey
    )

    $bindingConfig = @{
        routing_key = $bindingKey
    }

    $bindingUri = "$rmqUrl/api/bindings/%2F/e/$exchangeName/q/$queueName"
    Invoke-RestMethod -Uri $bindingUri -Method POST -Headers @{ Authorization = "Basic $base64AuthInfo" } -Body ($bindingConfig | ConvertTo-Json) -ContentType 'application/json'
}

# Main execution
CreateFanoutExchange -exchangeName $exchangeName
BindQueueToExchange -exchangeName $exchangeName -queueName $queueName -bindingKey $bindingKey

Write-Host "Fanout exchange '$exchangeName' created and bound to queue '$queueName'."

