# automatic-routing
Route requests to specific controllers and\or actions based on configurations

Based on configuration change the controller and action that will process the request.

Configuration Example
```JSON
"FeatureRouting": [
  {
    "Header": {
      "Key": "Identifier",
      "Value": "FeatureController"
    },
    "Action": {
      "From": {
        "Controller": "WeatherForecast",
        "Action": "Get"
      },
      "To": {
        "Controller": "CarInfo",
        "Action": "Get"
      }
    }
  }
```
  
