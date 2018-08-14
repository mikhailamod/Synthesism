Shader "Synth/Glow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_GlowColor("Color", Color) = (1,0,0,1)
		_GlowIntensity ("Glow Intensity", Range(0,3)) = 1.0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			half4 _MainTex_ST;
			half4 _GlowColor : Color;
			half _GlowIntensity;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				return o;
			}
			
			

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				
				col *= _GlowColor;
				col *= _GlowIntensity;

				return col;
			}
			ENDCG
		}
	}
}
