node('master') {     //master节点运行，以下stage也可指定节点
    stage 'Prepare'
        bat '''iisreset /stop'''
    
    //拉取git代码仓库
    stage 'Checkout'
        checkout([$class: 'GitSCM', branches: [[name: '*/master']], doGenerateSubmoduleConfigurations: false, extensions: [[$class: 'CleanCheckout']],
　　　　　　　submoduleCfg: [], userRemoteConfigs: [[credentialsId: 'c6d98bbd-5cfb-4e26-aa56-f70b054b350d', 
            url: 'https://github.com/liuyuexclusive/practice']]])
            
    //构建
    stage 'Build'
        bat '''
            cd "C:\\Program Files (x86)\\Jenkins\\workspace\\test\\LY.APIGateway"
            dotnet publish -c release --output D:\\MyFiles\\Code\\practicePublish\\LY.APIGateway
            cd "C:\\Program Files (x86)\\Jenkins\\workspace\\test\\LY.SysService"
            dotnet publish -c release --output D:\\MyFiles\\Code\\practicePublish\\LY.SysService
            cd "C:\\Program Files (x86)\\Jenkins\\workspace\\test\\LY.OrderService"
            dotnet publish -c release --output D:\\MyFiles\\Code\\practicePublish\\LY.OrderService
            cd "C:\\Program Files (x86)\\Jenkins\\workspace\\test\\LY.DaemonService"
            dotnet publish -c release --output D:\\MyFiles\\Code\\practicePublish\\LY.DaemonService
            '''
    
    stage 'deploy'
       bat '''iisreset /start''' 

}