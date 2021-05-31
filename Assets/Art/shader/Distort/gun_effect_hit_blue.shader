// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.06 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.06;sub:START;pass:START;ps:flbk:,lico:0,lgpr:1,nrmq:0,limd:0,uamb:True,mssp:True,lmpd:False,lprd:True,rprd:False,enco:False,frtr:True,vitr:True,dbil:True,rmgx:True,rpth:0,hqsc:False,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:False,dith:0,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.1280277,fgcg:0.1953466,fgcb:0.2352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32379,y:32926,varname:node_1,prsc:2|emission-128-OUT,alpha-127-A;n:type:ShaderForge.SFN_Tex2d,id:30,x:31722,y:32626,ptovrint:False,ptlb:diffuse,ptin:_diffuse,varname:node_9894,prsc:2,ntxv:0,isnm:False|UVIN-88-OUT;n:type:ShaderForge.SFN_Multiply,id:44,x:31144,y:32817,varname:node_44,prsc:2|A-106-T,B-100-OUT;n:type:ShaderForge.SFN_Vector4Property,id:60,x:30723,y:32855,ptovrint:False,ptlb:speed,ptin:_speed,varname:node_3656,prsc:2,glob:False,v1:1,v2:1,v3:1,v4:1;n:type:ShaderForge.SFN_Tex2d,id:64,x:30894,y:33046,ptovrint:False,ptlb:diffuse_noise,ptin:_diffuse_noise,varname:node_5151,prsc:2,ntxv:0,isnm:False|UVIN-111-OUT;n:type:ShaderForge.SFN_Multiply,id:70,x:30453,y:32963,varname:node_70,prsc:2|A-106-T,B-104-OUT;n:type:ShaderForge.SFN_Vector4Property,id:72,x:30047,y:33129,ptovrint:False,ptlb:speed_noise,ptin:_speed_noise,varname:node_8407,prsc:2,glob:False,v1:1,v2:1,v3:1,v4:1;n:type:ShaderForge.SFN_Add,id:88,x:31582,y:33043,varname:node_88,prsc:2|A-107-OUT,B-126-OUT;n:type:ShaderForge.SFN_Multiply,id:89,x:31144,y:33067,varname:node_89,prsc:2|A-64-R,B-90-OUT;n:type:ShaderForge.SFN_ValueProperty,id:90,x:30895,y:33269,ptovrint:False,ptlb:blend,ptin:_blend,varname:node_9463,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_ComponentMask,id:100,x:30910,y:32855,varname:node_100,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-60-XYZ;n:type:ShaderForge.SFN_ComponentMask,id:104,x:30237,y:33106,varname:node_104,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-72-XYZ;n:type:ShaderForge.SFN_Time,id:106,x:30234,y:32781,varname:node_106,prsc:2;n:type:ShaderForge.SFN_Add,id:107,x:31392,y:32679,varname:node_107,prsc:2|A-108-UVOUT,B-44-OUT;n:type:ShaderForge.SFN_TexCoord,id:108,x:30474,y:32422,varname:node_108,prsc:2,uv:0;n:type:ShaderForge.SFN_Add,id:111,x:30686,y:33046,varname:node_111,prsc:2|A-70-OUT,B-108-UVOUT;n:type:ShaderForge.SFN_Multiply,id:114,x:31161,y:33280,varname:node_114,prsc:2|A-64-G,B-116-OUT;n:type:ShaderForge.SFN_ValueProperty,id:116,x:30912,y:33482,ptovrint:False,ptlb:blend_02,ptin:_blend_02,varname:node_5512,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Add,id:126,x:31417,y:33170,varname:node_126,prsc:2|A-89-OUT,B-114-OUT;n:type:ShaderForge.SFN_VertexColor,id:127,x:31582,y:33258,varname:node_127,prsc:2;n:type:ShaderForge.SFN_Multiply,id:128,x:32165,y:33037,varname:node_128,prsc:2|A-129-OUT,B-131-OUT;n:type:ShaderForge.SFN_Multiply,id:129,x:32079,y:32722,varname:node_129,prsc:2|A-134-OUT,B-130-OUT;n:type:ShaderForge.SFN_ValueProperty,id:130,x:31829,y:33105,ptovrint:False,ptlb:dif_power,ptin:_dif_power,varname:node_4987,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:131,x:32068,y:33229,varname:node_131,prsc:2|A-127-RGB,B-132-RGB;n:type:ShaderForge.SFN_Color,id:132,x:31582,y:33442,ptovrint:False,ptlb:color,ptin:_color,varname:node_5808,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:134,x:31905,y:32682,varname:node_134,prsc:2|A-30-RGB,B-132-RGB;proporder:30-60-64-72-90-116-130-132;pass:END;sub:END;*/

Shader "Custom/gun_effect_hit_blue" {
    Properties {
        _diffuse ("diffuse", 2D) = "white" {}
        _speed ("speed", Vector) = (1,1,1,1)
        _diffuse_noise ("diffuse_noise", 2D) = "white" {}
        _speed_noise ("speed_noise", Vector) = (1,1,1,1)
        _blend ("blend", Float ) = 0
        _blend_02 ("blend_02", Float ) = 0
        _dif_power ("dif_power", Float ) = 1
        _color ("color", Color) = (1,1,1,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH_PROBE ( defined (LIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash 
//            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _diffuse; uniform half4 _diffuse_ST;
            uniform half4 _speed;
            uniform sampler2D _diffuse_noise; uniform half4 _diffuse_noise_ST;
            uniform half4 _speed_noise;
            uniform half _blend;
            uniform half _blend_02;
            uniform half _dif_power;
            uniform half4 _color;
            struct VertexInput {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
				float4 node_106 = _Time + _TimeEditor;
				float2 node_111 = ((node_106.g*_speed_noise.rgb.rg) + i.uv0);
                half4 _diffuse_noise_var = tex2D(_diffuse_noise,TRANSFORM_TEX(node_111, _diffuse_noise));
				float2 node_88 = ((i.uv0 + (node_106.g*_speed.rgb.rg)) + ((_diffuse_noise_var.r*_blend) + (_diffuse_noise_var.g*_blend_02)));
                half4 _diffuse_var = tex2D(_diffuse,TRANSFORM_TEX(node_88, _diffuse));
                fixed3 emissive = (((_diffuse_var.rgb*_color.rgb)*_dif_power)*(i.vertexColor.rgb*_color.rgb));
                fixed3 finalColor = emissive;
                return fixed4(finalColor,i.vertexColor.a);
            }
            ENDCG
        }
    }
//    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
