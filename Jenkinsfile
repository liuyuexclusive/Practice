node('master') {     //master节点运行，以下stage也可指定节点
    stage 'Prepare'  //清空发布目录
        bat '''if exist C:\\Users\\liuyu\\Desktop\\testJenkins (rd/s/q C:\\Users\\liuyu\\Desktop\\testJenkins)
               if exist C:\\Users\\Administrator\\.nuget (rd/s/q C:\\Users\\Administrator\\.nuget)
               exit'''
   
    //拉取git代码仓库
    stage 'Checkout'
        checkout([$class: 'GitSCM', branches: [[name: '*/master']], doGenerateSubmoduleConfigurations: false, extensions: [], 
　　　　　　　submoduleCfg: [], userRemoteConfigs: [[credentialsId: 'c6d98bbd-5cfb-4e26-aa56-f70b054b350d', 
            url: 'https://github.com/liuyuexclusive/practice']]])
            
    //构建
    stage 'Build'
        bat '''cd "C:\\Program Files (x86)\\Jenkins\\workspace\\test\\LY.SysService"
            dotnet restore
            dotnet build
            dotnet publish --configuration Release --output C:\\Users\\liuyu\\Desktop\\testJenkins'''

    //     //部署
    // stage 'Deploy'
    //     bat '''
    //     cd C:\\Users\\liuyu\\Desktop\\testJenkins
    //     dotnet LY.SysService.dll
    //     '''
}