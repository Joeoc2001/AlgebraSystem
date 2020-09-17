pipeline {
  agent any
  stages {
    stage('Init') {
      steps {
        sh '''#!/bin/bash

dotnet restore'''
      }
    }

    stage('Build') {
      steps {
        sh '''#!/bin/bash

dotnet build --no-restore'''
      }
    }

    stage('Test') {
      steps {
        sh '''#!/bin/bash

dotnet test --no-restore'''
      }
    }

  }
}