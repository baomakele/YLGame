// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Fx/Simple/Sequence" 
{
    Properties {
        _MainTex ("Base", 2D) = "white" {}
        _TintColor ("Tint Color", Color)= (1,1,1,1)
        _Intensity("Intensity", Float)  = 1
        
        _Columns("Columns", Float)    = 4
        _Rows("Rows", Float)       = 4
        _TotalImages("TotalImages", Float)= 16
        _FPS("FPS", Float)        = 12
        
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
        //half4 _MainTex_ST;

        half4 _TintColor;
        half _Intensity;
        
        half _Columns;
        half _Rows;
        float _TotalImages;
        float _FPS;
        
        struct appdata_t 
        {
            float4 vertex : POSITION;
            half2 texcoord : TEXCOORD0;
            half4 color : COLOR;
        };

        struct v2f 
        {
            float4 pos : SV_POSITION;
            half2 uv : TEXCOORD0;
            half4 colorFactor : COLOR;
        };


        v2f vert(appdata_t v) 
        {
        	v2f o;

            const half2 stepXY = half2(1 / _Columns, 1 / _Rows);

            float time = _Time.y;
            
            float frameId   = floor(frac(time * _FPS / _TotalImages) * _TotalImages);
            half rowId      = floor(((half)frameId + 0.001) / _Columns);
            half2 baseId    = half2(frameId - rowId * _Columns, _Rows - (rowId + 1));
            
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv.xy = (baseId + v.texcoord) * stepXY;
            o.colorFactor = v.color * _TintColor * _Intensity;
            
            return o; 
        }

		fixed _AlphaThreshold;
		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 ocolor = tex2D(_MainTex, i.uv.xy) * i.colorFactor;

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
