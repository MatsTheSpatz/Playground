﻿<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Recipe" generation="1" functional="0" release="0" Id="8cf84f2e-d239-4a90-b046-092b59211def" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="RecipeGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="RecipeWebRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Recipe/RecipeGroup/LB:RecipeWebRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="RecipeWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Recipe/RecipeGroup/MapRecipeWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="RecipeWebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Recipe/RecipeGroup/MapRecipeWebRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:RecipeWebRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Recipe/RecipeGroup/RecipeWebRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapRecipeWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Recipe/RecipeGroup/RecipeWebRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapRecipeWebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Recipe/RecipeGroup/RecipeWebRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="RecipeWebRole" generation="1" functional="0" release="0" software="C:\GitHub\Playground\Recipe\Recipe\csx\Debug\roles\RecipeWebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;RecipeWebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;RecipeWebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Recipe/RecipeGroup/RecipeWebRoleInstances" />
            <sCSPolicyFaultDomainMoniker name="/Recipe/RecipeGroup/RecipeWebRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="RecipeWebRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="RecipeWebRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="df6dc1db-281f-407b-87ca-d777fbc97bf8" ref="Microsoft.RedDog.Contract\ServiceContract\RecipeContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="9fa3d874-96df-430b-89ae-d7e93fe35060" ref="Microsoft.RedDog.Contract\Interface\RecipeWebRole:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/Recipe/RecipeGroup/RecipeWebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>