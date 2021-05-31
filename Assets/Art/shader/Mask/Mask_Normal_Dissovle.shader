// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Fx/Mask/Normal_Dissovle"
{
    Properties {
        _MainTex ("Base", 2D) = "white" {}
        _MaskTex ("Mask", 2D) = "white" {}
        _TintColor ("TintColor", Color) = (1.0, 1.0, 1.0, 1.0)
        _Intensity("Intensity", Float) = 1
        
        //[Enum(Add,1,Blend,5)] _SrcBlend ("Source Alpha Blend", Float) = 1
        [Enum(Add,1,Blend,10)] _DestBlend ("Dest Alpha Blend", Float) = 1
        [Enum(Off,0,On,2)] _CullBack ("Cull Back", Float) = 2
        [Enum(Off,4,On,8)] _ZTest ("Always Show", Float) = 4

		_DissovleTex("Dissovle", 2D) = "white" {}
		_AlphaThreshold("Alpha Threshold", Range(-0.1, 1.01)) = 0

		_DissovleColor("Dissovle Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_DissovleRange("Dissovle Range", Range(0, 1)) = 0
    }
    
    CGINCLUDE
        #include "UnityCG.cginc"
        #pragma vertex vert
        #pragma fragment frag
                
        sampler2D _MainTex;
        half4 _MainTex_ST;

        sampler2D _MaskTex;
        half4 _MaskTex_ST;

		sampler2D _DissovleTex;
		half4 _DissovleTex_ST;

		half4 _DissovleColor;
		half _DissovleRange;

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
			half2 uv1 : TEXCOORD1;
            half2 uv2 : TEXCOORD2;
            half4 colorFactor : COLOR;
        };

        v2f vert(appdata_t v)
        {
        	v2f o; 

            float4 inPos = v.vertex;
            
            o.pos = UnityObjectToClipPos(inPos);
            o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
            o.uv1.xy = TRANSFORM_TEX(v.texcoord, _MaskTex);
			o.uv2.xy = TRANSFORM_TEX(v.texcoord, _DissovleTex);
            
            o.colorFactor = v.color * _TintColor * _Intensity;  
            
            return o; 
        }

		fixed _AlphaThreshold;
        fixed4 frag( v2f i ) : SV_Target 
        {
			half4 maskcolor = tex2D(_MaskTex, i.uv1.xy);

			half4 fadecolor = tex2D(_DissovleTex, i.uv2.xy) * maskcolor;
			half deltaAlpha = fadecolor.r - _AlphaThreshold;
			clip(deltaAlpha);

			fixed4 ocolor = tex2D(_MainTex, i.uv.xy);
			half4 orgColor = half4(ocolor * maskcolor) * i.colorFactor;

			//half4 dissovleColor = clamp(_DissovleRange * pow(1 - deltaAlpha, 5), 0, 1) * _DissovleColor;
			//half4 dissovleColor = half4(0,0,0,1);// = clamp(_DissovleRange * pow(1 - deltaAlpha, 5), 0, 1) * _DissovleColor;

			if (deltaAlpha < _DissovleRange)
			{
				ocolor = _DissovleColor;// half4((dissovleColor).rgb, max(orgColor.a, 1 - deltaAlpha));
			}
			else
			{
				ocolor = orgColor;
			}

            return ocolor;
        }
    ENDCG
    
    SubShader 
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        
        Pass {
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
