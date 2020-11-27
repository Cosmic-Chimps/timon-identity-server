#dapr run --app-id timon-identity-server --app-port 5000  dotnet run

daprd --app-id "timon-identity-server" --app-port "5000" --components-path "./components" --dapr-grpc-port "50002" --dapr-http-port "3501" "--enable-metrics=false" --placement-address "localhost:50005"
