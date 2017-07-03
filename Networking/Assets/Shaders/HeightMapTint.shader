Shader "Custom/HeightMapTint" {
	Properties {
		_ColorMin ("Color Min", Color) = (0,0,0,1)
		_ColorMax ("Color Max", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_HeightMin("Height Min", Float) = -1
		_HeightMax("Height Max", Float) = 1

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 world_Position;
		};

		float _HeightMin;
		float _HeightMax;

		fixed4 _ColorMin;
		fixed4 _ColorMax;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			//Height
			float h = (_HeightMax - IN.world_Position.y) / (_HeightMax - _HeightMin);
			fixed4 tint = Lerp(_ColorMax.rgba, _ColorMin.rgba, h);
			o.Albedo = c.rgb * tint.rgb;
			o.Alpha = c.a * tint.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
