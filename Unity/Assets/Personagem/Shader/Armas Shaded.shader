Shader "Custom/Arma" {
    Properties {
      _Color ("Main Color", Color) = (1.0 ,1.0 ,1.0 ,1.0)
      _MainTex ("Texture", 2D) = "white" {}
      _OutlineColor ("Outline Color", Color) = (0,0,0,1)
	  _Outline ("Outline width", Range (.002, 0.03)) = .005
      _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      UsePass "Toon/Lit/FORWARD"
	  UsePass "Toon/Basic Outline/OUTLINE"
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float2 uv_MainTex;
          float3 viewDir;
      };
      sampler2D _MainTex;
      float4 _RimColor;
      float _RimPower;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
          half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          o.Emission = _RimColor.rgb * pow (rim, _RimPower);
      }
      ENDCG
    } 
	Fallback "Toon/Basic"
  }