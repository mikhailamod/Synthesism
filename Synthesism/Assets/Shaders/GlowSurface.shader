Shader "Synth/GlowSurface" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_GlowColor("Glow Color", Color) = (1,1,1,1)
		_GlowIntensity ("Glow Intensity", Range(0,3)) = 1.0
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf SimpleLambert

		sampler2D _MainTex;
		half4 _GlowColor;
		float _GlowIntensity;


		half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten)
		{
			if (s.Albedo.r == 1 && s.Albedo.g == 1 && s.Albedo.b == 1)
			{
				half4 c;
				c.rgb = s.Albedo * _GlowColor * _GlowIntensity;
				c.a = 1;
				return c;
			}
			else
			{
				half NdotL = dot(s.Normal, lightDir);
				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten);
				c.a = s.Alpha;
				return c;
			}
			
		}

		struct Input 
		{
			float2 uv_MainTex;
		};

		
		void surf(Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
