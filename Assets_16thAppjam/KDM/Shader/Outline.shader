Shader "Custom/Outline" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_OutlineColor ("Outline Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		[Toggle] _Outline ("Outline", float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		sampler2D _MainTex;
		
		float _Outline;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _OutlineColor;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex)*_Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			
			if(_Outline)
			{
			    float rim = dot(o.Normal, IN.viewDir);
			    o.Emission = pow(1-rim, 3)*_OutlineColor;
			}
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
