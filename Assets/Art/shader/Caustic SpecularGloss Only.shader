// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Ciconia Studio/Effects/Water And Caustic/Caustic/Only Caustic" {
    Properties {
        [Space(35)]_CausticColor ("Color", Color) = (1,1,1,1)
        _DetailMask ("Caustic", 2D) = "black" {}
        _Intensity ("Intensity", Float ) = 1
        [Space(20)]_causticAngle1 ("Angle1", Float ) = 0
        _CausticAnimationSpeed1 ("Animation Speed1", Range(0, 1)) = 0.05
        [Space(10)]_CausticAngle2 ("Angle2", Float ) = 0
        _CausticAnimationSpeed2 ("Animation Speed2", Range(0, 1)) = 0.05
        [Space(35)]_ExclusionMask ("Exclusion Mask", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _CausticAnimationSpeed2;
            uniform float _CausticAnimationSpeed1;
            uniform sampler2D _DetailMask; uniform float4 _DetailMask_ST;
            uniform float _CausticAngle2;
            uniform float _causticAngle1;
            uniform float4 _CausticColor;
            uniform float _Intensity;
            uniform sampler2D _ExclusionMask; uniform float4 _ExclusionMask_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                UNITY_FOG_COORDS(7)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD8;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - 0;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float node_9500_ang = ((_causticAngle1*3.141592654)/180.0);
                float node_9500_spd = 1.0;
                float node_9500_cos = cos(node_9500_spd*node_9500_ang);
                float node_9500_sin = sin(node_9500_spd*node_9500_ang);
                float2 node_9500_piv = float2(0.5,0.5);
                float2 node_9500 = (mul(i.uv0-node_9500_piv,float2x2( node_9500_cos, -node_9500_sin, node_9500_sin, node_9500_cos))+node_9500_piv);
                float4 node_5098 = _Time + _TimeEditor;
                float2 UVCaustic = (i.uv0/4.0);
                float2 node_2689 = (node_9500+(UVCaustic+(node_5098.g*_CausticAnimationSpeed1)*float2(0,0.6)));
                float4 _Caustic1 = tex2D(_DetailMask,TRANSFORM_TEX(node_2689, _DetailMask));
                float node_6185_ang = ((_CausticAngle2*3.141592654)/180.0);
                float node_6185_spd = 1.0;
                float node_6185_cos = cos(node_6185_spd*node_6185_ang);
                float node_6185_sin = sin(node_6185_spd*node_6185_ang);
                float2 node_6185_piv = float2(0.5,0.5);
                float2 node_6185 = (mul(i.uv0-node_6185_piv,float2x2( node_6185_cos, -node_6185_sin, node_6185_sin, node_6185_cos))+node_6185_piv);
                float4 node_2146 = _Time + _TimeEditor;
                float2 node_3516 = (node_6185+(1.0 - (UVCaustic+(node_2146.g*_CausticAnimationSpeed2)*float2(0,0.6))));
                float4 _Caustic2 = tex2D(_DetailMask,TRANSFORM_TEX(node_3516, _DetailMask));
                float4 _ExclusionMask_var = tex2D(_ExclusionMask,TRANSFORM_TEX(i.uv0, _ExclusionMask));
                float Caustic = ((saturate((_Caustic1.r*_Caustic2.r))*_Intensity)*_ExclusionMask_var.r);
                float3 node_1725 = (_CausticColor.rgb*Caustic);
                float3 diffuseColor = node_1725;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = node_1725;
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(finalColor,Caustic);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _CausticAnimationSpeed2;
            uniform float _CausticAnimationSpeed1;
            uniform sampler2D _DetailMask; uniform float4 _DetailMask_ST;
            uniform float _CausticAngle2;
            uniform float _causticAngle1;
            uniform float4 _CausticColor;
            uniform float _Intensity;
            uniform sampler2D _ExclusionMask; uniform float4 _ExclusionMask_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float node_9500_ang = ((_causticAngle1*3.141592654)/180.0);
                float node_9500_spd = 1.0;
                float node_9500_cos = cos(node_9500_spd*node_9500_ang);
                float node_9500_sin = sin(node_9500_spd*node_9500_ang);
                float2 node_9500_piv = float2(0.5,0.5);
                float2 node_9500 = (mul(i.uv0-node_9500_piv,float2x2( node_9500_cos, -node_9500_sin, node_9500_sin, node_9500_cos))+node_9500_piv);
                float4 node_5098 = _Time + _TimeEditor;
                float2 UVCaustic = (i.uv0/4.0);
                float2 node_2689 = (node_9500+(UVCaustic+(node_5098.g*_CausticAnimationSpeed1)*float2(0,0.6)));
                float4 _Caustic1 = tex2D(_DetailMask,TRANSFORM_TEX(node_2689, _DetailMask));
                float node_6185_ang = ((_CausticAngle2*3.141592654)/180.0);
                float node_6185_spd = 1.0;
                float node_6185_cos = cos(node_6185_spd*node_6185_ang);
                float node_6185_sin = sin(node_6185_spd*node_6185_ang);
                float2 node_6185_piv = float2(0.5,0.5);
                float2 node_6185 = (mul(i.uv0-node_6185_piv,float2x2( node_6185_cos, -node_6185_sin, node_6185_sin, node_6185_cos))+node_6185_piv);
                float4 node_2146 = _Time + _TimeEditor;
                float2 node_3516 = (node_6185+(1.0 - (UVCaustic+(node_2146.g*_CausticAnimationSpeed2)*float2(0,0.6))));
                float4 _Caustic2 = tex2D(_DetailMask,TRANSFORM_TEX(node_3516, _DetailMask));
                float4 _ExclusionMask_var = tex2D(_ExclusionMask,TRANSFORM_TEX(i.uv0, _ExclusionMask));
                float Caustic = ((saturate((_Caustic1.r*_Caustic2.r))*_Intensity)*_ExclusionMask_var.r);
                float3 node_1725 = (_CausticColor.rgb*Caustic);
                float3 diffuseColor = node_1725;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * Caustic,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _CausticAnimationSpeed2;
            uniform float _CausticAnimationSpeed1;
            uniform sampler2D _DetailMask; uniform float4 _DetailMask_ST;
            uniform float _CausticAngle2;
            uniform float _causticAngle1;
            uniform float4 _CausticColor;
            uniform float _Intensity;
            uniform sampler2D _ExclusionMask; uniform float4 _ExclusionMask_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float node_9500_ang = ((_causticAngle1*3.141592654)/180.0);
                float node_9500_spd = 1.0;
                float node_9500_cos = cos(node_9500_spd*node_9500_ang);
                float node_9500_sin = sin(node_9500_spd*node_9500_ang);
                float2 node_9500_piv = float2(0.5,0.5);
                float2 node_9500 = (mul(i.uv0-node_9500_piv,float2x2( node_9500_cos, -node_9500_sin, node_9500_sin, node_9500_cos))+node_9500_piv);
                float4 node_5098 = _Time + _TimeEditor;
                float2 UVCaustic = (i.uv0/4.0);
                float2 node_2689 = (node_9500+(UVCaustic+(node_5098.g*_CausticAnimationSpeed1)*float2(0,0.6)));
                float4 _Caustic1 = tex2D(_DetailMask,TRANSFORM_TEX(node_2689, _DetailMask));
                float node_6185_ang = ((_CausticAngle2*3.141592654)/180.0);
                float node_6185_spd = 1.0;
                float node_6185_cos = cos(node_6185_spd*node_6185_ang);
                float node_6185_sin = sin(node_6185_spd*node_6185_ang);
                float2 node_6185_piv = float2(0.5,0.5);
                float2 node_6185 = (mul(i.uv0-node_6185_piv,float2x2( node_6185_cos, -node_6185_sin, node_6185_sin, node_6185_cos))+node_6185_piv);
                float4 node_2146 = _Time + _TimeEditor;
                float2 node_3516 = (node_6185+(1.0 - (UVCaustic+(node_2146.g*_CausticAnimationSpeed2)*float2(0,0.6))));
                float4 _Caustic2 = tex2D(_DetailMask,TRANSFORM_TEX(node_3516, _DetailMask));
                float4 _ExclusionMask_var = tex2D(_ExclusionMask,TRANSFORM_TEX(i.uv0, _ExclusionMask));
                float Caustic = ((saturate((_Caustic1.r*_Caustic2.r))*_Intensity)*_ExclusionMask_var.r);
                float3 node_1725 = (_CausticColor.rgb*Caustic);
                o.Emission = node_1725;
                
                float3 diffColor = node_1725;
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
