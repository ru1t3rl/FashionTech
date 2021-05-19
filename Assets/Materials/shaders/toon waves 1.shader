Shader "Toon/LitWaterfall" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Foam("Foam", 2D) = "white" {}
		_Distortion("Distortion", 2D) = "white" {}
		_MainScroll("MainTex Scroll/Distort", Range(-2,2)) = 1

		_FoamThickness("Foamline Thickness", Range(0,3)) = 0.5
		_FoamColor("Foam Color", Color) = (1,1,1,1)
		_FoamSpeed("Foam Speed", Vector) = (0,0.5,0,0)
		_TopFoam("Foam", 2D) = "white" {}

		_WaveSpeed("Speed of waves", Range(0,10)) = 1
		_WaveAmount("Amount of waves", Range(0,10)) = 1
		_WaveHeight("Height of waves", Range(0,10)) = 1
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
				_FoamAmount("Foam Amount", Range(0, 2)) = 1

	}

		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "ForceNoShadowCasting" = "True"}
			LOD 500
			Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			#pragma surface surf ToonRamp vertex:vert addshadow
					#pragma target 4.0

			#include "UnityCG.cginc"

			sampler2D _Ramp;

	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
	#pragma lighting ToonRamp exclude_path:prepass
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
		#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
		#endif

		half d = dot(s.Normal, lightDir)*0.5 + 0.5;
		half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
		c.a = 0;
		return c;
	}

	sampler2D _MainTex, _Foam, _TopFoam, _Distortion;

	float4 _Color;
	float _Displacement;

	float _WaveSpeed;
	float _WaveAmount;
	float _WaveHeight, _FoamThickness, _MainScroll;
	float _FoamAmount;
	float4 _FoamSpeed;

	uniform sampler2D _CameraDepthTexture; //Depth Texture
	fixed4 _FoamColor;



	struct Input {
		float2 uv_MainTex ;
		float2 uv_Foam;
		float2 uv_TopFoam;


		float3 worldPos;
		float eyeDepth;

		float4 screenPos;

	};

	// #pragma instancing_options assumeuniformscaling
	UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)




	void vert(inout appdata_full v, out Input o)
	{
		UNITY_INITIALIZE_OUTPUT(Input, o);
		v.vertex.y += sin(_Time.z * _WaveSpeed + (v.vertex.x * (v.vertex.z/2) * _WaveAmount))* _WaveHeight;
		COMPUTE_EYEDEPTH(o.eyeDepth);

	}

	void surf(Input IN, inout SurfaceOutput o) {

		fixed distort = tex2D(_Distortion, IN.uv_MainTex - _FoamSpeed *_Time.x * 10); // distortion alpha

		half4 color = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		float4 foam = tex2D(_Foam, IN.uv_Foam +  _FoamSpeed * _Time.y) * _FoamColor;
		float4 topFoam = tex2D(_TopFoam, IN.uv_TopFoam + (distort * _MainScroll)); // first texture with distortion


		float4 projCoords = UNITY_PROJ_COORD(IN.screenPos);
		float rawZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, projCoords);
		float sceneZ = LinearEyeDepth(rawZ);
		float surfaceZ = IN.eyeDepth;
		float foamEdge = 1 - ((sceneZ - surfaceZ) / _FoamAmount);
		foamEdge = saturate(foamEdge) ;
		color += foamEdge * _FoamColor;
		color += foam + topFoam;


		o.Albedo = color;
		o.Alpha = color.a;
	}
ENDCG

	}
		Fallback "Diffuse"
}