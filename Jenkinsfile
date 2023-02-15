#!/groovy
@Library('cx-jenkins-pipeline-kit')

def ipAddress
def vmName = "Plugin-VisualStudio-" + UUID.randomUUID().toString()
def msbuildLocationVS2019 = "\"C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Enterprise\\MSBuild\\Current\\Bin\\MSBuild.exe\""
def msbuildLocationVS2022 = "\"C:\\Program Files\\Microsoft Visual Studio\\2022\\Professional\\MSBuild\\Current\\Bin\\MSBuild.exe\""

pipeline {
    parameters {
        string(name: "decommissionPeriod", defaultValue: "3 hour", description: "Decommission period")
		choice(name: "templateName", choices: ["VisualStudio2019-Template","VisualStudio2022-Template"], description: "Template name, if not specified then \"VisualStudio2019-Template\" template will be chosen")
        booleanParam(name: 'doNotDeleteVM', defaultValue: false, description: 'If selected, VM will not be deleted after process finished')
		gitParameter branchFilter: 'origin/(.*)', defaultValue: 'master', name: 'branch', type: 'PT_BRANCH'
    }
    agent { node { label 'install01' } }
    stages {
        stage('Create VM') {
            steps {
                script {
                    kit.Create_Vm_Terraform(vmName, templateName, "18000", "8", "VMWARE", decommissionPeriod, "Auto", "Plugins-Developers")
                    ipAddress = kit.getIpAddress(vmName, "VMWARE")
                    kit.Create_Jenkins_Slave_On_Master(vmName)
                    kit.Start_Jenkins_Slave_On_Windows_Pstools(ipAddress, vmName)
                }
            }
        }

        stage('Build and Pack') {
            agent { node { label vmName } }
            steps {
                script {
                    if(templateName == "VisualStudio2019-Template")
					{
						echo ${WORKSPACE}
						echo ${JOB_NAME}
						 // bat "${msbuildLocationVS2019} ${WORKSPACE}\\${JOB_NAME}\\ci.msbuild /t:CI /p:VisualStudioVersion=\"16.0\" /p:DeployExtension=false"
						bat "${msbuildLocationVS2019} ${WORKSPACE}\\ci.msbuild /t:CI /p:VisualStudioVersion=\"16.0\" /p:DeployExtension=false"
					}
					else {
						//bat "${msbuildLocationVS2022} ${WORKSPACE}\\${JOB_NAME}\\ci.msbuild /t:CI /p:VisualStudioVersion=\"17.0\" /p:DeployExtension=false"
						bat "${msbuildLocationVS2022} ${WORKSPACE}\\ci.msbuild /t:CI /p:VisualStudioVersion=\"17.0\" /p:DeployExtension=false"
					}
                }
            }
        }
		
		stage('Upload To Artifactory') {
            agent { node { label vmName } }
            steps {
                script {
		    if(templateName == "VisualStudio2019-Template")
					{
						fileOperations([folderRenameOperation(source: "${WORKSPACE}\\Artifacts\\CxViewerVSIX-2019.vsix", destination: "${WORKSPACE}\\${JOB_NAME}\\Artifacts\\CxViewerVSIX-9.00.19.vsix")])
						kit.Upload_To_Artifactory("${WORKSPACE}\\Artifacts\\CxViewerVSIX-9.00.19.vsix", "plugins-release-local/com/checkmarx/visual-studio/")
					}
					else 
					{
						fileOperations([folderRenameOperation(source: "${WORKSPACE}\\Artifacts\\CxViewerVSIX-2022.vsix", destination: "${WORKSPACE}\\${JOB_NAME}\\Artifacts\\CxViewerVSIX-9.00.19.vsix")])
						kit.Upload_To_Artifactory("${WORKSPACE}\\Artifacts\\CxViewerVSIX-9.00.19.vsix", "plugins-release-local/com/checkmarx/visual-studio/")
					}
                }
            }
        }
    }

    post {
        always {
            script {
               // logstashSend failBuild: false, maxLines: 1000
                if (ipAddress != null) {
                    try {
                        if (doNotDeleteVM == 'true') {
                            kit.Info_Msg("Not deleting VM since user chose to keep it")
						} else {
							deleteVm("VMWARE", ipAddress, vmName)
						}
                    } catch (Exception e) {
                        kit.Warning_Msg("Failed to delete vm. Exception:\n" + e.toString())
                        currentBuild.result = 'UNSTABLE'
                    }
                }
            }
        }
        cleanup {
            cleanWs()
        }
    }
}
