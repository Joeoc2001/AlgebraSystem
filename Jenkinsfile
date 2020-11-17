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

    stage('Document') {
      steps {
        sh 'dotnet add AlgebraSystem package docfx.console --version 2.56.5'
		sh '/tmp/dotnet_cli/.nuget/packages/docfx.console/2.56.5/tools/docfx.exe AlgebraSystem/docfx.json'
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
      step ([$class: 'MSTestPublisher', testResultsFile:"**/TestResults/UnitTests.trx", failOnError: false, keepLongStdio: true])
      cobertura coberturaReportFile: '**/coverage.cobertura.xml'
      archiveArtifacts artifacts: 'tmp/packages/*', fingerprint: true
      archiveArtifacts artifacts: 'AlgebraSystem/tmp/documentation.xml', fingerprint: true
    }
  }
}