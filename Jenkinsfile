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

dotnet test --no-restore --no-build --logger "trx;LogFileName=/tmp/unit_tests.xml"'''
      }
    }

    stage('Archive Tests') {
      steps {
        junit '/tmp/unit_tests.xml'
      }
    }

  }
}