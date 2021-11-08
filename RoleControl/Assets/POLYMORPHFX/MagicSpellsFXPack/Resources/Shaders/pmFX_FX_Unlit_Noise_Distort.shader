// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "pmFX/FX/FX_Unlit_Noise_Distort"
{
	Properties
	{
		_Emissive("Emissive", Float) = 1
		_NoiseTex("Noise Tex", 2D) = "white" {}
		_NoiseScale("Noise Scale", Float) = 1
		_NoiseSpeed("Noise Speed", Float) = 1
		_Opacity("Opacity", Float) = 1
		_MainTex("Main Tex", 2D) = "white" {}
		_MaskTex("Mask Tex", 2D) = "white" {}
		[HDR]_ParticleColor("Particle Color", Color) = (1,1,1,1)
		_DepthDistance("Depth Distance", Float) = 1
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		ZTest LEqual
		Blend One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 5.0
		#pragma surface surf Unlit keepalpha noshadow 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 vertexColor : COLOR;
			float4 screenPos;
		};

		uniform float _Emissive;
		uniform sampler2D _MaskTex;
		uniform float4 _MaskTex_ST;
		uniform float4 _ParticleColor;
		uniform sampler2D _MainTex;
		uniform sampler2D _NoiseTex;
		uniform float _NoiseSpeed;
		uniform float _NoiseScale;
		uniform float _Opacity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_MaskTex = i.uv_texcoord * _MaskTex_ST.xy + _MaskTex_ST.zw;
			float4 tex2DNode26 = tex2D( _MaskTex, uv_MaskTex );
			float2 appendResult43 = (float2(i.uv_tex4coord.z , i.uv_tex4coord.w));
			float2 uv_TexCoord6 = i.uv_texcoord + appendResult43;
			float2 panner7 = ( ( _NoiseSpeed * ( 1.0 - _Time.y ) ) * float2( 1,1 ) + ( _NoiseScale * uv_TexCoord6 ));
			float4 tex2DNode18 = tex2D( _MainTex, ( i.uv_texcoord + ( i.uv_texcoord.y * ( tex2D( _NoiseTex, panner7 ).r - 0.0 ) * i.uv_texcoord.y ) ) );
			float4 clampResult64 = clamp( ( _Emissive * ( tex2DNode26.r * _ParticleColor * tex2DNode18 * i.vertexColor ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			o.Emission = clampResult64.rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth66 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth66 = saturate( abs( ( screenDepth66 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) ) );
			o.Alpha = ( ( _Opacity * i.vertexColor.a * tex2DNode18.a * tex2DNode26.a ) * distanceDepth66 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18908
0;0;1920;1019;-1996.449;108.1034;1.316106;True;False
Node;AmplifyShaderEditor.TexCoordVertexDataNode;42;-2120.43,-153.3478;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;43;-1851.73,-82.3479;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;38;-1147.103,284.1541;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;44;-933.0804,262.0567;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1419.152,12.82077;Inherit;False;Property;_NoiseScale;Noise Scale;3;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1642.192,-128.699;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;46;-1072.384,97.52504;Inherit;False;Property;_NoiseSpeed;Noise Speed;4;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1232.103,-71.69764;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-691.887,159.9077;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;7;-512.642,-61.60715;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-211.3903,102.2608;Inherit;True;Property;_NoiseTex;Noise Tex;2;0;Create;True;0;0;0;False;0;False;-1;None;841d9433c030bc049912059bdf348016;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;365.2532,-83.63402;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;22;188.4165,209.6885;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;611.2278,100.3344;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;868.0262,-20.62416;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;26;1147.889,-530.6481;Inherit;True;Property;_MaskTex;Mask Tex;7;0;Create;True;0;0;0;False;0;False;-1;0b6e54cb876659641ac11d21ad606d90;a94e53355e7b7ee459d037577a8327c3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;59;1243.528,-292.7047;Inherit;False;Property;_ParticleColor;Particle Color;8;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;47;991.1024,131.5862;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;1068.47,-82.43616;Inherit;True;Property;_MainTex;Main Tex;6;0;Create;True;0;0;0;False;0;False;-1;None;0b6e54cb876659641ac11d21ad606d90;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;53;1213.748,177.3409;Inherit;False;Property;_Opacity;Opacity;5;0;Create;True;0;0;0;False;0;False;1;1.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;1754.602,579.712;Inherit;False;Property;_DepthDistance;Depth Distance;9;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;2149.627,-207.0039;Inherit;False;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;2202.823,-488.0433;Inherit;False;Property;_Emissive;Emissive;1;0;Create;True;0;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;1415.847,215.2589;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;66;2028.673,549.9465;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;2583.604,-162.6947;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;2379.061,420.1578;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;40;-969.635,371.0719;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;64;3054.135,-89.84393;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;60;3589.084,-135.2354;Float;False;True;-1;7;ASEMaterialInspector;0;0;Unlit;pmFX/FX/FX_Unlit_Noise_Distort;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;43;0;42;3
WireConnection;43;1;42;4
WireConnection;44;0;38;0
WireConnection;6;1;43;0
WireConnection;11;0;12;0
WireConnection;11;1;6;0
WireConnection;39;0;46;0
WireConnection;39;1;44;0
WireConnection;7;0;11;0
WireConnection;7;1;39;0
WireConnection;1;1;7;0
WireConnection;22;0;1;1
WireConnection;28;0;14;2
WireConnection;28;1;22;0
WireConnection;28;2;14;2
WireConnection;15;0;14;0
WireConnection;15;1;28;0
WireConnection;18;1;15;0
WireConnection;36;0;26;1
WireConnection;36;1;59;0
WireConnection;36;2;18;0
WireConnection;36;3;47;0
WireConnection;54;0;53;0
WireConnection;54;1;47;4
WireConnection;54;2;18;4
WireConnection;54;3;26;4
WireConnection;66;0;67;0
WireConnection;56;0;5;0
WireConnection;56;1;36;0
WireConnection;68;0;54;0
WireConnection;68;1;66;0
WireConnection;64;0;56;0
WireConnection;60;2;64;0
WireConnection;60;9;68;0
ASEEND*/
//CHKSM=62C00B1A16915373B56D41C5D44CCC352873639B