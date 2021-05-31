// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Fx/Simple/Normal" 
{
    Properties {
        _MainTex ("Base", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _Intensity("Intensity", Float) = 1
        
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

        sampler2D _MainTex;
        half4 _MainTex_ST;

        half4 _TintColor;
        half _Intensity;

        struct appdata_t {
            float4 vertex : POSITION;
            half2 texcoord : TEXCOORD0;
            half4 color : COLOR;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            half2 uv : TEXCOORD0;
            half4 colorFactor : COLOR;
        };

        v2f vert(appdata_t v) 
        {
        	v2f o;

            float4 inPos = v.vertex;
            
            o.pos = UnityObjectToClipPos(inPos);
            o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
            o.colorFactor = v.color * _TintColor * _Intensity;
            
            return o; 
        }

		fixed _AlphaThreshold;
        fixed4 frag( v2f i ) : SV_Target 
        {    
            fixed4 ocolor = tex2D (_MainTex, i.uv.xy) * i.colorFactor;

#ifdef _ALPHA_CUT_ON
			clip(ocolor.a - _AlphaThreshold);
#endif
            return ocolor;
        }
    ENDCG
    
    SubShader 
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        
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
