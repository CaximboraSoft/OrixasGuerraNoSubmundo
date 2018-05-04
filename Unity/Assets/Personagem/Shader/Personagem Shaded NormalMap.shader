Shader "Custom/PersonagemNormalMap" {
	Properties {
		_Color ("Main Color", Color) = (1.0 ,1.0 ,1.0 ,1.0)
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (.002, 0.03)) = .005
		_BumpMap ("Bumpmap", 2D) = "bump" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		UsePass "Toon/Lit/FORWARD"
		UsePass "Toon/Basic Outline/OUTLINE"
		CGPROGRAM
		#pragma surface surf Lambert
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};
		float4 _Color;
		sampler2D _MainTex;
		sampler2D _BumpMap;
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	} 
	Fallback "Toon/Basic"
}