﻿Shader "RetroFilters/ScanLines"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Tiling("Tiling", Float) = 0.0
		_Contrast("Contrast", Float) = 0.5
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
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _Tiling;
			float _Contrast;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				float multiplyer = ((i.vertex.y/i.vertex.w * _ScreenParams.y/_Tiling)%2);
				return col * multiplyer;
			}
			ENDCG
		}
	}
}
