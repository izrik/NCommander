#!/bin/bash

VERSION=$1
if [[ -z "$VERSION" ]]; then
  VERSION=$(git describe --tags | sed 's/^v//' )
  # c.f. grep AssemblyVersion Properties/AssemblyInfo.cs
fi
if [[ -z "$VERSION" ]]; then
  echo "Error: Could not determine the version string." >&2
  exit 1
fi

dotnet pack --include-source --include-symbols NCommander.sln /p:PackageVersion=$VERSION
