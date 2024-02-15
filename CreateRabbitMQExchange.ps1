# RabbitMQ Server Connection Information
$rabbitMqServer = "localhost"
$rabbitMqPort = 5672
$rabbitMqVirtualHost = "/"
$rabbitMqUsername = "guest"
$rabbitMqPassword = "guest"

# RabbitMQ Exchange and Queue Information
$exchangeName = "exchange"
$queueName = "myqueue"

# RabbitMQ Management HTTP API Base URL
$managementApiBaseUrl = "http://$rabbitMqServer:$rabbitMqPort/api"

# Function to Create RabbitMQ Fanout Exchange
function CreateRabbitMqFanoutExchange {
    $exchangeConfig = @{
        type = "fanout"
        durable = $true
    }

    Invoke-RestMethod -Uri "$managementApiBaseUrl/exchanges/$rabbitMqVirtualHost/$exchangeName" -Method PUT -Headers @{
        Authorization = "Basic " + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("$($rabbitMqUsername):$($rabbitMqPassword)"))
    } -Body ($exchangeConfig | ConvertTo-Json) -ContentType "application/json"
}

# Function to Bind Queue to RabbitMQ Fanout Exchange
function BindQueueToExchange {
    $bindingConfig = @{
        routing_key = ""
    }

    Invoke-RestMethod -Uri "$managementApiBaseUrl/bindings/$rabbitMqVirtualHost/e/exchange/q/$queueName" -Method POST -Headers @{
        Authorization = "Basic " + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("$($rabbitMqUsername):$($rabbitMqPassword)"))
    } -Body ($bindingConfig | ConvertTo-Json) -ContentType "application/json"
}

# Main Execution
try {
    # Create RabbitMQ Fanout Exchange
    CreateRabbitMqFanoutExchange

    # Bind Queue to RabbitMQ Fanout Exchange
    BindQueueToExchange

    Write-Host "RabbitMQ Fanout Exchange '$exchangeName' created and bound to Queue '$queueName' successfully."
}
catch {
    Write-Host "Error: $_"
}