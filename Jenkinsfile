pipeline {
  agent any
  stages {
    stage('Init') {
      steps {
        sh '''#!/bin/bash

# Get msbuild through .NET core
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

sudo apt-get update
sudo apt-get install -y apt-transport-https
sudo apt-get update
sudo apt-get install -y dotnet-sdk-3.1

sudo apt-get update
sudo apt-get install -y aspnetcore-runtime-3.1'''
      }
    }

  }
}