Base on https://github.com/grpc/grpc-dotnet/tree/master/examples/Certifier with added .NET Framework 4.8 client.


Client .NET 5.0 works.

Client .NET Framework 4.8 does not work.


You need to:
1. Install \grpc_certifier_example\Client\Certs\client.pfx to a "Local Machine\Trusted Root..." on the Server. Cert pass: 1111



Related to: https://stackoverflow.com/questions/65057450/how-to-connect-grpc-nuget-2-33-1-client-net-framework-with-grpc-asp-netcore
