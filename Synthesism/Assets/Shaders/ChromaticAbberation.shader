Shader "RetroFilters/ChromaticAbberation"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_rOffset ("Red Offset", Float) = 0.0
		_gOffset("Red Offset", Float) = 0.0
		_bOffset("Red Offset", Float) = 0.0
		_Factor("Factor", Float) = 1.0
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

			uniform float _rOffset;
			uniform float _gOffset;
			uniform float _bOffset;
			uniform float _Factor;

			fixed4 frag (v2f i) : SV_Target
			{
				_rOffset /= _Factor;
				_gOffset /= _Factor;
				_bOffset /= _Factor;

				//Red Channel
				float4 red = tex2D(_MainTex, i.uv + _rOffset);
				//Green Channel
				float4 green = tex2D(_MainTex, i.uv + _gOffset);
				//Blue Channel
				float4 blue = tex2D(_MainTex, i.uv + _bOffset);
				
				return float4 (red.r, green.g, blue.b, 1.0f);
			}
			ENDCG
		}
	}
}
