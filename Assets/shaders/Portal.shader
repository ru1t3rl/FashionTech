Shader "Custom/Portal"
{
	Properties
	{
		_InactiveColour("Inactive Colour", Color) = (1, 1, 1, 1)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO //Insert
			};

			sampler2D _MainTex;
			half4 _MainTex_ST;
			float4 _InactiveColour;
			int displayMask; // set to 1 to display texture, otherwise will draw test colour

			v2f vert(appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o.vertex);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				float2 uv = i.screenPos.xy / i.screenPos.w;

				#ifdef UNITY_SINGLE_PASS_STEREO
				// If Single-Pass Stereo mode is active, transform the
				// coordinates to get the correct output UV for the current eye.
				float4 scaleOffset = unity_StereoScaleOffset[unity_StereoEyeIndex];
				uv = (uv - scaleOffset.zw) / scaleOffset.xy;
				#endif

				fixed4 portalCol = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST);
				return portalCol * displayMask + _InactiveColour * (1 - displayMask);
			}
			ENDCG
		}
	}
		Fallback "Unverisal Render Pipeline/Lit" // for shadows
}
