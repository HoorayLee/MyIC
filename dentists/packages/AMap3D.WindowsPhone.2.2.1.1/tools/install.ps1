param($installPath, $toolsPath, $package, $project)

$a='$(ProjectDir)'

$part1 = 

'
IF $(ConfigurationName) == Debug IF $(PlatformName) == ARM goto DebugARM
IF $(ConfigurationName) == Debug IF $(PlatformName) == x86 goto Debugx86
IF $(ConfigurationName) == Release IF $(PlatformName) == ARM goto ReleaseARM
IF $(ConfigurationName) == Release IF $(PlatformName) == x86 goto Releasex86
'
$part2 =

"
:DebugARM
echo build SDK-DebugARM
copy `"$installPath\AMAP3DEngine\lib\arm\debug\LibMap.dll`" `"$a\LibMap.dll`" /Y
copy `"$installPath\AMAP3DEngine\references\arm\debug\AMapSDKV2Comp.dll`" `"$installPath\lib\AMapSDKV2Comp.dll`" /Y
exit 0

:Debugx86
echo build SDK-Debugx86
copy `"$installPath\AMAP3DEngine\lib\x86\debug\LibMap.dll`" `"$a\LibMap.dll`" /Y
copy `"$installPath\AMAP3DEngine\references\x86\debug\AMapSDKV2Comp.dll`" `"$installPath\lib\AMapSDKV2Comp.dll`" /Y
exit 0

:ReleaseARM
echo build SDK-ReleaseARM
copy `"$installPath\AMAP3DEngine\lib\arm\release\LibMap.dll`" `"$a\LibMap.dll`" /Y
copy `"$installPath\AMAP3DEngine\references\arm\release\AMapSDKV2Comp.dll`" `"$installPath\lib\AMapSDKV2Comp.dll`" /Y
exit 0

:Releasex86
echo build SDK-Releasex86
copy `"$installPath\AMAP3DEngine\lib\x86\release\LibMap.dll`" `"$a\LibMap.dll`" /Y
copy `"$installPath\AMAP3DEngine\references\x86\release\AMapSDKV2Comp.dll`" `"$installPath\lib\AMapSDKV2Comp.dll`" /Y
exit 0
"


$project.Properties.Item("PreBuildEvent").Value=$part1+$part2



$FolderAssets = "Assets";	
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("bktile.data").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("DrawLinePixelShader.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("DrawLineVertexShader.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("DrawPolygonPixelShader.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("DrawPolygonVertexShader.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("ft2.data").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("icn_hv3.data").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("icn_lv3.data").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("InsertPicturePixelShader.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("InsertPictureVertexShader.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("lr.data").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("lt.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("ltc.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("oc.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("ov2.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("ov3.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("ra.data").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("rd.data").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("style_lv3.data").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("style_slv3.data").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("v3t.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("vvt.cso").Properties.Item("BuildAction").Value = [int]2
(Get-Project).ProjectItems.Item($FolderAssets).ProjectItems.Item("wk.data").Properties.Item("BuildAction").Value = [int]2


#get the WMAppManifest.xml file (for Windows Phone)
$properties = $project.ProjectItems | where {$_.Name -eq "Properties"}
if($properties){
	$manifest = $properties.ProjectItems | where {$_.Name -eq "WMAppManifest.xml"}
	if($manifest){
		# find the WMAppManifest.xml path on the file system
		$localPath = $manifest.Properties | where {$_.Name -eq "LocalPath"}

		# load the WMAppManifest.xml as an XML doc
		$xml = New-Object xml
		$xml.Load($localPath.Value)

		#check to see if the ID_CAP_IDENTITY_DEVICE is available
		$idCap = $xml.Deployment.App.Capabilities.Capability | where {$_.Name -eq "ID_CAP_IDENTITY_DEVICE"}
		if(!$idCap){
			#Add the capability if it's not available
			$newNode = $xml.CreateElement("Capability")
			$newNode.SetAttribute("Name", "ID_CAP_IDENTITY_DEVICE")

			$xml.Deployment.App.Capabilities.AppendChild($newNode)

			$xml.Save($localPath.Value)
		}
		
		
				
		#check to see if the ID_CAP_LOCATION is available
		$idCap = $xml.Deployment.App.Capabilities.Capability | where {$_.Name -eq "ID_CAP_LOCATION"}
		if(!$idCap){
			#Add the capability if it's not available
			$newNode = $xml.CreateElement("Capability")
			$newNode.SetAttribute("Name", "ID_CAP_LOCATION")

			$xml.Deployment.App.Capabilities.AppendChild($newNode)

			$xml.Save($localPath.Value)
		}
	}
}
