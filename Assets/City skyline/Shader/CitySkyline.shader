// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CitySkyline"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_AtlasDiffuse("Atlas Diffuse", 2D) = "white" {}
		_Albedocolor("Albedo color", Color) = (1,1,1,0)
		_Atlas_Reflectivitymask("Atlas_Reflectivitymask", 2D) = "white" {}
		_cubemap("cubemap", CUBE) = "white" {}
		_Fresnelpower("Fresnel power", Range( 0 , 5)) = 0
		_Fresnelintensity("Fresnel intensity", Range( 0 , 10)) = 0
		_Fresnelcolor("Fresnel color", Color) = (1,1,1,0)
		_reflectionblurriness("reflection blurriness", Range( 0 , 5)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldRefl;
			INTERNAL_DATA
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _Albedocolor;
		uniform sampler2D _AtlasDiffuse;
		uniform float4 _AtlasDiffuse_ST;
		uniform samplerCUBE _cubemap;
		uniform float _reflectionblurriness;
		uniform float _Fresnelintensity;
		uniform float _Fresnelpower;
		uniform float4 _Fresnelcolor;
		uniform sampler2D _Atlas_Reflectivitymask;
		uniform float4 _Atlas_Reflectivitymask_ST;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_AtlasDiffuse = i.uv_texcoord * _AtlasDiffuse_ST.xy + _AtlasDiffuse_ST.zw;
			float4 tex2DNode1 = tex2D( _AtlasDiffuse,uv_AtlasDiffuse);
			float3 worldrefVec7 = i.worldRefl;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelFinalVal8 = (0.0 + _Fresnelintensity*pow( 1.0 - dot( worldNormal, worldViewDir ) , _Fresnelpower));
			float2 uv_Atlas_Reflectivitymask = i.uv_texcoord * _Atlas_Reflectivitymask_ST.xy + _Atlas_Reflectivitymask_ST.zw;
			o.Albedo = lerp( ( _Albedocolor * tex2DNode1 ) , ( ( tex2DNode1 * texCUBElod( _cubemap,float4( worldrefVec7, _reflectionblurriness)) ) + ( fresnelFinalVal8 * _Fresnelcolor ) ) , tex2D( _Atlas_Reflectivitymask,uv_Atlas_Reflectivitymask).x ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				float4 texcoords01 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.texcoords01.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.worldRefl = -worldViewDir;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=6001
2567;29;1666;974;919.7982;225.1;1.4;True;True
Node;AmplifyShaderEditor.RangedFloatNode;11;-531.6,614.6001;Float;False;Property;_Fresnelpower;Fresnel power;4;0;0;0;5;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;10;-530.1998,540.4;Float;False;Property;_Fresnelintensity;Fresnel intensity;5;0;0;0;10;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;17;-567.2994,430.9001;Float;False;Property;_reflectionblurriness;reflection blurriness;7;0;0;0;5;FLOAT
Node;AmplifyShaderEditor.WorldReflectionVector;7;-524.3001,272.4;Float;False;0;FLOAT3;0,0,0;False;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;14;-148.5999,650.3003;Float;False;Property;_Fresnelcolor;Fresnel color;6;0;1,1,1,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FresnelNode;8;-149.4,502.5001;Float;False;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-247.4001,300.1;Float;True;Property;_cubemap;cubemap;3;0;Assets/City skyline/Textures/cubemap.png;True;0;False;white;Auto;False;Object;-1;MipLevel;Cube;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;1.0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-247.6,102;Float;True;Property;_AtlasDiffuse;Atlas Diffuse;0;0;Assets/City skyline/Textures/Atlas Diffuse.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;13;-173.5999,-81.99976;Float;False;Property;_Albedocolor;Albedo color;1;0;1,1,1,0;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;106.4001,556.8003;Float;False;0;FLOAT;0,0,0,0;False;1;COLOR;0.0;False;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;112.0019,266.2998;Float;False;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;2;-218.3,898.9001;Float;True;Property;_Atlas_Reflectivitymask;Atlas_Reflectivitymask;2;0;Assets/City skyline/Textures/Atlas_Reflectivitymask.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;106.0002,89.70022;Float;False;0;COLOR;0.0;False;1;FLOAT4;0.0,0,0,0;False;FLOAT4
Node;AmplifyShaderEditor.SimpleAddOpNode;16;226.5999,403.0003;Float;False;0;FLOAT4;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;COLOR
Node;AmplifyShaderEditor.LerpOp;5;537.4,333.9002;Float;False;0;FLOAT4;0.0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT4;0.0;False;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;703,199;Float;False;True;2;Float;ASEMaterialInspector;StandardSpecular;CitySkyline;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
WireConnection;8;2;10;0
WireConnection;8;3;11;0
WireConnection;3;1;7;0
WireConnection;3;2;17;0
WireConnection;15;0;8;0
WireConnection;15;1;14;0
WireConnection;19;0;1;0
WireConnection;19;1;3;0
WireConnection;12;0;13;0
WireConnection;12;1;1;0
WireConnection;16;0;19;0
WireConnection;16;1;15;0
WireConnection;5;0;12;0
WireConnection;5;1;16;0
WireConnection;5;2;2;0
WireConnection;0;0;5;0
ASEEND*/
//CHKSM=B55BA5FC431793C65665ABC8C485E9F7ACAFFEC9