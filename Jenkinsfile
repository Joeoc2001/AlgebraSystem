pipeline {
  agent {
    docker {
      image 'mcr.microsoft.com/dotnet/core/sdk:3.1'
    }
  }

  environment {
    DOTNET_CLI_HOME = "/tmp/dotnet_cli"
  }

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
        sh 'dotnet test --no-restore --no-build --logger "trx;LogFileName=UnitTests.trx" --collect:"XPlat Code Coverage"'
      }
    }

    stage('Package') {
      steps {
        sh 'dotnet pack --no-restore --no-build --include-source --output "packages/"'
      }
    }
  }
  
  post {
    always {
      step ([$class: 'MSTestPublisher', testResultsFile:"**/TestResults/UnitTests.trx", failOnError: true, keepLongStdio: true])
      cobertura coberturaReportFile: '**/coverage.cobertura.xml'
      archiveArtifacts artifacts: 'packages/**/*.jar', fingerprint: true
    }
  }
}