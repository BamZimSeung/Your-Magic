// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Joo/Normal Extrusion" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Amount("Extrusion Amount", Range(-1, 1)) = 0.5
		_Outline ("Outline",Range(.002, 0.03)) = .005
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		struct Input {
			float2 uv_MainTex;
		};

		float _Amount;

		void vert(inout appdata_full v){
			v.vertex.xyz += v.normal * _Amount;
		}

		sampler2D _MainTex;

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}
		ENDCG
		Pass{
			Cull front
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _Outline;
			uniform float4 _OutlineColor;

			struct v2f{
				float4 pos : SV_POSITION;
				float3 color : COLOR0;
			};

			v2f vert(appdata_base v){
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
				float2 offset = TransformViewToProjection(norm.xy);
				
				o.pos.xy += offset * o.pos.z * _Outline;
				o.color = _OutlineColor;
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			half4 frag(v2f i) : COLOR{
				return half4(_OutlineColor);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
