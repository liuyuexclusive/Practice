node('master') {    
    //拉取git代码仓库
    stage 'Checkout'
        checkout([$class: 'GitSCM', branches: [[name: '*/master']], doGenerateSubmoduleConfigurations: false, extensions: [[$class: 'CleanCheckout']],
　　　　　　　submoduleCfg: [], userRemoteConfigs: [[url: 'https://github.com/liuyuexclusive/practice']]])

    //构建
    stage 'deploy'
        sh '''
           chmod 777 /var/jenkins_home/workspace/test/deploy.sh
           /var/jenkins_home/workspace/test/deploy.sh
           '''
            
    // //构建
    // stage 'deploy'
    //     bat '''
    //         cd C:\\Program Files (x86)\\Jenkins\\workspace\\test\\LY.AutoStart
    //         dotnet restore
    //         dotnet build
    //         dotnet publish -c Release
    //         cd C:\\Program Files (x86)\\Jenkins\\workspace\\test\\LY.AutoStart\\bin\\Release\\netcoreapp2.2\\publish
    //         dotnet LY.AutoStart.dll test all
    //         '''
}