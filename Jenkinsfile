node('master') {    
    //拉取git代码仓库
    stage 'Checkout'
        checkout([$class: 'GitSCM', branches: [[name: '*/master']], doGenerateSubmoduleConfigurations: false, extensions: [[$class: 'CleanCheckout']],
　　　　　　　submoduleCfg: [], userRemoteConfigs: [[url: 'https://github.com/liuyuexclusive/practice']]])
            
    //构建
    stage 'deploy'
        bat '''
            cd C:\\Program Files (x86)\\Jenkins\\workspace\\test\\LY.AutoStart
            dotnet restore
            dotnet build
            dotnet publish -c Release
            cd C:\\Program Files (x86)\\Jenkins\\workspace\\test\\LY.AutoStart\\bin\\Release\\netcoreapp2.2\\publish
            dotnet LY.AutoStart.dll test services,vue
            '''
}