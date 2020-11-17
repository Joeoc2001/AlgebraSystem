pipeline {
  agent {
    docker {
      image 'mcr.microsoft.com/dotnet/sdk:3.1'
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
        sh 'dotnet pack AlgebraSystem --no-restore --no-build --include-source --output "tmp/packages/"'
      }
    }
  }
  
  post {
    always {
      sh "find / -iname '*log.txt'"
      archiveArtifacts artifacts: 'AlgebraSystem/log.txt', fingerprint: true
      step ([$class: 'MSTestPublisher', testResultsFile:"**/TestResults/UnitTests.trx", failOnError: false, keepLongStdio: true])
      cobertura coberturaReportFile: '**/coverage.cobertura.xml'
      archiveArtifacts artifacts: 'tmp/packages/*', fingerprint: true
      archiveArtifacts artifacts: 'AlgebraSystem/tmp/documentation.xml', fingerprint: true
    }
  }
}