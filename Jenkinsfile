pipeline {
  agent any
  stages {
    stage('Init') {
      steps {
        sh 'dotnet restore'
      }
    }

    stage('Build') {
      steps {
        sh 'dotnet build --no-restore'
      }
    }

    stage('Test') {
      steps {
        sh 'dotnet test --no-restore --no-build --logger "trx;LogFileName=unit_tests.xml"'
      }
    }
  }
  
  post {
    always {
      junit 'unit_tests.xml'
    }
  }
}