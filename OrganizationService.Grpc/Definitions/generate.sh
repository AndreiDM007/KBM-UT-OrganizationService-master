#!/bin/bash
PROTOC="$HOME/.nuget/packages/google.protobuf.tools/3.6.1/tools/macosx_x64/protoc"
PLUGIN="$HOME/.nuget/packages/grpc.tools/1.18.0/tools/macosx_x64/grpc_csharp_plugin"

${PROTOC} --csharp_out ../Generated  AuthorizationService.proto --grpc_out ../Generated --plugin=protoc-gen-grpc=${PLUGIN}
${PROTOC} --csharp_out ../Generated  GroupAuthorizationService.proto --grpc_out ../Generated --plugin=protoc-gen-grpc=${PLUGIN}
${PROTOC} --csharp_out ../Generated  GroupService.proto --grpc_out ../Generated --plugin=protoc-gen-grpc=${PLUGIN}
${PROTOC} --csharp_out ../Generated  OrganizationService.proto --grpc_out ../Generated --plugin=protoc-gen-grpc=${PLUGIN}
${PROTOC} --csharp_out ../Generated  OrganizationUserService.proto --grpc_out ../Generated --plugin=protoc-gen-grpc=${PLUGIN}
${PROTOC} --csharp_out ../Generated  ProfileService.proto --grpc_out ../Generated --plugin=protoc-gen-grpc=${PLUGIN}
${PROTOC} --csharp_out ../Generated  UserInvitationService.proto --grpc_out ../Generated --plugin=protoc-gen-grpc=${PLUGIN}
${PROTOC} --csharp_out ../Generated  HealthService.proto --grpc_out ../Generated --plugin=protoc-gen-grpc=${PLUGIN}