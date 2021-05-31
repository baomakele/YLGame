// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Fx/Distort/UVAnim"
{
    Properties 
    {
        _NoiseTex ("Distort Texture (RG)", 2D) = "white" {}
        _MainTex ("Alpha (A)", 2D) = "white" {}
        _HeatTime  ("Heat Time", range (-1,1)) = 0
        _ForceX  ("Strength X", range (0,1)) = 0.1
        _ForceY  ("Strength Y", range (0,1)) = 0.1
        _UVRotateSpeed ("UVRotateSpeed", Float) = 0
        _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        
        //[Enum(Add,1,Blend,5)] _SrcBlend ("Source Alpha Blend", Float) = 1
        [Enum(Add,1,Blend,10)] _DestBlend ("Dest Alpha Blend", Float) = 1
        [Enum(Off,0,On,2)] _CullBack ("Cull Back", Float) = 2
        [Enum(Off,4,On,8)] _ZTest ("Always Show", Float) = 4

		[KeywordEnum(Off, On)] _ALPHA_CUT("AlphaCut", Float) = 0
		_AlphaThreshold("AlphaThreshold", Range(0, 1)) = 0
    }
    
    CGINCLUDE
		#pragma multi_compile _ALPHA_CUT_OFF _ALPHA_CUT_ON
		#include "UnityCG.cginc"

        #pragma vertex vert
        #pragma fragment frag
        
        struct appdata_t {
            float4 vertex : POSITION;
            half2 texcoord: TEXCOORD0;
            fixed4 color : COLOR;
        };

        struct v2f {
            float4 vertex : POSITION;
            half2 uvmain : TEXCOORD0;
            fixed4 colorFactor : COLOR;
        };

        half _ForceX;
        half _ForceY;
        float _HeatTime;

        sampler2D _MainTex;
        half4 _MainTex_ST;
        sampler2D _NoiseTex;
        half4 _NoiseTex_ST;

        half _UVRotateSpeed;

        fixed4 _TintColor;

        v2f vert (appdata_t v)
        {
            v2f o;

            const half TWO_PI = 3.14159 * 2;
            const half2 VEC_CENTER = half2(0.5h, 0.5h);

            o.vertex = UnityObjectToClipPos(v.vertex);
            o.colorFactor = v.color * _TintColor * 2;
            o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );

            float time = _Time.z;
            
            float absUVRotSpeed = abs(_UVRotateSpeed);
            half2 finaluv = o.uvmain.xy;
            if (absUVRotSpeed > 0.0)
            {
                finaluv -= VEC_CENTER;
                
                half rotation = TWO_PI * frac(time * _UVRotateSpeed);
                half sin_rot = sin(rotation);
                half cos_rot = cos(rotation);
                
                finaluv = half2(
                    finaluv.x * cos_rot - finaluv.y * sin_rot,
                    finaluv.x * sin_rot + finaluv.y * cos_rot);
                    
                finaluv += VEC_CENTER;
            }

            o.uvmain.xy = finaluv;
            
            return o;
        }

		fixed _AlphaThreshold;
        fixed4 frag( v2f i ) : SV_Target
        {
            float4 time = _Time;
            
            //noise effect
            half offsetColor1 = tex2D(_NoiseTex, i.uvmain + frac(time.xz * _HeatTime));
            half offsetColor2 = tex2D(_NoiseTex, i.uvmain + frac(time.yx * _HeatTime));
            
            i.uvmain.x += (offsetColor1 - 0.5h) * _ForceX;
            i.uvmain.y += (offsetColor2 - 0.5h) * _ForceY;

			fixed4 ocolor = tex2D(_MainTex, i.uvmain) * i.colorFactor;
			//ocolor.a *= (ocolor.r + ocolor.g + ocolor.b);

#ifdef _ALPHA_CUT_ON
			clip(ocolor.a - _AlphaThreshold);
#endif
            return ocolor;
        }

    ENDCG
          
    SubShader 
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        
        Pass 
        {
            Tags { "LIGHTMODE"="Always" }
            Blend       SrcAlpha [_DestBlend]
            Cull        [_CullBack]
            ZTest       [_ZTest]
            Lighting    Off 
            ZWrite      Off
        
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            ENDCG
        }
    }
}
